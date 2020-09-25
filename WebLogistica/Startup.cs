using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebLogistica.Startup))]
namespace WebLogistica
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
