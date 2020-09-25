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
    public class Impresora:IDisposable
    {

        #region IDisposable
        // Pointer to an external unmanaged resource. 
        private IntPtr handle;
        // Other managed resource this class uses. 
        private Component component = new Component();
        // Track whether Dispose has been called. 
        private bool disposed = false;

        // The class constructor. 
        public Impresora(IntPtr handle)
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
        ~Impresora()
        {
            // Do not re-create Dispose clean-up code here. 
            // Calling Dispose(false) is optimal in terms of 
            // readability and maintainability.
            Dispose(false);
        }
        #endregion

        public Impresora()
        { }

        public string LoadByCodigo(string sLocalCodigo)
        {
            string json = string.Empty;
            string sql = string.Empty;
            
            List<Entity.Lista> listatmp = new List<Entity.Lista>();

            sql = string.Format("select codigo, nombre from Impresora with(nolock) " + 
                                "where LocalCodigo='{0}' " +
                                "order by nombre ",
                                sLocalCodigo
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
                            id = reader["codigo"].ToString(),
                            name = reader["nombre"].ToString()
                        });
                    }
                }
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
             


            json = serializer.Serialize(listatmp);


            return json;
        }


        public void Agregar(string ruc, string nombre, string sexo, string fecha_nacimiento, string tipo_documento,
            string num_documento, string departamento, string provincia, string distrito, string direccion, string ubigeo, 
            string telefono, string email)
        {
            using (SqlConnection conn = new SqlConnection(Conexion.ObtenerConexionVentas()))
            {
                conn.Open();

                string sql = string.Format("set dateformat  dmy; " + 
                 "INSERT INTO Impresora(nombre, sexo, fecha_nacimiento, tipo_documento, num_documento, departamento, provincia, distrito, " + 
                 "direccion, ubigeo, telefono, email, ruc) " +
                 "VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}')", 
                 nombre.Replace("'","''"), sexo, fecha_nacimiento, tipo_documento, num_documento,departamento, provincia, distrito, direccion, ubigeo, 
                 telefono, email, ruc);

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Editar(string id, string nombre, string sexo, string fecha_nacimiento, string tipo_documento,
             string num_documento, string departamento, string provincia, string distrito, string direccion, string ubigeo,
            string telefono, string email)
        {
            using (SqlConnection conn = new SqlConnection(Conexion.ObtenerConexionVentas()))
            {
                conn.Open();

                string sql = string.Format("set dateformat  dmy; " + 
                "update Impresora " +
                "set nombre='{1}', sexo='{2}', fecha_nacimiento='{3}', tipo_documento='{4}', num_documento='{5}', "  +
                "departamento='{6}', provincia='{7}', distrito='{8}', direccion='{9}',ubigeo='{10}', telefono='{11}', email ='{12}' " +
                "where  idImpresora={0}", 
                id, nombre.Replace("'", "''"), sexo, fecha_nacimiento, tipo_documento, num_documento,
                departamento, provincia, distrito, direccion, ubigeo, telefono, email);

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

                string sql = string.Format("update Impresora set Anulado=1 where  idImpresora={0}", id);

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                }
            }
        }

    }
}
