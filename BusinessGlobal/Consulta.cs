using Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Net.NetworkInformation;


namespace BusinessGlobal
{
    public class Consulta : IDisposable
    {

        #region IDisposable
        // Pointer to an external unmanaged resource. 
        private IntPtr handle;
        // Other managed resource this class uses. 
        private Component component = new Component();
        // Track whether Dispose has been called. 
        private bool disposed = false;

        // The class constructor. 
        public Consulta(IntPtr handle)
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
        ~Consulta()
        {
            // Do not re-create Dispose clean-up code here. 
            // Calling Dispose(false) is optimal in terms of 
            // readability and maintainability.
            Dispose(false);
        }
        #endregion

        public Consulta() { }

        public  Empresa ConsultaEmpresaById(string codigo)
        {
            Empresa obj = new Empresa();

            using (SqlConnection connection = new SqlConnection(Conexion.ObtenerConexionSunat()))
            {
                connection.Open();
                string cmdText = string.Format( "SELECT codigo, BASEDATOS, ruc, Nombre,  " +
                    "USUA_FACT, CLAVE_FACT, " + 
                    "USUA_NCRE, CLAVE_NCRE," +
                    "USUA_NDEB, CLAVE_NDEB," +
                    "USUA_RETE, CLAVE_RETE," +
                    "USUA_PERC, CLAVE_PERC, "+
                    "USUA_GUIA, CLAVE_GUIA," +
                    "USUA_CONSULTA, CLAVE_CONSULTA," +
                    "Documento, GenerarPdf, " +
                    "PAIS, DEPARTAMENTO, PROVINCIA, DISTRITO, DIRECCION, " +
                    "RUTA_ARCHI,RUTA_ARCHI_C, ruta_enviad, ruta_enviad_debit, ruta_enviad_CREDIT, RUTA_ENVIAD_RETENCION, " +
                    "RUTA_ENVIAD_ANU, RUTA_ENVIAD_ANTICIPO,RUTA_ENVIAD_PERCEPCION, RUTA_ENVIAD_GUIA, " +
                    "RUTA_ENVIAD_RESPALDO, ruta_enviad_PDF, ruta_enviad_ticket, RUTA_RESPTA, " +
                    "COALESCE([TIMMER_ENVIO_CORREO],'') AS TIMMER_ENVIO_CORREO, " +
                    "COALESCE([CUENTA_ENVIO_CORREO],'') AS CUENTA_ENVIO_CORREO,  " +
                    "COALESCE([USUARIO_ENVIO_CORREO],'') AS USUARIO_ENVIO_CORREO,  " +
                    "COALESCE([CLAVE_ENVIO_CORREO],'') AS CLAVE_ENVIO_CORREO,  " +
                    "COALESCE([SERVIDOR_ENVIO_CORREO],'') AS SERVIDOR_ENVIO_CORREO,  " +
                    "COALESCE([PUERTO_ENVIO_CORREO],'') AS PUERTO_ENVIO_CORREO,  " +
                    "COALESCE([CORREOS_ENVIAR],'') AS CORREOS_ENVIAR, " +
                    "CORREOS_INTERNOS, URL, IMAGEN , CORREO_CONTACTO, TELEFONO_CONTACTO , FACTURA_RPT,  " +
                    "BOLETA_RPT, NOTACREDITO_RTP, NOTADEBITO_RPT, RETENCION_RPT, PERCEPCION_RPT,  " +
                    "FACTURA_ANU_RPT, BOLETA_ANU_RPT, NOTACREDITO_ANU_RTP, NOTADEBITO_ANU_RPT,  " +
                    "RETENCION_ANU_RPT, PERCEPCION_ANU_RPT " + 
                    "from [dbo].[EMPRESA] WITH(nolock) " +
                    "where codigo ={0}", codigo);

                using (SqlCommand command = new SqlCommand(cmdText, connection))
                {
                    command.CommandType = CommandType.Text;
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        obj = new Empresa
                        {
                            Id = reader["codigo"].ToString(),
                            Ruc = reader["ruc"].ToString(),
                            Nombre = reader["Nombre"].ToString(),

                            Usuario = reader["USUA_FACT"].ToString(),
                            ClaveSol = reader["CLAVE_FACT"].ToString(),

                            DataBase = reader["BASEDATOS"].ToString(),

                            Documento = reader["Documento"].ToString(),
                            GenerarPdf = reader["GenerarPdf"].ToString(),

                            Pais = reader["PAIS"].ToString(),
                            Departamento = reader["DEPARTAMENTO"].ToString(),
                            Provincia = reader["PROVINCIA"].ToString(),
                            Distrito = reader["DISTRITO"].ToString(),
                            Direccion = reader["DIRECCION"].ToString(),

                            CarpetaEnvioXml = reader["RUTA_ARCHI"].ToString(),
                            CarpetaRecepcionXml = reader["RUTA_ARCHI_C"].ToString(),

                            CarpetaCDR = reader["RUTA_ARCHI_C"].ToString(),

                            CarpetaXml = reader["RUTA_ARCHI"].ToString(),
                            CarpetaRecepcion = reader["ruta_enviad"].ToString(),

                            CarpetaRecepcionVoided = reader["RUTA_ENVIAD_ANU"].ToString(),

                            CarpetaRecepcionInvoice = reader["ruta_enviad"].ToString(),
                            CarpetaRecepcionCredit = reader["ruta_enviad_credit"].ToString(),
                            CarpetaRecepcionDebit = reader["ruta_enviad_debit"].ToString(),

                            CarpetaRecepcionAnticipo = reader["RUTA_ENVIAD_ANTICIPO"].ToString(),
                            CarpetaRecepcionGuia = reader["RUTA_ENVIAD_GUIA"].ToString(),
                            CarpetaRecepcionPercepcion = reader["RUTA_ENVIAD_PERCEPCION"].ToString(),
                            CarpetaRecepcionRetencion = reader["RUTA_ENVIAD_RETENCION"].ToString(),
                            CarpetaRespaldo = reader["RUTA_ENVIAD_RESPALDO"].ToString(),
                            CarpetaRecepcionPdf = reader["ruta_enviad_PDF"].ToString(),
                            CarpetaRecepcionTicket = reader["ruta_enviad_ticket"].ToString(),
                            CarpetaRespuesta = reader["RUTA_RESPTA"].ToString(),
                            CorreoTimmer = reader["TIMMER_ENVIO_CORREO"].ToString(),
                            CorreoCuenta = reader["CUENTA_ENVIO_CORREO"].ToString(),
                            CorreoUsuario = reader["USUARIO_ENVIO_CORREO"].ToString(),
                            CorreoClave = reader["CLAVE_ENVIO_CORREO"].ToString(),
                            CorreoServidor = reader["SERVIDOR_ENVIO_CORREO"].ToString(),
                            CorreoPuerto = reader["PUERTO_ENVIO_CORREO"].ToString(),
                            CorreoEnviar = reader["CORREOS_ENVIAR"].ToString(),

                            CorreoInterno = reader["CORREOS_INTERNOS"].ToString(),
                            Url = reader["URL"].ToString(),
                            Imagen = reader["IMAGEN"].ToString(),
                            ContactoCorreo = reader["CORREO_CONTACTO"].ToString(),
                            ContactoTelefono = reader["TELEFONO_CONTACTO"].ToString(),


                            FacturaReporte = reader["FACTURA_RPT"].ToString(),
                            RetencionReporte = reader["RETENCION_RPT"].ToString(),
                            PercepcionReporte = reader["PERCEPCION_RPT"].ToString(),
                            BoletaReporte = reader["BOLETA_RPT"].ToString(),
                            NotaCreditoReporte = reader["NOTACREDITO_RTP"].ToString(),
                            NotaDebitoReporte = reader["NOTADEBITO_RPT"].ToString(),
                            FacturaAnuladoReporte = reader["FACTURA_ANU_RPT"].ToString(),
                            BoletaAnuladoReporte = reader["BOLETA_ANU_RPT"].ToString(),
                            NotaCreditoAnuladoReporte = reader["NOTACREDITO_ANU_RTP"].ToString(),
                            NotaDebitoAnuladoReporte = reader["NOTADEBITO_ANU_RPT"].ToString(),
                            RetencionAnuladoReporte = reader["RETENCION_ANU_RPT"].ToString(),
                            PercepcionAnuladoReporte = reader["PERCEPCION_ANU_RPT"].ToString()
                        };
                        
                    }
                }
            }
            return obj;
        }

        public Empresa ConsultaEmpresaSimpleByRuc(string ruc)
        {
            Empresa obj = new Empresa();

            using (SqlConnection connection = new SqlConnection(Conexion.ObtenerConexionSunat()))
            {
                connection.Open();
                string cmdText = string.Format("SELECT codigo, ruc, Nombre " +
                    "from [dbo].[EMPRESA] WITH(nolock) " +
                    "where RUC ='{0}'", ruc);

                using (SqlCommand command = new SqlCommand(cmdText, connection))
                {
                    command.CommandType = CommandType.Text;
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        obj = new Empresa
                        {
                            Id = reader["codigo"].ToString(),
                            Ruc = reader["ruc"].ToString(),
                            Nombre = reader["Nombre"].ToString(),

                        };

                    }
                }
            }
            return obj;
        }
        public  Empresa ConsultaEmpresaByRuc(string ruc)
        {
            Empresa obj = new Empresa();

            using (SqlConnection connection = new SqlConnection(Conexion.ObtenerConexionSunat()))
            {
                connection.Open();
                string cmdText = string.Format("SELECT codigo, BASEDATOS, ruc, Nombre,  " +
                    "USUA_FACT, CLAVE_FACT, " +
                    "USUA_NCRE, CLAVE_NCRE," +
                    "USUA_NDEB, CLAVE_NDEB," +
                    "USUA_RETE, CLAVE_RETE," +
                    "USUA_PERC, CLAVE_PERC, " +
                    "USUA_GUIA, CLAVE_GUIA," +
                    "USUA_CONSULTA, CLAVE_CONSULTA," +
                    "Documento, GenerarPdf, " +
                    "UBIGEO, PAIS, DEPARTAMENTO, PROVINCIA, DISTRITO, DIRECCION, " +
                    "RUTA_ARCHI,RUTA_ARCHI_C, ruta_enviad, ruta_enviad_debit, ruta_enviad_CREDIT, RUTA_ENVIAD_RETENCION, " +
                    "RUTA_ENVIAD_ANU, RUTA_ENVIAD_ANTICIPO,RUTA_ENVIAD_PERCEPCION, RUTA_ENVIAD_GUIA, " +
                    "RUTA_ENVIAD_RESPALDO, ruta_enviad_PDF, RUTA_RESPTA, " +
                    "COALESCE([TIMMER_ENVIO_CORREO],'') AS TIMMER_ENVIO_CORREO, " +
                    "COALESCE([CUENTA_ENVIO_CORREO],'') AS CUENTA_ENVIO_CORREO,  " +
                    "COALESCE([USUARIO_ENVIO_CORREO],'') AS USUARIO_ENVIO_CORREO,  " +
                    "COALESCE([CLAVE_ENVIO_CORREO],'') AS CLAVE_ENVIO_CORREO,  " +
                    "COALESCE([SERVIDOR_ENVIO_CORREO],'') AS SERVIDOR_ENVIO_CORREO,  " +
                    "COALESCE([PUERTO_ENVIO_CORREO],'') AS PUERTO_ENVIO_CORREO,  " +
                    "COALESCE([CORREOS_ENVIAR],'') AS CORREOS_ENVIAR, " +
                    "CORREOS_INTERNOS, URL, IMAGEN , CORREO_CONTACTO, TELEFONO_CONTACTO , FACTURA_RPT,  " +
                    "BOLETA_RPT, NOTACREDITO_RTP, NOTADEBITO_RPT, RETENCION_RPT, PERCEPCION_RPT,  " +
                    "FACTURA_ANU_RPT, BOLETA_ANU_RPT, NOTACREDITO_ANU_RTP, NOTADEBITO_ANU_RPT,  " +
                    "RETENCION_ANU_RPT, PERCEPCION_ANU_RPT " +
                    "from [dbo].[EMPRESA] WITH(nolock) " +
                    "where ESTATUS_FELE in('2') and RUC ='{0}'", ruc);

                using (SqlCommand command = new SqlCommand(cmdText, connection))
                {
                    command.CommandType = CommandType.Text;
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        obj = new Empresa
                        {
                            Id = reader["codigo"].ToString(),
                            Ruc = reader["ruc"].ToString(),
                            Nombre = reader["Nombre"].ToString(),

                            Usuario = reader["USUA_FACT"].ToString(),
                            ClaveSol = reader["CLAVE_FACT"].ToString(),

                            DataBase = reader["BASEDATOS"].ToString(),

                            Documento = reader["Documento"].ToString(),
                            GenerarPdf = reader["GenerarPdf"].ToString(),

                            Ubigeo = reader["UBIGEO"].ToString(),
                            Pais = reader["PAIS"].ToString(),
                            Departamento = reader["DEPARTAMENTO"].ToString(),
                            Provincia = reader["PROVINCIA"].ToString(),
                            Distrito = reader["DISTRITO"].ToString(),
                            Direccion = reader["DIRECCION"].ToString(),

                            CarpetaEnvioXml = reader["RUTA_ARCHI"].ToString(),
                            CarpetaRecepcionXml = reader["RUTA_ARCHI_C"].ToString(),

                            CarpetaXml = reader["RUTA_ARCHI"].ToString(),
                            CarpetaRecepcion = reader["ruta_enviad"].ToString(),

                            CarpetaRecepcionVoided = reader["RUTA_ENVIAD_ANU"].ToString(),

                            CarpetaRecepcionInvoice = reader["ruta_enviad"].ToString(),
                            CarpetaRecepcionCredit = reader["ruta_enviad_credit"].ToString(),
                            CarpetaRecepcionDebit = reader["ruta_enviad_debit"].ToString(),

                            CarpetaRecepcionAnticipo = reader["RUTA_ENVIAD_ANTICIPO"].ToString(),
                            CarpetaRecepcionGuia = reader["RUTA_ENVIAD_GUIA"].ToString(),
                            CarpetaRecepcionPercepcion = reader["RUTA_ENVIAD_PERCEPCION"].ToString(),
                            CarpetaRecepcionRetencion = reader["RUTA_ENVIAD_RETENCION"].ToString(),
                            CarpetaRespaldo = reader["RUTA_ENVIAD_RESPALDO"].ToString(),
                            CarpetaRecepcionPdf = reader["ruta_enviad_PDF"].ToString(),
                            CarpetaRespuesta = reader["RUTA_RESPTA"].ToString(),
                            CorreoTimmer = reader["TIMMER_ENVIO_CORREO"].ToString(),
                            CorreoCuenta = reader["CUENTA_ENVIO_CORREO"].ToString(),
                            CorreoUsuario = reader["USUARIO_ENVIO_CORREO"].ToString(),
                            CorreoClave = reader["CLAVE_ENVIO_CORREO"].ToString(),
                            CorreoServidor = reader["SERVIDOR_ENVIO_CORREO"].ToString(),
                            CorreoPuerto = reader["PUERTO_ENVIO_CORREO"].ToString(),
                            CorreoEnviar = reader["CORREOS_ENVIAR"].ToString(),

                            CorreoInterno = reader["CORREOS_INTERNOS"].ToString(),
                            Url = reader["URL"].ToString(),
                            Imagen = reader["IMAGEN"].ToString(),
                            ContactoCorreo = reader["CORREO_CONTACTO"].ToString(),
                            ContactoTelefono = reader["TELEFONO_CONTACTO"].ToString(),


                            FacturaReporte = reader["FACTURA_RPT"].ToString(),
                            RetencionReporte = reader["RETENCION_RPT"].ToString(),
                            PercepcionReporte = reader["PERCEPCION_RPT"].ToString(),
                            BoletaReporte = reader["BOLETA_RPT"].ToString(),
                            NotaCreditoReporte = reader["NOTACREDITO_RTP"].ToString(),
                            NotaDebitoReporte = reader["NOTADEBITO_RPT"].ToString(),
                            FacturaAnuladoReporte = reader["FACTURA_ANU_RPT"].ToString(),
                            BoletaAnuladoReporte = reader["BOLETA_ANU_RPT"].ToString(),
                            NotaCreditoAnuladoReporte = reader["NOTACREDITO_ANU_RTP"].ToString(),
                            NotaDebitoAnuladoReporte = reader["NOTADEBITO_ANU_RPT"].ToString(),
                            RetencionAnuladoReporte = reader["RETENCION_ANU_RPT"].ToString(),
                            PercepcionAnuladoReporte = reader["PERCEPCION_ANU_RPT"].ToString()
                        };

                    }
                }
            }
            return obj;
        }

        public Empresa ConsultaEmpresaPruebaByRuc(string ruc)
        {
            Empresa obj = new Empresa();

            using (SqlConnection connection = new SqlConnection(Conexion.ObtenerConexionSunat()))
            {
                connection.Open();
                string cmdText = string.Format("SELECT codigo, BASEDATOS, ruc, Nombre,  " +
                    "USUA_FACT, CLAVE_FACT, " +
                    "USUA_NCRE, CLAVE_NCRE," +
                    "USUA_NDEB, CLAVE_NDEB," +
                    "USUA_RETE, CLAVE_RETE," +
                    "USUA_PERC, CLAVE_PERC, " +
                    "USUA_GUIA, CLAVE_GUIA," +
                    "USUA_CONSULTA, CLAVE_CONSULTA," +
                    "Documento, GenerarPdf, " +
                    "UBIGEO, PAIS, DEPARTAMENTO, PROVINCIA, DISTRITO, DIRECCION, " +
                    "RUTA_ARCHI,RUTA_ARCHI_C, ruta_enviad, ruta_enviad_debit, ruta_enviad_CREDIT, RUTA_ENVIAD_RETENCION, " +
                    "RUTA_ENVIAD_ANU, RUTA_ENVIAD_ANTICIPO,RUTA_ENVIAD_PERCEPCION, RUTA_ENVIAD_GUIA, " +
                    "RUTA_ENVIAD_RESPALDO, ruta_enviad_PDF, RUTA_RESPTA, " +
                    "COALESCE([TIMMER_ENVIO_CORREO],'') AS TIMMER_ENVIO_CORREO, " +
                    "COALESCE([CUENTA_ENVIO_CORREO],'') AS CUENTA_ENVIO_CORREO,  " +
                    "COALESCE([USUARIO_ENVIO_CORREO],'') AS USUARIO_ENVIO_CORREO,  " +
                    "COALESCE([CLAVE_ENVIO_CORREO],'') AS CLAVE_ENVIO_CORREO,  " +
                    "COALESCE([SERVIDOR_ENVIO_CORREO],'') AS SERVIDOR_ENVIO_CORREO,  " +
                    "COALESCE([PUERTO_ENVIO_CORREO],'') AS PUERTO_ENVIO_CORREO,  " +
                    "COALESCE([CORREOS_ENVIAR],'') AS CORREOS_ENVIAR, " +
                    "CORREOS_INTERNOS, URL, IMAGEN , CORREO_CONTACTO, TELEFONO_CONTACTO , FACTURA_RPT,  " +
                    "BOLETA_RPT, NOTACREDITO_RTP, NOTADEBITO_RPT, RETENCION_RPT, PERCEPCION_RPT,  " +
                    "FACTURA_ANU_RPT, BOLETA_ANU_RPT, NOTACREDITO_ANU_RTP, NOTADEBITO_ANU_RPT,  " +
                    "RETENCION_ANU_RPT, PERCEPCION_ANU_RPT " +
                    "from [dbo].[EMPRESA] WITH(nolock) " +
                    "where ESTATUS_FELE in('0','1') and RUC ='{0}'", ruc);

                using (SqlCommand command = new SqlCommand(cmdText, connection))
                {
                    command.CommandType = CommandType.Text;
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        obj = new Empresa
                        {
                            Id = reader["codigo"].ToString(),
                            Ruc = reader["ruc"].ToString(),
                            Nombre = reader["Nombre"].ToString(),

                            Usuario = reader["USUA_FACT"].ToString(),
                            ClaveSol = reader["CLAVE_FACT"].ToString(),

                            DataBase = reader["BASEDATOS"].ToString(),

                            Documento = reader["Documento"].ToString(),
                            GenerarPdf = reader["GenerarPdf"].ToString(),

                            Ubigeo = reader["UBIGEO"].ToString(),
                            Pais = reader["PAIS"].ToString(),
                            Departamento = reader["DEPARTAMENTO"].ToString(),
                            Provincia = reader["PROVINCIA"].ToString(),
                            Distrito = reader["DISTRITO"].ToString(),
                            Direccion = reader["DIRECCION"].ToString(),

                            CarpetaEnvioXml = reader["RUTA_ARCHI"].ToString(),
                            CarpetaRecepcionXml = reader["RUTA_ARCHI_C"].ToString(),

                            CarpetaXml = reader["RUTA_ARCHI"].ToString(),
                            CarpetaRecepcion = reader["ruta_enviad"].ToString(),

                            CarpetaRecepcionVoided = reader["RUTA_ENVIAD_ANU"].ToString(),

                            CarpetaRecepcionInvoice = reader["ruta_enviad"].ToString(),
                            CarpetaRecepcionCredit = reader["ruta_enviad_credit"].ToString(),
                            CarpetaRecepcionDebit = reader["ruta_enviad_debit"].ToString(),

                            CarpetaRecepcionAnticipo = reader["RUTA_ENVIAD_ANTICIPO"].ToString(),
                            CarpetaRecepcionGuia = reader["RUTA_ENVIAD_GUIA"].ToString(),
                            CarpetaRecepcionPercepcion = reader["RUTA_ENVIAD_PERCEPCION"].ToString(),
                            CarpetaRecepcionRetencion = reader["RUTA_ENVIAD_RETENCION"].ToString(),
                            CarpetaRespaldo = reader["RUTA_ENVIAD_RESPALDO"].ToString(),
                            CarpetaRecepcionPdf = reader["ruta_enviad_PDF"].ToString(),
                            CarpetaRespuesta = reader["RUTA_RESPTA"].ToString(),
                            CorreoTimmer = reader["TIMMER_ENVIO_CORREO"].ToString(),
                            CorreoCuenta = reader["CUENTA_ENVIO_CORREO"].ToString(),
                            CorreoUsuario = reader["USUARIO_ENVIO_CORREO"].ToString(),
                            CorreoClave = reader["CLAVE_ENVIO_CORREO"].ToString(),
                            CorreoServidor = reader["SERVIDOR_ENVIO_CORREO"].ToString(),
                            CorreoPuerto = reader["PUERTO_ENVIO_CORREO"].ToString(),
                            CorreoEnviar = reader["CORREOS_ENVIAR"].ToString(),

                            CorreoInterno = reader["CORREOS_INTERNOS"].ToString(),
                            Url = reader["URL"].ToString(),
                            Imagen = reader["IMAGEN"].ToString(),
                            ContactoCorreo = reader["CORREO_CONTACTO"].ToString(),
                            ContactoTelefono = reader["TELEFONO_CONTACTO"].ToString(),


                            FacturaReporte = reader["FACTURA_RPT"].ToString(),
                            RetencionReporte = reader["RETENCION_RPT"].ToString(),
                            PercepcionReporte = reader["PERCEPCION_RPT"].ToString(),
                            BoletaReporte = reader["BOLETA_RPT"].ToString(),
                            NotaCreditoReporte = reader["NOTACREDITO_RTP"].ToString(),
                            NotaDebitoReporte = reader["NOTADEBITO_RPT"].ToString(),
                            FacturaAnuladoReporte = reader["FACTURA_ANU_RPT"].ToString(),
                            BoletaAnuladoReporte = reader["BOLETA_ANU_RPT"].ToString(),
                            NotaCreditoAnuladoReporte = reader["NOTACREDITO_ANU_RTP"].ToString(),
                            NotaDebitoAnuladoReporte = reader["NOTADEBITO_ANU_RPT"].ToString(),
                            RetencionAnuladoReporte = reader["RETENCION_ANU_RPT"].ToString(),
                            PercepcionAnuladoReporte = reader["PERCEPCION_ANU_RPT"].ToString()
                        };

                    }
                }
            }
            return obj;
        }

        public  List<Empresa> ListaEmpresas(string Ambiente, string Ruc)
        {
            List<Empresa> list = new List<Empresa>();
            using (SqlConnection connection = new SqlConnection(Conexion.ObtenerConexionSunat()))
            {
                connection.Open();
                string cmdText = string.Format( "SELECT codigo, BASEDATOS, RUC, Nombre,  " +
                    "USUA_FACT, CLAVE_FACT, " +
                    "USUA_NCRE, CLAVE_NCRE," +
                    "USUA_NDEB, CLAVE_NDEB," +
                    "USUA_RETE, CLAVE_RETE," +
                    "USUA_PERC, CLAVE_PERC, " +
                    "USUA_GUIA, CLAVE_GUIA," +
                    "USUA_CONSULTA, CLAVE_CONSULTA," +
                    "Documento, GenerarPdf, " +
                    "PAIS, DEPARTAMENTO, PROVINCIA, DISTRITO, DIRECCION, UBIGEO, " +
                    "RUTA_ARCHI, RUTA_ARCHI_C, ruta_enviad, ruta_enviad_debit, ruta_enviad_CREDIT, RUTA_ENVIAD_RETENCION, " +
                    "RUTA_ENVIAD_ANU, RUTA_ENVIAD_ANTICIPO,RUTA_ENVIAD_PERCEPCION, RUTA_ENVIAD_GUIA, " +
                    "RUTA_ENVIAD_RESPALDO, ruta_enviad_PDF, RUTA_RESPTA, " +
                    "COALESCE([TIMMER_ENVIO_CORREO],'') AS TIMMER_ENVIO_CORREO, " +
                    "COALESCE([CUENTA_ENVIO_CORREO],'') AS CUENTA_ENVIO_CORREO,  " +
                    "COALESCE([USUARIO_ENVIO_CORREO],'') AS USUARIO_ENVIO_CORREO,  " +
                    "COALESCE([CLAVE_ENVIO_CORREO],'') AS CLAVE_ENVIO_CORREO,  " +
                    "COALESCE([SERVIDOR_ENVIO_CORREO],'') AS SERVIDOR_ENVIO_CORREO,  " +
                    "COALESCE([PUERTO_ENVIO_CORREO],'') AS PUERTO_ENVIO_CORREO,  " +
                    "COALESCE([CORREOS_ENVIAR],'') AS CORREOS_ENVIAR, " +
                    "CORREOS_INTERNOS, URL, IMAGEN , CORREO_CONTACTO, TELEFONO_CONTACTO , FACTURA_RPT,  " +
                    "BOLETA_RPT, NOTACREDITO_RTP, NOTADEBITO_RPT, RETENCION_RPT, PERCEPCION_RPT,  " +
                    "FACTURA_ANU_RPT, BOLETA_ANU_RPT, NOTACREDITO_ANU_RTP, NOTADEBITO_ANU_RPT,  " +
                    "RETENCION_ANU_RPT, PERCEPCION_ANU_RPT " +
                    "from [dbo].[EMPRESA] WITH(nolock) " +
                    "where ESTATUS_FELE={0} and RUC='{1}';", Ambiente, Ruc);

                using (SqlCommand command = new SqlCommand(cmdText, connection))
                {
                    command.CommandType = CommandType.Text;
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Empresa item = new Empresa
                        {
                            Id = reader["codigo"].ToString(),
                            Ruc = reader["ruc"].ToString(),
                            Nombre = reader["Nombre"].ToString(),
                            Usuario = reader["USUA_FACT"].ToString(),
                            ClaveSol = reader["CLAVE_FACT"].ToString(),

                            DataBase = reader["BASEDATOS"].ToString(),
                            Ubigeo = reader["UBIGEO"].ToString(),

                            Documento = reader["Documento"].ToString(),
                            GenerarPdf = reader["GenerarPdf"].ToString(),

                            Pais = reader["PAIS"].ToString(),
                            Departamento = reader["DEPARTAMENTO"].ToString(),
                            Provincia = reader["PROVINCIA"].ToString(),
                            Distrito = reader["DISTRITO"].ToString(),
                            Direccion = reader["DIRECCION"].ToString(),

                            CarpetaEnvioXml = reader["RUTA_ARCHI"].ToString(),
                            CarpetaRecepcionXml = reader["RUTA_ARCHI_C"].ToString(),

                            CarpetaXml = reader["RUTA_ARCHI"].ToString(),
                            CarpetaRecepcion = reader["ruta_enviad"].ToString(),

                            CarpetaRecepcionVoided = reader["RUTA_ENVIAD_ANU"].ToString(),

                            CarpetaRecepcionInvoice = reader["ruta_enviad"].ToString(),
                            CarpetaRecepcionCredit = reader["ruta_enviad_credit"].ToString(),
                            CarpetaRecepcionDebit = reader["ruta_enviad_debit"].ToString(),

                            CarpetaRecepcionAnticipo = reader["RUTA_ENVIAD_ANTICIPO"].ToString(),
                            CarpetaRecepcionGuia = reader["RUTA_ENVIAD_GUIA"].ToString(),
                            CarpetaRecepcionPercepcion = reader["RUTA_ENVIAD_PERCEPCION"].ToString(),
                            CarpetaRecepcionRetencion = reader["RUTA_ENVIAD_RETENCION"].ToString(),
                            CarpetaRespaldo = reader["RUTA_ENVIAD_RESPALDO"].ToString(),
                            CarpetaRecepcionPdf = reader["ruta_enviad_PDF"].ToString(),
                            CarpetaRespuesta = reader["RUTA_RESPTA"].ToString(),
                            CorreoTimmer = reader["TIMMER_ENVIO_CORREO"].ToString(),
                            CorreoCuenta = reader["CUENTA_ENVIO_CORREO"].ToString(),
                            CorreoUsuario = reader["USUARIO_ENVIO_CORREO"].ToString(),
                            CorreoClave = reader["CLAVE_ENVIO_CORREO"].ToString(),
                            CorreoServidor = reader["SERVIDOR_ENVIO_CORREO"].ToString(),
                            CorreoPuerto = reader["PUERTO_ENVIO_CORREO"].ToString(),
                            CorreoEnviar = reader["CORREOS_ENVIAR"].ToString(),

                            CorreoInterno = reader["CORREOS_INTERNOS"].ToString(),
                            Url = reader["URL"].ToString(),
                            Imagen = reader["IMAGEN"].ToString(),
                            ContactoCorreo = reader["CORREO_CONTACTO"].ToString(),
                            ContactoTelefono = reader["TELEFONO_CONTACTO"].ToString(),


                            FacturaReporte = reader["FACTURA_RPT"].ToString(),
                            RetencionReporte = reader["RETENCION_RPT"].ToString(),
                            PercepcionReporte = reader["PERCEPCION_RPT"].ToString(),
                            BoletaReporte = reader["BOLETA_RPT"].ToString(),
                            NotaCreditoReporte = reader["NOTACREDITO_RTP"].ToString(),
                            NotaDebitoReporte = reader["NOTADEBITO_RPT"].ToString(),
                            FacturaAnuladoReporte = reader["FACTURA_ANU_RPT"].ToString(),
                            BoletaAnuladoReporte = reader["BOLETA_ANU_RPT"].ToString(),
                            NotaCreditoAnuladoReporte = reader["NOTACREDITO_ANU_RTP"].ToString(),
                            NotaDebitoAnuladoReporte = reader["NOTADEBITO_ANU_RPT"].ToString(),
                            RetencionAnuladoReporte = reader["RETENCION_ANU_RPT"].ToString(),
                            PercepcionAnuladoReporte = reader["PERCEPCION_ANU_RPT"].ToString()
                        };
                        list.Add(item);
                    }
                }
            }
            return list;
        }

        public  List<Empresa> ListaEmpresas()
        {
            List<Empresa> list = new List<Empresa>();
            using (SqlConnection connection = new SqlConnection(Conexion.ObtenerConexionSunat()))
            {
                connection.Open();
                string cmdText = "SELECT codigo, BASEDATOS, ruc, Nombre,  " +
                    "USUA_FACT, CLAVE_FACT, " +
                    "USUA_NCRE, CLAVE_NCRE," +
                    "USUA_NDEB, CLAVE_NDEB," +
                    "USUA_RETE, CLAVE_RETE," +
                    "USUA_PERC, CLAVE_PERC, " +
                    "USUA_GUIA, CLAVE_GUIA," +
                    "USUA_CONSULTA, CLAVE_CONSULTA," +
                    "Documento, GenerarPdf, " +
                    "PAIS, DEPARTAMENTO, PROVINCIA, DISTRITO, DIRECCION, " +
                    "RUTA_ARCHI, RUTA_ARCHI_C, ruta_enviad, ruta_enviad_debit, ruta_enviad_CREDIT, RUTA_ENVIAD_RETENCION, " +
                    "RUTA_ENVIAD_ANU, RUTA_ENVIAD_ANTICIPO,RUTA_ENVIAD_PERCEPCION, RUTA_ENVIAD_GUIA, " +
                    "RUTA_ENVIAD_RESPALDO, ruta_enviad_PDF, ruta_enviad_ticket, RUTA_RESPTA, " +
                    "COALESCE([TIMMER_ENVIO_CORREO],'') AS TIMMER_ENVIO_CORREO, " +
                    "COALESCE([CUENTA_ENVIO_CORREO],'') AS CUENTA_ENVIO_CORREO,  " +
                    "COALESCE([USUARIO_ENVIO_CORREO],'') AS USUARIO_ENVIO_CORREO,  " +
                    "COALESCE([CLAVE_ENVIO_CORREO],'') AS CLAVE_ENVIO_CORREO,  " +
                    "COALESCE([SERVIDOR_ENVIO_CORREO],'') AS SERVIDOR_ENVIO_CORREO,  " +
                    "COALESCE([PUERTO_ENVIO_CORREO],'') AS PUERTO_ENVIO_CORREO,  " +
                    "COALESCE([CORREOS_ENVIAR],'') AS CORREOS_ENVIAR, " +
                    "COALESCE([CORREO_SSL],'0') AS CORREO_SSL, " +
                    "COALESCE([CORREO_ACUSE],'') AS CORREO_ACUSE, " +
                    "CORREOS_INTERNOS, URL, IMAGEN , CORREO_CONTACTO, TELEFONO_CONTACTO ,TICKET_NC_RPT, TICKET_RPT, FACTURA_RPT,  " +
                    "BOLETA_RPT, NOTACREDITO_RTP, NOTADEBITO_RPT, RETENCION_RPT, PERCEPCION_RPT,  " +
                    "FACTURA_ANU_RPT, BOLETA_ANU_RPT, NOTACREDITO_ANU_RTP, NOTADEBITO_ANU_RPT,  " +
                    "RETENCION_ANU_RPT, PERCEPCION_ANU_RPT, " +
                    "COALESCE(FACTURA_PDF,'') AS FACTURA_PDF,"+
                    "SERVIDOR_FTP, CUENTA_FTP, CLAVE_FTP, PUERTO_FTP, " +
                    "PATH_ARCHIVOS, PATH_DATOS_PDF, PATH_DATOS_XML ,PATH_DATOS_ORIGINAL, CLAVE_EJE,  " +
                    "URL_CONSULTA, PROVEEDOR, HABILITADO, COALESCE(XML_BACKUS,'') as XML_BACKUS " +
                    "from [dbo].[EMPRESA] WITH(nolock) "+
                    "where habilitado='1' " +
                    "ORDER BY PROVEEDOR DESC";

                using (SqlCommand command = new SqlCommand(cmdText, connection))
                {
                    command.CommandType = CommandType.Text;
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Empresa item = new Empresa
                        {
                            Id = reader["codigo"].ToString(),
                            Ruc = reader["ruc"].ToString(),
                            Nombre = reader["Nombre"].ToString(),
                            Usuario = reader["USUA_FACT"].ToString(),
                            ClaveSol = reader["CLAVE_FACT"].ToString(),
                            ClaveEje = reader["CLAVE_EJE"].ToString(),
                            Permiso = (reader["CLAVE_EJE"].ToString()==Encrypt.EncryptString(GetMACAddress(),reader["ruc"].ToString())),

                            DataBase = reader["BASEDATOS"].ToString(),

                            CorreoSSL = ((reader["CORREO_SSL"].ToString()=="0")?false:true),

                            Documento = reader["Documento"].ToString(),
                            GenerarPdf = reader["GenerarPdf"].ToString(),

                            Pais = reader["PAIS"].ToString(),
                            Departamento = reader["DEPARTAMENTO"].ToString(),
                            Provincia = reader["PROVINCIA"].ToString(),
                            Distrito = reader["DISTRITO"].ToString(),
                            Direccion = reader["DIRECCION"].ToString(),

                            CarpetaXml = reader["RUTA_ARCHI"].ToString(),
                            CarpetaRecepcion = reader["ruta_enviad"].ToString(),
                            CarpetaRecepcionXml = reader["RUTA_ARCHI_C"].ToString(),

                            CarpetaRecepcionVoided = reader["RUTA_ENVIAD_ANU"].ToString(),

                            CarpetaRecepcionInvoice = reader["ruta_enviad"].ToString(),
                            CarpetaRecepcionCredit = reader["ruta_enviad_credit"].ToString(),
                            CarpetaRecepcionDebit = reader["ruta_enviad_debit"].ToString(),

                            CarpetaRecepcionAnticipo = reader["RUTA_ENVIAD_ANTICIPO"].ToString(),
                            CarpetaRecepcionGuia = reader["RUTA_ENVIAD_GUIA"].ToString(),
                            CarpetaRecepcionPercepcion = reader["RUTA_ENVIAD_PERCEPCION"].ToString(),
                            CarpetaRecepcionRetencion = reader["RUTA_ENVIAD_RETENCION"].ToString(),
                            CarpetaRespaldo = reader["RUTA_ENVIAD_RESPALDO"].ToString(),
                            CarpetaRecepcionPdf = reader["ruta_enviad_PDF"].ToString(),
                            CarpetaRecepcionTicket = reader["ruta_enviad_ticket"].ToString(),
                            CarpetaRespuesta = reader["RUTA_RESPTA"].ToString(),
                            CorreoTimmer = reader["TIMMER_ENVIO_CORREO"].ToString(),
                            CorreoCuenta = reader["CUENTA_ENVIO_CORREO"].ToString(),
                            CorreoUsuario = reader["USUARIO_ENVIO_CORREO"].ToString(),
                            CorreoClave = reader["CLAVE_ENVIO_CORREO"].ToString(),
                            CorreoServidor = reader["SERVIDOR_ENVIO_CORREO"].ToString(),
                            CorreoPuerto = reader["PUERTO_ENVIO_CORREO"].ToString(),
                            CorreoEnviar = reader["CORREOS_ENVIAR"].ToString(),

                            CorreoAcuse = reader["CORREO_ACUSE"].ToString(),

                            CorreoInterno = reader["CORREOS_INTERNOS"].ToString(),
                            Url = reader["URL"].ToString(),
                            Imagen = reader["IMAGEN"].ToString(),
                            ContactoCorreo = reader["CORREO_CONTACTO"].ToString(),
                            ContactoTelefono = reader["TELEFONO_CONTACTO"].ToString(),

                            TicketNCReporte = reader["TICKET_NC_RPT"].ToString(),
                            TicketReporte = reader["TICKET_RPT"].ToString(),
                            FacturaReporte = reader["FACTURA_RPT"].ToString(),
                            RetencionReporte = reader["RETENCION_RPT"].ToString(),
                            PercepcionReporte = reader["PERCEPCION_RPT"].ToString(),
                            BoletaReporte = reader["BOLETA_RPT"].ToString(),
                            NotaCreditoReporte = reader["NOTACREDITO_RTP"].ToString(),
                            NotaDebitoReporte = reader["NOTADEBITO_RPT"].ToString(),
                            FacturaAnuladoReporte = reader["FACTURA_ANU_RPT"].ToString(),
                            BoletaAnuladoReporte = reader["BOLETA_ANU_RPT"].ToString(),
                            NotaCreditoAnuladoReporte = reader["NOTACREDITO_ANU_RTP"].ToString(),
                            NotaDebitoAnuladoReporte = reader["NOTADEBITO_ANU_RPT"].ToString(),
                            RetencionAnuladoReporte = reader["RETENCION_ANU_RPT"].ToString(),
                            PercepcionAnuladoReporte = reader["PERCEPCION_ANU_RPT"].ToString(),

                            FacturaPdf = reader["FACTURA_PDF"].ToString(),

                            CarpetaWebArchivos = reader["PATH_ARCHIVOS"].ToString(),
                            //CarpetaWebDatos = reader["PATH_DATOS"].ToString(),
                            CarpetaWebPdf = reader["PATH_DATOS_PDF"].ToString(),
                            CarpetaWebXml = reader["PATH_DATOS_XML"].ToString(),
                            CarpetaWebOriginal = reader["PATH_DATOS_ORIGINAL"].ToString(),
                            
                            ServidorFtp = reader["SERVIDOR_FTP"].ToString(),
                            CuentaFtp = reader["CUENTA_FTP"].ToString(),
                            ClaveFtp = reader["CLAVE_FTP"].ToString(),
                            PuertoFtp = reader["PUERTO_FTP"].ToString(),
                            Url_Consulta= reader["URL_CONSULTA"].ToString(),
                            HABILITADO = reader["HABILITADO"].ToString(),
                            Proveedor = reader["PROVEEDOR"].ToString(),

                            XML_BACKUS = reader["XML_BACKUS"].ToString()
                        };
                        list.Add(item);
                    }
                }
            }
            return list;
        }

        public string GetMACAddress()
        {
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            string sMacAddress = string.Empty;
            foreach (NetworkInterface adapter in nics)
            {
                if (sMacAddress == String.Empty)// only return MAC Address from first card  
                {
                    IPInterfaceProperties properties = adapter.GetIPProperties();
                    sMacAddress = adapter.GetPhysicalAddress().ToString();
                }
            } 
            
            return sMacAddress;
        }
    }
}
