using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(major_web.Startup))]
namespace major_web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
