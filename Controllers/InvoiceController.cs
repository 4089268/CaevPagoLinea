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
    private readonly MultipagoSettings multipagoSettings;
    private readonly PagoLineaContext pagoLineaContext;

    public InvoiceController(ILogger<InvoiceController> logger, PadronService padronService, IOptions<MultipagoSettings> options, PagoLineaContext pagoLineaContext)
    {
        _logger = logger;
        this.padronService = padronService;
        this.multipagoSettings = options.Value;
        this.pagoLineaContext = pagoLineaContext;
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

        // * make order number and save into the dbcontext

        var orderID = new string(Guid.NewGuid().ToString().Replace("-","").Take(30).ToArray());
        
        // * prepare the paylod for sending to the payment sevice
        var layouRequest = new LayoutEnvio
        {
            Account = this.multipagoSettings.Account,
            Product = this.multipagoSettings.Product,
            Node = this.multipagoSettings.Node.ToString(),
            Concept = LayoutEnvioConceptos.PANUCO.ToString(),
            Ammount = padron.Total,
            Customername = padron.RazonSocial,
            Currency = LayoutEnvioCurrency.PesosMexicanos,
            Urlsuccess = "https://caev.gob.mx/caev/confirmar-pago?status=1",
            Urlfailure = "https://caev.gob.mx/caev/confirmar-pago?status=0",
            Order = orderID.ToString(),
            Reference = ReferenceMaker.GetReference(padron)
        };
        
        // * make the signature 
        layouRequest.Signature = HashUtils.GetHash2(
            string.Format("{0}{1}{2}", layouRequest.Order, layouRequest.Reference, layouRequest.AmmountString),
            this.multipagoSettings.Key
        );

        return View("PreparePayment", layouRequest);
    }

}
