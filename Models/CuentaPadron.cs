using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using Microsoft.EntityFrameworkCore;

namespace CAEV.PagoLinea.Models
{
    [Table("CuentasPadron")]
    public class CuentaPadron {
        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id {get;set;}
        public int IdLocalidad {get;set;}
        public string? Localidad {get;set;}
        public int IdPadron {get;set;}
        public int IdCuenta {get;set;}
        public string RazonSocial {get;set;} = "";
        public string Localizacion {get;set;} = "";
        
        [Precision(16, 2)]
        public decimal Subtotal {get;set;}
        [Precision(16, 2)]
        public decimal IVA {get;set;}
        [Precision(16, 2)]
        public decimal Total {get;set;}
        public string PeriodoFactura {get;set;} = "";
        public DateTime CreatedAt {get;set;}
        public DateTime UpdatedAt {get;set;}

        public string TotalFormated {
            get => Total.ToString("c2", new CultureInfo("es-MX"));
        }
    
    }

}