using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using System.Web.Script.Serialization;
using System.Collections;

namespace Repositorio
{
    public class Serie:IDisposable
    {

        #region IDisposable
        // Pointer to an external unmanaged resource. 
        private IntPtr handle;
        // Other managed resource this class uses. 
        private Component component = new Component();
        // Track whether Dispose has been called. 
        private bool disposed = false;

        // The class constructor. 
        public Serie(IntPtr handle)
        {
            this.handle = handle;
        }

        // Implement IDisposable. 
        // Do not make this method virtual. 
        // A derived class should not be able to override this method. 
        public void Dispose()
        {
            Dispose(true);
            // This object will be cleaned up by the Dispose method. 
            // Therefore, you should call GC.SupressFinalize to 
            // take this object off the finalization queue 
            // and prevent finalization code for this object 
            // from executing a second time.
            GC.SuppressFinalize(this);
        }

        // Dispose(bool disposing) executes in two distinct scenarios. 
        // If disposing equals true, the method has been called directly 
        // or indirectly by a user's code. Managed and unmanaged resources 
        // can be disposed. 
        // If disposing equals false, the method has been called by the 
        // runtime from inside the finalizer and you should not reference 
        // other objects. Only unmanaged resources can be disposed. 
        protected virtual void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called. 
            if (!this.disposed)
            {
                // If disposing equals true, dispose all managed 
                // and unmanaged resources. 
                if (disposing)
                {
                    // Dispose managed resources.
                    component.Dispose();
                }

                // Call the appropriate methods to clean up 
                // unmanaged resources here. 
                // If disposing is false, 
                // only the following code is executed.
                CloseHandle(handle);
                handle = IntPtr.Zero;

                // Note disposing has been done.
                disposed = true;

            }
        }

        // Use interop to call the method necessary 
        // to clean up the unmanaged resource.
        [System.Runtime.InteropServices.DllImport("Kernel32")]
        private extern static Boolean CloseHandle(IntPtr handle);

        // Use C# destructor syntax for finalization code. 
        // This destructor will run only if the Dispose method 
        // does not get called. 
        // It gives your base class the opportunity to finalize. 
        // Do not provide destructors in types derived from this class.
        ~Serie()
        {
            // Do not re-create Dispose clean-up code here. 
            // Calling Dispose(false) is optimal in terms of 
            // readability and maintainability.
            Dispose(false);
        }
        #endregion

        public Serie()
        { }

        public string LoadByCodigo(string ruc, string codigo)
        {
            string json = string.Empty;
            string sql = string.Empty;

            List<Entity.Serie> listatmp = new List<Entity.Serie>();

            sql = string.Format("SELECT idserie, tipo, indicador, serie, correlativo, " +
                                " case tipo " +
                                " when '01' then 'Factura' " +
                                " when '03' then 'Boleta' " +
                                " when '07' then 'Nota Credito' " +
                                " when '08' then 'Nota debito' end tipodes, " +
                                 " case indicador " +
                                " when 'F' then 'F (Electronico)' " +
                                " when 'B' then 'B (Electronico)' " +
                                " when '0' then 'Manual' end indicadordes " +
                                "FROM [dbo].[serie] " +
                                "where anulado=0 and ruc='{0}' and idserie={1}  ",
                                ruc, codigo
                             );

            using (SqlConnection conn = new SqlConnection(Conexion.ObtenerConexionVentas()))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    SqlDataReader reader = cmd.ExecuteReader();


                    while (reader.Read())
                    {
                        listatmp.Add(new Entity.Serie
                        {
                            id = reader["idserie"].ToString(),
                            tipo = reader["tipo"].ToString(),
                            tipodes = reader["tipodes"].ToString(),
                            indicador = reader["indicador"].ToString(),
                            indicadordes = reader["indicadordes"].ToString(),
                            serie = reader["serie"].ToString(),
                            correlativo = reader["correlativo"].ToString()
                        });
                    }
                }
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            json = serializer.Serialize(listatmp);


            return json;
        }

        public string Load(string ruc, string nombre)
        {
            string json = string.Empty;
            string sql = string.Empty;
            
            List<Entity.Serie> listatmp = new List<Entity.Serie>();
            List<string> comodin = new List<string>();

            string[] nombres = nombre.Split('%');

            foreach (string item in nombres)
            {
                comodin.Add(string.Format(" (serie like '%{0}%') ", item));
            }

            sql = string.Format("SELECT idserie, tipo, indicador, serie, correlativo, " +
                                " case tipo " +
                                " when '01' then 'Factura' " +
                                " when '03' then 'Boleta' " +
                                " when '07' then 'Nota Credito' " +
                                " when '08' then 'Nota Debito' end tipodes, " +
                                 " case indicador " +
                                " when 'F' then 'F (Electronico)' " +
                                " when 'B' then 'B (Electronico)' " +
                                " when '0' then 'Manual' end indicadordes " +
                                "FROM [dbo].[serie] " +
                                "where anulado=0 and ruc='{0}' {1} " +
                                "order by tipo, indicador, serie desc ",
                                ruc,
                                (comodin.Count == 0) ? "" : string.Format(" and ( {0} )", String.Join(" or ", comodin.ToArray()))
                             );

            using (SqlConnection conn = new SqlConnection(Conexion.ObtenerConexionVentas()))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    SqlDataReader reader = cmd.ExecuteReader();
                    

                    while (reader.Read())
                    {
                        listatmp.Add(new Entity.Serie
                        {
                            id = reader["idserie"].ToString(),
                            tipo = reader["tipo"].ToString(),
                            tipodes = reader["tipodes"].ToString(),
                            indicador = reader["indicador"].ToString(),
                            indicadordes = reader["indicadordes"].ToString(),
                            serie = reader["serie"].ToString(),
                            correlativo = reader["correlativo"].ToString()
                        });
                    }
                }
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            json = serializer.Serialize(listatmp);


            return json;
        }


        public string LoadSelect(string ruc)
        {
            string json = string.Empty;
            string sql = string.Empty;

            List<Entity.Lista> listatmp = new List<Entity.Lista>();

            sql = string.Format("SELECT distinct tipo as id , " +
                                " case tipo " +
                                " when '01' then 'Factura' " +
                                " when '03' then 'Boleta' " +
                                " when '07' then 'Nota Cred' " +
                                " when '08' then 'Nota Deb' end  as name, orden " +
                                "FROM [dbo].[serie] wiht(nolock) " +
                                "where anulado=0 and indicador<>'0' and ruc='{0}' " +
                                "order by orden ",
                                ruc
                             );

            using (SqlConnection conn = new SqlConnection(Conexion.ObtenerConexionVentas()))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    SqlDataReader reader = cmd.ExecuteReader();


                    while (reader.Read())
                    {
                        listatmp.Add(new Entity.Lista
                        {
                            id = reader["id"].ToString(),
                            name = reader["name"].ToString()
                        });
                    }
                }
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            json = serializer.Serialize(listatmp);


            return json;
        }

        public string LoadSeries(string ruc, string tipo, string indicador, string serie)
        {
            string json = string.Empty;
            string sql = string.Empty;
            
            List<Entity.Lista> listatmp = new List<Entity.Lista>();

            sql = string.Format("select serie, (correlativo+1) as correlativo " + 
                                "from serie " + 
                                "where anulado=0 and ruc='{0}' and tipo='{1}' and indicador='{2}' {3}  " +
                                "order by serie asc ",
                                ruc,
                                tipo, 
                                indicador, 
                                (serie=="")?"": string.Format( " and serie='{0}'",serie)
                             );

            using (SqlConnection conn = new SqlConnection(Conexion.ObtenerConexionVentas()))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    SqlDataReader reader = cmd.ExecuteReader();


                    while (reader.Read())
                    {
                        listatmp.Add(new Entity.Lista
                        {
                            id= reader["correlativo"].ToString(),
                            name= reader["serie"].ToString()
                        });
                    }
                }
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            json = serializer.Serialize(listatmp);
            return json;
        }


        public string LoadSeriesCorrelativo(string ruc, string tipo, string indicador, string serie)
        {
            string json = string.Empty;
            string sql = string.Empty;

            List<Entity.Lista> listatmp = new List<Entity.Lista>();

            sql = string.Format("select serie, (correlativo+1) as correlativo " +
                                "from serie " +
                                "where anulado=0 and ruc='{0}' and tipo='{1}' and indicador='{2}' {3}  " +
                                "order by serie asc ",
                                ruc,
                                tipo,
                                indicador,
                                (serie == "") ? "" : string.Format(" and serie='{0}'", serie)
                             );

            using (SqlConnection conn = new SqlConnection(Conexion.ObtenerConexionVentas()))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    SqlDataReader reader = cmd.ExecuteReader();


                    while (reader.Read())
                    {
                        json = reader["correlativo"].ToString();
                    }
                }
            }

            return json;
        }

        public bool SerieExisteByDocumento(string ruc, string tipo, string indicador, string serie)
        {
            string json = string.Empty;
            string sql = string.Empty;
            bool existe = false;


            sql = string.Format("select 1 from serie with(nolock) " +
                "where anulado=0 and ruc='{0}' and tipo ='{1}' and indicador='{2}' and serie='{3}'",
                ruc, tipo, indicador, serie);

            using (SqlConnection conn = new SqlConnection(Conexion.ObtenerConexionVentas()))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    SqlDataReader reader = cmd.ExecuteReader();
                    existe = reader.HasRows;
                }
            }

            return existe;
        }
        public void Agregar(string ruc, string tipo, string indicador, string serie, string correlativo)
        {
            using (SqlConnection conn = new SqlConnection(Conexion.ObtenerConexionVentas()))
            {
                conn.Open();

                string sql = string.Format("set dateformat  dmy;" +
                 "INSERT INTO serie(tipo, indicador, serie, correlativo, ruc ) " +
                 "VALUES ('{0}','{1}','{2}','{3}','{4}')", tipo, indicador, serie, correlativo, ruc);

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Editar(string id, string tipo, string indicador, string serie, string correlativo)
        {
            using (SqlConnection conn = new SqlConnection(Conexion.ObtenerConexionVentas()))
            {
                conn.Open();

                string sql = string.Format("set dateformat  dmy; " + 
                "update serie " +
                "set tipo='{1}', indicador='{2}', serie='{3}', correlativo='{4}' " +
                "where  idserie={0}", id, tipo, indicador, serie, correlativo);

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Eliminar(string Id)
        {
            using (SqlConnection conn = new SqlConnection(Conexion.ObtenerConexionVentas()))
            {
                conn.Open();

                string sql = string.Format("update serie " +
                "set anulado=1 " +
                "where  idserie={0}", Id);

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                }
            }
        }

    }
}
