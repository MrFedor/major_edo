using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using Hangfire;

[assembly: OwinStartup(typeof(major_console.Startup))]

namespace major_console
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Дополнительные сведения о настройке приложения см. по адресу: http://go.microsoft.com/fwlink/?LinkID=316888
           
            GlobalConfiguration.Configuration.UseSqlServerStorage("DBConnection");            
            app.UseHangfireDashboard();
        }
    }
}
