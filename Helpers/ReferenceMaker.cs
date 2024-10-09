using System;
using CAEV.PagoLinea.Models;

namespace CAEV.PagoLinea
{

    public class ReferenceMaker {

        public static string GetReference(CuentaPadron padron){
            return string.Format("{0}{1}{2}{3}{4}",
                padron.IdLocalidad.ToString().PadLeft(3,'0'),
                padron.IdPadron.ToString().PadLeft(10,'0'),
                padron.Af.ToString().PadLeft(4,'0'),
                padron.Mf.ToString().PadLeft(2,'0'),
                Convert.ToUInt32(padron.Total * 100).ToString().PadLeft(11,'0')
            );
        }

    }
}