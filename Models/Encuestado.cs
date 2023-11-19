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

        [Required]
        public string Nombre { get; set; }

        [Required]
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