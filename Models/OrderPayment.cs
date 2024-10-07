using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using Microsoft.EntityFrameworkCore;

namespace CAEV.PagoLinea.Models
{
    [Table("Opr_OrdenPago")]
    public class OrderPayment {
        
        [Key]
        public string Code {get;set;} = null!;
        public int IdLocalidad {get;set;} = default!;
        public int IdPadron {get;set;} = default!;
        [Precision(16,2)]
        public decimal Ammount {get;set;}
        public string Reference {get;set;} = default!;
        public string Concept {get;set;} = default!;
        public string Node {get;set;} = default!;
        public string? ResponseCode {get;set;}
        public string? Authorization {get;set;}
        public DateTime? CreatedAt {get;set;}
        public DateTime? ResponseAt {get;set;}

        [NotMapped]
        public string ConceptName {
            get {
                return "--";
            }
        }

    }

}