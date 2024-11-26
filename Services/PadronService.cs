using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using CAEV.PagoLinea.Data;
using CAEV.PagoLinea.Exceptions;
using CAEV.PagoLinea.Models;

namespace CAEV.PagoLinea.Services
{
    public class PadronService {

        private readonly PagoLineaContext context;

        public PadronService(PagoLineaContext context){
            this.context = context;
        }

        /// <summary>
        /// find the user data and throw a exception if the office is disabled
        /// </summary>
        /// <param name="idLocalidad"></param>
        /// <param name="idCuenta"></param>
        /// <param name="sector"></param>
        /// <returns></returns>
        /// <exception cref="OfficeDisabledException"></exception>
        public CuentaPadron? GetPadron(int idLocalidad, int idCuenta, int sector){
            // * find the padron
            var cuentaPadron = context.CuentasPadron
                .Where( item => item.IdLocalidad == idLocalidad)
                .Where( item => item.IdCuenta == idCuenta)
                .Where( item => item.Sector == sector)
                .FirstOrDefault();

            // * check if the office is enabled
            if(cuentaPadron != null)
            {
                var localidad = context.Localidades.Include(item => item.Oficina).FirstOrDefault(item => item.Id == cuentaPadron.IdLocalidad);
                var oficina = context.Oficinas.Find(localidad!.OficinaId);
                if(oficina == null || oficina.Inactivo == true)
                {
                    throw new OfficeDisabledException();
                }
            }

            return cuentaPadron;
        }
        public CuentaPadron? GetPadron(int idPadron){
            return context.CuentasPadron.FirstOrDefault(element=> element.Id == idPadron);
        }

    }

}