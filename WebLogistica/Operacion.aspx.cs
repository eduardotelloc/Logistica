using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity;
using System.Web.Script.Serialization;
using System.IO;
using System.Text;
using System.Configuration;
using System.Web.Security;
using System.Web.Services;
using Newtonsoft.Json;
using Entity;
using RestSharp;

namespace WebLogistica
{
    public partial class Operacion : System.Web.UI.Page
    {
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

            string empresaid = ticket.UserData.Split('|')[0];
            string ruc = ticket.UserData.Split('|')[1];
            string usuario_acceso = ticket.UserData.Split('|')[3];
            string usuario_serie = ticket.UserData.Split('|')[7];
            string puntosid = ticket.UserData.Split('|')[8];
            string ipcliente = ticket.UserData.Split('|')[9];
            string localcodigo = ticket.UserData.Split('|')[10];
            //Response.Expires = -1;
            Response.ContentType = "application/text";
            string respuesta = "";

            

            
            
            if (Request.QueryString["ope"] == "100")
            {

                //string sTop = Request.QueryString["Top"].ToString();
                //string sFechaInicio = string.Empty;
                //string sFechaFin = string.Empty;

                //if (Request.QueryString["FechaInicio"].ToString() != "")
                //    sFechaInicio = Request.QueryString["FechaInicio"].ToString().ConverTo_YYYYMMDD();

                //if (Request.QueryString["FechaFin"].ToString() != "")
                //    sFechaFin = Request.QueryString["FechaFin"].ToString().ConverTo_YYYYMMDD();

                //string sUsuario = Context.User.Identity.Name;

                //using (Repositorio.CierreCaja obj = new Repositorio.CierreCaja())
                //{
                //    respuesta = obj.Load(ruc, sFechaInicio, sFechaFin, sTop);
                //}
            }


            Response.Write(respuesta);
            Response.ContentEncoding = Encoding.UTF8;
            Response.Flush();
            Response.End();
        }

       
        
    }
}