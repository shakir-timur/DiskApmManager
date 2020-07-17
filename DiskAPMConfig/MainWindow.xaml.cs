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


        public MainWindow()
        {
            InitializeComponent();

            //var diskList = DiskAPMmanager.Static.StaticMethods.GetRotaryDrives();

            //DiskCollection = new ObservableCollection<DiskData>(diskList);
            
            DiskData dummy1 = new DiskData("Drive Dummy1", "Dummy Model1", "Serial No1", "Status", "Size1", 0, true);
            DiskData dummy2 = new DiskData("Drive Dummy2", "Dummy Model2", "Serial No2", "Status", "Size2", 0, true);

            DiskCollection = new ObservableCollection<DiskData>();
            DiskCollection.Add(dummy1);
            DiskCollection.Add(dummy2);

            DataContext = this;
        }

        public ObservableCollection<DiskData> DiskCollection { get; set; }
    }
}
