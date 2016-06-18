using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(major_edo.Startup))]
namespace major_edo
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
