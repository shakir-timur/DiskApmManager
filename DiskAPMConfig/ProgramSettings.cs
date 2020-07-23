using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace DiskAPMConfig
{
    public class ProgramSettings 
    {
        IConfigReadWrite configRW;

        MainWindow mainWindow;

        public bool AllowAPMdisable { get; set; } = false;

        public string Version => typeof(MainWindow).Assembly.GetName().Version.ToString();

        public string GitUrl => "https://github.com/shakir-timur/DiskApmManager";

        public ProgramSettings(MainWindow mainWindow, IConfigReadWrite configRW)
        {
            this.mainWindow = mainWindow;

            this.configRW = configRW;
        }

        
    }
}