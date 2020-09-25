using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Transactions;

namespace BusinessGlobal
{
    public class Log
    {
        public static void SaveLog(string descripcion, string empresa, string proyecto, string clase, string metodo)
        {
            string hora = DateTime.Now.ToString("HH:mm:ss");

            FileStream stream = new FileStream(ConfigurationManager.AppSettings.Get("PathLog") + @"\" + DateTime.Now.ToString("yyyyMMdd") + ".txt", FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);
            StreamWriter writer = new StreamWriter(stream);
            writer.BaseStream.Seek(0L, SeekOrigin.End);
            writer.WriteLine(hora +".:" + descripcion);
            writer.Flush();
            writer.Close();

            Agregar(empresa,proyecto, clase, metodo, descripcion);
        }

        public static void SaveLogSimple(string descripcion)
        {
            string hora = DateTime.Now.ToString("HH:mm:ss");

            FileStream stream = new FileStream(ConfigurationManager.AppSettings.Get("PathLog") + @"\" + DateTime.Now.ToString("yyyyMMdd") + ".txt", FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);
            StreamWriter writer = new StreamWriter(stream);
            writer.BaseStream.Seek(0L, SeekOrigin.End);
            writer.WriteLine( hora +".:"+ descripcion);
            writer.Flush();
            writer.Close();
        }

        private static void Agregar(string empresa,string proyecto, string clase, string metodo, string descripcion )
        {
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress, new TimeSpan(0, 0, 10)))
                {
                    using (SqlConnection connection = new SqlConnection(Conexion.ObtenerConexionSunat()))
                    {
                        connection.Open();
                        string str = string.Format("insert into [dbo].[TransaccionError](empresa, proyecto, clase, metodo, descripcion) " +
                        "values ('{0}','{1}','{2}','{3}','{4}');", empresa, proyecto, clase, metodo, descripcion.Replace("'", "''"));


                        using (SqlCommand command = new SqlCommand(str, connection))
                        {
                            command.CommandType = CommandType.Text;
                            command.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.SaveLogSimple(string.Format("Error General {0}", ex.Message));
            }
            
        }
    }
}

