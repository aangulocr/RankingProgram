using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BodegaHerramientas.Models
{
    public class Empleado
    {
        public int cedula { get; set; }

        public string nombre { get; set; }

        public string apellido { get; set; }

        //public DateTime fecha_ingreso { get; set; }
        public string fecha_ingreso { get; set; }

        public bool activo { get; set; }

    }
}