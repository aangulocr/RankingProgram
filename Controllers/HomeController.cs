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

            try
            {
                using (SqlConnection objCon = new SqlConnection(conexion))
                {
                    SqlCommand cmd = new SqlCommand("sp_GetTopRanking", objCon);

                    cmd.CommandType = CommandType.StoredProcedure;
                    objCon.Open();

                    SqlCommand cmd2 = new SqlCommand("sp_GetTotalPuntos", objCon);
                    cmd2.CommandType = CommandType.StoredProcedure;

                    var numTotal = cmd2.ExecuteScalar();
                    

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
                            //Cambiar el puntaje a Porcentaje 
                            nuevoRanking.Puntos = Convert.ToDouble(reader["Puntos"]);                            
                            nuevoRanking.Diferencia = 0;
                            _rankings.Add(nuevoRanking);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }

            // Devolver la vista
            return View(_rankings);
        }

       
    }
}