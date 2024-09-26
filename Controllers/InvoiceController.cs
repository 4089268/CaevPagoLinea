using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CAEV.PagoLinea.Models;

namespace CAEV.PagoLinea.Controllers;

public class InvoiceController : Controller
{
    private readonly ILogger<InvoiceController> _logger;

    public InvoiceController(ILogger<InvoiceController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public ActionResult Index()
    {
        return View( new InvoiceRequest(){
            Localidad = 1,
            Cuenta = 12,
        });
    }

    [HttpPost]
    public ActionResult Index(InvoiceRequest model)
    {
        if (ModelState.IsValid)
        {
            // Store data in TempData
            TempData["Localidad"] = model.Localidad;
            TempData["Cuenta"] = model.Cuenta;

            // You can access model.Localidad and model.Cuenta
            return RedirectToAction("InvoiceData");
        }
        
        // If validation fails, return the view with the model to display errors
        return View("Index");
    }


    public ActionResult InvoiceData()
    {
        // Retrieve data from TempData
        int? localidad = TempData["Localidad"] as int?;
        int? cuenta = TempData["Cuenta"] as int?;
        
        // Check if data exists
        if (localidad.HasValue && cuenta.HasValue) {
            // Use the data as needed
            ViewBag.Message = $"Localidad: {localidad}, Cuenta: {cuenta}";
        }
        else {
            ViewBag.Message = "No data available.";
        }

        return View();
    }
}
