using DiskAPMmanager.Structs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DiskAPMConfig
{
    public class DiskDataConfigWriter : IConfigReadWrite
    {
        const string configFolderName = "DiskAMPManager";
        const string configFileName = "DiskDataConfig.json";

        readonly string ConfigDirectoryPath;

        readonly string ConfigFilePath;

        public DiskDataConfigWriter()
        {
            ConfigDirectoryPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
            configFolderName);

            ConfigFilePath = Path.Combine(ConfigDirectoryPath, configFileName);
        }

        public IEnumerable<DiskData> ReadConfigurationFile()
        {
            HashSet<DiskData> diskSet = new HashSet<DiskData>();

            if (File.Exists(ConfigFilePath))
            {
                try
                {
                    string config = File.ReadAllText(ConfigFilePath);

                    using (Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(config)))
                    {
                        DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(DiskData[]));

                        IEnumerable<DiskData> diskDatas = (DiskData[])serializer.ReadObject(stream);

                        diskSet.UnionWith(diskDatas);
                    }
                }
                catch (Exception e)
                {
                    System.Windows.MessageBox.Show(e.StackTrace, e.GetType().Name);
                    throw;
                }
            }

            return diskSet;
        }

        public bool WriteConfigurationFile(IEnumerable<DiskData> disks)
        {
            if (!Directory.Exists(ConfigDirectoryPath))
            {
                Directory.CreateDirectory(ConfigDirectoryPath);
            }

            try
            {
                DiskData[] diskDatas = disks.ToArray();

                using (MemoryStream stream = new MemoryStream())
                {
                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(DiskData[]));

                    serializer.WriteObject(stream, diskDatas);

                    File.WriteAllBytes(ConfigFilePath, stream.ToArray());
                }
            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show(e.StackTrace, e.GetType().Name);
                throw;
            }
            

            return true;
        }

        public bool WarnIfDiskStatusChange(DiskData disk, IEnumerable<DiskData> disks)
        {
            HashSet<DiskData> diskSet = disks as HashSet<DiskData>;

            if (diskSet == null)
            {
                diskSet = new HashSet<DiskData>(disks);
            }

            if (diskSet.TryGetValue(disk, out DiskData oldDiskValue))
            {
                if(disk.Status != oldDiskValue.Status)
                {
                    
                    string title = "Attention";
                    string tipText =
$@"DiskAPMManager:
Disk status change detected
from {oldDiskValue.Status}
to   {disk.Status}
on   {disk.Model}
s/n: {disk.SerialNo}";

                    MessageBox.Show(tipText, title);

                    return true;
                }
            }

            return false;
        }
    }
}
