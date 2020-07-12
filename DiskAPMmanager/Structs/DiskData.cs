using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiskAPMmanager.Structs
{
    public struct DiskData
    {
        public readonly string DeviceName;
        public readonly string Model;
        public readonly string SerialNo;
        public readonly string Status;
        public readonly byte APMvalue;
        public readonly bool APMenabled;

        public DiskData(string DeviceName, string Model, string SerialNo, string Status, byte APMvalue, bool APMenabled)
        {
            this.DeviceName = DeviceName;
            this.Model = Model;
            this.SerialNo = SerialNo;
            this.Status = Status;
            this.APMvalue = APMvalue;
            this.APMenabled = APMenabled;
        }

        public DiskData(string DeviceName, string Status)
        {
            this.DeviceName = DeviceName;
            this.Status = Status;

            this.Model = null;
            this.SerialNo = null;
            this.APMvalue = 0;
            this.APMenabled = false;
        }
    }


}
