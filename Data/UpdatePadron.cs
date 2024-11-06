using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using CAEV.PagoLinea.Models;
using CAEV.PagoLinea.Services;

namespace CAEV.PagoLinea.Data;

public class UpdatePadron {

    private readonly PagoLineaContext context;
    public UpdatePadron(PagoLineaContext context){
        this.context = context;
    }
    
    public async Task<dynamic> Update(CatOficina oficina){
        using var transaction = await context.Database.BeginTransactionAsync();
        try {

            var arquosService = new ArquosService(oficina.GetConnectionString());

            // * get the new padron
            var padron = arquosService.GetPadron();

            // Disable change tracking to improve bulk insert performance
            context.ChangeTracker.AutoDetectChangesEnabled = false;

            // Load existing records into a dictionary for faster lookup
            var existingRecords = context.CuentasPadron
                .Where(item => item.Oficina == oficina)
                .ToDictionary(item => item.IdPadron);
            
            var newRecords = new List<CuentaPadron>(); // List to hold new records for batch insert

            var createdRecords = 0;
            var updatedRecords = 0;

            // * fill the database with the new records
            foreach( var p in padron){
                
                if (existingRecords.TryGetValue(p.IdPadron, out var record)) {
                    // Update existing record
                    record.RazonSocial = p.RazonSocial;
                    record.Total = p.Total;
                    record.Subtotal = p.Subtotal;
                    record.IVA = p.Iva;
                    record.PeriodoFactura = p.MesFacturado;
                    record.Af = p.Af;
                    record.Mf = p.Mf;
                    record.UpdatedAt = DateTime.Now;

                    // Mark the record as modified
                    context.Entry(record).State = EntityState.Modified;

                    updatedRecords++;
                } else {
                    var newRecord = new CuentaPadron(){
                        IdLocalidad = p.IdLocalidad,
                        Localidad = p.Localidad.ToUpper(),
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
                    newRecords.Add(newRecord);
                    createdRecords ++;
                }
            }

            // Perform batch insert for new records
            if (newRecords.Count > 0) {
                await context.CuentasPadron.AddRangeAsync(newRecords);
            }

            // * save last update
            oficina.UltimaActualizacion = DateTime.Now;
            context.Oficinas.Update(oficina);

            await context.SaveChangesAsync();
            await transaction.CommitAsync();

            context.ChangeTracker.AutoDetectChangesEnabled = true;

            return new {
                TotalRecords = createdRecords,
                UpdatedRecords = updatedRecords
            };
        }
        catch (Exception ex) {
            Console.WriteLine("Error updating the padron office: " + ex.Message );
            await transaction.RollbackAsync();
            throw;
        }
    }

}