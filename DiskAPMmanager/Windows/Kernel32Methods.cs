using Microsoft.Win32.SafeHandles;
using System;
using System.Runtime.InteropServices;
using DiskAPMmanager.Structs;
using System.Text;

namespace DiskAPMmanager.Windows
{
    static class Kernel32Methods
    {
        
        public const uint GENERIC_READ = 0x80000000;
        public const uint GENERIC_WRITE = 0x40000000;
        public const uint FILE_SHARE_READ = 0x00000001;
        public const uint FILE_SHARE_WRITE = 0x00000002;
        public const uint OPEN_EXISTING = 3;
        public const uint FILE_ATTRIBUTE_NORMAL = 0x00000080;

        // CreateFile to get handle to drive
        // dwDesiredAccess parameter can be zero, allowing the application to query device attributes without accessing a device
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern SafeFileHandle CreateFileW(
            [MarshalAs(UnmanagedType.LPWStr)]
            string  lpFileName,
            uint    dwDesiredAccess,
            uint    dwShareMode,
            IntPtr  lpSecurityAttributes,
            uint    dwCreationDisposition,
            uint    dwFlagsAndAttributes,
            IntPtr  hTemplateFile);


        [DllImport("kernel32.dll", EntryPoint = "DeviceIoControl", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeviceIoControl(
            SafeFileHandle hDevice,
            uint dwIoControlCode,
            ref ATADeviceQuiry lpInBuffer,
            uint nInBufferSize,
            ref ATADeviceQuiry lpOutBuffer,
            uint nOutBufferSize,
            out uint lpBytesReturned,
            IntPtr lpOverlapped);

        // https://docs.microsoft.com/en-us/windows/desktop/api/winbase/nf-winbase-formatmessage
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern uint FormatMessage(
            uint dwFlags,
            IntPtr lpSource,
            uint dwMessageId,
            uint dwLanguageId,
            StringBuilder lpBuffer,
            uint nSize,
            IntPtr Arguments);
    }
}

/* 
 * REFERENCES:
 * 
 * #define IOCTL_ATA_PASS_THROUGH CTL_CODE(IOCTL_SCSI_BASE, 0x040b, METHOD_BUFFERED, FILE_READ_ACCESS | FILE_WRITE_ACCESS)
 * 
 * 
 * // The system defines the following device type values
 * #define FILE_DEVICE_MASS_STORAGE        0x0000002d
 * #define FILE_DEVICE_CONTROLLER          0x00000004
 * 
 * #define IOCTL_SCSI_BASE          FILE_DEVICE_CONTROLLER
 * #define IOCTL_STORAGE_BASE       FILE_DEVICE_MASS_STORAGE
 * 
 * #define CTL_CODE( DeviceType, Function, Method, Access ) ((DWORD)((DeviceType) << 16) | ((Access) << 14) | ((Function) << 2) | (Method))
 * 
 * 
 * // Define the method codes for how buffers are passed for I/O and FS controls
 * #define METHOD_BUFFERED                 0
 * #define METHOD_IN_DIRECT                1
 * #define METHOD_OUT_DIRECT               2
 * #define METHOD_NEITHER                  3
 * 
 * 
 * // Define the access check value for any access
 * #define FILE_ANY_ACCESS   0
 * #define FILE_READ_ACCESS  ( 0x0001 ) // file & pipe
 * #define FILE_WRITE_ACCESS ( 0x0002 ) // file & pipe
 * 
 * 
 * #define ATA_FLAGS_DATA_IN    (1 << 1)     Read data from the device. 
 * #define ATA_FLAGS_DATA_OUT   (1 << 2)     Write data to the device. 
 * 
 */
