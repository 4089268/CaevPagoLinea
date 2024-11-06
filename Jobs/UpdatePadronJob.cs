using System;
using System.Numerics;
using CAEV.PagoLinea.Data;
using Quartz;

namespace CAEV.PagoLinea.Services;

public class UpdatePadronJob : IJob {

    private readonly PagoLineaContext pagoLineaContext;
    public UpdatePadronJob(PagoLineaContext pagoLineaContext){
        this.pagoLineaContext = pagoLineaContext;
    }

    public async Task Execute(IJobExecutionContext context) {
        
        Console.WriteLine("Initialized Update Padron Job");
        
        var enlaces = this.pagoLineaContext.Oficinas.Where( item => item.Actualizable == true).ToArray();

        UpdatePadron updatePadron = new(this.pagoLineaContext);
        foreach( var enlace in enlaces){
            Console.WriteLine("Start Ofice " + enlace.Oficina);
            var response = await updatePadron.Update(enlace);
        }

        Console.WriteLine("Finished Update Padron Job");
    }

}
