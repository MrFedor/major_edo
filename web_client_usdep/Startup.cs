using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(web_client_usdep.Startup))]
namespace web_client_usdep
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
