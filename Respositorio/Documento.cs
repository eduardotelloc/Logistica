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
    public class Documento : IDisposable
    {

        #region IDisposable
        // Pointer to an external unmanaged resource. 
        private IntPtr handle;
        // Other managed resource this class uses. 
        private Component component = new Component();
        // Track whether Dispose has been called. 
        private bool disposed = false;

        // The class constructor. 
        public Documento(IntPtr handle)
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
        ~Documento()
        {
            // Do not re-create Dispose clean-up code here. 
            // Calling Dispose(false) is optimal in terms of 
            // readability and maintainability.
            Dispose(false);
        }
        #endregion

        public Documento()
        { }

        public string ConsultaRUC(string ruc)
        {
            string json = string.Empty;


            using (SqlConnection conn = new SqlConnection(Conexion.ObtenerConexionReporte()))
            {
                conn.Open();

                string sql = string.Empty;

                sql = string.Format("select ruc, nombre, estado, ubigeo, direccion, departamento, provincia, distrito " +
                                    "from Padron " +
                                    "where ruc='{0}';", ruc);


                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    SqlDataReader reader = cmd.ExecuteReader();
                    Entity.Padron obj =  new Entity.Padron();

                    while (reader.Read())
                    {
                        obj=new Entity.Padron
                        {
                            ruc = reader["ruc"].ToString(),
                            nombre = reader["nombre"].ToString(),
                            estado = reader["estado"].ToString(),
                            ubigeo = reader["ubigeo"].ToString(),
                            direccion = reader["direccion"].ToString(),
                            departamento = reader["departamento"].ToString(),
                            provincia = reader["provincia"].ToString(),
                            distrito = reader["distrito"].ToString()
                        };
                    }

                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    serializer.MaxJsonLength = 2147483644;
                    json = serializer.Serialize(obj);

                }
            }

            return json;
        }

        public string Resumen(string sEmpresaCodigo, string sAnio, string sMes)
        {
            string json = string.Empty;

           
            using (SqlConnection conn = new SqlConnection(Conexion.ObtenerConexionReporte()))
            {
                conn.Open();

                string sql = string.Empty;
                
                sql = string.Format("select TIPO_DOCU, " + 
	                                "       case TIPO_DOCU  " + 
	                                "       when '01' then 'FACTURA' " + 
	                                "       when '03' then 'BOLETA' " + 
	                                "       when '07' then 'NOTA CREDITO' " + 
	                                "       when '08' then 'NOTA DEBITO'  " + 
	                                "       end as TIPO_DESC,  " + 
	                                "       Count(*) as CANTIDAD, ESTADO_DOCU, " +
                                    "       CASE WHEN MONEDA_DOCU='PEN' THEN  SUM( cast( replace(IMPO_TOTAL,',','') as decimal(12,2)) ) ELSE 0 END as SOLES, " +
                                    "       CASE WHEN MONEDA_DOCU='USD' THEN  SUM( cast( replace(IMPO_TOTAL,',','') as decimal(12,2)) ) ELSE 0 END as DOLARES, " + 
	                                "       case when ESTADO_DOCU='ACT' THEN  Count(*) else COUNT(*)*2 end as TRANSACCIONES " + 
                                    "from vDOCUMENTO_REPORTE_CLIENTE " +
                                    "       where EMPRESA_CODIGO={0} and left(FECHA_DOCU,4)={1} and left(right(FECHA_DOCU,4),2)={2}  " +
                                    "group by TIPO_DOCU, ESTADO_DOCU, MONEDA_DOCU;", sEmpresaCodigo, sAnio, sMes);


                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    SqlDataReader reader = cmd.ExecuteReader();
                    List<Entity.Resumen> lista = null;

                    if (reader.HasRows) lista = new List<Entity.Resumen>();

                    while (reader.Read())
                    {
                        lista.Add(new Entity.Resumen
                        {
                            Tipo_Docu = reader["Tipo_Docu"].ToString(),
                            Tipo_Desc = reader["Tipo_Desc"].ToString(),
                            Cantidad = reader["Cantidad"].ToString(),
                            Estado_Docu = reader["Estado_Docu"].ToString(),
                            Soles = reader["Soles"].ToString(),
                            Dolares = reader["Dolares"].ToString(),
                            Transacciones = reader["Transacciones"].ToString()
                        });
                    }

                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    serializer.MaxJsonLength = 2147483644;
                    json = serializer.Serialize(lista);

                }
            }

            return json;
        }


        public string Load(string sEmpresa, string sTipo, string sSerie, string sCorrelativo,
                           string sFechaInicio, string sFechaFin, string sEstado, string sSunat)
        {
            string json = string.Empty;

            Entity.Empresa empresa = new BusinessGlobal.Consulta().ConsultaEmpresaById(sEmpresa);

            using (SqlConnection conn = new SqlConnection(Conexion.ObtenerConexionReporte()))
            {
                conn.Open();

                string sql = string.Empty;
                string filtro = (sTipo != "") ? string.Format(" and tipocodigo='{0}'", sTipo) : "";

                sql = string.Format("select " +
                "ID, CODIGO, CLIENTE_RUC_DNI, CLIENTE_NOMBRE, TIPO_DOCU, TIPO_DOCU_DES, NUMERO_SERIE_DOCU, NUMERO_DOC_DOCU,  " +
                "DOCUMENTO, FECHA_DOCU, FECHA_DOCU_FORMATO, MONEDA_DOCU, IMPO_TOTAL, ESTADO_DOCU, ESTATUS_ENVIADO,  " +
                "ESTATUS_RECEP, ESTATUS_ANUL, ESTATUS_RECHAZADO, ESTATUS_SUNAT, CODIGO_SUNAT, ESTADO_ENVIO_CORREO_CLIENTE, " +
                "CODIGO_ERROR, DESCRIPCION_ERROR, CODIGO_OBS, DESCRIPCION_OBS, ARCHIVO_PDF_NOMBRE, ARCHIVO_RESU, ESTADO, " +
                "FECHA_ENVIADO, CLIENTE_CORREO, EMPRESA_CODIGO " +
                "from vDOCUMENTO_REPORTE_CLIENTE " +
                "WHERE EMPRESA_CODIGO={0} {1} {2} {3} {4} {5} {6} {7} " +
                    //"ORDER BY FECHA_DOCU desc, TIPO_DOCU, NUMERO_SERIE_DOCU, NUMERO_DOC_DOCU DESC",
                "ORDER BY FECHA_DOCU desc , TIPO_DOCU, NUMERO_SERIE_DOCU, NUMERO_DOC_DOCU DESC",
                sEmpresa,
                (sTipo == "00" || sTipo == "") ? "" : string.Format("AND TIPO_DOCU='{0}'", sTipo),
                (sSerie == "") ? "" : string.Format("AND NUMERO_SERIE_DOCU like'%{0}%'", sSerie),
                (sCorrelativo == "") ? "" : string.Format("AND CAST( NUMERO_DOC_DOCU AS NUMERIC(8))='{0}'", sCorrelativo),
                (sFechaInicio == "") ? "" : string.Format("AND FECHA_DOCU>={0}", sFechaInicio),
                (sFechaFin == "") ? "" : string.Format("AND FECHA_DOCU<={0}", sFechaFin),
                (sEstado == "00" || sEstado == "") ? "" : string.Format("AND ESTADO_DOCU='{0}'", sEstado),
                (sSunat == "00" || sSunat == "") ? "" : (sSunat == "ACEPTADO") ? "AND ESTADO='ACEPTADO'" : "AND coalesce(ESTADO,'')<>'ACEPTADO'"
                );


                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    SqlDataReader reader = cmd.ExecuteReader();
                    List<Entity.DocumentoInterno> lista = null;

                    if (reader.HasRows) lista = new List<Entity.DocumentoInterno>();

                    while (reader.Read())
                    {
                        lista.Add(new Entity.DocumentoInterno
                        {
                            ID = reader["ID"].ToString(),
                            CODIGO = reader["CODIGO"].ToString(),
                            CLIENTE_RUC_DNI = reader["CLIENTE_RUC_DNI"].ToString(),
                            CLIENTE_NOMBRE = reader["CLIENTE_NOMBRE"].ToString(),
                            TIPO_DOCU_DES = reader["TIPO_DOCU_DES"].ToString(),
                            DOCUMENTO = reader["DOCUMENTO"].ToString(),
                            FECHA_DOCU_FORMATO = reader["FECHA_DOCU_FORMATO"].ToString(),
                            MONEDA_DOCU = reader["MONEDA_DOCU"].ToString(),
                            IMPO_TOTAL = reader["IMPO_TOTAL"].ToString(),
                            ESTADO_DOCU = reader["ESTADO_DOCU"].ToString(),
                            ESTADO_ENVIO_CORREO_CLIENTE = reader["ESTADO_ENVIO_CORREO_CLIENTE"].ToString(),
                            ESTADO = reader["ESTADO"].ToString(),
                            ERROR = string.Format("{0}-{1}", reader["CODIGO_ERROR"].ToString(), reader["DESCRIPCION_ERROR"].ToString()),
                            FECHA_ENVIADO = reader["FECHA_ENVIADO"].ToString(),
                            CLIENTE_CORREO = reader["CLIENTE_CORREO"].ToString(),
                            EMPRESA_CODIGO = reader["EMPRESA_CODIGO"].ToString(),
                            ARCHIVO_RESU = reader["ARCHIVO_RESU"].ToString()
                            //ARCHIVO_PDF_PATH = string.Format(@"{0}\{1}" ,empresa.CarpetaRecepcionPdf,reader["ARCHIVO_PDF_NOMBRE"].ToString()),
                            //ARCHIVO_XML_PATH = string.Format(@"{0}\{1}\{2}.xml", empresa.CarpetaXml, reader["TIPO_DOCU"].ToString(),reader["CODIGO"].ToString())
                        });
                    }

                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    serializer.MaxJsonLength = 2147483644;
                    json = serializer.Serialize(lista);

                }
            }

            return json;
        }

        public string Load(string sEmpresa, string sDocumento, string Top)
        {
            string json = string.Empty;

            Entity.Empresa empresa = new BusinessGlobal.Consulta().ConsultaEmpresaById(sEmpresa);

            using (SqlConnection conn = new SqlConnection(Conexion.ObtenerConexionReporte()))
            {
                conn.Open();

                string sql = string.Format("select " + ((Top != "") ? (" top " + Top) : "") + " " + 
                "ID, CODIGO, CLIENTE_RUC_DNI, CLIENTE_NOMBRE, TIPO_DOCU, TIPO_DOCU_DES, NUMERO_SERIE_DOCU, NUMERO_DOC_DOCU,  " +
                "DOCUMENTO, FECHA_DOCU, FECHA_DOCU_FORMATO, MONEDA_DOCU, IMPO_TOTAL, ESTADO_DOCU, ESTATUS_ENVIADO,  " +
                "ESTATUS_RECEP, ESTATUS_ANUL, ESTATUS_RECHAZADO, ESTATUS_SUNAT, CODIGO_SUNAT, ESTADO_ENVIO_CORREO_CLIENTE, " +
                "CODIGO_ERROR, DESCRIPCION_ERROR, CODIGO_OBS, DESCRIPCION_OBS, ARCHIVO_PDF_NOMBRE, ARCHIVO_RESU, ESTADO " +
                "from vDOCUMENTO_REPORTE_CLIENTE " +
                "WHERE EMPRESA_CODIGO={0} {1} " +
                "ORDER BY TIPO_DOCU, NUMERO_SERIE_DOCU, NUMERO_DOC_DOCU DESC",
                sEmpresa,
                (sDocumento == "") ? "" : string.Format("AND ESTADO_DOCU='ACT' AND ESTADO='ACEPTADO' AND CLIENTE_RUC_DNI='{0}'", sDocumento)
                );


                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    SqlDataReader reader = cmd.ExecuteReader();
                    List<Entity.DocumentoInterno> lista = null;

                    if (reader.HasRows) lista = new List<Entity.DocumentoInterno>();

                    while (reader.Read())
                    {
                        lista.Add(new Entity.DocumentoInterno
                        {
                            ID = reader["ID"].ToString(),
                            CODIGO = reader["CODIGO"].ToString(),
                            CLIENTE_RUC_DNI = reader["CLIENTE_RUC_DNI"].ToString(),
                            CLIENTE_NOMBRE = reader["CLIENTE_NOMBRE"].ToString(),
                            TIPO_DOCU_DES = reader["TIPO_DOCU_DES"].ToString(),
                            DOCUMENTO = reader["DOCUMENTO"].ToString(),
                            FECHA_DOCU_FORMATO = reader["FECHA_DOCU_FORMATO"].ToString(),
                            MONEDA_DOCU = reader["MONEDA_DOCU"].ToString(),
                            IMPO_TOTAL = reader["IMPO_TOTAL"].ToString(),
                            ESTADO_DOCU = reader["ESTADO_DOCU"].ToString(),
                            ESTADO_ENVIO_CORREO_CLIENTE = reader["ESTADO_ENVIO_CORREO_CLIENTE"].ToString(),
                            ESTADO = reader["ESTADO"].ToString()

                            //ARCHIVO_PDF_PATH = string.Format(@"{0}\{1}" ,empresa.CarpetaRecepcionPdf,reader["ARCHIVO_PDF_NOMBRE"].ToString()),
                            //ARCHIVO_XML_PATH = string.Format(@"{0}\{1}\{2}.xml", empresa.CarpetaXml, reader["TIPO_DOCU"].ToString(),reader["CODIGO"].ToString())
                        });
                    }

                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    serializer.MaxJsonLength = 2147483644;
                    json = serializer.Serialize(lista);

                }
            }

            return json;
        }

        public List<Entity.Documento> LoadByMonth(string sEmpresa, string sID)
        {
            string json = string.Empty;
            List<Entity.Documento> lista = new List<Entity.Documento>();
            Entity.Empresa empresa = new BusinessGlobal.Consulta().ConsultaEmpresaById(sEmpresa);

            using (SqlConnection conn = new SqlConnection(Conexion.ObtenerConexionReporte()))
            {
                conn.Open();

                string sql = string.Format("select " +
                "ID, CODIGO, CLIENTE_RUC_DNI, CLIENTE_NOMBRE, TIPO_DOCU, TIPO_DOCU_DES, NUMERO_SERIE_DOCU, NUMERO_DOC_DOCU,  " +
                "DOCUMENTO, FECHA_DOCU, FECHA_DOCU_FORMATO, MONEDA_DOCU, IMPO_TOTAL, ESTADO_DOCU, ESTATUS_ENVIADO,  " +
                "ESTATUS_RECEP, ESTATUS_ANUL, ESTATUS_RECHAZADO, ESTADO_ENVIO_CORREO_CLIENTE, " +
                "CODIGO_ERROR, DESCRIPCION_ERROR, CODIGO_OBS, DESCRIPCION_OBS, ARCHIVO_PDF_NOMBRE , ARCHIVO_RESU, ESTADO, NUMERO_TICKET " +
                "from vDOCUMENTO_REPORTE_CLIENTE " +
                "WHERE [EMPRESA_CODIGO]={0} AND LEFT(FECHA_DOCU,6)='{1}'",
                sEmpresa, sID);


                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        string doc = reader["NUMERO_SERIE_DOCU"].ToString().Substring(0, 1);

                        lista.Add(new Entity.Documento
                        {
                            ID = reader["ID"].ToString(),
                            CODIGO = reader["CODIGO"].ToString(),
                            CLIENTE_RUC_DNI = reader["CLIENTE_RUC_DNI"].ToString(),
                            CLIENTE_NOMBRE = reader["CLIENTE_NOMBRE"].ToString(),
                            TIPO_DOCU = reader["TIPO_DOCU"].ToString(),
                            TIPO_DOCU_DES = reader["TIPO_DOCU_DES"].ToString(),
                            NUMERO_SERIE_DOCU = reader["NUMERO_SERIE_DOCU"].ToString(),
                            NUMERO_DOC_DOCU = reader["NUMERO_DOC_DOCU"].ToString(),
                            DOCUMENTO = reader["DOCUMENTO"].ToString(),
                            FECHA_DOCU = reader["FECHA_DOCU"].ToString(),
                            FECHA_DOCU_FORMATO = reader["FECHA_DOCU_FORMATO"].ToString(),
                            MONEDA_DOCU = reader["MONEDA_DOCU"].ToString(),
                            IMPO_TOTAL = reader["IMPO_TOTAL"].ToString(),
                            ESTADO_DOCU = reader["ESTADO_DOCU"].ToString(),
                            ESTATUS_RECEP = reader["ESTATUS_RECEP"].ToString(),
                            ESTATUS_ANUL = reader["ESTATUS_ANUL"].ToString(),
                            ESTATUS_RECHAZADO = reader["ESTATUS_RECHAZADO"].ToString(),
                            ESTADO_ENVIO_CORREO_CLIENTE = reader["ESTADO_ENVIO_CORREO_CLIENTE"].ToString(),
                            CODIGO_ERROR = reader["CODIGO_ERROR"].ToString(),
                            DESCRIPCION_ERROR = reader["DESCRIPCION_ERROR"].ToString(),
                            CODIGO_OBS = reader["CODIGO_OBS"].ToString(),
                            NUMERO_TICKET = reader["NUMERO_TICKET"].ToString(),
                            DESCRIPCION_OBS = reader["DESCRIPCION_OBS"].ToString(),
                            ARCHIVO_PDF_NOMBRE = reader["ARCHIVO_PDF_NOMBRE"].ToString(),
                            ARCHIVO_PDF_PATH = string.Format(@"{0}\{1}", empresa.CarpetaRecepcionPdf, reader["ARCHIVO_PDF_NOMBRE"].ToString()),
                            ARCHIVO_TICKET_PATH = string.Format(@"{0}\{1}", empresa.CarpetaRecepcionTicket, reader["ARCHIVO_PDF_NOMBRE"].ToString()),

                            ARCHIVO_XML_PATH = string.Format(@"{0}\{1}\{2}.xml", empresa.CarpetaXml,
                           (doc == "B") ? "RC" : reader["TIPO_DOCU"].ToString(),
                           (doc == "B") ? reader["ARCHIVO_RESU"].ToString() : reader["CODIGO"].ToString()),

                            ARCHIVO_CDR_PATH = string.Format(@"{0}\{1}\R-{2}.xml", empresa.CarpetaCDR,
                            (doc == "B") ? "RC" : reader["TIPO_DOCU"].ToString(),
                            (doc == "B") ? reader["ARCHIVO_RESU"].ToString() : reader["CODIGO"].ToString()),

                            ARCHIVO_FILE1_PATH = string.Format(@"{0}\{1}.xml", empresa.CarpetaRespaldo, reader["ARCHIVO_PDF_NOMBRE"].ToString().Replace(".pdf", "")),
                            ARCHIVO_FILE2_PATH = string.Format(@"{0}\{1}-A.xml", empresa.CarpetaRespaldo, reader["ARCHIVO_PDF_NOMBRE"].ToString().Replace(".pdf", "")),
                            ARCHIVO_FILE3_PATH = string.Format(@"{0}\{1}.txt", empresa.CarpetaRespaldo, reader["ARCHIVO_PDF_NOMBRE"].ToString().Replace(".pdf", ""))
                            //ARCHIVO_XML_PATH_RESUMEN_ENVIO = (reader["ESTADO_DOCU"].ToString()=="ACT")?
                            //string.Format(@"{0}\RC\{1}.xml", empresa.CarpetaXml, reader["ARCHIVO_RESU"].ToString()):
                            //string.Format(@"{0}\{1}\{2}.xml", empresa.CarpetaXml, 
                            //(reader["NUMERO_SERIE_DOCU"].ToString().Substring(1,1)=="B")?"RA":"RR",
                            //reader["ARCHIVO_RESU"].ToString()),

                            //ARCHIVO_XML_PATH_RESUMEN_RESPUESTA = (reader["ESTADO_DOCU"].ToString()=="ACT")?
                            //string.Format(@"{0}\RC\{1}.xml", empresa.CarpetaRecepcion,reader["CODIGO"].ToString()):
                            //string.Format(@"{0}\{1}\{2}.xml", empresa.CarpetaRecepcion,
                            //(reader["NUMERO_SERIE_DOCU"].ToString().Substring(1, 1) == "B") ? "RA" : "RR",
                            //reader["CODIGO"].ToString())
                        });
                    }

                }
            }

            return lista;
        }

        public Entity.Documento LoadByCodigo(string sEmpresa,  string sID)
        {
            string json = string.Empty;
            Entity.Documento documento = new Entity.Documento();
            Entity.Empresa empresa = new BusinessGlobal.Consulta().ConsultaEmpresaById(sEmpresa);

            using (SqlConnection conn = new SqlConnection(Conexion.ObtenerConexionReporte()))
            {
                conn.Open();

                string sql = string.Format("select " +
                "ID, CODIGO, CLIENTE_RUC_DNI, CLIENTE_NOMBRE, TIPO_DOCU, TIPO_DOCU_DES, NUMERO_SERIE_DOCU, NUMERO_DOC_DOCU,  " +
                "DOCUMENTO, FECHA_DOCU, FECHA_DOCU_FORMATO, MONEDA_DOCU, IMPO_TOTAL, ESTADO_DOCU, ESTATUS_ENVIADO,  " +
                "ESTATUS_RECEP, ESTATUS_ANUL, ESTATUS_RECHAZADO, ESTADO_ENVIO_CORREO_CLIENTE, " +
                "CODIGO_ERROR, DESCRIPCION_ERROR, CODIGO_OBS, DESCRIPCION_OBS, ARCHIVO_PDF_NOMBRE , ARCHIVO_RESU, ESTADO, NUMERO_TICKET " +
                "from vDOCUMENTO_REPORTE_CLIENTE " +
                "WHERE [EMPRESA_CODIGO]={0} AND ID={1}",
                sEmpresa, sID);


                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    SqlDataReader reader = cmd.ExecuteReader();
                    
                    while (reader.Read())
                    {
                        string doc = reader["NUMERO_SERIE_DOCU"].ToString().Substring(0, 1);

                        documento = new Entity.Documento
                        {
                            ID = reader["ID"].ToString(),
                            CODIGO = reader["CODIGO"].ToString(),
                            CLIENTE_RUC_DNI = reader["CLIENTE_RUC_DNI"].ToString(),
                            CLIENTE_NOMBRE = reader["CLIENTE_NOMBRE"].ToString(),
                            TIPO_DOCU = reader["TIPO_DOCU"].ToString(),
                            TIPO_DOCU_DES = reader["TIPO_DOCU_DES"].ToString(),
                            NUMERO_SERIE_DOCU = reader["NUMERO_SERIE_DOCU"].ToString(),
                            NUMERO_DOC_DOCU = reader["NUMERO_DOC_DOCU"].ToString(),
                            DOCUMENTO = reader["DOCUMENTO"].ToString(),
                            FECHA_DOCU = reader["FECHA_DOCU"].ToString(),
                            FECHA_DOCU_FORMATO = reader["FECHA_DOCU_FORMATO"].ToString(),
                            MONEDA_DOCU = reader["MONEDA_DOCU"].ToString(),
                            IMPO_TOTAL = reader["IMPO_TOTAL"].ToString(),
                            ESTADO_DOCU = reader["ESTADO_DOCU"].ToString(),
                            ESTATUS_RECEP = reader["ESTATUS_RECEP"].ToString(),
                            ESTATUS_ANUL = reader["ESTATUS_ANUL"].ToString(),
                            ESTATUS_RECHAZADO = reader["ESTATUS_RECHAZADO"].ToString(),
                            ESTADO_ENVIO_CORREO_CLIENTE = reader["ESTADO_ENVIO_CORREO_CLIENTE"].ToString(),
                            CODIGO_ERROR = reader["CODIGO_ERROR"].ToString(),
                            DESCRIPCION_ERROR = reader["DESCRIPCION_ERROR"].ToString(),
                            CODIGO_OBS = reader["CODIGO_OBS"].ToString(),
                            NUMERO_TICKET = reader["NUMERO_TICKET"].ToString(),
                            DESCRIPCION_OBS = reader["DESCRIPCION_OBS"].ToString(),
                            ARCHIVO_PDF_NOMBRE = reader["ARCHIVO_PDF_NOMBRE"].ToString(),
                            ARCHIVO_PDF_PATH = string.Format(@"{0}\{1}" ,empresa.CarpetaRecepcionPdf,reader["ARCHIVO_PDF_NOMBRE"].ToString()),
                            ARCHIVO_TICKET_PATH = string.Format(@"{0}\{1}", empresa.CarpetaRecepcionTicket, reader["ARCHIVO_PDF_NOMBRE"].ToString()),

                            ARCHIVO_XML_PATH = string.Format(@"{0}\{1}\{2}.xml", empresa.CarpetaXml,
                           (doc == "B") ? "RC" : reader["TIPO_DOCU"].ToString(),
                           (doc == "B") ? reader["ARCHIVO_RESU"].ToString() : reader["CODIGO"].ToString()),

                            ARCHIVO_CDR_PATH = string.Format(@"{0}\{1}\R-{2}.xml", empresa.CarpetaCDR,
                            (doc == "B") ? "RC" : reader["TIPO_DOCU"].ToString(),
                            (doc == "B") ? reader["ARCHIVO_RESU"].ToString() : reader["CODIGO"].ToString()),

                            ARCHIVO_FILE1_PATH = string.Format(@"{0}\{1}.xml", empresa.CarpetaRespaldo, reader["ARCHIVO_PDF_NOMBRE"].ToString().Replace(".pdf", "")),
                            ARCHIVO_FILE2_PATH = string.Format(@"{0}\{1}-A.xml", empresa.CarpetaRespaldo, reader["ARCHIVO_PDF_NOMBRE"].ToString().Replace(".pdf", "")),
                            ARCHIVO_FILE3_PATH = string.Format(@"{0}\{1}.txt", empresa.CarpetaRespaldo, reader["ARCHIVO_PDF_NOMBRE"].ToString().Replace(".pdf", ""))
                            //ARCHIVO_XML_PATH_RESUMEN_ENVIO = (reader["ESTADO_DOCU"].ToString()=="ACT")?
                            //string.Format(@"{0}\RC\{1}.xml", empresa.CarpetaXml, reader["ARCHIVO_RESU"].ToString()):
                            //string.Format(@"{0}\{1}\{2}.xml", empresa.CarpetaXml, 
                            //(reader["NUMERO_SERIE_DOCU"].ToString().Substring(1,1)=="B")?"RA":"RR",
                            //reader["ARCHIVO_RESU"].ToString()),
                            
                            //ARCHIVO_XML_PATH_RESUMEN_RESPUESTA = (reader["ESTADO_DOCU"].ToString()=="ACT")?
                            //string.Format(@"{0}\RC\{1}.xml", empresa.CarpetaRecepcion,reader["CODIGO"].ToString()):
                            //string.Format(@"{0}\{1}\{2}.xml", empresa.CarpetaRecepcion,
                            //(reader["NUMERO_SERIE_DOCU"].ToString().Substring(1, 1) == "B") ? "RA" : "RR",
                            //reader["CODIGO"].ToString())
                        };
                    }

                }
            }

            return documento;
        }

        public string Agregar(string ruc, string idcliente, string idtrabajador, string fecha, string cambio_venta, 
            string moneda, string tipo, string indicador, string serie, string correlativo, string referencia, 
            string valorventa, string igv, string total, string ipcliete, string impresora, 
            string tipopago, string tipoenvio, string observacion)
        {
            string codigo = string.Empty;

            using (SqlConnection conn = new SqlConnection(Conexion.ObtenerConexionVentas()))
            {
                conn.Open();

                string sql = string.Format("exec spinsertar_venta02 '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}'," +
                    "'{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}'", 
                     ruc,  idcliente,  idtrabajador,  fecha,  cambio_venta,  moneda,  tipo, indicador,  serie,  correlativo,  
                     referencia,  valorventa,  igv,  total, ipcliete, impresora, tipopago, tipoenvio, observacion);

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

                string sql = string.Format("exec spinsertar_detalle_venta02 '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}'",
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

                string sql = string.Format("update venta set transaccion=1 where idventa= '{0}';" + 
                "update serie set correlativo= b.correlativo " +
                "from serie a join " +
	            "        venta b on a.tipo= tipo_comprobante_codigo and a.indicador=b.indicador and a.serie=b.serie " +
                "where b.idventa={0} and a.ruc= '{1}' and b.ruc='{1}';",codigo, ruc);

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    codigo = cmd.ExecuteNonQuery().ToString();
                }
            }
        }

        public string LoadTransaccion(string ruc, string codigo)
        {
            string json = string.Empty;
            string sql = string.Empty;

            List<Entity.Lista> listatmp = new List<Entity.Lista>();

            sql = string.Format("select tipo_comprobante + ':'+ indicador + rtrim(ltrim(serie)) +'-' +  right('0000000' + cast(correlativo as varchar(7)),7) as documento " +
                                "from venta " +
                                "where ruc='{0}' and idventa='{1}';" ,
                                ruc,
                                codigo);

            using (SqlConnection conn = new SqlConnection(Conexion.ObtenerConexionVentas()))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    SqlDataReader reader = cmd.ExecuteReader();


                    while (reader.Read())
                    {
                        json = reader["documento"].ToString();
                    }
                }
            }

          
            return json;
        }

        public void Eliminar(string codigo)
        {
            using (SqlConnection conn = new SqlConnection(Conexion.ObtenerConexionVentas()))
            {
                conn.Open();

                string sql = string.Format("update venta set anulado=1, enviado=0 " + 
                "where  idventa = " + 
                "( " + 
                "   select a.NUMERO_TICKET " + 
                "   from SUNAT_VENTAS.dbo.CABECERA_DOC a join " +
                "        SUNAT_REPORTE.dbo.vDOCUMENTO_REPORTE_CLIENTE b " + 
                "               on a.EMPRESA_CODIGO= b.EMPRESA_CODIGO and a.DOCUMENTO_XML=b.CODIGO " +      
                "   where b.ID={0} " + 
                ");", codigo);

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Imprimir(string empresa, string codigo)
        {
            using (SqlConnection conn = new SqlConnection(Conexion.ObtenerConexionVentas()))
            {
                conn.Open();

                string sql = string.Format("update SUNAT_VENTAS.dbo.CABECERA_DOC set  IMPRESO=null " +
                "where EMPRESA_CODIGO={0} and DOCUMENTO_XML='{1}';", empresa, codigo);

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                }
            }
        }
        
        
    }
}
