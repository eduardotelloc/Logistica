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
    public class Pedido : IDisposable
    {

        #region IDisposable
        // Pointer to an external unmanaged resource. 
        private IntPtr handle;
        // Other managed resource this class uses. 
        private Component component = new Component();
        // Track whether Dispose has been called. 
        private bool disposed = false;

        // The class constructor. 
        public Pedido(IntPtr handle)
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
        ~Pedido()
        {
            // Do not re-create Dispose clean-up code here. 
            // Calling Dispose(false) is optimal in terms of 
            // readability and maintainability.
            Dispose(false);
        }
        #endregion

        public Pedido()
        { }


        public string LoadMesa(string sRuc)
        {
            string json = string.Empty;
            string sql = string.Empty;
            List<Entity.Lista> listatmp = new List<Entity.Lista>();

            sql = string.Format("select mesa from pedido where ruc='{0}' and transaccion=1 and enviado=0 and anulado=0;",sRuc);

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
                            mesa = reader["mesa"].ToString()
                        });
                    }
                }
            }


            JavaScriptSerializer serializer = new JavaScriptSerializer();
            json = serializer.Serialize(listatmp);


            return json;
        }

        public string LoadHeader(string sRuc, string sMesa)
        {
            string json = string.Empty;
            string sql = string.Empty;
            List<Entity.PedidoHader> listatmp = new List<Entity.PedidoHader>();

            sql = string.Format("select idpedido as id, idtrabajador, " +
            "num_documento =(select num_documento as trabajador from trabajador where trabajador.idtrabajador= pedido.idtrabajador ), " + 
            "trabajador=(select nombre + ' ' + apellidos as trabajador from trabajador where trabajador.idtrabajador= pedido.idtrabajador )," +
            "convert(varchar(10),fecha,103) as fecha, itbis, valor_venta, igv, total " + 
            "from pedido " + 
            "where ruc='{0}' and mesa='{1}' and transaccion=1 and enviado=0 and anulado=0;", sRuc, sMesa);

            using (SqlConnection conn = new SqlConnection(Conexion.ObtenerConexionVentas()))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    SqlDataReader reader = cmd.ExecuteReader();


                    while (reader.Read())
                    {
                        listatmp.Add(new Entity.PedidoHader
                        {
                            id = reader["id"].ToString(),
                            idtrabajador = reader["idtrabajador"].ToString(),
                            num_documento = reader["num_documento"].ToString(),
                            trabajador = reader["trabajador"].ToString(),
                            fecha = reader["fecha"].ToString(),
                            itbis = reader["itbis"].ToString().Replace(",","."),
                            valor_venta = reader["valor_venta"].ToString().Replace(",","."),
                            igv = reader["igv"].ToString().Replace(",","."),
                            total = reader["total"].ToString().Replace(",",".")
                        });
                    }
                }
            }


            JavaScriptSerializer serializer = new JavaScriptSerializer();
            json = serializer.Serialize(listatmp);


            return json;
        }

        public string LoadHeader(string sId)
        {
            string json = string.Empty;
            string sql = string.Empty;
            List<Entity.PedidoHader> listatmp = new List<Entity.PedidoHader>();

            sql = string.Format("select idpedido as id, idtrabajador, " +
            "num_documento =(select num_documento as trabajador from trabajador where trabajador.idtrabajador= pedido.idtrabajador ), " +
            "trabajador=(select nombre + ' ' + apellidos as trabajador from trabajador where trabajador.idtrabajador= pedido.idtrabajador )," +
            "convert(varchar(10),fecha,103) as fecha, itbis, valor_venta, igv, total " +
            "from pedido " +
            "where idpedido='{0}' and transaccion=1 and enviado=0 and anulado=0;", sId);

            using (SqlConnection conn = new SqlConnection(Conexion.ObtenerConexionVentas()))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    SqlDataReader reader = cmd.ExecuteReader();


                    while (reader.Read())
                    {
                        listatmp.Add(new Entity.PedidoHader
                        {
                            id = reader["id"].ToString(),
                            idtrabajador = reader["idtrabajador"].ToString(),
                            num_documento = reader["num_documento"].ToString(),
                            trabajador = reader["trabajador"].ToString(),
                            fecha = reader["fecha"].ToString(),
                            itbis = reader["itbis"].ToString().Replace(",", "."),
                            valor_venta = reader["valor_venta"].ToString().Replace(",", "."),
                            igv = reader["igv"].ToString().Replace(",", "."),
                            total = reader["total"].ToString().Replace(",", ".")
                        });
                    }
                }
            }


            JavaScriptSerializer serializer = new JavaScriptSerializer();
            json = serializer.Serialize(listatmp);


            return json;
        }

        public string LoadDetail(string sId)
        {
            string json = string.Empty;
            string sql = string.Empty;
            List<Entity.PedidoDetail> listatmp = new List<Entity.PedidoDetail>();

            sql = string.Format("select iddetalle_pedido as id, idarticulo, codarticulo, articulo, unidad, cantidad, preciounit, precio_venta, descuento " +
            "from detalle_pedido " +
            "where idpedido={0};", sId);

            using (SqlConnection conn = new SqlConnection(Conexion.ObtenerConexionVentas()))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    SqlDataReader reader = cmd.ExecuteReader();


                    while (reader.Read())
                    {
                        listatmp.Add(new Entity.PedidoDetail
                        {
                            id = reader["id"].ToString(),
                            idarticulo = reader["idarticulo"].ToString(),
                            codarticulo = reader["codarticulo"].ToString(),
                            articulo = reader["articulo"].ToString(),
                            unidad = reader["unidad"].ToString(),
                            cantidad = reader["cantidad"].ToString(),
                            preciounit = reader["preciounit"].ToString().Replace(",","."),
                            precio_venta = reader["precio_venta"].ToString().Replace(",","."),
                            descuento = reader["descuento"].ToString().Replace(",",".")
                        });
                    }
                }
            }


            JavaScriptSerializer serializer = new JavaScriptSerializer();
            json = serializer.Serialize(listatmp);


            return json;
        }
        
        public string Agregar(string ruc, string idtrabajador, string fecha, string mesa, string valorventa, string igv, string total)
        {
            string codigo = string.Empty;

            using (SqlConnection conn = new SqlConnection(Conexion.ObtenerConexionVentas()))
            {
                conn.Open();

                string sql = string.Format("exec spinsertar_pedido02 '{0}','{1}','{2}','{3}','{4}','{5}','{6}'", 
                     ruc,  idtrabajador,  mesa, fecha,  valorventa,  igv,  total);

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    codigo = cmd.ExecuteScalar().ToString();
                }
            }

            return codigo;
        }

        public void AgregarDetalle(string ruc, string codigo, string idproducto, string producto, string unidad, string cantidad, string preciounit, string precioventa)
        {
            using (SqlConnection conn = new SqlConnection(Conexion.ObtenerConexionVentas()))
            {
                conn.Open();

                string sql = string.Format("exec spinsertar_detalle_pedido02 '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}'",
                     ruc, codigo,  idproducto,  producto,  unidad,  cantidad,  preciounit,  precioventa);

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery().ToString();
                }
            }
        }

        public void AgregarTransaccion(string ruc, string codigo)
        {
            using (SqlConnection conn = new SqlConnection(Conexion.ObtenerConexionVentas()))
            {
                conn.Open();

                string sql = string.Format("update pedido set transaccion=1 where ruc='{1}' and idpedido= '{0}';",codigo, ruc);

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    codigo = cmd.ExecuteNonQuery().ToString();
                }
            }
        }

        public void Eliminar(string id)
        {
            using (SqlConnection conn = new SqlConnection(Conexion.ObtenerConexionVentas()))
            {
                conn.Open();

                string sql = string.Format("update pedido set anulado=1 where  idpedido = {0};", id);

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                }
            }
        }

    }
}
