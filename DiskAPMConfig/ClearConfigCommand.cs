using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DiskAPMConfig
{
    public class ClearConfigCommand : ICommand
    {
        private string configPath;

        public ClearConfigCommand(string configPath)
        {
            this.configPath = configPath;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return File.Exists(configPath);
        }

        public void Execute(object parameter)
        {
            try
            {
                File.Delete(configPath);
                string configDirectory = Directory.GetParent(configPath).FullName;
                Directory.Delete(configDirectory); 
            }
            catch (Exception e)
            {
                MessageBox.Show($"Error. Cannot delete config {Environment.NewLine} {e.Message}");
            }

            SignalCanExecuteChanged();
        }

        public void SignalCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
