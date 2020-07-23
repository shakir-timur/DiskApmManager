using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiskAPMmanager.Structs
{
    public struct DiskData
    {
        public string DeviceName { get; }
        public string Model { get; }
        public string SerialNo { get; }
        public string Status { get; }
        public string Size { get; }
        public byte APMvalue { get; }
        public bool APMenabled { get; }

        public DiskData(string DeviceName, string Model, string SerialNo, string Status, string Size, byte APMvalue, bool APMenabled)
        {
            this.DeviceName = DeviceName;
            this.Model = Model;
            this.SerialNo = SerialNo;
            this.Status = Status;
            this.Size = Size;
            this.APMvalue = APMvalue;
            this.APMenabled = APMenabled;
        }

        public DiskData(string DeviceName, string Status, string Size)
        {
            this.DeviceName = DeviceName;
            this.Status = Status;
            this.Size = Size;

            this.Model = null;
            this.SerialNo = null;
            this.APMvalue = 0;
            this.APMenabled = false;
        }

        public override bool Equals(object obj)
        {
            return obj is DiskData data &&
                   Model == data.Model &&
                   SerialNo == data.SerialNo;
        }

        public override int GetHashCode()
        {
            int hashCode = -496536840;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Model);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(SerialNo);
            return hashCode;
        }
    }


}
