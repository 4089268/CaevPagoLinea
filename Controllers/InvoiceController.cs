using System;
using System.Collections;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CAEV.PagoLinea.Models;
using CAEV.PagoLinea.Services;
using CAEV.PagoLinea.Data;
using Microsoft.Extensions.Options;
using CAEV.PagoLinea.Helpers;

namespace CAEV.PagoLinea.Controllers;

public class InvoiceController : Controller
{
    private readonly ILogger<InvoiceController> _logger;
    private readonly PadronService padronService;
    private readonly MultipagoSettings multipagoSettings;

    public InvoiceController(ILogger<InvoiceController> logger, PadronService padronService, IOptions<MultipagoSettings> options)
    {
        _logger = logger;
        this.padronService = padronService;
        this.multipagoSettings = options.Value;
    }

    [HttpGet]
    [Route("/")]
    public ActionResult Index() {
        return View( new InvoiceRequest(){
            Localidad = 275,
            Cuenta = 12
        });
    }

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

        // * make the reference number
        var layouRequest = new LayoutEnvio();
        layouRequest.Account = this.multipagoSettings.Account;
        layouRequest.Product = this.multipagoSettings.Product;
        layouRequest.Node = this.multipagoSettings.Node.ToString();
        layouRequest.Concept = LayoutEnvioConceptos.PANUCO.ToString();
        layouRequest.Ammount = padron.Total;
        layouRequest.Customername = padron.RazonSocial;
        layouRequest.Currency = LayoutEnvioCurrency.PesosMexicanos;
        layouRequest.Urlsuccess = "https://caev.gob.mx/caev/confirmar-pago";
        layouRequest.Urlfailure = "https://caev.gob.mx/caev/confirmar-pago";

        layouRequest.Order = Guid.NewGuid().ToString().Replace("-","");
        layouRequest.Reference = string.Format("{0}{1}{2}{3}{4}",
            padron.IdLocalidad.ToString().PadLeft(6,'0'),
            padron.IdCuenta.ToString().PadLeft(12,'0'),
            padron.IdPadron.ToString().PadLeft(12,'0'),
            padron.PeriodoFactura.ToString().ToUpper().Replace(" ","").Trim(),
            (padron.Total * 100).ToString("no").PadLeft(12,'0')
        );

        layouRequest.Signature = HashUtils.GetHash(
            this.multipagoSettings.Key,
            string.Format("{0}{1}{2}", layouRequest.Order, layouRequest.Reference, layouRequest.Ammount)
        );

        return View("PreparePayment", layouRequest);
    }

}
