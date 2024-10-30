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
            ViewBag.UpdatedRecords = results.UpdatedRecords;
            ViewBag.NewRecords = results.NewRecords;
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
            var padron = await Task.Run<ICollection<PadronRecord>>( () => arquosService.GetPadron() );

            var newRecords = 0;
            var updatedRecords = 0;

            // * fill the database with the new records
            foreach( var p in padron){
                var _record = this.pagoLineaContext.CuentasPadron.FirstOrDefault( item => item.IdPadron == p.IdPadron && item.IdCuenta == p.IdCuenta && item.IdLocalidad == p.IdLocalidad);

                if(_record != null){
                    _record.RazonSocial = p.RazonSocial;
                    _record.Subtotal = p.Subtotal;
                    _record.IVA = p.Iva;
                    _record.Total = p.Total;
                    _record.PeriodoFactura = p.MesFacturado;
                    _record.Af = p.Af;
                    _record.Mf = p.Mf;
                    _record.UpdatedAt = DateTime.Now;
                    this.pagoLineaContext.CuentasPadron.Update(_record);
                    updatedRecords ++;
                    _logger.LogInformation("Padron Id '{padron}' updated!", p.IdPadron);
                }
                else{
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
                        Mf = p.Mf
                    };
                    this.pagoLineaContext.CuentasPadron.Add(newRecord);
                    newRecords ++;
                    _logger.LogInformation("Padron Id '{padron}' added!", p.IdPadron);
                }
            }

            // * save last update
            oficina.UltimaActualizacion = DateTime.Now;
            pagoLineaContext.Oficinas.Update(oficina);

            await this.pagoLineaContext.SaveChangesAsync();
            await transaction.CommitAsync();

            stopwatch.Stop();

            return new {
                UpdatedRecords = updatedRecords,
                NewRecords = newRecords,
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
