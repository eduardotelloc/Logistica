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
    public class Usuario:IDisposable
    {

        #region IDisposable
        // Pointer to an external unmanaged resource. 
        private IntPtr handle;
        // Other managed resource this class uses. 
        private Component component = new Component();
        // Track whether Dispose has been called. 
        private bool disposed = false;

        // The class constructor. 
        public Usuario(IntPtr handle)
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
        ~Usuario()
        {
            // Do not re-create Dispose clean-up code here. 
            // Calling Dispose(false) is optimal in terms of 
            // readability and maintainability.
            Dispose(false);
        }
        #endregion

        public Usuario()
        { }

        public string Load(string sEmpresa, string login, string rolid)
        {
            string json = string.Empty;
            string sql = string.Empty;
            
            List<Entity.Usuario> listatmp = new List<Entity.Usuario>();

            sql = string.Format("select a.codigo, a.CLIENTE_RUC_DNI, a.CLIENTE_NOMBRE, a.ROL_CODIGO, " +
	                            "       rol=(select z.name from rol z where z.id=a.ROL_CODIGO ) " +
                                "from CLIENTE a where a.EMPRESA_CODIGO={0} {1} {2} order by a.CLIENTE_NOMBRE ",
                                sEmpresa,
                                ((rolid != "") ? "and a.ROL_CODIGO=" + rolid : ""),
                                ((login != "") ? "and a.CLIENTE_NOMBRE like '%" + login + "%'" : "")
                             );

            using (SqlConnection conn = new SqlConnection(Conexion.ObtenerConexionReporte()))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        listatmp.Add(new Entity.Usuario
                        {
                            codigo = reader["codigo"].ToString(),
                            cliente_ruc_dni = reader["CLIENTE_RUC_DNI"].ToString(),
                            cliente_nombre = reader["CLIENTE_NOMBRE"].ToString(),
                            rol_codigo = reader["rol_codigo"].ToString(),
                            rol = reader["rol"].ToString()
                        });
                    }
                }
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            json = serializer.Serialize(listatmp);


            return json;
        }

        public void Agregar(string sEmpresa, string documento, string nombre, string clave, string rolid)
        {
            using (SqlConnection conn = new SqlConnection(Conexion.ObtenerConexionReporte()))
            {
                conn.Open();

                string sql = string.Format( "INSERT INTO Cliente(EMPRESA_CODIGO, CLIENTE_RUC_DNI, CLIENTE_NOMBRE, CLIENTE_CLAVE, ROL_CODIGO) " +
                 "VALUES ('{0}','{1}','{2}','{3}','{4}')", sEmpresa, documento, nombre, clave, rolid);

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Editar(string sEmpresa, string codigo, string rolid)
        {
            using (SqlConnection conn = new SqlConnection(Conexion.ObtenerConexionReporte()))
            {
                conn.Open();

                string sql = string.Format("update cliente set ROL_CODIGO={2} where  EMPRESA_CODIGO={0} and codigo='{1}'", sEmpresa, codigo, rolid);

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Eiminar(string sEmpresa, string codigo)
        {
            using (SqlConnection conn = new SqlConnection(Conexion.ObtenerConexionReporte()))
            {
                conn.Open();

                string sql = string.Format("delete from cliente where EMPRESA_CODIGO={0} and codigo='{1}'", sEmpresa, codigo);

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public string LoadRol()
        {
            string json = string.Empty;

            using (SqlConnection conn = new SqlConnection(Conexion.ObtenerConexionReporte()))
            {
                conn.Open();

                string sql = "select id, name as nombre from [dbo].[Rol] order by name";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    SqlDataReader reader = cmd.ExecuteReader();
                    List<Entity.Entidad> lista = null;

                    if (reader.HasRows) lista = new List<Entity.Entidad>();

                    while (reader.Read())
                    {
                        lista.Add(new Entity.Entidad
                        {
                            id = reader["id"].ToString(),
                            nombre = reader["nombre"].ToString()
                        });
                    }

                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    json = serializer.Serialize(lista);

                }
            }

            return json;
        }

        public string LoadUsuario(string sEmpresa)
        {
            string json = string.Empty;

            using (SqlConnection conn = new SqlConnection(Conexion.ObtenerConexionReporte()))
            {
                conn.Open();

                string sql = string.Format( "select distinct cliente_nombre name from cliente with(nolock) " +
               "where EMPRESA_CODIGO={0} order by cliente_nombre", sEmpresa);

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    SqlDataReader reader = cmd.ExecuteReader();
                    List<Entity.Entidad> lista = null;

                    if (reader.HasRows) lista = new List<Entity.Entidad>();

                    while (reader.Read())
                    {
                        lista.Add(new Entity.Entidad
                        {
                            nombre = reader["name"].ToString()
                        });
                    }

                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    json = serializer.Serialize(lista);

                }
            }

            return json;
        }

        public bool Modificar(string sEmpresa, string sCliente, string sClave)
        {
            bool blnOK = false;

            using (SqlConnection conn = new SqlConnection(Conexion.ObtenerConexionReporte()))
            {
                conn.Open();

                string sql = string.Format("UPDATE [dbo].[CLIENTE] set CLIENTE_CLAVE='{2}' where EMPRESA_CODIGO={0} and CLIENTE_RUC_DNI='{1}';", sEmpresa, sCliente, sClave);

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    int rows= cmd.ExecuteNonQuery();

                    if (rows > 0)
                        blnOK = true;
                }
            }

            return blnOK;
        }


        public bool CambiarClave(string sRuc, string sCliente, string sClave)
        {
            bool blnOK = false;

            using (SqlConnection conn = new SqlConnection(Conexion.ObtenerConexionVentas()))
            {
                conn.Open();

                string sql = string.Format("UPDATE [dbo].[trabajador] set password='{2}' where ruc={0} and idtrabajador='{1}';", sRuc, sCliente, sClave);

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    int rows = cmd.ExecuteNonQuery();

                    if (rows > 0)
                        blnOK = true;
                }
            }

            return blnOK;
        }

        public bool ValidateClient(string sEmpresa, string sCliente, string sClave)
        {
            bool blnOK = false;

            using (SqlConnection conn = new SqlConnection(Conexion.ObtenerConexionReporte()))
            {
                conn.Open();

                string sql = string.Format("select 1 from [dbo].[cliente] where EMPRESA_CODIGO={0} and CLIENTE_RUC_DNI ='{1}' and CLIENTE_CLAVE='{2}'", sEmpresa, sCliente, sClave);

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                        blnOK = true;
                }
            }

            return blnOK ;
        }

        public bool ValidateClientRuc(string sRuc, string sCliente, string sClave)
        {
            bool blnOK = false;

            using (SqlConnection conn = new SqlConnection(Conexion.ObtenerConexionVentas()))
            {
                conn.Open();

                string sql = string.Format("select 1 from [dbo].[trabajador] "  +
                "where ruc='{0}' and usuario ='{1}' and password='{2}'", sRuc, sCliente, sClave);

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                        blnOK = true;
                }
            }

            return blnOK;
        }


        public bool ValidateAdminRuc(string sRuc, string sCliente, string sClave)
        {
            bool blnOK = false;

            using (SqlConnection conn = new SqlConnection(Conexion.ObtenerConexionVentas()))
            {
                conn.Open();

                string sql = string.Format("select 1 from [dbo].[trabajador] " +
                "where acceso='Administrador' and ruc='{0}' and usuario ='{1}' and password='{2}'", sRuc, sCliente, sClave);

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                        blnOK = true;
                }
            }

            return blnOK;
        }

        public Entity.Usuario GetUsuario(string ruc, string usuario)
        {
            Entity.Usuario usu = new Entity.Usuario();

            using (SqlConnection conn = new SqlConnection(Conexion.ObtenerConexionVentas()))
            {
                conn.Open();

                string sql = string.Format("select idtrabajador, num_documento,  acceso, usuario, " + 
                    "coalesce(serie,'') as serie, puntosid, localcodigo " +
                    "from [dbo].[trabajador] " + 
                    "where ruc='{0}' and usuario='{1}'", ruc, usuario);

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    SqlDataReader reader = cmd.ExecuteReader();


                    while (reader.Read())
                    {
                        usu = new Entity.Usuario
                        {
                            codigo = reader["idtrabajador"].ToString(),
                            num_documento = reader["num_documento"].ToString(),
                            acceso = reader["acceso"].ToString(),
                            usuario = reader["usuario"].ToString(),
                            serie = reader["serie"].ToString(),
                            puntosid = reader["puntosid"].ToString(),
                            localcodigo = reader["localcodigo"].ToString()
                        };
                    }
                }
            }

            return usu;
        }

        public string GetUsuariosByNombreToJson(string sEmpresa, string sCliente)
        {
            string json = string.Empty;
            List<Entity.Usuario> lista = new List<Entity.Usuario>();

            using (SqlConnection conn = new SqlConnection(Conexion.ObtenerConexionReporte()))
            {
                conn.Open();

                string sql = string.Format("select codigo, empresa_codigo, cliente_ruc_dni, cliente_nombre,rol_codigo " +
                    "from [dbo].[cliente] with(nolock) " +
                    "where Empresa_codigo='{0}' and cliente_nombre like '%{1}%' " +
                    "order by cliente_nombre asc", sEmpresa, sCliente);

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    SqlDataReader reader = cmd.ExecuteReader();



                    while (reader.Read())
                    {
                        lista.Add( new Entity.Usuario
                        {
                            codigo = reader["codigo"].ToString(),
                            empresa_codigo = reader["empresa_codigo"].ToString(),
                            cliente_ruc_dni = reader["cliente_ruc_dni"].ToString(),
                            cliente_nombre = reader["cliente_nombre"].ToString(),
                            rol_codigo = reader["rol_codigo"].ToString()
                        });
                    }

                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    json = serializer.Serialize(lista);
                }
            }

            return json;
        }

        


        public List<Entity.Lista> GetUsuarios(string sEmpleado)
        {
            List<Entity.Lista> lista= new List<Entity.Lista>();

            using (SqlConnection conn = new SqlConnection(Conexion.ObtenerConexionSunat()))
            {
                conn.Open();

                string sql = string.Format("select distinct CodigoUsuario as id, CodigoUsuario +  '-' + NombreUsuario as  name from [dbo].[vUsuariosEncargosporPersona] " +
                "where CodigoEmpleado='{0}' and CodigoUsuario<>'0' ", sEmpleado);

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    SqlDataReader reader = cmd.ExecuteReader();


                    while (reader.Read())
                    {
                        lista.Add
                        ( 
                            new  Entity.Lista
                            {
                                id = reader["id"].ToString(),
                                name = reader["name"].ToString()
                            }
                        );
                    }
                }
            }

            return lista;
        }


        

        

    }
}
