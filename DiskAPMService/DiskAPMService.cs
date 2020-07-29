using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace DiskAPMService
{
    public partial class DiskAPMService : ServiceBase
    {
        public const string DiskAPMServiceName = "DiskAPMService";
        public const string DiskAPMServiceDescription = "Applies DiskAPMManager settings for configured disks";

        private DirectoryInfo serviceAssemblyFolder;
        private string logPath;

        public DiskAPMService()
        {
            InitializeComponent();

            string assemblyLocation = Assembly.GetExecutingAssembly().Location;
            serviceAssemblyFolder = new FileInfo(assemblyLocation).Directory;
            logPath = Path.Combine(serviceAssemblyFolder.FullName, "log.txt");
        }

        protected override void OnStart(string[] args)
        {
            File.AppendAllText(logPath ?? "log.txt", $"logged loc-n {logPath} {Environment.NewLine}");
            File.AppendAllText(logPath ?? "log.txt", $"logged start {DateTime.Now} {Environment.NewLine}");
        }

        protected override void OnStop()
        {
            File.AppendAllText(logPath ?? "log.txt", $"logged stop {DateTime.Now} {Environment.NewLine}");
        }

        protected override bool OnPowerEvent(PowerBroadcastStatus powerStatus)
        {
            File.AppendAllText(logPath ?? "log.txt", $"logged PowerEvent {powerStatus} at {DateTime.Now} {Environment.NewLine}");

            return base.OnPowerEvent(powerStatus);
        }
    }
}
