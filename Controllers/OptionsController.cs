using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using CAEV.PagoLinea.Models;
using CAEV.PagoLinea.Services;
using CAEV.PagoLinea.Data;
using CAEV.PagoLinea.Helpers;

namespace CAEV.PagoLinea.Controllers;

[Authorize]
[Route("/admin/[Controller]")]
public class OptionsController : Controller
{
    private readonly ILogger<OptionsController> _logger;
    private readonly PagoLineaContext pagoLineaContext;

    public OptionsController(ILogger<OptionsController> logger, PagoLineaContext context){
        _logger = logger;
        this.pagoLineaContext = context;
    }


    [HttpGet]
    public ActionResult Index(){

        // * get the current configuration values
        var viewModel = new OptionsIndexViewModel {
            MaintenainceMode = this.pagoLineaContext.SystemOptions.FirstOrDefault(i => i.Key == "ON-MAINTENANCE")?.Value == "1",
            MaintenainceText = (this.pagoLineaContext.SystemOptions.FirstOrDefault(i => i.Key == "MAINTENANCE-TEXT")?.Value) ?? ""
        };

        return View(viewModel);
    }

    [HttpPost]
    [Route("change-status")]
    public IActionResult ChangeStatus([FromForm] int modoMainteinance){
        var modoMainteinanceValue = modoMainteinance == 1;
        var systemOption = this.pagoLineaContext.SystemOptions.FirstOrDefault( item =>item.Key == "ON-MAINTENANCE");
        if(systemOption != null && !string.IsNullOrEmpty(systemOption.Value)){
            systemOption.Value = modoMainteinanceValue ?"1" : "0";
            this.pagoLineaContext.SystemOptions.Update(systemOption);
            this.pagoLineaContext.SaveChanges();
        }
        return Ok();
    }

    [HttpPost]
    [Route("change-mainteinance-text")]
    public IActionResult ChangechangeMainteinanceText([FromForm] string mainteinanceText){
        var systemOption = this.pagoLineaContext.SystemOptions.FirstOrDefault( item =>item.Key == "MAINTENANCE-TEXT");
        if(systemOption != null && !string.IsNullOrEmpty(systemOption.Value)){
            systemOption.Value = mainteinanceText;
            this.pagoLineaContext.SystemOptions.Update(systemOption);
            this.pagoLineaContext.SaveChanges();
        }
        return Ok();
    }

}
