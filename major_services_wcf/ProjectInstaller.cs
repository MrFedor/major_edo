using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace major_services_wcf
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        private ServiceProcessInstaller process;
        private ServiceInstaller service;

        public ProjectInstaller()
        {
            //InitializeComponent();
            process = new ServiceProcessInstaller();
            process.Account = ServiceAccount.User;
            service = new ServiceInstaller();
            service.ServiceName = "WCFWindowsService";
            Installers.Add(process);
            Installers.Add(service);
        }
    }
}
