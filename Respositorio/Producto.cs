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
    public class Producto:IDisposable
    {

        #region IDisposable
        // Pointer to an external unmanaged resource. 
        private IntPtr handle;
        // Other managed resource this class uses. 
        private Component component = new Component();
        // Track whether Dispose has been called. 
        private bool disposed = false;

        // The class constructor. 
        public Producto(IntPtr handle)
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
        ~Producto()
        {
            // Do not re-create Dispose clean-up code here. 
            // Calling Dispose(false) is optimal in terms of 
            // readability and maintainability.
            Dispose(false);
        }
        #endregion

        public Producto()
        { }

        public string Load(string sRuc, string sNombre, string sSerie)
        {
            string json = string.Empty;
            string sql = string.Empty;
            
            List<Entity.Articulo> listatmp = new List<Entity.Articulo>();

            sql = string.Format(
                 "if exists(select 1 from articulo where anulado=0 and ruc='{0}' and coalesce(serie,'') ='{1}') " + Environment.NewLine +
                "begin " + Environment.NewLine +
                "   select a.idarticulo, a.codigo, a.idcategoria, coalesce(a.serie,'') as serie , b.nombre as categoria, a.idpresentacion, c.nombre as presentacion, " + Environment.NewLine +
                "           coalesce(a.nombre,'') as nombre, coalesce(a.descripcion,'') as descripcion, " + Environment.NewLine +
                "           coalesce(a.stock,0) as stock, coalesce(a.precio,0.00) as precio, coalesce(a.total,0.00) as total, " + Environment.NewLine +
                "           coalesce(a.bonificacion,0) as bonificacion  " + Environment.NewLine +
                "   from articulo a join " + Environment.NewLine +
                "       categoria b with(nolock) on a.idcategoria=b.idcategoria join " + Environment.NewLine +
                "       presentacion c with(nolock) on a.idpresentacion= c.idpresentacion  " + Environment.NewLine +
                "   where a.anulado=0 and b.anulado=0 and c.anulado=0 and a.ruc='{0}' and serie ='{1}' {2} " + Environment.NewLine +
                "   order by serie, c.nombre, b.nombre, a.nombre ; " + Environment.NewLine +
                "end   " + Environment.NewLine +
                "else  " + Environment.NewLine +
                "begin " + Environment.NewLine +
                 "   select a.idarticulo, a.codigo, a.idcategoria, coalesce(a.serie,'') as serie , b.nombre as categoria, a.idpresentacion, c.nombre as presentacion, " + Environment.NewLine +
                "           coalesce(a.nombre,'') as nombre, coalesce(a.descripcion,'') as descripcion, " + Environment.NewLine +
                "           coalesce(a.stock,0) as stock, coalesce(a.precio,0.00) as precio, coalesce(a.total,0.00) as total, " + Environment.NewLine +
                "           coalesce(a.bonificacion,0) as bonificacion  " + Environment.NewLine +
                "   from articulo a join " + Environment.NewLine +
                "       categoria b with(nolock) on a.idcategoria=b.idcategoria join " + Environment.NewLine +
                "       presentacion c with(nolock) on a.idpresentacion= c.idpresentacion  " + Environment.NewLine +
                "   where a.anulado=0 and b.anulado=0 and c.anulado=0 and a.ruc='{0}' {2} " + Environment.NewLine +
                "   order by serie, c.nombre, b.nombre, a.nombre ; " + Environment.NewLine +
                "end; "
               , sRuc
               ,sSerie 
               , ((sNombre != "") ? "and ( a.nombre like '%" + sNombre.Trim() + "%' or a.codigo='" + sNombre.Trim() + "' )" : "")
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
                        listatmp.Add(new Entity.Articulo
                        {
                            id = reader["idarticulo"].ToString(),
                            codigo = reader["codigo"].ToString(),
                            serie = reader["serie"].ToString(),
                            nombre = reader["nombre"].ToString(),
                            descripcion = reader["descripcion"].ToString(),
                            categoria = reader["categoria"].ToString(),
                            presentacion = reader["presentacion"].ToString(),
                            idcategoria = reader["idcategoria"].ToString(),
                            idpresentacion = reader["idpresentacion"].ToString(),
                            stock = Convert.ToDecimal(reader["stock"]).ToString(System.Globalization.CultureInfo.InvariantCulture),
                            precio = Convert.ToDecimal(reader["precio"]).ToString(System.Globalization.CultureInfo.InvariantCulture),
                            total = Convert.ToDecimal(reader["total"]).ToString(System.Globalization.CultureInfo.InvariantCulture),
                            bonificacion = reader["bonificacion"].ToString()

                        });
                    }
                }
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
             
            json = serializer.Serialize(listatmp);


            return json;
        }

        public string LoadNombre(string sRuc, string usuario_serie)
        {
            string json = string.Empty;
            string sql = string.Empty;
            List<Entity.Entidad> listatmp = new List<Entity.Entidad>();

            sql = string.Format(
                "if exists(select 1 from articulo where anulado=0 and ruc='{0}' and serie ='{1}') " +
                "begin " +
                "   select nombre  from articulo with(nolock) where anulado=0 and ruc='{0}' and serie ='{1}' order by nombre; " +
                "end   " +
                "else  " +
                "begin " +
                "   select nombre  from articulo with(nolock) where anulado=0 and ruc='{0}' and coalesce(serie,'') ='' order by nombre; " +
                "end; "
                , sRuc, usuario_serie);

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


        public string LoadId(string sRuc, string usuario_serie)
        {
            string json = string.Empty;
            string sql = string.Empty;
            List<Entity.Entidad> listatmp = new List<Entity.Entidad>();

            //sql = string.Format("select codigo  from articulo with(nolock) where anulado=0 and ruc='{0}' order by codigo",sRuc);

            sql = string.Format(
               "if exists(select 1 from articulo where anulado=0 and ruc='{0}' and serie ='{1}') " +
               "begin " +
               "   select codigo  from articulo with(nolock) where anulado=0 and ruc='{0}' and serie ='{1}' order by codigo; " +
               "end   " +
               "else  " +
               "begin " +
               "   select codigo  from articulo with(nolock) where anulado=0 and ruc='{0}' and coalesce(serie,'') ='' order by codigo; " +
               "end; " 
              ,sRuc, usuario_serie);

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
                            nombre = reader["codigo"].ToString()
                        });
                    }
                }
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            json = serializer.Serialize(listatmp);


            return json;
        }

        public string LoadByNombre(string sRuc, string sNombre, string sSerie)
        {
            string json = string.Empty;
            string sql = string.Empty;
            string codigo = string.Empty;


            sql = string.Format(
           "if exists(select 1 from articulo where anulado=0 and ruc='{0}' and nombre ='{1}' and serie ='{2}') " +
           "begin " +
           "  select a.idpresentacion, a.codigo, a.stock, a.precio, a.total, a.nombre, " +
           "          presentacion= ( select b.nombre from presentacion b where b.idpresentacion=a.idpresentacion), a.bonificacion " +
           "  from ( " +
           "   select idpresentacion, codigo, precio, total, stock, nombre, bonificacion " +
           "   from articulo with(nolock) " +
           "   where anulado=0 and ruc='{0}' and nombre ='{1}' and serie='{2}' " +
           "    ) a; " +
           "end   " +
           "else  " +
           "begin " +
           "  select a.idpresentacion, a.codigo, a.stock, a.precio, a.total, a.nombre, " +
           "          presentacion= ( select b.nombre from presentacion b where b.idpresentacion=a.idpresentacion), a.bonificacion " +
           "  from ( " +
           "   select idpresentacion, codigo, precio, total, stock, nombre, bonificacion " +
           "   from articulo with(nolock) " +
           "   where anulado=0 and ruc='{0}' and nombre ='{1}' and coalesce(serie,'') =''  " +
           "    ) a; " +
           "end; "
          , sRuc, sNombre, sSerie);




            using (SqlConnection conn = new SqlConnection(Conexion.ObtenerConexionVentas()))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        codigo = reader["codigo"].ToString() + "|" +
                        reader["nombre"].ToString() + "|" +
                        Convert.ToDecimal(reader["stock"]).ToString(System.Globalization.CultureInfo.InvariantCulture) + "|" +
                        Convert.ToDecimal(reader["precio"]).ToString(System.Globalization.CultureInfo.InvariantCulture) + "|" +
                        Convert.ToDecimal(reader["total"]).ToString(System.Globalization.CultureInfo.InvariantCulture) + "|" +
                        reader["presentacion"].ToString() + "|" + 
                        reader["bonificacion"].ToString();

                    }
                }
            }

            return codigo;
        }

        public string LoadById(string sRuc, string sId, string sSerie)
        {
            string json = string.Empty;
            string sql = string.Empty;
            string codigo = string.Empty;

            sql = string.Format(
             "if exists(select 1 from articulo where anulado=0 and ruc='{0}' and codigo ='{1}' and serie ='{2}') " +
             "begin " +
             "  select a.idpresentacion, a.codigo, a.stock, a.precio, a.total, a.nombre, " +
             "          presentacion= ( select b.nombre from presentacion b where b.idpresentacion=a.idpresentacion), a.bonificacion " +
             "  from ( " +
             "   select idpresentacion, codigo, precio, total, stock, nombre, bonificacion " +
             "   from articulo with(nolock) " +
             "   where anulado=0 and ruc='{0}' and codigo ='{1}' and serie='{2}' " +
             "    ) a; " +
             "end   " +
             "else  " +
             "begin " +
             "  select a.idpresentacion, a.codigo, a.stock, a.precio, a.total, a.nombre, " +
             "          presentacion= ( select b.nombre from presentacion b where b.idpresentacion=a.idpresentacion), a.bonificacion " +
             "  from ( " +
             "   select idpresentacion, codigo, precio, total, stock, nombre, bonificacion " +
             "   from articulo with(nolock) " +
             "   where anulado=0 and ruc='{0}' and codigo ='{1}' and coalesce(serie,'') =''  " +
             "    ) a; " +
             "end; "
            , sRuc, sId, sSerie);


       

            using (SqlConnection conn = new SqlConnection(Conexion.ObtenerConexionVentas()))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {

                        codigo = reader["codigo"].ToString() + "|" +
                        reader["nombre"].ToString() + "|" +
                        Convert.ToDecimal(reader["stock"]).ToString(System.Globalization.CultureInfo.InvariantCulture) + "|" +
                        Convert.ToDecimal(reader["precio"]).ToString(System.Globalization.CultureInfo.InvariantCulture) + "|" +
                        Convert.ToDecimal(reader["total"]).ToString(System.Globalization.CultureInfo.InvariantCulture) + "|" +
                        reader["presentacion"].ToString() + "|" +
                        reader["bonificacion"].ToString();

                    }
                }
            }

            return codigo;
        }

        
        public string LoadUnidadByNombre(string sRuc, string sNombre)
        {
            string json = string.Empty;
            string sql = string.Empty;
            string nombre = string.Empty;


            sql = string.Format("select nombre from presentacion where idpresentacion = ( " +
                                "select top 1 idpresentacion from articulo with(nolock) where anulado=0 and ruc='{0}' {1} )",
                                sRuc,
                                ((sNombre != "") ? "and nombre ='" + sNombre + "'" : "")
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

        public void AgregarMasivo(string sRuc, string sCodigo, string sNombre, string sSerie, string sStock, string sPrecio, string sBonificacion)
        {
            using (SqlConnection conn = new SqlConnection(Conexion.ObtenerConexionVentas()))
            {
                conn.Open();

                string sql = string.Format(
                 "declare @idpresentacion int;" +
                 "declare @idcategoria int;" +
                 "set @idpresentacion = (select idpresentacion from presentacion where ruc = '{4}' and CODIGO_SUNAT = 'NIU' and anulado = 0);" +
                 "set @idcategoria = (select idcategoria from categoria where ruc = '{4}' and anulado = 0);" +
                 "if exists(select 1 from articulo where ruc='{4}' and codigo='{0}' and serie='{5}') " +
                 "begin " +
                 "  update articulo set stock={2}, precio={3}, total={3}, bonificacion={6}, anulado=0 " + 
                 "  where ruc='{4}' and codigo='{0}' and serie='{5}'; " +
                 "end   " +
                 "else  " +
                 "begin " +
                 "  INSERT INTO articulo(codigo, nombre, descripcion,idpresentacion, idcategoria, stock, precio, total , ruc, serie, bonificacion) " +
                 "  VALUES ('{0}', '{1}', '',@idpresentacion, @idcategoria, '{2}', '{3}', '{3}', '{4}', '{5}', {6}); " +
                 "end;",
                 sCodigo, sNombre, sStock, sPrecio, sRuc, sSerie, sBonificacion);

                

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Agregar(string sRuc, string sCodigo, string sSerie, string sNombre, string sDescripcion, string sIdCategoria, 
            string sIdPresentacion, string sStock, string sPrecio, string sTotal, string sBonificacion)
        {
            using (SqlConnection conn = new SqlConnection(Conexion.ObtenerConexionVentas()))
            {
                conn.Open();

                string sql = string.Format("INSERT INTO articulo(codigo, nombre, descripcion,idpresentacion, " + 
                 "idcategoria, stock, precio, total , ruc, serie, bonificacion) " +
                 "VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}')", 
                 sCodigo, sNombre, sDescripcion, sIdPresentacion, sIdCategoria, sStock, sPrecio, sTotal, sRuc, sSerie, sBonificacion);

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Editar(string Id, string sCodigo, string sSerie,  string sNombre, string sDescripcion, string sIdCategoria, 
            string sIdPresentacion, string sStock, string sPrecio, string sTotal, string sBonificacion)
        {
            using (SqlConnection conn = new SqlConnection(Conexion.ObtenerConexionVentas()))
            {
                conn.Open();

                string sql = string.Format("update articulo " +
                "set codigo='{1}', Nombre='{2}', Descripcion='{3}', IdCategoria='{4}', IdPresentacion='{5}', Stock='{6}', " +
                "Precio='{7}', Total='{8}', Serie='{9}', Bonificacion='{10}' " +
                "where  idarticulo={0}", 
                Id,sCodigo,  sNombre, sDescripcion, sIdCategoria,  sIdPresentacion, sStock, sPrecio, sTotal, sSerie, sBonificacion);

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

                string sql = string.Format("update articulo set Anulado=1 where  idarticulo={0}", Id);

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                }
            }
        }

    }
}
