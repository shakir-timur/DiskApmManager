using DiskAPMmanager.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiskAPMConfig
{
    public interface IConfigReadWrite
    {
        string ConfigPath { get; }

        IEnumerable<DiskData> ReadConfigurationFile();

        bool WriteConfigurationFile(IEnumerable<DiskData> disks);

        bool WarnIfDiskStatusChange(DiskData disk, IEnumerable<DiskData> diskSet);
    }
}
