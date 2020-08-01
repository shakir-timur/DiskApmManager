using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiskAPMmanager.Structs
{
    [Serializable]
    public struct DiskData
    {
        private readonly string deviceName;
        private readonly string model;
        private readonly string serialNo;
        private readonly string status;
        private readonly string size;
        private readonly byte apmValue;
        private readonly bool apmEnabled;

        public string DeviceName => deviceName;
        public string Model => model;
        public string SerialNo => serialNo;
        public string Status => status;
        public string Size => size;
        public byte APMvalue => apmValue;
        public bool APMenabled => apmEnabled;

        public DiskData(string DeviceName, string Model, string SerialNo, string Status, string Size, byte APMvalue, bool APMenabled)
        {
            deviceName = DeviceName;
            model = Model;
            serialNo = SerialNo;
            status = Status;
            size = Size;
            apmValue = APMvalue;
            apmEnabled = APMenabled;
        }

        public DiskData(string DeviceName, string Status, string Size)
        {
            deviceName = DeviceName;
            status = Status;
            size = Size;

            model = null;
            serialNo = null;
            apmValue = 0;
            apmEnabled = false;
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
