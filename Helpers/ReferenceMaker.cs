using System;
using CAEV.PagoLinea.Models;

namespace CAEV.PagoLinea
{

    public class ReferenceMaker {

        public static string GetReference(CuentaPadron padron){
            return string.Format("{0}{1}{2}{3}",
                padron.IdLocalidad.ToString().PadLeft(3,'0'),
                padron.IdPadron.ToString().PadLeft(10,'0'),
                padron.IdCuenta.ToString().PadLeft(7,'0'),
                Convert.ToUInt32(padron.Total * 100).ToString().PadLeft(10,'0')
            );
        }

    }
}