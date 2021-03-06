﻿using DiskAPMmanager.Structs;
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

        public string ConfigPath { get; }

        public DiskDataConfigWriter()
        {
            ConfigDirectoryPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                configFolderName);

            ConfigPath = Path.Combine(ConfigDirectoryPath, configFileName);
        }

        public IEnumerable<DiskData> ReadConfigurationFile()
        {
            HashSet<DiskData> diskSet = new HashSet<DiskData>();

            if (File.Exists(ConfigPath))
            {
                try
                {
                    string config = File.ReadAllText(ConfigPath);

                    using (Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(config)))
                    {
                        DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(DiskData[]));

                        IEnumerable<DiskData> diskData = (DiskData[])serializer.ReadObject(stream);

                        diskSet.UnionWith(diskData);
                    }
                }
                catch (Exception e)
                {

#if !SERVICE
                    MessageBox.Show(e.StackTrace, e.GetType().Name);
#endif
                }
            }

            return diskSet;
        }

        public bool WriteConfigurationFile(IEnumerable<DiskData> disks)
        {
            try
            {
                if (!Directory.Exists(ConfigDirectoryPath))
                {
                    Directory.CreateDirectory(ConfigDirectoryPath);
                }

                DiskData[] diskData = disks.ToArray();

                using (MemoryStream stream = new MemoryStream())
                {
                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(DiskData[]));

                    serializer.WriteObject(stream, diskData);

                    File.WriteAllBytes(ConfigPath, stream.ToArray());
                }
            }
            catch (Exception e)
            {
#if !SERVICE
                MessageBox.Show(e.StackTrace, e.GetType().Name);
#endif
                return false;
            }

            return true;
        }

        public bool WarnIfDiskStatusChange(DiskData diskNew, IEnumerable<DiskData> disks)
        {
            HashSet<DiskData> diskSet = disks as HashSet<DiskData>;

            if (diskSet == null)
            {
                diskSet = new HashSet<DiskData>(disks);
            }

            if (diskSet.Contains(diskNew))
            {
                foreach (var disk in diskSet)
                {
                    if (disk.Equals(diskNew) && diskNew.Status != disk.Status)
                    {
#if !SERVICE
                        string title = "Attention";
                        string tipText =
    $@"DiskAPMManager:
Disk status change detected
from {disk.Status}
to   {diskNew.Status}
on   {diskNew.Model}
s/n: {diskNew.SerialNo}";


                        MessageBox.Show(tipText, title);
#endif
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
