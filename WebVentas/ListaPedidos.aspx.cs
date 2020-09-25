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
    public partial class ListaPedidos : Page
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

            string empresaid = ticket.UserData.Split('|')[0];
            string rucEmpresa = ticket.UserData.Split('|')[1];
            string usuarioanum_documento = ticket.UserData.Split('|')[7];

            ViewState["usuarioanum_documento"] = usuarioanum_documento;


            if (!Page.IsPostBack)
            {



                if (!Context.User.Identity.IsAuthenticated)
                    // This is an unauthorized, authenticated request...
                    Response.Redirect("Default.aspx");
                else 
                {
                    Repositorio.Cliente objcliente = new Repositorio.Cliente();
                    string limitepago = objcliente.LimitePago(rucEmpresa);
                    if (limitepago != "")
                    {
                        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text",
                           string.Format("alert('Tiene monto de deuda pendiente de pago, el sistema sera bloqueado dentro de {0} !');", limitepago), true);

                    }
                }
            }

            if (!String.IsNullOrEmpty(Request.QueryString["id"]))
            {

                string id = Request.QueryString["id"].ToString();
                Entity.Documento documento = new Repositorio.Documento().LoadByCodigo(empresaid, id);


                if (!String.IsNullOrEmpty(Request.QueryString["tipo"]))
                {
                    if (Request.QueryString["tipo"] == "1")
                    {
                        using (ZipFile zip = new ZipFile())
                        {
                            zip.AlternateEncodingUsage = ZipOption.AsNecessary;
                            zip.AddDirectoryByName(documento.CODIGO);

                            FileInfo file1 = new FileInfo(documento.ARCHIVO_PDF_PATH);
                            FileInfo file2 = new FileInfo(documento.ARCHIVO_XML_PATH);
                            FileInfo file3 = new FileInfo(documento.ARCHIVO_CDR_PATH);

                            if (file1.Exists)
                                zip.AddFile(documento.ARCHIVO_PDF_PATH, documento.CODIGO);

                            if (file2.Exists)
                                zip.AddFile(documento.ARCHIVO_XML_PATH, documento.CODIGO);

                            if (file3.Exists)
                                zip.AddFile(documento.ARCHIVO_CDR_PATH, documento.CODIGO);

                            Response.Clear();
                            Response.BufferOutput = false;
                            string zipName = String.Format("{0}.zip", documento.CODIGO);
                            Response.ContentType = "application/zip";
                            Response.AddHeader("content-disposition", "attachment; filename=" + zipName);
                            zip.Save(Response.OutputStream);
                            Response.End();
                        }
                    }

                    if (Request.QueryString["tipo"] == "2")
                    {
                        FileInfo fileXml = new FileInfo(documento.ARCHIVO_XML_PATH);
                        Response.Clear();
                        string docname = String.Format("{0}.xml", documento.CODIGO);
                        Response.ContentType = "application/xml";
                        Response.AddHeader("content-disposition", "attachment; filename=" + docname);
                        Response.WriteFile(documento.ARCHIVO_XML_PATH);
                        Response.End();
                    }


                    if (Request.QueryString["tipo"] == "3")
                    {
                        FileInfo filePdf = new FileInfo(documento.ARCHIVO_PDF_PATH);
                        Response.Clear();
                        string docname = String.Format("{0}.pdf", documento.CODIGO);
                        Response.ContentType = "application/pdf";
                        Response.AddHeader("content-disposition", "attachment; filename=" + docname);
                        Response.WriteFile(documento.ARCHIVO_PDF_PATH);
                        Response.End();
                    }

                    if (Request.QueryString["tipo"] == "4")
                    {
                        using (ZipFile zip = new ZipFile())
                        {
                            zip.AlternateEncodingUsage = ZipOption.AsNecessary;
                            zip.AddDirectoryByName(documento.CODIGO);

                            FileInfo file1 = new FileInfo(documento.ARCHIVO_FILE1_PATH);
                            FileInfo file2 = new FileInfo(documento.ARCHIVO_FILE2_PATH);
                            FileInfo file3 = new FileInfo(documento.ARCHIVO_FILE3_PATH);

                            if (file1.Exists)
                                zip.AddFile(documento.ARCHIVO_FILE1_PATH, documento.CODIGO);

                            if (file2.Exists)
                                zip.AddFile(documento.ARCHIVO_FILE2_PATH, documento.CODIGO);

                            if (file3.Exists)
                                zip.AddFile(documento.ARCHIVO_FILE3_PATH, documento.CODIGO);

                            Response.Clear();
                            Response.BufferOutput = false;
                            string zipName = String.Format("{0}.zip", documento.CODIGO);
                            Response.ContentType = "application/zip";
                            Response.AddHeader("content-disposition", "attachment; filename=" + zipName);
                            zip.Save(Response.OutputStream);
                            Response.End();
                        }
                    }
                }

            }
        }
    }
}