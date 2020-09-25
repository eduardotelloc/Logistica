using System;
using System.Configuration;

namespace BusinessGlobal
{
    public class Conexion
    {
        public static string ObtenerConexionReporte()
        {
            return ConfigurationManager.ConnectionStrings["dbreporte"].ConnectionString.ToString();
        }

        public static string ObtenerConexionSoluflex()
        {
            return ConfigurationManager.ConnectionStrings["dbsoluflex"].ConnectionString.ToString();
        }

        public static string ObtenerConexionSunat()
        {
            return ConfigurationManager.ConnectionStrings["dbsunat"].ConnectionString.ToString();
        }

        public static string ObtenerConexionVentas()
        {
            return ConfigurationManager.ConnectionStrings["dbventas"].ConnectionString.ToString();
        }

        public static string ObtenerConexionSunat(string database)
        {
            return string.Format(ConfigurationManager.ConnectionStrings["db"].ConnectionString.ToString(), database);
        }
    }
}

