using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using Microsoft.EntityFrameworkCore;

namespace CAEV.PagoLinea.Models
{
    [Table("Cat_Localidades")]
    public class CatLocalidad {
        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id {get;set;}
        public string Localidad {get;set;} = default!;
        
        public int OficinaId { get; set; } = default!;

        [ForeignKey("OficinaId")]
        public CatOficina Oficina {get;set;} = default!;
        
    }

}