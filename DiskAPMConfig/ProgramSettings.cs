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

        public ProgramSettings(MainWindow mainWindow, IConfigReadWrite configRW)
        {
            this.mainWindow = mainWindow;

            this.configRW = configRW;

            
        }

        
    }
}