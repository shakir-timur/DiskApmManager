using DiskAPMmanager.Structs;

namespace DiskAPMmanager.Static
{
    public static partial class StaticMethods
    {
        public static bool APMSupported(IDENTIFY_DEVICE_DATA idData)
        {
            // APM support is represented by 3-rd bit (zero based) of word 83

            int s = idData.Feature_set_support_083 & 0x08;

            return s == 0x08;
        }

        public static bool APMEnabled(IDENTIFY_DEVICE_DATA idData)
        {
            // APM state is represented by 3-rd bit of word 86

            int s = idData.Feature_set_enabled_086 & 0x08;

            return s == 0x08;
        }

        public static ushort CurrentAPM(IDENTIFY_DEVICE_DATA idData)
        {
            return idData.CurrentAPMvalue;
        }

        public static bool? IsRotativeDevice(IDENTIFY_DEVICE_DATA idData)
        {
            //0401h-FFFEh for rotative, 1 for Solid state

            if (idData.NomMediaRotRate_217 == 1)
                return false;

            if (idData.NomMediaRotRate_217 >= 0x0401 &&
                idData.NomMediaRotRate_217 <= 0xFFFE)
                return true;

            return null;
        }
    }
}
