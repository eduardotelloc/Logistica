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
    public partial class Cierre : Page
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
                    { 
                        // This is an unauthorized, authenticated request...
                        Response.Redirect("Default.aspx");
                    }
            }
        }
    }
}