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
    public class Trabajador:IDisposable
    {

        #region IDisposable
        // Pointer to an external unmanaged resource. 
        private IntPtr handle;
        // Other managed resource this class uses. 
        private Component component = new Component();
        // Track whether Dispose has been called. 
        private bool disposed = false;

        // The class constructor. 
        public Trabajador(IntPtr handle)
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
        ~Trabajador()
        {
            // Do not re-create Dispose clean-up code here. 
            // Calling Dispose(false) is optimal in terms of 
            // readability and maintainability.
            Dispose(false);
        }
        #endregion

        public Trabajador()
        { }

        public string LoadByCodigo(string ruc, string codigo)
        {
            string json = string.Empty;
            string sql = string.Empty;

            List<Entity.Trabajador> listatmp = new List<Entity.Trabajador>();
           
            sql = string.Format(
                "select idtrabajador, nombre, apellidos, sexo, convert( char(10), fecha_nacimiento,103) as fecha_nacimiento, " +
                "       tipo_documento, num_documento, departamento, provincia, distrito, direccion, telefono, email, acceso, " +
                "       usuario, password, serie, puntosid " +
                "from Trabajador with(nolock) " +
                "where anulado=0 and ruc='{0}' and idtrabajador={1} " +
                "order by nombre, apellidos ",
                ruc, codigo );

            using (SqlConnection conn = new SqlConnection(Conexion.ObtenerConexionVentas()))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        listatmp.Add(new Entity.Trabajador
                        {
                            id = reader["idTrabajador"].ToString(),
                            nombre = reader["nombre"].ToString(),
                            apellidos = reader["apellidos"].ToString(),
                            sexo = reader["sexo"].ToString(),
                            fecha_nacimiento = reader["fecha_nacimiento"].ToString(),
                            tipo_documento = reader["tipo_documento"].ToString(),
                            num_documento = reader["num_documento"].ToString(),
                            departamento = reader["departamento"].ToString(),
                            provincia = reader["provincia"].ToString(),
                            distrito = reader["distrito"].ToString(),
                            direccion = reader["direccion"].ToString(),
                            telefono = reader["telefono"].ToString(),
                            email = reader["email"].ToString(),
                            acceso = reader["acceso"].ToString(),
                            usuario = reader["usuario"].ToString(),
                            password = reader["password"].ToString(),
                            serie = reader["serie"].ToString(),
                            puntosid = reader["puntosid"].ToString()
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
            
            List<Entity.Trabajador> listatmp = new List<Entity.Trabajador>();
            List<string> comodin = new List<string>();

            string[] nombres = nombre.Split('%');

            foreach (string item in nombres)
            {
                comodin.Add(string.Format(" (nombre like '%{0}%' or num_documento like '%{0}%') ", item));
            }

            sql = string.Format(
                "select idTrabajador, nombre, apellidos, sexo, convert( char(10), fecha_nacimiento,103) as fecha_nacimiento, " +
                "       tipo_documento, num_documento, departamento, provincia, distrito, direccion, telefono, email, acceso, " +
                "       usuario, password, serie, puntosid " +
                "from Trabajador with(nolock) " +
                "where anulado=0 and ruc='{0}' {1} " +
                "order by nombre, apellidos ",
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
                        listatmp.Add(new Entity.Trabajador
                        {
                            id = reader["idTrabajador"].ToString(),
                            nombre = reader["nombre"].ToString(),
                            apellidos = reader["apellidos"].ToString(),
                            sexo = reader["sexo"].ToString(),
                            fecha_nacimiento = reader["fecha_nacimiento"].ToString(),
                            tipo_documento = reader["tipo_documento"].ToString(),
                            num_documento = reader["num_documento"].ToString(),
                            departamento = reader["departamento"].ToString(),
                            provincia = reader["provincia"].ToString(),
                            distrito = reader["distrito"].ToString(),
                            direccion = reader["direccion"].ToString(),
                            telefono = reader["telefono"].ToString(),
                            email = reader["email"].ToString(),
                            acceso = reader["acceso"].ToString(),
                            usuario = reader["usuario"].ToString(),
                            password = reader["password"].ToString(),
                            serie = reader["serie"].ToString(),
                            puntosid = reader["puntosid"].ToString()
                        });
                    }
                }
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            json = serializer.Serialize(listatmp);


            return json;
        }

        public string LoadByRol(string ruc, string sAcceso)
        {
            string json = string.Empty;
            string sql = string.Empty;

            List<Entity.Trabajador> listatmp = new List<Entity.Trabajador>();

            sql = string.Format("select idTrabajador, nombre, apellidos, sexo, convert( char(10), fecha_nacimiento,103) as fecha_nacimiento, " +
                                "       tipo_documento, num_documento, direccion, telefono, email, acceso, usuario, password, serie, puntosid " +
                                "from Trabajador with(nolock) " +
                                "where anulado=0 and ruc='{0}' {1} " +
                                "order by nombre, apellidos ",
                                ruc,
                                ((sAcceso != "") ? "and acceso='" + sAcceso + "'" : "")
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
                        listatmp.Add(new Entity.Trabajador
                        {
                            id = reader["idTrabajador"].ToString(),
                            nombre = reader["nombre"].ToString(),
                            apellidos = reader["apellidos"].ToString(),
                            sexo = reader["sexo"].ToString(),
                            fecha_nacimiento = reader["fecha_nacimiento"].ToString(),
                            tipo_documento = reader["tipo_documento"].ToString(),
                            num_documento = reader["num_documento"].ToString(),
                            direccion = reader["direccion"].ToString(),
                            telefono = reader["telefono"].ToString(),
                            email = reader["email"].ToString(),
                            acceso = reader["acceso"].ToString(),
                            usuario = reader["usuario"].ToString(),
                            password = reader["password"].ToString(),
                            serie = reader["serie"].ToString(),
                            puntosid = reader["puntosid"].ToString()
                        });
                    }
                }
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            json = serializer.Serialize(listatmp);


            return json;
        }

        public string LoadDocumento(string ruc)
        {
            string json = string.Empty;
            string sql = string.Empty;
            List<Entity.Entidad> listatmp = new List<Entity.Entidad>();

            sql = string.Format("select num_documento as nombre  " +
                                "from Trabajador with(nolock) " +
                                "where anulado=0 and num_documento<>'' and ruc='{0}' " +
                                "order by nombre, apellidos ",
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

        public string LoadNombreByDocumento(string ruc, string sDocumento)
        {
            string json = string.Empty;
            string sql = string.Empty;
            string nombre = string.Empty;
            

            sql = string.Format("select rtrim(ltrim(nombre + ' ' + apellidos)) as nombre  " +
                                "from Trabajador with(nolock) " +
                                "where anulado=0 and ruc='{0}' {1}" ,
                                ruc,
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

        public void Agregar(string ruc, string nombre, string apellidos, string sexo, string fecha_nacimiento, string tipo_documento,
            string num_documento, string departamento, string provincia, string distrito,  string direccion, 
            string telefono, string email, string acceso, string usuario, string password, string serie, string puntosid)
        {
            using (SqlConnection conn = new SqlConnection(Conexion.ObtenerConexionVentas()))
            {
                conn.Open();

                string sql = string.Format("set dateformat  dmy; " +
                 "INSERT INTO Trabajador(nombre, apellidos, sexo, fecha_nacimiento, tipo_documento, num_documento, " +
                 " departamento, provincia, distrito, direccion, telefono, email, ruc, acceso, usuario, password, serie , puntosid) VALUES " + 
                 "('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}')", 
                 nombre, apellidos, sexo, fecha_nacimiento, tipo_documento, num_documento, departamento, provincia, distrito, 
                 direccion, telefono, email, ruc, acceso, usuario, password, serie, puntosid);

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Editar(string id, string nombre, string apellidos, string sexo, string fecha_nacimiento, string tipo_documento, 
            string num_documento, string departamento, string provincia, string distrito,  string direccion, 
            string telefono, string email, string acceso, string usuario, string password, string serie, string puntosid)
        {
            using (SqlConnection conn = new SqlConnection(Conexion.ObtenerConexionVentas()))
            {
                conn.Open();

                string sql = string.Format("set dateformat  dmy; " + 
                "update Trabajador " +
                "set nombre='{1}', apellidos='{2}', sexo='{3}', fecha_nacimiento='{4}', tipo_documento='{5}', num_documento='{6}', "  +
                "departamento='{7}', provincia='{8}', distrito='{9}', direccion='{10}', telefono='{11}', email ='{12}', acceso ='{13}', " + 
                "usuario='{14}', password='{15}', serie='{16}', puntosid='{17}' " +
                "where  idTrabajador={0}", 
                id, nombre, apellidos, sexo, fecha_nacimiento, tipo_documento, num_documento, departamento, provincia, distrito, 
                direccion,  telefono, email, acceso, usuario, password, serie, puntosid);

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

                string sql = string.Format("update Trabajador " +
                "set anulado=1 " +
                "where  idtrabajador={0}", id);

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                }
            }
        }

    }
}
