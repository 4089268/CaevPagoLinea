using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace CAEV.PagoLinea.Models {

    public class InvoiceRequest
    {
        [Required(ErrorMessage = "El codigo de Localidad es requerido.")]
        public int Localidad { get; set; }


        [Required(ErrorMessage = "El numero de cuenta es requerido.")]
        public int Cuenta { get; set; }
    }

}