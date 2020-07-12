using System;
using System.Runtime.InteropServices;


namespace DiskAPMmanager.Structs
{
    // https://docs.microsoft.com/en-us/windows-hardware/drivers/ddi/content/ntdddisk/ns-ntdddisk-_ideregs
    [StructLayout(LayoutKind.Sequential)]
    struct IDEREGS
    {
        // Features / Error
        public byte bFeaturesReg;
        public byte bSectorCountReg;
        public byte bSectorNumberReg;
        public byte bCylLowReg;
        public byte bCylHighReg;
        public byte bDriveHeadReg;
        // Command / Status
        public byte bCommandReg;
        public byte bReserved;
    }


    [StructLayout(LayoutKind.Sequential)]
    struct ATA_PASS_THROUGH_EX                                  // https://docs.microsoft.com/en-us/windows-hardware/drivers/ddi/content/ntddscsi/ns-ntddscsi-_ata_pass_through_ex
    {                                                           //   struct _ATA_PASS_THROUGH_EX 
        public ushort Length;                                   //   USHORT    Length;
        public ushort AtaFlags;                                 //   USHORT    AtaFlags;
        public byte PathId;                                     //   UCHAR     PathId;
        public byte TargetId;                                   //   UCHAR     TargetId;
        public byte Lun;                                        //   UCHAR     Lun;
        public byte ReservedAsUchar;                            //   UCHAR     ReservedAsUchar;
        public uint DataTransferLength;                         //   ULONG     DataTransferLength;
        public uint TimeOutValue;                               //   ULONG     TimeOutValue;
        public uint ReservedAsUlong;                            //   ULONG     ReservedAsUlong;
        public IntPtr DataBufferOffset;                         //   ULONG_PTR DataBufferOffset;
        public IDEREGS PreviousTaskFile;                        //   UCHAR     PreviousTaskFile[8];
        public IDEREGS CurrentTaskFile;                         //   UCHAR     CurrentTaskFile[8]; 
    }

    // ATA_PASS_THROUGH_EX with a buffer to store data returned from query

    [StructLayout(LayoutKind.Sequential)]
    struct ATADeviceQuiry
    {
        public ATA_PASS_THROUGH_EX header;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 512)]
        public byte[] reqDataBuf;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct STORAGE_PROPERTY_QUERY
    {
        public uint PropertyId;
        public uint QueryType;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        public byte[] AdditionalParameters;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct DEVICE_SEEK_PENALTY_DESCRIPTOR
    {
        public uint Version;
        public uint Size;

        [MarshalAs(UnmanagedType.U1)]
        public bool IncursSeekPenalty;
    }
}

