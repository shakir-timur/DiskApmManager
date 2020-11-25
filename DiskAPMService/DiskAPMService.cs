#define SERVICE

using DiskAPMmanager.Structs;
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

        DiskAPMConfig.IConfigReadWrite configReadWrite;

        private DirectoryInfo serviceAssemblyFolder;
        private string logPath;

        public DiskAPMService()
        {
            InitializeComponent();

            configReadWrite = new DiskAPMConfig.DiskDataConfigWriter();

            string assemblyLocation = Assembly.GetExecutingAssembly().Location;
            serviceAssemblyFolder = new FileInfo(assemblyLocation).Directory;
            logPath = Path.Combine(serviceAssemblyFolder.FullName, "log.txt");

            Log($"Service init at {DateTime.Now}");
        }

        protected override void OnStart(string[] args)
        {
            Log($"Service start at {DateTime.Now}");

            ApplyConfiguraton();
        }

        protected override void OnStop()
        {
            Log($"Service stop at {DateTime.Now}");

            base.OnStop();
        }

        protected override bool OnPowerEvent(PowerBroadcastStatus powerStatus)
        {
            if (powerStatus == PowerBroadcastStatus.ResumeAutomatic) // || powerStatus == PowerBroadcastStatus.ResumeSuspend)
            {
                Log($"Service powerEvent {powerStatus} at {DateTime.Now}");

                ApplyConfiguraton();
            }

            return base.OnPowerEvent(powerStatus);
        }

        private void ApplyConfiguraton()
        {
            Log($"Service apply config start at {DateTime.Now}");

            var detectedDisks = DiskAPMmanager.Static.StaticMethods.GetAPMRotaryDrives();

            var savedConfig = configReadWrite.ReadConfigurationFile();

            HashSet<DiskData> savedDisksSet = (savedConfig as HashSet<DiskData>) ?? new HashSet<DiskData>(savedConfig);

            foreach (var detectedDisk in detectedDisks)
            {
                if (savedDisksSet.Contains(detectedDisk))
                {
                    DiskData savedDisk = savedDisksSet.Where(d => d.Equals(detectedDisk)).Single();

                    bool apmEnabled = savedDisk.APMenabled;

                    byte newApm = apmEnabled ?
                        savedDisk.APMvalue :
                        DiskAPMmanager.Static.StaticMethods.DISABLE_APM_VALUE;

                    Log($"Service set APM {newApm} on {detectedDisk.DeviceName} at {DateTime.Now}");

                    DiskAPMmanager.Static.StaticMethods.SetAPM(detectedDisk.DeviceName, newApm);

                    if (configReadWrite.WarnIfDiskStatusChange(detectedDisk, savedDisksSet))
                    {
                        Log($"Service disk status change on {detectedDisk.DeviceName} at {DateTime.Now}");

                        savedDisksSet.Remove(detectedDisk);
                        savedDisksSet.Add(detectedDisk);

                        configReadWrite.WriteConfigurationFile(savedDisksSet);
                    }
                }
            }
        }

        [Conditional("DEBUG")]
        void Log(string message)
        {
            File.AppendAllText(logPath, message + Environment.NewLine);
        }
    }
}
