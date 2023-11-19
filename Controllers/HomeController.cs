using RankingProgram.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RankingProgram.Controllers
{
    public class HomeController : Controller
    {
        private static string conexion = ConfigurationManager.ConnectionStrings["cadena"].ToString();
        private static List<Ranking> _rankings = new List<Ranking>();

        public ActionResult Index()
        {
            //Generar Ranking
            //Tomar datos de una variable de Aplicacion = Application["NombreVariable"]
            //Tomar datos de una variable de Session = Session["NombreVariable"]
            //Crear variables para llevar el conteo y de los programas y los porcentajes


            // Crear el modelo de datos para el grid view
            _rankings = new List<Ranking>();

            //var test = GetRanking() as List<Ranking>;


            try
            {
                using (SqlConnection objCon = new SqlConnection(conexion))
                {
                    SqlCommand cmd = new SqlCommand("sp_GetTopRanking", objCon);

                    cmd.CommandType = CommandType.StoredProcedure;
                    objCon.Open();

                    SqlCommand cmd2 = new SqlCommand("sp_GetTotalPuntos", objCon);
                    cmd2.CommandType = CommandType.StoredProcedure;

                    HttpContext.Session["TotalPuntos"] = cmd2.ExecuteScalar();


                    //var numTotal = cmd2.ExecuteScalar();

                    var total = Convert.ToDecimal(HttpContext.Session["TotalPuntos"]);


                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        int contador = 0;

                        while (reader.Read())
                        {
                            contador++;
                            Ranking nuevoRanking = new Ranking();

                            nuevoRanking.Id = Convert.ToInt32(reader["Id"]);
                            nuevoRanking.Posicion = contador;
                            nuevoRanking.LenguajeProgramacion = reader["Programa"].ToString().ToUpper();
                            nuevoRanking.Puntos = Convert.ToDouble(reader["Puntos"]);
                            //Cambiar el puntaje a Porcentaje 
                            nuevoRanking.Porcentaje = Convert.ToDouble(reader["Porcentaje"]);                            
                            nuevoRanking.Diferencia = 0;
                            _rankings.Add(nuevoRanking);

                        }
                    }
                    HttpContext.Session["Top20"] = _rankings;
                    SetDiferencia();

                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }
            _rankings = (List<Ranking>)HttpContext.Session["Top20"];
            
            // Devolver la vista
            return View(_rankings);
        }

        private void SetDiferencia()
        {
            //HttpContext.Session["Top20"] = _rankings;
            List<Ranking> _UpdateDiferencia = (List<Ranking>)HttpContext.Session["Top20"];
            
            var _diff = 0.0;
                        
            for (int i = 0; i < _UpdateDiferencia.Count; i++)
            {
               if(i < _UpdateDiferencia.Count - 1)
                {
                    _diff = _UpdateDiferencia[i].Porcentaje - _UpdateDiferencia[i + 1].Porcentaje;
                    _UpdateDiferencia[i].Diferencia = _diff;
                }
                else
                {
                    _UpdateDiferencia[i].Diferencia = 0;
                }

            }

            HttpContext.Session["Top20"] = _UpdateDiferencia;

        }





        private List<Ranking> GetRanking()
        {
            // Guardar un valor en la sesión
            Session["Nombre"] = "Juan";

            // Obtener un valor de la sesión
            string nombre = Session["Nombre"] as string;


            // Otors ejemplos serian
            // Guardar y obtener valores en la sesión y la aplicación
            HttpContext.Session["Nombre2"] = "Marco";
            HttpContext.Application["Titulo"] = "Mi sitio web";
            string nombre2 = HttpContext.Session["Nombre2"] as string;
            string titulo = HttpContext.Application["Titulo"] as string;

            return new List<Ranking>
            {
                new Ranking{Posicion = 1, LenguajeProgramacion = "JavaScript", Puntos = 20.5, Diferencia = 2.5},
                 new Ranking{Posicion = 2, LenguajeProgramacion = "Java", Puntos = 10.5, Diferencia = 6.5},
            };


        }
    }
}