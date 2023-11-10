using BodegaHerramientas.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BodegaHerramientas.Controllers
{
    public class HomeController : Controller
    {
        private static string conexion = ConfigurationManager.ConnectionStrings["cadena"].ToString();   

        private static List<Empleado> objEmpleado = new List<Empleado>();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Empleados()
        {
            objEmpleado = new List<Empleado>();

            using (SqlConnection objCon = new SqlConnection(conexion)) {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Empleados", objCon);
                cmd.CommandType = CommandType.Text;
                objCon.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        Empleado nuevoEmpleado = new Empleado();

                        nuevoEmpleado.cedula = Convert.ToInt32( reader["cedula"]);
                        nuevoEmpleado.nombre = reader["nombre"].ToString();
                        nuevoEmpleado.apellido = reader["apellido"].ToString();
                        //nuevoEmpleado.fecha_ingreso = Convert.ToDateTime(reader["fecha_ingreso"]);
                        nuevoEmpleado.fecha_ingreso = reader["fecha_ingreso"].ToString();
                        nuevoEmpleado.activo = Convert.ToBoolean(reader["activo"]);

                        objEmpleado.Add(nuevoEmpleado);
                    }
                }
            }
                return View(objEmpleado);
        }

        public ActionResult Herramienta() { 
            return View(); 
        }

        public ActionResult Movimiento()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AgregarEmpleado()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AgregarEmpleado(Empleado objEmpleado) 
        {
            using (SqlConnection objCon = new SqlConnection(conexion))
            {
                //****** Procedimiento Almacenado de BdInventario ******
                SqlCommand cmd = new SqlCommand("sp_InsertarEmpleado", objCon);
                cmd.Parameters.AddWithValue("cedula", objEmpleado.cedula);
                cmd.Parameters.AddWithValue("nombre", objEmpleado.nombre);
                cmd.Parameters.AddWithValue("apellido", objEmpleado.apellido);               
                cmd.Parameters.AddWithValue("fecha_ingreso", objEmpleado.fecha_ingreso);

                var bActivo = objEmpleado.activo.ToString();
                cmd.Parameters.AddWithValue("activo", objEmpleado.activo);
                cmd.CommandType = CommandType.StoredProcedure;
                objCon.Open();
                
                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("AgregarEmpleado","Home");
        }
    }
}