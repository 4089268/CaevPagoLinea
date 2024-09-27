using CAEV.PagoLinea.Data;
using CAEV.PagoLinea.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<PagoLineaContext>(options=>{
    options.UseSqlServer( builder.Configuration.GetConnectionString("PagoLinea") );
});
builder.Services.AddScoped<ArquosService>();
builder.Services.AddScoped<PadronService>();
builder.Services.Configure<MultipagoSettings>(builder.Configuration.GetSection("MultipagoSettings"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
