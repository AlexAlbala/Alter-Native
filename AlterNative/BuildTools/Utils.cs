using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace AlterNative.BuildTools
{
    class Utils
    {
        public static void WriteToConsole(string message)
        {
            AttachConsole(-1);
            Console.WriteLine(message);
        }

        [DllImport("Kernel32.dll")]
        public static extern bool AttachConsole(int processId);
    }
}
