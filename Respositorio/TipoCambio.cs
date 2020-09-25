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
    public class TipoCambio:IDisposable
    {

        #region IDisposable
        // Pointer to an external unmanaged resource. 
        private IntPtr handle;
        // Other managed resource this class uses. 
        private Component component = new Component();
        // Track whether Dispose has been called. 
        private bool disposed = false;

        // The class constructor. 
        public TipoCambio(IntPtr handle)
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
        ~TipoCambio()
        {
            // Do not re-create Dispose clean-up code here. 
            // Calling Dispose(false) is optimal in terms of 
            // readability and maintainability.
            Dispose(false);
        }
        #endregion

        public TipoCambio()
        { }

        public string Load(string sRuc)
        {
            string json = string.Empty;
            string sql = string.Empty;
            
            List<Entity.TipoCambio> listatmp = new List<Entity.TipoCambio>();

            sql = string.Format("SELECT top 10 idtipocambio, convert( varchar(10), fecha_tipo_cambio, 103 ) as fecha_tipo_cambio2, " + 
                                "cambio_venta, cambio_compra " +
                                "FROM [dbo].[tipo_cambio] " +
                                "where anulado=0 and ruc='{0}'" +
                                "order by fecha_tipo_cambio desc ",
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
                        listatmp.Add(new Entity.TipoCambio
                        {
                            id = reader["idtipocambio"].ToString(),
                            fecha_tipo_cambio = reader["fecha_tipo_cambio2"].ToString(),
                            cambio_venta = Convert.ToDecimal(reader["cambio_venta"]).ToString(System.Globalization.CultureInfo.InvariantCulture),
                            cambio_compra = Convert.ToDecimal(reader["cambio_compra"]).ToString(System.Globalization.CultureInfo.InvariantCulture)
                        });
                    }
                }
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            json = serializer.Serialize(listatmp);


            return json;
        }

        public string LoadMontoLast(string sRuc)
        {
            string json = string.Empty;
            string sql = string.Empty;
            string nombre = string.Empty;


            sql = string.Format("SELECT top 1 cambio_venta  " +
                                "FROM [dbo].[tipo_cambio] " +
                                "where anulado=0 and ruc='{0}'" +
                                "order by fecha_tipo_cambio desc ",
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
                        nombre = Convert.ToDecimal(reader["cambio_venta"]).ToString(System.Globalization.CultureInfo.InvariantCulture);
                    }
                }
            }

            return nombre;
        }

        public void Agregar(string ruc, string fecha_tipo_cambio, string cambio_venta, string cambio_compra)
        {
            using (SqlConnection conn = new SqlConnection(Conexion.ObtenerConexionVentas()))
            {
                conn.Open();

                string sql = string.Format("set dateformat  dmy;" +
                 "INSERT INTO tipo_cambio(fecha_tipo_cambio, cambio_venta, cambio_compra, ruc ) " +
                 "VALUES ('{0}','{1}','{2}','{3}')", fecha_tipo_cambio, cambio_venta, cambio_compra, ruc);

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Editar(string id, string fecha_tipo_cambio, string cambio_venta, string cambio_compra)
        {
            using (SqlConnection conn = new SqlConnection(Conexion.ObtenerConexionVentas()))
            {
                conn.Open();

                string sql = string.Format("set dateformat  dmy; " + 
                "update tipo_cambio " +
                "set fecha_tipo_cambio='{1}', cambio_venta='{2}', cambio_compra='{3}' " +
                "where  idtipocambio={0}", id, fecha_tipo_cambio,  cambio_venta,  cambio_compra);

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

                string sql = string.Format("update tipo_cambio " +
                "set Anulado=1 " +
                "where  idtipocambio={0}", Id);

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                }
            }
        }

    }
}
