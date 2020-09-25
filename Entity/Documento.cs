using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class Documento
    {
       
        public string Codigo { get; set; }
        public string Tipo { get; set; }
        public string Serie { get; set; }
        public string Correlativo { get; set; }
        public string Archivo { get; set; }
        public string Estado { get; set; }
        public string TipoPdf { get; set; }
        public string FechaDocu { get; set; }
        public string NumeroTicket { get; set; }
        

        public string ARCHIVO_CDR_PATH { get; set; }
        public string ID { get; set; }
        public string EMPRESA_CODIGO { get; set; }
        public string CODIGO { get; set; }
        public string CLIENTE_RUC_DNI { get; set; }
        public string CLIENTE_NOMBRE { get; set; }
        public string TIPO_DOCU { get; set; }
        public string TIPO_DOCU_DES { get; set; }
        public string NUMERO_SERIE_DOCU { get; set; }
        public string NUMERO_DOC_DOCU { get; set; }
        public string DOCUMENTO { get; set; }
        public string FECHA_DOCU { get; set; }
        public string FECHA_DOCU_FORMATO { get; set; }
        public string MONEDA_DOCU { get; set; }
        public string IMPO_TOTAL { get; set; }
        public string ESTADO_DOCU { get; set; }
        public string ESTATUS_RECEP { get; set; }
        public string ESTATUS_ANUL { get; set; }
        public string ESTATUS_RECHAZADO { get; set; }
        public string ESTADO_ENVIO_CORREO_CLIENTE { get; set; }
        public string CODIGO_ERROR { get; set; }
        public string DESCRIPCION_ERROR { get; set; }
        public string CODIGO_OBS { get; set; }
        public string DESCRIPCION_OBS { get; set; }
        public string ARCHIVO_PDF_NOMBRE { get; set; }

        public string ARCHIVO_TICKET_PATH { get; set; }
        public string ARCHIVO_PDF_PATH { get; set; }
        public string ARCHIVO_XML_PATH { get; set; }

        public string ARCHIVO_FILE1_PATH { get; set; }
        public string ARCHIVO_FILE2_PATH { get; set; }
        public string ARCHIVO_FILE3_PATH { get; set; }

        public string ARCHIVO_XML_PATH_RESUMEN_ENVIO { get; set; }
        public string ARCHIVO_XML_PATH_RESUMEN_RESPUESTA { get; set; }

        public string ARCHIVO_RESU { get; set; }
        public string ESTADO { get; set; }
        public string NUMERO_TICKET { get; set; }

        
    }


    public class DocumentoInterno
    {
        public string ID { get; set; }
        public string EMPRESA_CODIGO { get; set; }
        public string EMPRESA_NOMBRE { get; set; }
        public string CODIGO { get; set; }
        public string CLIENTE_RUC_DNI { get; set; }
        public string CLIENTE_NOMBRE { get; set; }
        public string TIPO_DOCU_DES { get; set; }
        public string DOCUMENTO { get; set; }
        public string FECHA_DOCU_FORMATO { get; set; }
        public string MONEDA_DOCU { get; set; }
        public string IMPO_TOTAL { get; set; }
        public string ESTADO_DOCU { get; set; }
        public string ESTADO_ENVIO_CORREO_CLIENTE { get; set; }
        public string ESTADO { get; set; }
        public string ERROR { get; set; }
        public string FECHA_ENVIADO { get; set; }
        public string CLIENTE_CORREO { get; set; }
        public string ARCHIVO_RESU { get; set; }
        
    }
}
