using DiskAPMmanager.Structs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace DiskAPMConfig
{
    public class ProgramSettings
    {
        IConfigReadWrite configRW;

        public MainWindow MainWindow { get; }

        public bool AllowAPMdisable { get; set; } = false;

        internal bool IsInRegisterTransitState { get; set; } = false;

        public ICommand ServiceInstall { get; }

        public string Version => typeof(MainWindow).Assembly.GetName().Version.ToString();

        public ProgramSettings(MainWindow mainWindow, IConfigReadWrite configRW)
        {
            this.MainWindow = mainWindow;

            this.configRW = configRW;

            ServiceInstall = new ServiceInstallCommand(this);

        }

        internal void SaveDiskConfig(DiskData disk)
        {
            var disks = configRW.ReadConfigurationFile();

            configRW.WarnIfDiskStatusChange(disk, disks);

            HashSet<DiskData> diskSet = disks as HashSet<DiskData>;

            if (diskSet == null)
            {
                diskSet = new HashSet<DiskData>(disks);
            }

            diskSet.Remove(disk);
            diskSet.Add(disk);

            configRW.WriteConfigurationFile(diskSet);
        }

    }
}