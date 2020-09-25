using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebVentas.Startup))]
namespace WebVentas
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
