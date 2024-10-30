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
    public async Task<ActionResult> UpdateOffice(int officeId){
        
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

        // update the office
        try {

            var results = await UpdatePadronOffice(office);

            ViewBag.Message = "Oficina actualizada";
            ViewBag.MessageClass = "alert-success";
            ViewBag.TotalRecords = results.TotalRecords;
            ViewBag.ExecutionTimes = (results.ExecutionTimes / 1000);
            return View();
        }
        catch (System.Exception err) {
            ViewBag.Message = "Error al actualizar la oficina: " + err.Message ;
            ViewBag.MessageClass = "alert-danger";
            return View();
        }

    }


    #region Private functions

    private async Task<dynamic> UpdatePadronOffice(CatOficina oficina){
        using var transaction = await this.pagoLineaContext.Database.BeginTransactionAsync();
        try {

            var stopwatch = Stopwatch.StartNew();

            var arquosService = new ArquosService(oficina.GetConnectionString());

            // * get the new padron
            var padron = arquosService.GetPadron();

            // Disable change tracking to improve bulk insert performance
            pagoLineaContext.ChangeTracker.AutoDetectChangesEnabled = false;

            var c = 0;

            // * fill the database with the new records
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
                    Sector = p.Sector,
                    Af = p.Af,
                    Mf = p.Mf,
                    Oficina = oficina
                };
                this.pagoLineaContext.CuentasPadron.Add(newRecord);
                c ++;
            }

            // * save last update
            oficina.UltimaActualizacion = DateTime.Now;
            pagoLineaContext.Oficinas.Update(oficina);

            await this.pagoLineaContext.SaveChangesAsync();
            await transaction.CommitAsync();

            stopwatch.Stop();

            pagoLineaContext.ChangeTracker.AutoDetectChangesEnabled = true;

            return new {
                TotalRecords = c,
                ExecutionTimes = stopwatch.ElapsedMilliseconds
            };
        }
        catch (Exception ex) {
            _logger.LogError(ex, "Error updating the padron office");
            await transaction.RollbackAsync();
            throw;
        }
    }
    #endregion


}
