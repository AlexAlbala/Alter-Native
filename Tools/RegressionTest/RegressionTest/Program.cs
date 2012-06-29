using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace RegressionTest
{
    class Program
    {
        List<DirectoryInfo> Tests = new List<DirectoryInfo>();

        static void Main(string[] args)
        {
            Program p = new Program();
            p.AvailableTests();
            p.Run();
        }

        public void AvailableTests()
        {
            DirectoryInfo d = new DirectoryInfo(Environment.CurrentDirectory);

            foreach (DirectoryInfo di in d.GetDirectories())
            {
                if (ContainsDirectory(di, "Target") &&
                    ContainsDirectory(di, "src") &&
                    /*ContainsDirectory(di, "Output") &&*/
                    ContainsDirectory(di, "bin"))
                {
                    Tests.Add(di);
                    Console.WriteLine("Found test " + di.Name);
                }
            }
        }

        private bool ContainsDirectory(DirectoryInfo directory, string directoryName)
        {
            foreach (DirectoryInfo di in directory.GetDirectories())
            {
                if (di.Name == directoryName)
                    return true;
            }

            return false;
        }

        private void CleanDirectory(DirectoryInfo d)
        {
            if (d.Exists)
            {
                foreach (DirectoryInfo di in d.GetDirectories())
                    CleanDirectory(di);

                foreach (FileInfo fi in d.GetFiles())
                    fi.Delete();
                d.Delete();
            }
        }

        public void Run()
        {
            foreach (DirectoryInfo di in Tests)
            {               
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Running test " + di.Name);
                Console.ResetColor();

                DirectoryInfo outd = new DirectoryInfo(di.FullName + "/Output");
                CleanDirectory(outd);

                Process runAlt = new Process();
                runAlt.StartInfo = new ProcessStartInfo(Environment.CurrentDirectory + "/../AlterNative/bin/Debug/AlterNative.exe", di.FullName + "/bin/" + di.Name + ".exe" + " "
                                                        + di.FullName + "/Output/" + " "
                                                        + "CXX" + " "
                                                        + di.FullName + "/../../Lib/");
                runAlt.Start();
                runAlt.WaitForExit();
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("************************ TEST ************************");
            Console.ResetColor();

            foreach(DirectoryInfo di in Tests)
            {
                Process diff = new Process();
                diff.StartInfo = new ProcessStartInfo("diff", "-qr " + di.Name + "/Output " + di.Name + "/Target");
                
                diff.Start();
                diff.WaitForExit();

                if (diff.ExitCode == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("TEST " + di.Name + " OK");
                    Console.ResetColor();
                }

                else if (diff.ExitCode == 1)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("TEST " + di.Name + " FAIL");
                    Console.ResetColor();
                }

                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Something was wrong with diff command");
                    Console.ResetColor();
                }
            }

            //Console.ReadLine();
        }
    }
}
