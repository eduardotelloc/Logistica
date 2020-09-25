using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebLogistica
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                HttpCookie cookie_empresaruc = Request.Cookies["ruc"];
                if (cookie_empresaruc != null)
                    Ruc.Value = cookie_empresaruc.Value;

                HttpCookie cookie_usuario =  Request.Cookies["usuario"];
                if (cookie_usuario != null)
                    Email.Value = cookie_usuario.Value;

                HttpCookie cookie_recordar =  Request.Cookies["recordar"];
                if (cookie_recordar != null)
                    chkrecordar.Checked = ((cookie_recordar.Value=="1")?true:false);
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {
                string userRuc = Ruc.Value;
                string userName = Email.Value;
                string password = Password.Value;
                bool Authenticated = false;
                string userNameCompuesto = string.Empty;


                Repositorio.Usuario obj = new Repositorio.Usuario();
                Authenticated = obj.ValidateClientRuc(userRuc, userName, password);



                if (Authenticated)
                {
                    BusinessGlobal.Consulta objconsulta = new BusinessGlobal.Consulta();
                    Entity.Empresa objempresa = objconsulta.ConsultaEmpresaSimpleByRuc(userRuc);
                    Entity.Usuario usu = obj.GetUsuario(userRuc, userName);


                    FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, userName, DateTime.Now, DateTime.Now.AddMinutes(30), true,
                        objempresa.Id.Trim() + "|" +
                        objempresa.Ruc.Trim() + "|" +
                        objempresa.Nombre.Trim() + "|" +
                        usu.acceso.Trim() + "|" +
                        usu.codigo.Trim() + "|" +
                        usu.usuario.Trim() + "|" +
                        usu.num_documento.Trim() + "|" +
                        usu.serie.Trim() + "|" +
                        usu.puntosid.Trim() + "|" +
                        "" + "|" +
                        usu.localcodigo.Trim() + "|",
                        FormsAuthentication.FormsCookiePath);

                    string encryptedCookie = FormsAuthentication.Encrypt(ticket);
                    HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedCookie);
                    cookie.Expires = DateTime.Now.AddMinutes(30);
                    Response.Cookies.Add(cookie);
                    //FormsAuthentication.RedirectFromLoginPage(userName, false);



                    if (chkrecordar.Checked)
                    {
                        DateTime now = DateTime.Now;
                        HttpCookie cookie_ruc = new HttpCookie("ruc");
                        cookie_ruc.Value = userRuc;
                        cookie_ruc.Expires = now.AddYears(50);
                        Response.Cookies.Add(cookie_ruc);

                        HttpCookie cookie_usuario = new HttpCookie("usuario");
                        cookie_usuario.Value = userName;
                        cookie_usuario.Expires = now.AddYears(50);
                        Response.Cookies.Add(cookie_usuario);

                        HttpCookie cookie_recordar = new HttpCookie("recordar");
                        cookie_recordar.Value = ((chkrecordar.Checked) ? "1" : "0");
                        cookie_recordar.Expires = now.AddYears(50);
                        Response.Cookies.Add(cookie_recordar);
                    }

                    Response.Redirect("Menu.aspx");

                    //if (usu.acceso == "Administrador")
                    //    Response.Redirect("DocumentosAdmin.aspx");


                    //if (usu.acceso == "Vendedor")
                    //    Response.Redirect("DocumentosAdmin.aspx");
                }
                else
                {
                    FailureText.Text = "Cuenta o clave inválido!";
                    ErrorMessage.Visible = true;
                }
            }
        }
    }
}