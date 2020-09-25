using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Web.Security;

namespace WebVentas
{
    public partial class Reporte : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            FormsAuthenticationTicket ticket = null;

            try
            {
                FormsIdentity formsIdentity = HttpContext.Current.User.Identity as FormsIdentity;
                ticket = formsIdentity.Ticket;
            }
            catch 
            {
                Response.Redirect("Default.aspx");
            }

            
            if (!Page.IsPostBack)
            {
                if (!Context.User.Identity.IsAuthenticated)
                    // This is an unauthorized, authenticated request...
                    Response.Redirect("Default.aspx");
            }


          

            string empresaid = ticket.UserData.Split('|')[0];
            string usuarioanum_documento = ticket.UserData.Split('|')[7];

           
            if (!String.IsNullOrEmpty(Request.QueryString["ope"]))
            {
                ZipFile zip = null;
                string id = Request.QueryString["ope"].ToString();
                string filtro = string.Format("{0}{1}", Request.QueryString["anio"].ToString() ,Request.QueryString["mes"].ToString());

                List<Entity.Documento> lista = new Repositorio.Documento().LoadByMonth(empresaid, filtro);

                if (lista.Count > 0) {
                    zip = new ZipFile();
                    zip.AlternateEncodingUsage = ZipOption.AsNecessary;
                    //zip.AddDirectoryByName(filtro);
                }

                foreach (var documento in lista)
                {
                      
                    FileInfo file1 = new FileInfo(documento.ARCHIVO_PDF_PATH);
                    FileInfo file2 = new FileInfo(documento.ARCHIVO_XML_PATH);
                    FileInfo file3 = new FileInfo(documento.ARCHIVO_CDR_PATH);

                    if (file1.Exists)
                        zip.AddFile(documento.ARCHIVO_PDF_PATH, documento.CODIGO);

                    if (file2.Exists)
                        zip.AddFile(documento.ARCHIVO_XML_PATH, documento.CODIGO);

                    if (file3.Exists)
                        zip.AddFile(documento.ARCHIVO_CDR_PATH, documento.CODIGO);

                   
                    
                }

                if (lista.Count > 0) {
                    Response.Clear();
                    Response.BufferOutput = false;
                    string zipName = String.Format("{0}.zip", filtro);
                    Response.ContentType = "application/zip";
                    Response.AddHeader("content-disposition", "attachment; filename=" + zipName);
                    zip.Save(Response.OutputStream);
                    Response.End();
                }
            }
        }
    }
}