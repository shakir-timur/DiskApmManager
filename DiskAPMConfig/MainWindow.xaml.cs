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
using DiskAPMmanager.Structs;


namespace DiskAPMConfig
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<DiskData> DiskCollection { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            DiskCollectionInit();

            DataContext = this;
        }

        private void DiskCollectionInit()
        {
            //var diskList = DiskAPMmanager.Static.StaticMethods.GetRotaryDrives();

            //DiskCollection = new ObservableCollection<DiskData>(diskList);

            DiskData dummy1 = new DiskData("Drive Dummy1", "Dummy Model1", "Serial No1", "Status", "500 Gb", 128, true);
            DiskData dummy2 = new DiskData("Drive Dummy2", "Dummy Model2", "Serial No2", "Status", "320 Gb", 254, true);

            DiskCollection = new ObservableCollection<DiskData>();

            DiskCollection.Add(dummy1);
            DiskCollection.Add(dummy2);
        }

        private void APMslider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (APMsettingDescription == null) return;

            int v = (byte)APMslider.Value;

            if (v == 0xFF) { APMsettingDescription.Text = "APM disabled"; return; }
            if (v == 0xFE) { APMsettingDescription.Text = "Maximum performance"; return; }
            if (v >= 0x81) { APMsettingDescription.Text = "Intermediate power management levels without Standby"; return; }
            if (v == 0x80) { APMsettingDescription.Text = "Minimum power consumption without Standby"; return; }
            if (v >= 0x02) { APMsettingDescription.Text = "Intermediate power management levels with Standby"; return; }
            if (v == 0x01) { APMsettingDescription.Text = "Minimum power consumption with Standby"; return; }

            if (v == 0x0) { APMsettingDescription.Text = "Invalid APM value"; return; }
        }

        private void applyAPMbutton_Click(object sender, RoutedEventArgs e)
        {

            byte newAPM = (byte) APMslider.Value;
            DiskData? drive = DiskList.SelectedItem as DiskData?;
            bool successfull = false;

            if (drive.HasValue)
            {
                successfull = DiskAPMmanager.Static.StaticMethods.SetAPM(drive.Value.DeviceName, newAPM);
            }

            if (successfull)
            {
                statusBar.Text = "APM set successfully";
            }
            else
            {
                statusBar.Text = "APM set failed";
            }
        }

        private void disableAPMbutton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void reloadDiskListButton_Click(object sender, RoutedEventArgs e)
        {
            DiskCollectionInit();
        }
    }
}
