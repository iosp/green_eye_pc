using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration.Install;
using System.ComponentModel;
using System.ServiceProcess;

namespace GreenEyeAPI
{
    [RunInstaller(true)]
    public class EntityGatewayServiceInstaller : Installer
    {
        public EntityGatewayServiceInstaller()
        {
            ServiceProcessInstaller processInstaller = new ServiceProcessInstaller();
            ServiceInstaller serviceInstaller = new ServiceInstaller();

            //set the privileges
            processInstaller.Account = ServiceAccount.LocalSystem;

            serviceInstaller.DisplayName = "GreenEye Entity Gateway Service";
            serviceInstaller.StartType = ServiceStartMode.Automatic;

            //must be the same as what was set in Program's constructor
            serviceInstaller.ServiceName = "GreenEye Entity Gateway Service";

            this.Installers.Add(processInstaller);
            this.Installers.Add(serviceInstaller);
            this.AfterInstall += new InstallEventHandler(ServiceInstaller_AfterInstall);
        }

        void ServiceInstaller_AfterInstall(object sender, InstallEventArgs e)
        {
            ServiceController sc = new ServiceController("GreenEye Entity Gateway Service");
            sc.Start();
        }
    }
}
