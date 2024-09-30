using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using Microsoft.EntityFrameworkCore;

namespace CAEV.PagoLinea.Models
{
    [Table("Cat_Oficinas")]
    public class CatOficina {
        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id {get;set;}
        public string Oficina {get;set;} = default!;
        public string Servidor {get;set;} = default!;
        public string BaseDatos{get;set;} = default!;
        public string Usuario {get;set;} = default!;
        public string Contraseña {get;set;} = default!;
        public bool Actualizable {get;set;}
        public DateTime? UltimaActualizacion {get;set;}

        public ICollection<CatLocalidad> Localidades { get; set; } = new List<CatLocalidad>();

    }

}