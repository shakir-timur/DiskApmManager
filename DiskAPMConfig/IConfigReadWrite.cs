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
        IEnumerable<DiskData> ReadConfigurationFile();

        bool WriteConfigurationFile(IEnumerable<DiskData> disks);
    }
}
