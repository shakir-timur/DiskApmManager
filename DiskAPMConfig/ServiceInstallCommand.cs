using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Windows;
using System.Windows.Input;
using DiskAPMService;

namespace DiskAPMConfig
{
    public class ServiceInstallCommand : ICommand
    {
        protected ProgramSettings programSettings;

        public event EventHandler CanExecuteChanged;

        readonly string netPath = System.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeDirectory();
        readonly string instUtilName = "InstallUtil.exe";
        readonly string serviceAppName = "DiskAPMService.exe";

        public enum InstallType
        {
            Install,
            Uninstall
        }

        public ServiceInstallCommand(ProgramSettings programSettings)
        {
            this.programSettings = programSettings;
        }

        public bool CanExecute(object parameter)
        {
            bool isEnum = parameter is InstallType;

            if (!isEnum) throw new ArgumentException("Command parameter must be of InstallType.");

            if (programSettings.IsInRegisterTransitState) return false;

            InstallType type = (InstallType)parameter;

            bool serviceInstalled = ServiceController.GetServices()
                 .Any(service => service.ServiceName == DiskAPMService.DiskAPMService.DiskAPMServiceName);

            switch (type)
            {
                case InstallType.Install:
                    return !serviceInstalled;
                case InstallType.Uninstall:
                    return serviceInstalled;
                default:
                    throw new InvalidOperationException("Switch default case in CanExecute");
            }

        }

        public void Execute(object parameter)
        {
            bool isEnum = parameter is InstallType;

            if (!isEnum) throw new ArgumentException("Command parameter must be of InstallType.");

            InstallType type = (InstallType)parameter;

            bool success = LaunchInstallUtil(type);

            string message;

            switch (type)
            {
                case InstallType.Install:
                    message = success ? "Service installed." : "Service installation failed";
                    break;
                case InstallType.Uninstall:
                    message = success ? "Service uninstalled." : "Service uninstallation failed";
                    break;
                default:
                    throw new InvalidOperationException("Switch default case in Execute");
            }

            programSettings.MainWindow.StatusBarText = message;
        }

        protected bool LaunchInstallUtil(InstallType installType)
        {
            string utilPath = Path.Combine(netPath, instUtilName);
            string configAppPath = Assembly.GetExecutingAssembly().Location;

            var serviceAssemblyFolder = new FileInfo(configAppPath).Directory;
            string serviceAppPath = Path.Combine(serviceAssemblyFolder.FullName, serviceAppName);

            if (!File.Exists(serviceAppPath))
            {
                MessageBox.Show($"Error. {serviceAppName} not found");
                return false;
            }

            if (!File.Exists(utilPath))
            {
                MessageBox.Show($"Error. {instUtilName} not found");
                return false;
            }

            using (Process installProc = new Process())
            {
                string args = serviceAppPath;

                if (installType == InstallType.Uninstall) args = "/u " + args;

                ProcessStartInfo startInfo = new ProcessStartInfo(utilPath, args)
                {
                    UseShellExecute = false
                };

                installProc.StartInfo = startInfo;

                installProc.Start();

                installProc.WaitForExit();

                CanExecuteChanged?.Invoke(this, EventArgs.Empty);

                return installProc.ExitCode == 0;
            }

        }
    }
}