using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CAEV.PagoLinea.Models;
using CAEV.PagoLinea.Data;
using CAEV.PagoLinea.Services;
using System.Reflection.Metadata.Ecma335;
using Microsoft.EntityFrameworkCore;

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

    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }


    // TODO: move this to a his own controller
    public ActionResult LoadPadron(){

        var padron = this.arquosService.GetPadron();

        this.context.CuentasPadron.ExecuteDelete();
        this.context.SaveChanges();
        
        foreach( var p in padron){
            var newRecord = new CuentaPadron(){
                IdLocalidad = p.IdLocalidad,
                Localidad = p.Localidad,
                IdPadron = p.IdPadron,
                IdCuenta = p.IdCuenta,
                RazonSocial = p.RazonSocial,
                Localizacion = p.Localizacion??"",
                Subtotal = p.Subtotal,
                IVA = p.Iva,
                Total = p.Total,
                PeriodoFactura = p.MesFacturado,
                Sector = p.Sector
            };
            _logger.LogInformation("Padron '{padron}' added", p.IdPadron);
            this.context.CuentasPadron.Add(newRecord);
        }

        ViewBag.Total = this.context.SaveChanges();
        
        return View();
    }

}
