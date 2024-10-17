using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using Microsoft.EntityFrameworkCore;

namespace CAEV.PagoLinea.Models
{
    [Table("Options")]
    public class SystemOption {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id {get;set;}
        public string Key {get;set;} = default!;
        public string Value {get;set;} = default!;
    }

}