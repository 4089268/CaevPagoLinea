using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CAEV.PagoLinea.Models;
using CAEV.PagoLinea.Services;

namespace CAEV.PagoLinea.Controllers;

public class InvoiceController : Controller
{
    private readonly ILogger<InvoiceController> _logger;
    private readonly PadronService padronService;

    public InvoiceController(ILogger<InvoiceController> logger, PadronService padronService)
    {
        _logger = logger;
        this.padronService = padronService;
    }

    [HttpGet]
    public ActionResult Index()
    {
        return View( new InvoiceRequest(){
            Localidad = 275,
            Cuenta = 1,
        });
    }

    [HttpPost]
    public ActionResult Index(InvoiceRequest model)
    {
        if (!ModelState.IsValid){
            return View("Index");
        }

        // * validate if padron exist
        var padron = this.padronService.GetPadron(model.Localidad, model.Localidad);
        if( padron == null){
            
            ViewBag.ErrorMessage = "No se encontró coincidencias en el sistema";
            
            // Return the view with the model to show validation errors
            return View(model);
        }
        
        
        // Store data in TempData
        TempData["Localidad"] = model.Localidad;
        TempData["Cuenta"] = model.Cuenta;

        // You can access model.Localidad and model.Cuenta
        return RedirectToAction("InvoiceData");

    }


    public ActionResult InvoiceData()
    {
        // Retrieve data from TempData
        int? localidad = TempData["Localidad"] as int?;
        int? cuenta = TempData["Cuenta"] as int?;

        // * get the padron
        var padron = this.padronService.GetPadron( localidad!.Value, cuenta!.Value);
        
        // Check if data exists
        if (localidad.HasValue && cuenta.HasValue) {
            // Use the data as needed
            ViewBag.Message = $"Localidad: {localidad}, Cuenta: {cuenta}";
        }
        else {
            ViewBag.Message = "No data available.";
        }

        return View(padron);
    }
}
