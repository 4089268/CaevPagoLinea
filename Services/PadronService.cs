using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using CAEV.PagoLinea.Data;
using CAEV.PagoLinea.Models;

namespace CAEV.PagoLinea.Services
{
    public class PadronService {

        private readonly PagoLineaContext context;

        public PadronService(PagoLineaContext context){
            this.context = context;
        }

        public CuentaPadron? GetPadron(int idLocalidad, int idCuenta, int sector){
            
            return context.CuentasPadron
                .Where( item => item.IdLocalidad == idLocalidad)
                .Where( item => item.IdCuenta == idCuenta)
                .Where( item => item.Sector == sector)
                .FirstOrDefault();
            

        }

    }

}