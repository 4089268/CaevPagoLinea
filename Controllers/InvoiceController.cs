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
using System.Globalization;
using CAEV.PagoLinea.Exceptions;

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

        var onMaintenance = this.pagoLineaContext.SystemOptions.FirstOrDefault( item => item.Key == "ON-MAINTENANCE");
        if( onMaintenance != null){
            if( onMaintenance.Value.Trim() == "1"){

                // get the message to display from the configuration table
                string maintenanceMessage = "Disculpa las molestias, Estamos realizando algunas mejoras en nuestro sitio para brindarte un mejor servicio. Por favor, vuelve a intentarlo pronto o contáctanos si tienes alguna duda.";
                try
                {
                    var systemOption = this.pagoLineaContext.SystemOptions.FirstOrDefault( item =>item.Key == "MAINTENANCE-TEXT");
                    if(systemOption != null && !string.IsNullOrEmpty(systemOption.Value)){
                        maintenanceMessage = systemOption.Value;
                    }
                }
                catch (System.Exception) { }

                ViewBag.MaintenanceMessage = maintenanceMessage;
                return View("OnMaintenance");
            }
        }

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
        CuentaPadron? padron = null;
        try
        {
            padron = this.padronService.GetPadron(model.Localidad, model.Cuenta, model.Sector);
        }
        catch (OfficeDisabledException)
        {

            // get the message to display from the configuration table
            string officeDisabledMessage = "La oficina está desactivada actualmente. Por favor, inténtelo más tarde.";
            try
            {
                var systemOption = this.pagoLineaContext.SystemOptions.FirstOrDefault( item =>item.Key == "OFFICE-INACTIVE-TEXT");
                if(systemOption != null && !string.IsNullOrEmpty(systemOption.Value)){
                    officeDisabledMessage = systemOption.Value;
                }
            }
            catch (System.Exception) { }

            ViewBag.ErrorMessage = officeDisabledMessage;
            return View(model);
        }

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

        return View(padron);
    }

    [HttpPost]
    public ActionResult InvoiceData(CuentaPadron padron) {
        
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

    [HttpPost]
    [Consumes("application/x-www-form-urlencoded")]
    [Route("/api/invoice/validate")]
    public ActionResult ValidatePayment([FromForm] IFormCollection formValues, [FromQuery] string? s){

        // * cast the response
        var layoutResponse = new LayoutResponse(){
            Success = Convert.ToInt32(s) == 1,
            MpAccount = Convert.ToInt32( formValues["mp_account"] ),
            MpOrder = formValues["mp_order"]!,
            MpReference = formValues["mp_reference"]!,
            MpConcept = formValues["mp_concept"]!,
            MpResponse = formValues["mp_response"]!,
            MpSignature = formValues["mp_signature"]!,
            MpAmount = Convert.ToDecimal( formValues["mp_amount"] ),
            MpPaymentMethod = formValues["mp_paymentMethod"]!,
            MpAuthorization = formValues["mp_authorization"]!,
            MpResponseMsg = formValues["mp_responsemsg"]
        };

        // * process the response
        var model = this.invoiceService.ProcessPayment(layoutResponse );
        
        return View(model);
    }

}
