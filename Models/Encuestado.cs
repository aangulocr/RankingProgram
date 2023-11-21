using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RankingProgram.Models
{
    public class Encuestado
    {
        public int IdEncuesta { get; set; }

        [Required(ErrorMessage = "El Nombre es Requerido")]
        [StringLength(15, MinimumLength = 3, ErrorMessage = "Nombre debe ser entre 3 a 20 Caracteres")]        
        [RegularExpression(@"^[a-zA-Z]+\s?\w*\s?\w*\s?$", ErrorMessage = "Error de Texto")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El Nombre es Requerido")]
        [StringLength(15, MinimumLength = 3, ErrorMessage = "Nombre debe ser entre 3 a 30 Caracteres")]
        [RegularExpression(@"^[a-zA-Z]+\s?\w*\s?\w*\s?$", ErrorMessage = "Error de Texto")]
        public string Apellido { get; set; }

        [Required]
        public List<string> Pais { get; set; }

        [Required]
        public List<string> Role { get; set; }

        [Required]
        public List<string> LenguajePrimario { get; set; }

        [Required]
        public List<string> LenguajeSecundario { get; set; }

    }
}