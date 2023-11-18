using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RankingProgram.Models
{
    public class Ranking
    {
        public int Id { get; set; }

        public int? Posicion { get; set; }

        public string LenguajeProgramacion { get; set; }

        public double Puntos { get; set; }

        public double? Diferencia { get; set; }
    }
}