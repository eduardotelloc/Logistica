using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebVentas
{
    public partial class ContrasenaCambio : System.Web.UI.Page
    {
        string empresaruc = "";
        string usuariocodigo = "";
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                if (!Context.User.Identity.IsAuthenticated)
                    // This is an unauthorized, authenticated request...
                    Response.Redirect("Default.aspx");
            }

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

            empresaruc = ticket.UserData.Split('|')[1];
            usuariocodigo = ticket.UserData.Split('|')[4];

        }

        protected void btnCambio_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {
                FailureText.Text = "";
                ErrorMessage.Visible = false;

                if (CurrentPassword.Value == NewPassword.Value)
                {
                    Repositorio.Usuario obj = new Repositorio.Usuario();
                    bool blnresult= obj.CambiarClave(empresaruc, usuariocodigo, NewPassword.Value);

                   
                    if (blnresult)
                    {
                        FailureText.Text = "Clave modificada!";
                        ErrorMessage.Visible = true;
                        btnCambio.Enabled = false;
                    }
                    else
                    {
                        FailureText.Text = "clave inválido!";
                        ErrorMessage.Visible = true;
                    }
                }
                else
                {
                    FailureText.Text = "La clave deben ser iguales!";
                    ErrorMessage.Visible = true;
                }
            }
        }
    }
}