using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebVentas
{
    public partial class ViewPDF : System.Web.UI.Page
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
            
            //Response.Expires = -1;
            Response.ContentType = "application/pdf";

            if (!Page.IsPostBack)
            {

                if (!Context.User.Identity.IsAuthenticated)
                    // This is an unauthorized, authenticated request...
                    Response.Redirect("Default.aspx");
            }

            string id = Request.QueryString["id"].ToString();


            Entity.Documento documento = new Repositorio.Documento().LoadByCodigo(empresaid, id);


            if (documento.ARCHIVO_PDF_PATH != null)
            {
                FileInfo file = new FileInfo(documento.ARCHIVO_PDF_PATH);

                if (file.Exists)
                {
                    Response.Clear();
                    Response.ContentType = "application/pdf";
                    //Response.AddHeader("content-disposition", string.Format(@"attachment;filename=""{0}""", documento.ARCHIVO_PDF_NOMBRE));
                    Response.WriteFile(documento.ARCHIVO_PDF_PATH);
                }
                else
                {
                    Response.Clear();
                    Response.ContentType = "text/plain";
                    Response.Write(string.Format("El archivo no fue encontrado:{0}", documento.ARCHIVO_PDF_PATH));
                }
            }
        }
    }
}