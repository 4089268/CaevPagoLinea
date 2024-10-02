using System;
using System.Collections.Generic;
using CAEV.PagoLinea.Models;

namespace CAEV.PagoLinea {

    public class ReferenceFactory {

        public static string Generate(CuentaPadron padron) {
            return string.Format("{0}{1}{2}{3}{4}{5}",
                padron.IdLocalidad.ToString().PadLeft(3,'0'),
                padron.IdCuenta.ToString().PadLeft(12,'0'),
                padron.IdPadron.ToString().PadLeft(12,'0'),
                padron.Sector.ToString().PadLeft(3,'0'),
                Convert.ToInt32(padron.Total * 100).ToString().PadLeft(12,'0'),
                padron.PeriodoFactura.ToString().ToUpper().Replace(" ","").Trim()
            );
        }

    }
}