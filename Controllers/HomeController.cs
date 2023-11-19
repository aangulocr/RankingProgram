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

                    //######### PRIMER COMANDO SQL OBTENER LENGUAJES DE PROGRAMACION #######
                    string query = "SELECT Programa FROM Tbl_ProgramScore";
                    SqlCommand command = new SqlCommand(query, objCon);
                    objCon.Open();

                    SqlDataReader readProg = command.ExecuteReader();
                    List<string> programas = new List<string>();

                    while (readProg.Read())
                    {
                        programas.Add(readProg["Programa"].ToString());
                    }

                    HttpContext.Session["Programas"] = programas;
                    objCon.Close();

                    //######### SEGUNDO COMANDO SQL TOP RANKING ############################
                    SqlCommand cmd = new SqlCommand("sp_GetTopRanking", objCon);

                    cmd.CommandType = CommandType.StoredProcedure;
                    objCon.Open();


                    //####### TERCER COMANDO SQL PUNTOS TOTALES #########################
                    SqlCommand cmd2 = new SqlCommand("sp_GetTotalPuntos", objCon);
                    cmd2.CommandType = CommandType.StoredProcedure;

                    HttpContext.Session["TotalPuntos"] = cmd2.ExecuteScalar();
                    //HttpContext.Application["Lenguajes"]                   

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

        public ActionResult Encuestado()
        {
            
            Encuestado _encuestado = new Encuestado();

            _encuestado.Pais = GetPaises();
            _encuestado.Role = GetRoles();

            //ojo lenguaje primario y secundario revisar esta vacio..
            _encuestado.LenguajePrimario = (List<string>)HttpContext.Session["Programas"];
            _encuestado.LenguajeSecundario = (List<string>)HttpContext.Session["Programas"];

            return View(_encuestado);
        }

        [HttpPost]
        public ActionResult Encuestado(Encuestado _Encuestado)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Error = "Debe Completar Todos los Campos";
                return View();
            }
            string lenguajePrimario = _Encuestado.LenguajePrimario[0].ToString().ToUpper();
            string lenguajeSecundario = _Encuestado.LenguajeSecundario[0].ToString().ToUpper();

            try { 
                using (SqlConnection objCon = new SqlConnection(conexion))
                {
                    // Lenguaje de Programacion primario
                    SqlCommand cmd = new SqlCommand("sp_UpdateRanking", objCon);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Programa", lenguajePrimario);
                    cmd.Parameters.AddWithValue("@Puntos", 1.0);

                    objCon.Open();
                    cmd.ExecuteNonQuery();

                    // Lenguaje de Programacion secundario
                    SqlCommand cmd2 = new SqlCommand("sp_UpdateRanking", objCon);
                    cmd2.CommandType = CommandType.StoredProcedure;

                    cmd2.Parameters.AddWithValue("@Programa", lenguajeSecundario);
                    cmd2.Parameters.AddWithValue("@Puntos", 0.5);

                    //objCon.Open();
                    cmd2.ExecuteNonQuery();

                    TempData["Mensaje"] = "Gracias por completar la encuesta.";
                    var m = _Encuestado;
                    //return View(_Encuestado);
                    return RedirectToAction("Index", "Home");
                }
            }catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Index", "Home");
            }
        }

        private List<string> GetPaises()
        {
            List<string> _paises = new List<string>();

            _paises.Add("Costa Rica");
            _paises.Add("Panama");
            _paises.Add("Nicaragua");
            _paises.Add("El Salvador");
            _paises.Add("Honduras");
            _paises.Add("Guatemala");
            _paises.Add("Mexico");
            _paises.Add("Estados Unidos");
            _paises.Add("Canada");
            _paises.Add("Belice");
            _paises.Add("Argentina");
            _paises.Add("Brazil");
            _paises.Add("Chile");
            _paises.Add("Colombia");
            _paises.Add("Venezuela");
            _paises.Add("Uruguay");

            return _paises;
        }

        private List<string> GetRoles()
        {
            List<string> _roles = new List<string>();

            _roles.Add("Programador Front-end");
            _roles.Add("Programador Back-end");
            _roles.Add("Analista de sistemas");
            _roles.Add("Diseñador gráfico");
            _roles.Add("Administrador de proyectos de TI");            

            return _roles;
        }


    }
}