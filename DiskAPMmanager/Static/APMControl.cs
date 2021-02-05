using System;
using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;

using DiskAPMmanager.Structs;
using DiskAPMmanager.Windows;

namespace DiskAPMmanager.Static
{

    public static partial class StaticMethods
    {
        /*
        * SET FEATURES - EFh
        *
        * Feature field definitions
        * 05h Enable the APM feature set 
        * 85h Disable the APM feature set 
        * Subcommand code 05h allows the host to enable APM.  To enable APM, the host writes the Count field with the 
        * desired APM level and then executes a SET FEATURES command with subcommand code 05h.The APM level 
        * is a scale from the lowest power consumption setting of 01h to the maximum performance level of FEh.
        * 
        * APM levels:
        * Count     Level
        * 00h       Reserved
        * 01h       Minimum power consumption with Standby
        * 02h-7Fh   Intermediate power management levels with Standby
        * 80h       Minimum power consumption without Standby
        * 81h-FDh   Intermediate power management levels without Standby
        * FEh       Maximum performance
        * FFh       Reserved
        */

        const byte SET_FEATURES = 0xEF;

        const byte SETFEATURES_DIS_APM = 0x85;
        const byte SETFEATURES_EN_APM = 0x05;

        // Program uses this value for disabling APM, though technically it is reserved
        public const byte DISABLE_APM_VALUE = 255; 

        public static bool SetAPM(string driveName, byte apmVal)
        {
            ATA_PASS_THROUGH_EX aptx = new ATA_PASS_THROUGH_EX();
            ATADeviceQuiry adq = new ATADeviceQuiry();

            adq.reqDataBuf = new byte[512];

            aptx.Length = (ushort)Marshal.SizeOf(aptx); ;
            aptx.AtaFlags = ATA_FLAGS_DATA_IN;
            aptx.DataTransferLength = (ushort)adq.reqDataBuf.Length; //512;                    
            aptx.TimeOutValue = 1;
            aptx.DataBufferOffset = Marshal.OffsetOf(typeof(ATADeviceQuiry), "reqDataBuf");
            aptx.PreviousTaskFile = new IDEREGS();
            aptx.CurrentTaskFile = new IDEREGS();
            aptx.CurrentTaskFile.bCommandReg = SET_FEATURES;

            switch (apmVal)
            {
                case 0:
                    {
                        throw new ArgumentException("APM value cannot be 0");
                    }
                // Disable APM
                case DISABLE_APM_VALUE:
                    {
                        aptx.CurrentTaskFile.bFeaturesReg = SETFEATURES_DIS_APM;
                        break;
                    }
                // Set APM to value
                default:
                    {
                        aptx.CurrentTaskFile.bFeaturesReg = SETFEATURES_EN_APM;
                        aptx.CurrentTaskFile.bSectorCountReg = apmVal;
                        break;
                    }
            }

            adq.header = aptx;

            uint IOCTL_ATA_PASS_THROUGH = CTL_CODE(
                IOCTL_SCSI_BASE,
                0x040b, 
                METHOD_BUFFERED,
                FILE_READ_ACCESS | FILE_WRITE_ACCESS);

            SafeFileHandle driveHandle = Kernel32Methods.CreateFileW(
                lpFileName: driveName,
                dwDesiredAccess: Kernel32Methods.GENERIC_READ | Kernel32Methods.GENERIC_WRITE, // Administrative privilege is required
                dwShareMode: Kernel32Methods.FILE_SHARE_READ | Kernel32Methods.FILE_SHARE_WRITE,
                lpSecurityAttributes: IntPtr.Zero,
                dwCreationDisposition: Kernel32Methods.OPEN_EXISTING,
                dwFlagsAndAttributes: Kernel32Methods.FILE_ATTRIBUTE_NORMAL,
                hTemplateFile: IntPtr.Zero);

            if (driveHandle == null || driveHandle.IsInvalid)
            {
#if DEBUG
                string message = GetErrorMessage(Marshal.GetLastWin32Error());
                Console.WriteLine($"CreateFile (APMControl) with disk {driveName} failed. Error: " + message);
#endif
                driveHandle?.Close();
                return false;
            }

            uint returnBytesCount;

            bool result = Kernel32Methods.DeviceIoControl(
                hDevice: driveHandle,
                dwIoControlCode: IOCTL_ATA_PASS_THROUGH,
                lpInBuffer: ref adq,
                nInBufferSize: (uint)Marshal.SizeOf(adq),
                lpOutBuffer: ref adq,
                nOutBufferSize: (uint)Marshal.SizeOf(adq),
                lpBytesReturned: out returnBytesCount,
                lpOverlapped: IntPtr.Zero);

            driveHandle.Close();

            if (result == false)
            {
#if DEBUG
                string message = GetErrorMessage(Marshal.GetLastWin32Error());
                Console.WriteLine($"DeviceIoControl (APMControl) with disk {driveName} failed. Error: " + message);
#endif
                return false;
            }

            return result;
        }
    }
}
