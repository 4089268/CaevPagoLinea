using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace CAEV.PagoLinea.Models {

    public class LoginRequest
    {
        [Required(ErrorMessage = "El campo es requerido")]
        [DataType(DataType.EmailAddress)]
        public string? Usuario { get; set; }
        

        [Required(ErrorMessage = "El campo es requerido")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }

}