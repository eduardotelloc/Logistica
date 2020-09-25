using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositorio
{
    public class Conexion
    {
        

        public static string ObtenerConexionSunat()
        {
            return ConfigurationManager.ConnectionStrings["dbsunat"].ConnectionString.ToString();
        }

        public static string ObtenerConexionVentas()
        {
            return ConfigurationManager.ConnectionStrings["dbventas"].ConnectionString.ToString();
        }

        public static string ObtenerConexionReporte()
        {
            return ConfigurationManager.ConnectionStrings["dbreporte"].ConnectionString.ToString();
        }

       
    }
}
