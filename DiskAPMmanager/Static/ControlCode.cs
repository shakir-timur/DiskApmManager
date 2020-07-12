using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiskAPMmanager.Static
{
    public static partial class StaticMethods
    {
        public const uint FILE_DEVICE_MASS_STORAGE = 0x0000002d;
        public const uint IOCTL_STORAGE_BASE = FILE_DEVICE_MASS_STORAGE;

        public const uint FILE_DEVICE_CONTROLLER = 0x00000004;
        public const uint IOCTL_SCSI_BASE = FILE_DEVICE_CONTROLLER;

        public const uint METHOD_BUFFERED = 0;

        public const uint FILE_ANY_ACCESS = 0;
        public const uint FILE_READ_ACCESS = 0x00000001;
        public const uint FILE_WRITE_ACCESS = 0x00000002;


        public static uint CTL_CODE(uint DeviceType, uint Function, uint Method, uint Access)
        {
            return ((DeviceType << 16)  | 
                    (Access     << 14)  |
                    (Function   << 2)   | 
                     Method);
        }

        // Probably can be hardcoded as IOCTL_ATA_PASS_THROUGH = 0x4d02c (http://www.ioctls.net/)
    }
}
