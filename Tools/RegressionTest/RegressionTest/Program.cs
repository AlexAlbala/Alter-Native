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
        Dictionary<DirectoryInfo, TestResult> Tests = new Dictionary<DirectoryInfo, TestResult>();

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
                    ContainsDirectory(di, "NETbin"))
                {
                    Tests.Add(di, new TestResult());
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

        private void Cmake(TestResult res)
        {
            //Run cmake
            Process runCmake = new Process();
            runCmake.StartInfo = new ProcessStartInfo("cmake", "..");
            runCmake.StartInfo.RedirectStandardOutput = true;
            runCmake.StartInfo.CreateNoWindow = true;
            runCmake.StartInfo.UseShellExecute = false;
            runCmake.OutputDataReceived += (sender, args) => Console.WriteLine(args.Data);
            runCmake.Start();
            runCmake.BeginOutputReadLine();
            runCmake.WaitForExit();

            res.cmakeCode = (short)runCmake.ExitCode;
        }

        private void msbuild(DirectoryInfo di, TestResult res)
        {
            //Compile the code
            string msbuildPath = System.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeDirectory();
            msbuildPath += @"msbuild.exe";

            //Run msbuild
            Process msbuild = new Process();
            msbuild.StartInfo = new ProcessStartInfo(msbuildPath, di.Name.Split('.')[1] + "Proj.sln");
            msbuild.StartInfo.RedirectStandardOutput = true;
            msbuild.StartInfo.CreateNoWindow = true;
            msbuild.StartInfo.UseShellExecute = false;
            msbuild.OutputDataReceived += (sender, args) => Console.WriteLine(args.Data);
            msbuild.Start();
            msbuild.BeginOutputReadLine();
            msbuild.WaitForExit();

            res.msbuildCode = (short)msbuild.ExitCode;
        }

        private void compareOutput(DirectoryInfo di, TestResult res)
        {
            //Run original app
            Process orig = new Process();
            orig.StartInfo = new ProcessStartInfo(di.FullName + @"/NETbin/" + di.Name + ".exe");
            orig.StartInfo.RedirectStandardOutput = true;
            orig.StartInfo.CreateNoWindow = true;
            orig.StartInfo.UseShellExecute = false;
            orig.Start();
            orig.WaitForExit();
            String originalOutput = orig.StandardOutput.ReadToEnd();

            Process final = new Process();
            final.StartInfo = new ProcessStartInfo(di.FullName + @"/Output/build/Debug/" + di.Name.Split('.')[1] + ".exe");
            final.StartInfo.RedirectStandardOutput = true;
            final.StartInfo.CreateNoWindow = true;
            final.StartInfo.UseShellExecute = false;
            final.Start();
            final.WaitForExit();
            String finalOutput = final.StandardOutput.ReadToEnd();

            res.output = string.Compare(originalOutput, finalOutput) == 0;
        }

        private void diff(DirectoryInfo di, TestResult res)
        {
            Process diff = new Process();
            diff.StartInfo = new ProcessStartInfo("diff", "-qr " + di.Name + "/Output " + di.Name + "/Target");
            diff.StartInfo.RedirectStandardOutput = true;
            diff.StartInfo.CreateNoWindow = true;
            diff.StartInfo.UseShellExecute = false;
            diff.OutputDataReceived += (sender, args) => Console.WriteLine(args.Data);
            diff.Start();
            diff.BeginOutputReadLine();
            diff.WaitForExit();

            if (diff.ExitCode == 0)
                res.fileDiff = false;

            else if (diff.ExitCode == 1)
                res.fileDiff = true;

            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Something was wrong with diff command. Exit code was: " + diff.ExitCode);
                Console.ResetColor();
            }
        }

        public void Run()
        {
            foreach (KeyValuePair<DirectoryInfo, TestResult> kvp in Tests)
            {
                DirectoryInfo di = kvp.Key;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Running test " + di.Name);
                Console.ResetColor();

                DirectoryInfo outd = new DirectoryInfo(di.FullName + "/Output");
                CleanDirectory(outd);

                Process runAlt = new Process();
                runAlt.StartInfo = new ProcessStartInfo(Environment.CurrentDirectory + "/../AlterNative/bin/Debug/AlterNative.exe", di.FullName + "/NETbin/" + di.Name + ".exe" + " "
                                                        + di.FullName + "/Output/" + " "
                                                        + "CXX" + " "
                                                        + di.FullName + "/../../Lib/");
                runAlt.Start();
                runAlt.WaitForExit();
            }

            foreach (KeyValuePair<DirectoryInfo, TestResult> kvp in Tests)
            {
                DirectoryInfo di = kvp.Key;
                TestResult res = kvp.Value;

                //Diff files
                diff(di, res);

                //Create folder and run cmake                
                Directory.CreateDirectory(di.FullName + "/Output/build");
                Directory.SetCurrentDirectory(di.FullName + "/Output/build");

                
                Cmake(res);
                if (res.cmakeCode == 0)
                    msbuild(di, res);                
                if (kvp.Value.msbuildCode == 0)
                    compareOutput(di, res);
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("******************************************* TEST RESULTS *****************************************");
            Console.ResetColor();

            string[,] arr = new string[Tests.Count + 1, 5];
            arr[0, 0] = "NAME";
            arr[0, 1] = "CMAKE CODE";
            arr[0, 2] = "MSBUILD CODE";
            arr[0, 3] = "FILE DIFFER";
            arr[0, 4] = "OUTPUT";
            int i = 1;
            foreach (KeyValuePair<DirectoryInfo, TestResult> kvp in Tests)
            {
                arr[i, 0] = kvp.Key.Name;
                arr[i, 1] = kvp.Value.cmakeCode == 0 ? "SUCCESS": "FAIL. Code: " + kvp.Value.cmakeCode;
                arr[i, 2] = kvp.Value.msbuildCode == 0 ? "BUILD SUCCEEDED" : "FAIL. Code: " + kvp.Value.msbuildCode;
                arr[i, 3] = (kvp.Value.fileDiff ? "Differ" : "No differ");
                arr[i, 4] = (kvp.Value.output ? "OK" : "FAIL");

                i++;
            }
            ArrayPrinter.PrintToConsole(arr);
        }
    }
}
