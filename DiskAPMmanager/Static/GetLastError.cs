using DiskAPMmanager.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiskAPMmanager.Static
{
    public static partial class StaticMethods
    {
        public static string LastError { get; private set; } = "";

        private static string GetErrorMessage(int code)
        {
            uint FORMAT_MESSAGE_FROM_SYSTEM = 0x00001000;

            StringBuilder message = new StringBuilder(255);

            Kernel32Methods.FormatMessage(
              FORMAT_MESSAGE_FROM_SYSTEM,
              IntPtr.Zero,
              (uint)code,
              0,
              message,
              (uint)message.Capacity,
              IntPtr.Zero);

            LastError = message.ToString();

            return LastError;
        }
    }
}
