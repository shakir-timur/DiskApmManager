using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace DiskAPMService
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : Installer
    {
        private ServiceProcessInstaller serviceProcessInstaller;
        private ServiceInstaller serviceInstaller;


        public ProjectInstaller()
        {
            serviceProcessInstaller = new ServiceProcessInstaller();
            serviceProcessInstaller.Account = ServiceAccount.LocalSystem;

            serviceInstaller = new ServiceInstaller();

            serviceProcessInstaller.Password = null;
            serviceProcessInstaller.Username = null;

            serviceInstaller.ServiceName = DiskAPMService.DiskAPMServiceName;
            serviceInstaller.DisplayName = DiskAPMService.DiskAPMServiceName;
            serviceInstaller.Description = DiskAPMService.DiskAPMServiceDescription;

            serviceInstaller.StartType = ServiceStartMode.Automatic;
            serviceInstaller.DelayedAutoStart = true;

            serviceInstaller.AfterInstall += (senger, args) =>
            {
                new ServiceController(serviceInstaller.ServiceName).Start();
            };

            Installers.AddRange(new Installer[] { this.serviceProcessInstaller, this.serviceInstaller });

            InitializeComponent();
        }
    }
}
