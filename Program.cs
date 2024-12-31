using System.Collections.Specialized;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection;
using AspNetCore.ReCaptcha;
using CAEV.PagoLinea.Data;
using CAEV.PagoLinea.Services;
using Quartz;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<PagoLineaContext>(options=>{
    options.UseSqlServer( builder.Configuration.GetConnectionString("PagoLinea") );
});
builder.Services.AddScoped<PadronService>();
builder.Services.AddScoped<InvoiceService>();
builder.Services.AddScoped<IPaymentResume,PaymentResumeService>();
builder.Services.Configure<MultipagoSettings>(builder.Configuration.GetSection("MultipagoSettings"));
builder.Services.AddReCaptcha(builder.Configuration.GetSection("ReCaptcha"));
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromMinutes(15);
        options.SlidingExpiration = true;
        options.AccessDeniedPath = "/";
        options.LoginPath = "/Auth/Login";
    });


builder.Services.AddQuartz( q => {

    q.SchedulerId = "Scheduler-Core";

    // these are the defaults
    q.UseSimpleTypeLoader();
    q.UseInMemoryStore();
    q.UseDefaultThreadPool(tp =>
    {
        tp.MaxConcurrency = 10;
    });

    // Define the table prefix (this will add "QRZ." before the default table names)
    q.UsePersistentStore(store =>
    {
        store.UseSqlServer( builder.Configuration.GetConnectionString("PagoLinea")!);
        store.UseProperties = true;
        store.UseClustering();
        store.UseSystemTextJsonSerializer();

        // Set table prefix for Quartz
        store.Properties.Add( new NameValueCollection(){
            ["quartz.jobStore.tablePrefix"] = "QRTZ.QRTZ_",
        });
    });


    // Register the job and trigger
    var jobKey = new JobKey("UpdatePadronJob", "group1");
    q.AddJob<UpdatePadronJob>(jobKey, j => j.WithDescription("Update Padron Job"));
    q.AddTrigger(opts => opts
        .ForJob(jobKey)
        .WithIdentity("Cron Trigger", "group1")
        .StartAt( DateTimeOffset.Now.AddSeconds(15))
        .WithCronSchedule("0 0 1 * * ?")
        .WithDescription("Job scheduled with cron")
    );
});

builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()) {
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseCookiePolicy(new CookiePolicyOptions {
    MinimumSameSitePolicy = SameSiteMode.Lax,
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
