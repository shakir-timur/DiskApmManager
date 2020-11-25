using System;
using System.Linq;
using System.Collections.Generic;

using DiskAPMmanager.Static;
using DiskAPMmanager.Structs;

namespace DiskAPMmanager
{
    class Program
    {
        // return values:
        // 0 - no errors
        // 1 - invalid input
        // 2 - error while setting APM
        static int Main(string[] args)
        {

            if (args.Length == 2 &&
                int.TryParse(args[0], out int diskNo) &&
                byte.TryParse(args[1], out byte apmVal))
            {
                if (apmVal < 1)
                {
                    Console.WriteLine("APM value must be 1-255");
                    return 1;
                }

                if (diskNo < 0)
                {
                    Console.WriteLine("Disk # cannot be less than 0");
                    return 1;
                }

                string drName = "\\\\.\\PHYSICALDRIVE" + diskNo;

                List<DiskData> driveNames = StaticMethods.GetAPMRotaryDrives();

                if (!driveNames.Any(dd => dd.DeviceName == drName))
                {
                    Console.WriteLine("Eneterd drive number does not appear to be valid");
                    return 1;
                }

                bool result = StaticMethods.SetAPM(drName, apmVal);

                if (result)
                {
                    Console.WriteLine("APM set successfully");
                    Console.WriteLine();
                    PrintDiskStatus();

                    return 0;
                }
                else
                {
                    Console.WriteLine("APM set failed");
                    Console.WriteLine("Error: " + StaticMethods.LastError);

                    return 2;
                }
            }
            else
            {
                PrintUsage();
            }

            // Console.WriteLine("\nPress any key");
            // Console.ReadKey();

            return 0;
        }

        static void PrintUsage()
        {
            string usageDescription =
@"
Usage:
DiskAPMmanager.exe <drive number> <APM value>

<drive number> is the order number of the physical drive in system.
<APM value> is a number from 1 to 255.
APM range from 1 to 254 enables and sets the disk's APM value to this number. 
Seting APM value to 255 disables the APM on the drive.

For example, the command:
DiskAPMmanager.exe 0 254
will set PHYSICALDRIVE0 APM value to 254.

Suitable disks found on your machine (PHYSICALDRIVE0 is drive number 0):
";

            Console.WriteLine(usageDescription);

            PrintDiskStatus();
        }

        static void PrintDiskStatus()
        {
            var drivesList = StaticMethods.GetAPMRotaryDrives();

            var dummyDd = new DiskData("Drive Name", "Model", "Serial No", "Status", "Size", 0, true);

            drivesList.Add(dummyDd);

            int nameLength = drivesList.Select(dd => dd.DeviceName.Trim(' ', '.', '\\')).Max(str => str.Length) + 2;
            int modelLength = drivesList.Select(dd => dd.Model).Max(str => str.Length) + 2;
            int serNoLength = drivesList.Select(dd => dd.SerialNo).Max(str => str.Length) + 2;
            int statusLength = drivesList.Select(dd => dd.Status).Max(str => str.Length) + 2;
            int sizeLength = drivesList.Select(dd => dd.Size).Max(str => str.Length) + 2;
            int apmEnLength = Math.Max("APM on".Length, drivesList.Select(dd => dd.APMenabled).Max(str => str.ToString().Length)) + 2;
            int apmValLength = Math.Max("APM".Length, drivesList.Select(dd => dd.APMvalue).Max(str => str.ToString().Length)) + 2;

            drivesList.RemoveAt(drivesList.Count - 1);

            Console.Write("Drive Name".PadLeft(nameLength));
            Console.Write("Model".PadLeft(modelLength));
            Console.Write("Size".PadLeft(sizeLength));
            Console.Write("Serial No".PadLeft(serNoLength));
            Console.Write("APM on".PadLeft(apmEnLength));
            Console.Write("APM".PadLeft(apmValLength));
            Console.Write("Status".PadLeft(statusLength));

            Console.WriteLine();

            foreach (DiskData dd in drivesList)
            {
                Console.Write(dd.DeviceName.Trim(' ', '.', '\\').PadLeft(nameLength));
                Console.Write(dd.Model.PadLeft(modelLength));
                Console.Write(dd.Size.PadLeft(sizeLength));
                Console.Write(dd.SerialNo.PadLeft(serNoLength));
                Console.Write(dd.APMenabled.ToString().PadLeft(apmEnLength));
                Console.Write(dd.APMvalue.ToString().PadLeft(apmValLength));
                Console.Write(dd.Status.PadLeft(statusLength));

                Console.WriteLine();
            }
        }

    }
}
