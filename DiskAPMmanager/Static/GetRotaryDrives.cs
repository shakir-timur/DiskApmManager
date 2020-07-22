using System;
using System.Collections.Generic;

using DiskAPMmanager.Structs;
using DiskAPMmanager.Windows;

namespace DiskAPMmanager.Static
{
    public static partial class StaticMethods
    {
        public static List<DiskData> GetAPMRotaryDrives()
        {
            List<DiskData> disksNames = WMIMethods.GetPhysicalDiskNames();

            List<DiskData> result = new List<DiskData>();

            foreach (DiskData dd in disksNames)
            {
                IDENTIFY_DEVICE_DATA? idd = IdentifyDefice(dd.DeviceName);

                if (idd == null) continue;

                bool? b = IsRotativeDevice(idd.Value);

                if (b.HasValue && b.Value == true && APMSupported(idd.Value))
                {
                    DiskData ndd = new DiskData(
                        DeviceName: dd.DeviceName,
                        Model: ATACharsToString(idd.Value.model).Trim(),
                        SerialNo: ATACharsToString(idd.Value.serial_no).Trim(),
                        APMenabled: APMEnabled(idd.Value),
                        APMvalue: idd.Value.CurrentAPMvalue,
                        Status: dd.Status,
                        Size: dd.Size
                    );

                    result.Add(ndd);
                }
            }

            return result;
        }
    }
}
