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
            ViewBag.DeletedRecords = results.DeletedRecords;
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
            
            // * get the localidaded of the office
            var localidades = this.pagoLineaContext.Localidades.Where( item => item.OficinaId == oficina.Id).ToList();

            // * delete the old records
            int[] _locadlidadesIds = localidades.Select(item => item.Id).ToArray<int>();
            var DeletedRecords = await this.pagoLineaContext.CuentasPadron.Where( item => _locadlidadesIds.Contains(item.Id)).ExecuteDeleteAsync();
            
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
                    Sector = p.Sector
                };
                _logger.LogInformation("Padron Id '{padron}' added!", p.IdPadron);
                this.pagoLineaContext.CuentasPadron.Add(newRecord);
            }

            var NewRecords = padron.Count;

            // * save last update
            oficina.UltimaActualizacion = DateTime.Now;
            pagoLineaContext.Oficinas.Update(oficina);

            await this.pagoLineaContext.SaveChangesAsync();
            await transaction.CommitAsync();

            stopwatch.Stop();

            return new {
                DeletedRecords,
                NewRecords,
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
