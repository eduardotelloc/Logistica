using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Entity
{
    public class DocumentoTicket
    {

        public string Codigo { get; set; }
        public string Tipo { get; set; }
        public string Ticket { get; set; }

        public byte[] ArrayPDF { get; set; }
        public bool ExistePDF { get; set; }
        public string PathPdf { get; set; }
        public DataTable Cabecera { get; set; }
        public DataTable Detalle { get; set; }
        public DataTable Pago { get; set; }
    }
}
