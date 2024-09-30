using System;
using System.Collections;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CAEV.PagoLinea.Models;
using CAEV.PagoLinea.Services;
using CAEV.PagoLinea.Data;
using CAEV.PagoLinea.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace CAEV.PagoLinea.Controllers;

[Authorize]
[Route("/admin/[Controller]")]
public class OfficeController : Controller
{
    private readonly ILogger<OfficeController> _logger;
    private readonly PagoLineaContext pagoLineaContext;

    public OfficeController(ILogger<OfficeController> logger, PagoLineaContext context){
        _logger = logger;
        this.pagoLineaContext = context;
    }


    [HttpGet]
    public ActionResult Index(){

        var offices = this.pagoLineaContext.Oficinas.ToList();

        return View( offices );
    }

    [HttpGet]
    [Route("updateOffice/{officeId:int}")]
    public ActionResult UpdateOffice(int officeId){
        
        // * retrive the office
        var office = this.pagoLineaContext.Oficinas.FirstOrDefault(item => item.Id == officeId);
        if(office == null){
            ViewBag.Message = "Oficina no encontrada en el sistema.";
            ViewBag.MessageClass = "alert-danger";
            return View();
        }

        ViewBag.Office = office;

        // * validate if the office can update
        if( !office.Actualizable ){
            ViewBag.Message = "No es posible actualizar esta oficina, está configurada como no actualizable.";
            ViewBag.MessageClass = "alert-warning";
            return View();
        }

        return View();
    }


}
