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
    public partial class Mesas : Page
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
        }
    }
}