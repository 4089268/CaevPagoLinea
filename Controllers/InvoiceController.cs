using System;
using System.Collections;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CAEV.PagoLinea.Models;
using CAEV.PagoLinea.Services;
using CAEV.PagoLinea.Data;
using Microsoft.Extensions.Options;
using CAEV.PagoLinea.Helpers;
using AspNetCore.ReCaptcha;

namespace CAEV.PagoLinea.Controllers;

public class InvoiceController : Controller
{
    private readonly ILogger<InvoiceController> _logger;
    private readonly PadronService padronService;
    private readonly PagoLineaContext pagoLineaContext;
    private readonly InvoiceService invoiceService;

    public InvoiceController(ILogger<InvoiceController> logger, PadronService padronService, PagoLineaContext pagoLineaContext, InvoiceService invoiceService ) {
        _logger = logger;
        this.padronService = padronService;
        this.pagoLineaContext = pagoLineaContext;
        this.invoiceService = invoiceService;
    }

    [HttpGet]
    [Route("/")]
    public ActionResult Index() {
        return View( new InvoiceRequest());
    }

    [ValidateReCaptcha]
    [HttpPost]
    [Route("/")]
    public ActionResult Index(InvoiceRequest model) {
        if (!ModelState.IsValid){
            return View("Index");
        }

        // * validate if padron exist
        var padron = this.padronService.GetPadron(model.Localidad, model.Cuenta, model.Sector);
        if( padron == null){
            
            ViewBag.ErrorMessage = "No se encontró coincidencias en el sistema";
            
            // Return the view with the model to show validation errors
            return View(model);
        }
        
        // Store data in TempData
        TempData["Localidad"] = model.Localidad;
        TempData["Cuenta"] = model.Cuenta;
        TempData["Sector"] = model.Sector;

        // You can access model.Localidad and model.Cuenta
        return RedirectToAction("InvoiceData");

    }

    public ActionResult InvoiceData(){
        
        // Retrieve data from TempData
        int localidad = 0;
        int cuenta = 0;
        int sector = 0;

        try {
            localidad = Convert.ToInt32(TempData["Localidad"]);
            cuenta = Convert.ToInt32(TempData["Cuenta"]);
            sector = Convert.ToInt32(TempData["Sector"]);
        }
        catch (System.Exception) {
            return RedirectToAction("Index");
        }

        if( localidad == 0 || cuenta == 0 || sector == 0){
            return RedirectToAction("Index");
        }

        // * get the padron
        var padron = this.padronService.GetPadron( localidad, cuenta, sector);
        if( padron != null){
            // * mask the name
            padron!.RazonSocial = MaskString.Mask(padron.RazonSocial);
        }

        
        ViewBag.Message = $"Localidad: {localidad}, Cuenta: {cuenta}";
        
        return View(padron);
    }

    [HttpPost]
    public ActionResult InvoiceData(CuentaPadron padron)
    {
        
        // * reload the data
        this.pagoLineaContext.Entry(padron).Reload();

        // * prepare the paylod for sending to the payment sevice
        try {
            var layouRequest = this.invoiceService.MakeInvoiceRequestPayload(padron);
            return View("PreparePayment", layouRequest);
        }catch(Exception ex){
            this._logger.LogError(ex, "Fail to make the layoutRequest for the payment");
            ViewBag.ErrorMessage = "Error al generar la solicitud de pago, intente de nuevo o comuníquese con el administrador. ";
            return View("PreparePayment", null);
        }
    }

}
