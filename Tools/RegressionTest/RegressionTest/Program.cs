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
        private bool Debug = false;
        private bool Verbose = false;
        Dictionary<DirectoryInfo, TestResult> Tests = new Dictionary<DirectoryInfo, TestResult>();
        string testPath = Environment.CurrentDirectory;
        string alternativePath = Environment.CurrentDirectory + "/../AlterNative/bin/Debug/AlterNative.exe";

        static void Main(string[] args)
        {
            List<string> _args = new List<string>();
            List<string> _opts = new List<string>();

            foreach (string s in args)
            {
                if (s.StartsWith("-"))
                    _opts.Add(s);
                else
                    _args.Add(s);
            }

            Program p = new Program();

            foreach (string s in _opts)
            {
                if (s.ToLowerInvariant().Contains("d"))
                    p.Debug = true;
                if (s.ToLowerInvariant().Contains("v"))
                    p.Verbose = true;
            }


            p.AvailableTests();
            if (_args.Count == 0)
                p.RunAll();
            else
                p.RunTests(_args.ToArray());

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
                    DebugMessage("Found test " + di.Name);
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
            Console.WriteLine("Configuring native source project...");
            //Run cmake
            Process runCmake = new Process();
            runCmake.StartInfo = new ProcessStartInfo("cmake", "..");
            runCmake.StartInfo.CreateNoWindow = true;
            runCmake.StartInfo.UseShellExecute = false;
            if (Verbose)
            {
                runCmake.StartInfo.RedirectStandardOutput = true;
                runCmake.OutputDataReceived += (sender, args) => Console.WriteLine(args.Data);
            }
            runCmake.Start();
            if (Verbose)
                runCmake.BeginOutputReadLine();
            runCmake.WaitForExit();

            res.cmakeCode = (short)runCmake.ExitCode;
            DebugMessage("Exit Code: " + res.cmakeCode);
        }

        private void msbuild(DirectoryInfo di, TestResult res)
        {
            Console.WriteLine("Building native code...");
            //Compile the code
            string msbuildPath = System.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeDirectory();
            msbuildPath += @"msbuild.exe";

            string targetFile = di.Name.Split('.')[1] + "Proj.sln";
            DebugMessage("Target file: " + targetFile);
            //Run msbuild
            Process msbuild = new Process();
            msbuild.StartInfo = new ProcessStartInfo(msbuildPath, targetFile);
            msbuild.StartInfo.UseShellExecute = false;
            msbuild.StartInfo.CreateNoWindow = true;
            if (Verbose)
            {
                msbuild.StartInfo.RedirectStandardOutput = true;
                msbuild.OutputDataReceived += (sender, args) => Console.WriteLine(args.Data);
            }
            msbuild.Start();
            if (Verbose)
                msbuild.BeginOutputReadLine();
            msbuild.WaitForExit();

            res.msbuildCode = (short)msbuild.ExitCode;
            DebugMessage("Exit Code: " + res.msbuildCode);
        }

        private void DebugMessage(string message)
        {
            if (Debug)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("DEBUG >>> " + message);
                Console.ResetColor();
            }
        }

        private void compareOutput(DirectoryInfo di, TestResult res)
        {
            Console.WriteLine("Comparing outputs...");
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

            int maxLengthMsg = 300;
            DebugMessage("ORIGINAL");
            DebugMessage(originalOutput.Length > maxLengthMsg ? originalOutput.Substring(0, maxLengthMsg) + " [......] " : originalOutput);
            DebugMessage("FINAL");
            DebugMessage(finalOutput.Length > maxLengthMsg ? finalOutput.Substring(0, maxLengthMsg) + " [......] " : finalOutput);
        }

        private void alternative(DirectoryInfo di)
        {
            Console.WriteLine("Running alternative...");
            DirectoryInfo outd = new DirectoryInfo(di.FullName + "/Output");
            CleanDirectory(outd);

            Process runAlt = new Process();

            string altArgs = di.FullName + "/NETbin/" + di.Name + ".exe" + " "
                                                    + di.FullName + "/Output/" + " "
                                                    + "CXX" + " "
                                                    + testPath + "/../Lib/";

            DebugMessage("ALTERNATIVE COMMAND:");
            DebugMessage(alternativePath + " " + altArgs);

            runAlt.StartInfo = new ProcessStartInfo(alternativePath, altArgs);
            runAlt.StartInfo.CreateNoWindow = true;
            runAlt.StartInfo.UseShellExecute = false;
            if (Verbose)
            {
                runAlt.StartInfo.RedirectStandardOutput = true;
                runAlt.OutputDataReceived += (sender, args) => Console.WriteLine(args.Data);
            }
            runAlt.Start();
            if (Verbose)
                runAlt.BeginOutputReadLine();
            runAlt.WaitForExit();
        }

        private int diffDirectory(DirectoryInfo di1, DirectoryInfo di2)
        {
            if (!di1.Exists || !di2.Exists)
                return 1;

            Process diff = new Process();
            string diffArgs = "-q " +
                /*"-x \"" + di.Name + "/Output/System/*\" " +
                 "-x \"" + di.Name + "/Output/gc/*\" " +*/
                    di1.FullName + " " + di2.FullName;

            DebugMessage("DIFF COMMAND:");
            DebugMessage("diff " + diffArgs);


            diff.StartInfo = new ProcessStartInfo("diff", diffArgs);
            if (Verbose)
            {
                diff.StartInfo.CreateNoWindow = true;
                diff.StartInfo.UseShellExecute = false;
                diff.StartInfo.RedirectStandardOutput = true;
                diff.OutputDataReceived += (sender, args) => Console.WriteLine(args.Data);
            }
            diff.Start();
            if (Verbose)
                diff.BeginOutputReadLine();
            diff.WaitForExit();

            return diff.ExitCode;
        }

        private void diff(DirectoryInfo di, TestResult res)
        {
            int exitCode = 0;
            Console.WriteLine("Diff output source with target source...");
            Directory.SetCurrentDirectory(testPath);
            DebugMessage("Current directory: " + Environment.CurrentDirectory);


            DirectoryInfo output = new DirectoryInfo(di.FullName + "/Output");
            DirectoryInfo target = new DirectoryInfo(di.FullName + "/Target");
            int rootDirectoryCode = diffDirectory(output, target);

            if (rootDirectoryCode == 0)
            {
                foreach (DirectoryInfo d in output.GetDirectories())
                {
                    if (d.Name == "System" || d.Name == "gc")
                    {
                        DebugMessage("Ignoring " + d.Name + " folder");
                        continue;
                    }

                    DirectoryInfo d1 = new DirectoryInfo(di.FullName + "/Output/" + d.Name);
                    DirectoryInfo d2 = new DirectoryInfo(di.FullName + "/Target/" + d.Name);

                    int dirCode = diffDirectory(d1, d2);
                    if (dirCode != 0)
                    {
                        exitCode = dirCode;
                        break;
                    }
                }
            }
            else
                exitCode = rootDirectoryCode;

            res.diffCode = (short)exitCode;
            DebugMessage("Exit Code: " + res.diffCode);
        }

        public void RunTests(string[] tests)
        {
            foreach (string s in tests)
            {
                KeyValuePair<DirectoryInfo, TestResult> kvp = Tests.First(x => x.Key.Name == s);
                DirectoryInfo di = kvp.Key;
                TestResult res = kvp.Value;

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Running test " + di.Name);
                Console.ResetColor();

                //Run alternative
                alternative(di);

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

            string[,] arr = new string[tests.Length + 1, 5];
            arr[0, 0] = "NAME";
            arr[0, 1] = "CMAKE CODE";
            arr[0, 2] = "MSBUILD CODE";
            arr[0, 3] = "FILE DIFFER";
            arr[0, 4] = "OUTPUT";
            int i = 1;
            foreach (string s in tests)
            {
                KeyValuePair<DirectoryInfo, TestResult> kvp = Tests.First(x => x.Key.Name == s);
                arr[i, 0] = kvp.Key.Name;
                arr[i, 1] = kvp.Value.cmakeCode == 0 ? "SUCCESS" : "FAIL. Code: " + kvp.Value.cmakeCode;
                arr[i, 2] = kvp.Value.msbuildCode == 0 ? "BUILD SUCCEEDED" : "FAIL. Code: " + kvp.Value.msbuildCode;
                arr[i, 3] = (kvp.Value.diffCode == 0 ? "No Differ" : (kvp.Value.diffCode == 1 ? "Differ" : "Error. Code: " + kvp.Value.diffCode));
                arr[i, 4] = (kvp.Value.output ? "OK" : "FAIL");

                i++;
            }
            ArrayPrinter.PrintToConsole(arr);

        }

        public void RunAll()
        {
            foreach (KeyValuePair<DirectoryInfo, TestResult> kvp in Tests)
            {
                DirectoryInfo di = kvp.Key;
                TestResult res = kvp.Value;

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Running test " + di.Name);
                Console.ResetColor();

                //Run alternative
                alternative(di);

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
                arr[i, 1] = kvp.Value.cmakeCode == 0 ? "SUCCESS" : "FAIL. Code: " + kvp.Value.cmakeCode;
                arr[i, 2] = kvp.Value.msbuildCode == 0 ? "BUILD SUCCEEDED" : "FAIL. Code: " + kvp.Value.msbuildCode;
                arr[i, 3] = (kvp.Value.diffCode == 0 ? "No Differ" : (kvp.Value.diffCode == 1 ? "Differ" : "Error. Code: " + kvp.Value.diffCode));
                arr[i, 4] = (kvp.Value.output ? "OK" : "FAIL");

                i++;
            }
            ArrayPrinter.PrintToConsole(arr);
        }
    }
}
