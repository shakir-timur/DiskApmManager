using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;


namespace DiskAPMConfig
{
    class RegisterServiceCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var p = System.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeDirectory();

            MessageBox.Show($"Register service! \n {p}");

            Process.Start("services.msc");
        }
    }
}
