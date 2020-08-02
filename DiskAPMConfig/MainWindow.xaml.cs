using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Security.Principal;

using DiskAPMmanager.Structs;
using System.Runtime.CompilerServices;

namespace DiskAPMConfig
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public ObservableCollection<DiskData> DiskCollection { get; set; }

        private readonly byte DefaultAPMvalue = 128;
        private readonly byte DisableAPMValue = DiskAPMmanager.Static.StaticMethods.DISABLE_APM_VALUE;

        public ProgramSettings Settings { get; private set; }

        private string statusBarText = "Status";
        public string StatusBarText
        {
            get => statusBarText;
            set
            {
                statusBarText = value;
                NotifyPropertyChanged(nameof(StatusBarText));
            }
        }

        public MainWindow()
        {
            InitializeComponent();

            DiskCollectionInit();

            DataContext = this;

            Settings = new ProgramSettings(this, new DiskDataConfigWriter());

            NotifyPropertyChanged(nameof(Settings));

            CheckAdministratorPriviledges();
        }

        private DiskData? selectedDrive => DiskList.Items.CurrentItem as DiskData?;

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void DiskCollectionInit()
        {
            var diskList = DiskAPMmanager.Static.StaticMethods.GetAPMRotaryDrives();

            DiskCollection = new ObservableCollection<DiskData>(diskList);

            //DiskData dummy1 = new DiskData("Drive Dummy1", "Dummy Model1", "Serial No1", "Status", "500 Gb", 0, false);
            //DiskData dummy2 = new DiskData("Drive Dummy2", "Dummy Model2", "Serial No2", "Status", "320 Gb", 254, true);
            //DiskCollection.Add(dummy1);
            //DiskCollection.Add(dummy2);

            NotifyPropertyChanged(nameof(DiskCollection));
        }

        private void APMslider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (APMsettingDescription == null) return;

            if (selectedDrive.HasValue && !selectedDrive.Value.APMenabled)
            {
                APMsettingDescription.Text = "APM disabled";
                return;
            }

            int v = (byte)APMslider.Value;

            if (v == 0xFE) { APMsettingDescription.Text = "Maximum performance"; return; }
            if (v >= 0x81) { APMsettingDescription.Text = "Intermediate power management levels without Standby"; return; }
            if (v == 0x80) { APMsettingDescription.Text = "Minimum power consumption without Standby"; return; }
            if (v >= 0x02) { APMsettingDescription.Text = "Intermediate power management levels with Standby"; return; }
            if (v == 0x01) { APMsettingDescription.Text = "Minimum power consumption with Standby"; return; }

            if (v == 0x0) { APMsettingDescription.Text = "Invalid APM value"; return; }
        }

        private void applyAPMbutton_Click(object sender, RoutedEventArgs e)
        {
            DiskData? drive = selectedDrive;

            if (!drive.HasValue) return;

            byte newAPM = (byte)APMslider.Value;

            ApplyAPM(newAPM, drive.Value);
        }

        private void enableOrDisableAPMbutton_Click(object sender, RoutedEventArgs e)
        {
            DiskData? drive = selectedDrive;

            if (!drive.HasValue) return;

            if (drive.Value.APMenabled)
            {
                byte newAPM = DisableAPMValue;

                ApplyAPM(newAPM, drive.Value);
            }
            else
            {
                byte newAPM = DefaultAPMvalue;

                ApplyAPM(newAPM, drive.Value);
            }

            APMslider_ValueChanged(null, null);
        }

        private void ApplyAPM(byte APMvalue, DiskData drive)
        {
            bool successfull = DiskAPMmanager.Static.StaticMethods.SetAPM(drive.DeviceName, APMvalue);

            if (successfull)
            {
                StatusBarText = "APM set successfully";
            }
            else
            {
                StatusBarText = "APM set failed";
            }

            int selectedIndex = DiskList.SelectedIndex;

            DiskCollectionInit();

            DiskList.SelectedIndex = selectedIndex;

            APMslider_ValueChanged(null, null);

            if (successfull)
            {
                int index = DiskCollection.IndexOf(drive);
                DiskData newDrive = DiskCollection[index];
                Settings.SaveDiskConfig(newDrive);
            }

        }

        private void CheckAdministratorPriviledges()
        {
            var identity = WindowsIdentity.GetCurrent();
            var principal = new WindowsPrincipal(identity);
            bool isAdmin = principal.IsInRole(WindowsBuiltInRole.Administrator);

            if (!isAdmin)
                StatusBarText = "This program requires Administrator privileges";
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }


    }
}
