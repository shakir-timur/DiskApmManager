using DiskAPMmanager.Structs;
using System;
using System.Collections.Generic;
using System.Management;


namespace DiskAPMmanager.Windows
{
    static internal class WMIMethods
    {

        public static List<DiskData> GetPhysicalDiskNames()
        {
            List<DiskData> result = new List<DiskData>();

            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive"); // 

            ManagementObjectCollection myDiskCollection = searcher.Get();

            foreach (ManagementObject mo in myDiskCollection)
            {
                string name = mo.GetPropertyValue("Name").ToString();

                string status = mo.GetPropertyValue("Status").ToString();

                DiskData ns = new DiskData(
                    DeviceName: name,
                    Status: status
                );

                result.Add(ns);

                // mo.GetPropertyValue("Model").ToString();

                // mo.GetPropertyValue("SerialNumber").ToString();

                // mo.GetPropertyValue("Status").ToString();
            }

            return result;
        }
    }
}
