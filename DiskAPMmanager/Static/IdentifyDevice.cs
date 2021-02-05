using System;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32.SafeHandles;

using DiskAPMmanager.Structs;
using DiskAPMmanager.Windows;

namespace DiskAPMmanager.Static
{
    public static partial class StaticMethods
    {
        const ushort ATA_FLAGS_DRDY_REQUIRED = 0x01;
        const ushort ATA_FLAGS_DATA_IN = 0x02;

        const byte WIN_IDENTIFYDEVICE = 0xEC;

        public static IDENTIFY_DEVICE_DATA? IdentifyDefice(string deviceName)
        {
            ATA_PASS_THROUGH_EX aptx = new ATA_PASS_THROUGH_EX();
            ATADeviceQuiry adqry = new ATADeviceQuiry();
            IDENTIFY_DEVICE_DATA idd = new IDENTIFY_DEVICE_DATA();

            adqry.reqDataBuf = new byte[512];
            adqry.header.Length = (ushort)Marshal.SizeOf(aptx);
            adqry.header.AtaFlags = ATA_FLAGS_DATA_IN;
            adqry.header.DataTransferLength = (ushort)adqry.reqDataBuf.Length; //512
            adqry.header.DataBufferOffset = Marshal.OffsetOf(typeof(ATADeviceQuiry), "reqDataBuf");
            adqry.header.TimeOutValue = 1;
            adqry.header.PreviousTaskFile = new IDEREGS();
            adqry.header.CurrentTaskFile = new IDEREGS();
            adqry.header.CurrentTaskFile.bCommandReg = WIN_IDENTIFYDEVICE;

            uint IOCTL_ATA_PASS_THROUGH = CTL_CODE(
                IOCTL_SCSI_BASE,
                0x040b,
                METHOD_BUFFERED,
                FILE_READ_ACCESS | FILE_WRITE_ACCESS);

            SafeFileHandle driveHandle = Kernel32Methods.CreateFileW(
                lpFileName: deviceName,
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
                Console.WriteLine($"CreateFile (IdentifyDevice) with disk {deviceName} failed. Error: " + message);
#endif
                driveHandle?.Close();
                return null;
            }

            uint returnBytesCount;

            bool result = Kernel32Methods.DeviceIoControl(
                hDevice: driveHandle,
                dwIoControlCode: IOCTL_ATA_PASS_THROUGH,
                lpInBuffer: ref adqry,
                nInBufferSize: (uint)Marshal.SizeOf(adqry),
                lpOutBuffer: ref adqry,
                nOutBufferSize: (uint)Marshal.SizeOf(adqry),
                lpBytesReturned: out returnBytesCount,
                lpOverlapped: IntPtr.Zero);

            driveHandle.Close();

            if (result == false)
            {
#if DEBUG
                string message = GetErrorMessage(Marshal.GetLastWin32Error());
                Console.WriteLine($"DeviceIoControl (IdentifyDevice) with disk {deviceName} failed. Error: " + message);
#endif
                return null;
            }

            // Raw memory copy of reqDataBuf byte array to IDENTIFY_DEVICE struct

            IntPtr tempPtr = Marshal.AllocHGlobal(Marshal.SizeOf(idd));

            Marshal.Copy(adqry.reqDataBuf, 0, tempPtr, adqry.reqDataBuf.Length);

            idd = (IDENTIFY_DEVICE_DATA)Marshal.PtrToStructure(tempPtr, idd.GetType());

            Marshal.FreeHGlobal(tempPtr);

            return idd;
        }

        // SATA strings byte order is as 2,1,4,3,...
        // Flipping every two bytes to make a normal order
        public static string ATACharsToString(byte[] array)
        {
            for (int i = 0; i < array.Length - 1; i += 2)
            {
                byte temp = array[i];
                array[i] = array[i + 1];
                array[i + 1] = temp;
            }

            return Encoding.ASCII.GetString(array).Trim();
        }
    }
}