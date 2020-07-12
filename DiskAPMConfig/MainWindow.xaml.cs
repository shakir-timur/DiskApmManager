using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        ObservableCollection<DiskData> diskCollection;

        public MainWindow()
        {
            InitializeComponent();

            //var diskList = DiskAPMmanager.Static.StaticMethods.GetRotaryDrives();

            //diskCollection = new ObservableCollection<DiskData>(diskList);

            DiskData dummy = new DiskData("Drive Dummy", "Dummy Model", "Serial No", "Status", 0, true);

            diskCollection = new ObservableCollection<DiskData>();
            diskCollection.Add(dummy);
        }


    }
}
