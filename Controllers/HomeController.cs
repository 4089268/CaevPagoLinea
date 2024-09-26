using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CAEV.PagoLinea.Models;
using CAEV.PagoLinea.Data;
using CAEV.PagoLinea.Services;
using System.Reflection.Metadata.Ecma335;

namespace CAEV.PagoLinea.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly PagoLineaContext context;
    private readonly ArquosService arquosService;

    public HomeController(ILogger<HomeController> logger, PagoLineaContext pagoLineaContext, ArquosService arquosService)
    {
        _logger = logger;
        this.context = pagoLineaContext;
        this.arquosService = arquosService;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public ActionResult LoadPadron(){

        var idLocalidad = 1;

        var padron = this.arquosService.GetPadron();
        
        foreach( var p in padron){
            var newRecord = new CuentaPadron(){
                IdLocalidad = idLocalidad,
                Localidad = p.Ciudad,
                IdPadron = p.IdPadron,
                IdCuenta = p.IdCuenta,
                RazonSocial = p.RazonSocial,
                Localizacion = p.Localizacion??"",
                Subtotal = p.Subtotal,
                IVA = p.Iva,
                Total = p.Total,
                PeriodoFactura = p.MesFacturado
            };
            _logger.LogInformation("Padron '{padron}' added", p.IdPadron);
            this.context.CuentasPadron.Add(newRecord);
        }

        var accounts = this.context.SaveChanges();

        return View();
    }

}
