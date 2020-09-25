using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebVentas
{
    public partial class Menu : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (!Context.User.Identity.IsAuthenticated)
                    // This is an unauthorized, authenticated request...
                    Response.Redirect("Default.aspx");
                else
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
                    string empresaruc = ticket.UserData.Split('|')[1];
                    string empresanombre = ticket.UserData.Split('|')[2];
                    string acceso = ticket.UserData.Split('|')[3];
                    string usuario = ticket.UserData.Split('|')[5];
                    string usuarioanum_documento = ticket.UserData.Split('|')[6];


                    lblEmpresa.Text = string.Format("{0}-{1} {2}", empresaruc, empresanombre, usuario);


                    ViewState["acceso"] = acceso;
                    ViewState["usuario"] = usuario;
                    ViewState["empresaruc"] = empresaruc;
                    ViewState["empresanombre"] = empresanombre;
                    ViewState["usuarioanum_documento"] = usuarioanum_documento;


                    Repositorio.Cliente objcliente = new Repositorio.Cliente();
                    string limitepago = objcliente.LimitePago(empresaruc);

                    if (limitepago != "")
                    {
                        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text",
                           string.Format("alert('Tiene monto de deuda pendiente de pago, el sistema sera bloqueado dentro de {0} !');", limitepago), true);

                    }
                }
            }
        }

        protected void Unnamed_LoggingOut(object sender, LoginCancelEventArgs e)
        {
            Context.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
        }
    }
}