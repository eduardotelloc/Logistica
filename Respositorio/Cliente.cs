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
    public class Cliente:IDisposable
    {

        #region IDisposable
        // Pointer to an external unmanaged resource. 
        private IntPtr handle;
        // Other managed resource this class uses. 
        private Component component = new Component();
        // Track whether Dispose has been called. 
        private bool disposed = false;

        // The class constructor. 
        public Cliente(IntPtr handle)
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
        ~Cliente()
        {
            // Do not re-create Dispose clean-up code here. 
            // Calling Dispose(false) is optimal in terms of 
            // readability and maintainability.
            Dispose(false);
        }
        #endregion

        public Cliente()
        { }

        public bool Validar(string sRuc)
        {
            bool blnOK = false ;
            string sql = string.Format("select 1 from empresa where ruc='{0}' and pago=1",sRuc);

            using (SqlConnection conn = new SqlConnection(Conexion.ObtenerConexionVentas()))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    SqlDataReader reader = cmd.ExecuteReader();
                    blnOK = reader.HasRows;
                }
            }

            return blnOK;
        }

        public string LimitePago(string sRuc)
        {
            string strcadena = string.Empty;
            string sql = string.Format("select HORAS_BLOQUEO from empresa where ruc='{0}' and HORAS_BLOQUEO<>0;", sRuc);

            using (SqlConnection conn = new SqlConnection(Conexion.ObtenerConexionVentas()))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        strcadena = string.Format("{0} Horas",reader["HORAS_BLOQUEO"].ToString());
                          
                    }
                }
            }

            return strcadena;
        }

        public string LoadByCodigo(string sRuc, string sCodigo)
        {
            string json = string.Empty;
            string sql = string.Empty;

            List<Entity.Cliente> listatmp = new List<Entity.Cliente>();

            sql = string.Format(
                "select  " +
                "   idcliente, left(nombre,50) as nombre, sexo, convert( char(10), fecha_nacimiento,103) as fecha_nacimiento, " +
                "   tipo_documento, num_documento, departamento, provincia, distrito, left(direccion,50) as direccion, " +
                "   ubigeo, telefono, email, latitud, longitud " +
                "from cliente with(nolock) " +
                "where anulado=0 and ruc='{0}' and idcliente={1} ",
                sRuc, sCodigo);

            using (SqlConnection conn = new SqlConnection(Conexion.ObtenerConexionVentas()))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        listatmp.Add(new Entity.Cliente
                        {
                            id = reader["idcliente"].ToString(),
                            nombre = reader["nombre"].ToString(),
                            sexo = reader["sexo"].ToString(),
                            fecha_nacimiento = reader["fecha_nacimiento"].ToString(),
                            tipo_documento = reader["tipo_documento"].ToString(),
                            num_documento = reader["num_documento"].ToString(),
                            departamento = reader["departamento"].ToString(),
                            provincia = reader["provincia"].ToString(),
                            distrito = reader["distrito"].ToString(),
                            direccion = reader["direccion"].ToString(),
                            ubigeo = reader["ubigeo"].ToString(),
                            telefono = reader["telefono"].ToString(),
                            email = reader["email"].ToString(),
                            latitud = reader["latitud"].ToString(),
                            longitud = reader["longitud"].ToString()
                        });
                    }
                }
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            json = serializer.Serialize(listatmp);
            return json;
        }

        public string Load(string sRuc, string sNombre)
        {
            string json = string.Empty;
            string sql = string.Empty;
            
            List<Entity.Cliente> listatmp = new List<Entity.Cliente>();
            List<string> comodin = new List<string>();

            string[] nombres = sNombre.Split('%');

            foreach (string item in nombres) {
                comodin.Add(string.Format(" (nombre like '%{0}%' or num_documento like '%{0}%') ", item));
            }


            sql = string.Format(
                "select  " +
                "   idcliente, left(nombre,50) as nombre, sexo, convert( char(10), fecha_nacimiento,103) as fecha_nacimiento, " +
                "   tipo_documento, num_documento, departamento, provincia, distrito, left(direccion,50) as direccion, " +
                "   ubigeo, telefono, email,  latitud, longitud " +
                "from cliente with(nolock) " +
                "where anulado=0 and ruc='{0}' {1} " +
                "order by nombre ",
                sRuc,
                (comodin.Count==0)?"": string.Format( " and ( {0} )", String.Join(" or ", comodin.ToArray()))
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
                        listatmp.Add(new Entity.Cliente
                        {
                            id = reader["idcliente"].ToString(),
                            nombre = reader["nombre"].ToString(),
                            sexo = reader["sexo"].ToString(),
                            fecha_nacimiento = reader["fecha_nacimiento"].ToString(),
                            tipo_documento = reader["tipo_documento"].ToString(),
                            num_documento = reader["num_documento"].ToString(),
                            departamento = reader["departamento"].ToString(),
                            provincia = reader["provincia"].ToString(),
                            distrito = reader["distrito"].ToString(),
                            direccion = reader["direccion"].ToString(),
                            ubigeo = reader["ubigeo"].ToString(),
                            telefono = reader["telefono"].ToString(),
                            email = reader["email"].ToString(),
                            latitud = reader["latitud"].ToString(),
                            longitud = reader["longitud"].ToString()
                        });
                    }
                }
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
             
            json = serializer.Serialize(listatmp);
            return json;
        }

        public string LoadDocumento(string sRuc)
        {
            string json = string.Empty;
            string sql = string.Empty;
            List<Entity.Entidad> listatmp = new List<Entity.Entidad>();

            sql = string.Format("select num_documento as nombre  " +
                                "from cliente with(nolock) " +
                                "where anulado=0 and num_documento<>'' and ruc='{0}' " +
                                "order by nombre ",
                                sRuc
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
                        listatmp.Add(new Entity.Entidad
                        {
                            nombre = reader["nombre"].ToString()
                        });
                    }
                }
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            json = serializer.Serialize(listatmp);


            return json;
        }

        public string LoadNombre(string sRuc)
        {
            string json = string.Empty;
            string sql = string.Empty;
            List<Entity.Entidad> listatmp = new List<Entity.Entidad>();

            sql = string.Format("select nombre  " +
                                "from cliente with(nolock) " +
                                "where anulado=0 and nombre<>'' and ruc='{0}' " +
                                "order by nombre ",
                                sRuc
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
                        listatmp.Add(new Entity.Entidad
                        {
                            nombre = reader["nombre"].ToString()
                        });
                    }
                }
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            json = serializer.Serialize(listatmp);


            return json;
        }




        public string LoadDocumentoByTipoDocu(string sRuc, string sTipo)
        {
            string json = string.Empty;
            string sql = string.Empty;
            List<Entity.Entidad> listatmp = new List<Entity.Entidad>();

            sql = string.Format("select num_documento as nombre  " +
                                "from cliente with(nolock) " +
                                "where anulado=0 and num_documento<>'' and ruc='{0}' and tipo_documento='{1}' " +
                                "order by nombre ",
                                sRuc, sTipo
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
                        listatmp.Add(new Entity.Entidad
                        {
                            nombre = reader["nombre"].ToString()
                        });
                    }
                }
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            json = serializer.Serialize(listatmp);


            return json;
        }

        public string LoadNombreByTipoDocu(string sRuc, string sTipo)
        {
            string json = string.Empty;
            string sql = string.Empty;
            List<Entity.Entidad> listatmp = new List<Entity.Entidad>();

            sql = string.Format("select nombre  " +
                                "from cliente with(nolock) " +
                                "where anulado=0 and nombre<>'' and ruc='{0}' and tipo_documento='{1}'  " +
                                "order by nombre ",
                                sRuc, sTipo
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
                        listatmp.Add(new Entity.Entidad
                        {
                            nombre = reader["nombre"].ToString()
                        });
                    }
                }
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            json = serializer.Serialize(listatmp);


            return json;
        }

        public bool ClienteExisteByDocumento(string sRuc, string sDocumento)
        {
            string json = string.Empty;
            string sql = string.Empty;
            bool existe = false;


            sql = string.Format("select 1 from cliente with(nolock) where anulado=0 and ruc='{0}' and num_documento ='{1}'", sRuc, sDocumento );

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

        public string LoadDocumentoByNombre(string sRuc, string sNombre)
        {
            string json = string.Empty;
            string sql = string.Empty;
            string nombre = string.Empty;
            

            sql = string.Format("select num_documento  " +
                                "from cliente with(nolock) " +
                                "where anulado=0 and ruc='{0}' and nombre ='{1}'",sRuc, sNombre.Replace("'","''") );

            using (SqlConnection conn = new SqlConnection(Conexion.ObtenerConexionVentas()))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        nombre = reader["num_documento"].ToString();
                    }
                }
            }

            return nombre;
        }

        public string LoadNombreByDocumento(string sRuc, string sDocumento)
        {
            string json = string.Empty;
            string sql = string.Empty;
            string nombre = string.Empty;
            

            sql = string.Format("select rtrim(ltrim(nombre)) as nombre  " +
                                "from cliente with(nolock) " +
                                "where anulado=0 and ruc='{0}' {1}" ,
                                sRuc,
                                ((sDocumento != "") ? "and num_documento ='" + sDocumento + "'" : "")
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
                        nombre = reader["nombre"].ToString();
                    }
                }
            }

            return nombre;
        }

        public void Agregar(string ruc, string nombre, string sexo, string fecha_nacimiento, string tipo_documento,
            string num_documento, string departamento, string provincia, string distrito, string direccion, string ubigeo, 
            string telefono, string email, string latitud, string longitud)
        {
            using (SqlConnection conn = new SqlConnection(Conexion.ObtenerConexionVentas()))
            {
                conn.Open();

                string sql = string.Format("set dateformat  dmy; " + 
                 "INSERT INTO cliente(nombre, sexo, fecha_nacimiento, tipo_documento, num_documento, departamento, provincia, distrito, " +
                 "direccion, ubigeo, telefono, email, latitud, longitud, ruc) " +
                 "VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}')",
                 nombre.Replace("'", "''").Replace(Environment.NewLine, " ").Trim(),
                 sexo.Trim(),
                 fecha_nacimiento.Trim(),
                 tipo_documento.Trim(),
                 num_documento.Replace("'", "''").Replace(Environment.NewLine, " ").Trim(),
                 departamento.Replace("'","''").Replace(Environment.NewLine, " ").Trim(),
                 provincia.Replace("'", "''").Replace(Environment.NewLine, " ").Trim(),
                 distrito.Replace("'", "''").Replace(Environment.NewLine, " ").Trim(),
                 direccion.Replace("'", "''").Replace(Environment.NewLine, " ").Trim(),
                 ubigeo.Replace("'", "''").Replace(Environment.NewLine, " ").Trim(),
                 telefono.Replace("'", "''").Replace(Environment.NewLine, " ").Trim(),
                 email.Replace("'", "''").Replace(Environment.NewLine, " ").Trim(), 
                 latitud.Replace("'", "''").Replace(Environment.NewLine, " ").Trim(),
                 longitud.Replace("'", "''").Replace(Environment.NewLine, " ").Trim(),
                 ruc);

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Editar(string id, string nombre, string sexo, string fecha_nacimiento, string tipo_documento,
             string num_documento, string departamento, string provincia, string distrito, string direccion, string ubigeo,
            string telefono, string email, string latitud, string longitud)
        {
            using (SqlConnection conn = new SqlConnection(Conexion.ObtenerConexionVentas()))
            {
                conn.Open();

                string sql = string.Format("set dateformat  dmy; " + 
                "update cliente " +
                "set nombre='{1}', sexo='{2}', fecha_nacimiento='{3}', tipo_documento='{4}', num_documento='{5}', "  +
                "departamento='{6}', provincia='{7}', distrito='{8}', direccion='{9}',ubigeo='{10}', telefono='{11}', " +
                "email ='{12}', latitud='{13}', longitud='{14}' " +
                "where  idcliente={0}", 
                id,
                nombre.Replace("'", "''").Replace(Environment.NewLine, " ").Trim(),
                sexo.Trim(),
                fecha_nacimiento.Trim(),
                tipo_documento.Trim(),
                num_documento.Replace("'", "''").Replace(Environment.NewLine, " ").Trim(),
                departamento.Replace("'", "''").Replace(Environment.NewLine, " ").Trim(),
                provincia.Replace("'", "''").Replace(Environment.NewLine, " ").Trim(),
                distrito.Replace("'", "''").Replace(Environment.NewLine, " ").Trim(),
                direccion.Replace("'", "''").Replace(Environment.NewLine, " ").Trim(),
                ubigeo.Replace("'", "''").Replace(Environment.NewLine, " ").Trim(),
                telefono.Replace("'", "''").Replace(Environment.NewLine, " ").Trim(),
                email.Replace("'", "''").Replace(Environment.NewLine, " ").Trim(),
                latitud.Replace("'", "''").Replace(Environment.NewLine, " ").Trim(),
                longitud.Replace("'", "''").Replace(Environment.NewLine, " ").Trim()
                 );

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Eliminar(string id)
        {
            using (SqlConnection conn = new SqlConnection(Conexion.ObtenerConexionVentas()))
            {
                conn.Open();

                string sql = string.Format("update cliente set Anulado=1 where  idcliente={0}", id);

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                }
            }
        }

    }
}
