using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace DiskAPMConfig
{
    public class ProgramSettings 
    {
        IConfigReadWrite configRW;

        MainWindow mainWindow;

        public bool AllowAPMdisable { get; set; } = false;

        public ICommand RegisterService { get; } = new RegisterServiceCommand();
        public ICommand UnregisterService { get; } = new UnregisterServiceCommand();

        public string Version => typeof(MainWindow).Assembly.GetName().Version.ToString();

        public ProgramSettings(MainWindow mainWindow, IConfigReadWrite configRW)
        {
            this.mainWindow = mainWindow;

            this.configRW = configRW;
        }

        
    }
}