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
        private bool Unlimited = false;
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
                if (s.ToLowerInvariant().Contains("u"))
                    p.Unlimited = true;
            }


            p.AvailableTests();
            if (_args.Count == 0)
            {
                List<string> t = new List<string>();
                foreach (DirectoryInfo di in p.Tests.Keys)
                    t.Add(di.Name);
                p.RunTests(t.ToArray());
            }
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
            try
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
            catch (IOException e)
            {
                DebugMessage("IOException: " + e.Message);
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

            int maxLengthMsg;
            if (Unlimited)
                maxLengthMsg = int.MaxValue;
            else
                maxLengthMsg = 100;

            DebugMessage("ORIGINAL");
            DebugMessage(originalOutput.Length > maxLengthMsg ? originalOutput.Substring(0, maxLengthMsg) + " [......] " : originalOutput);
            DebugMessage("FINAL");
            DebugMessage(finalOutput.Length > maxLengthMsg ? finalOutput.Substring(0, maxLengthMsg) + " [......] " : finalOutput);
        }

        private void alternative(DirectoryInfo di, TestResult res)
        {
            DirectoryInfo outd = new DirectoryInfo(di.FullName + "/Output");
            Console.WriteLine("Cleanning directory: " + outd.Name);
            CleanDirectory(outd);
            Console.WriteLine("Running alternative...");

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
            try
            {
                runAlt.Start();
            }
            catch (UnauthorizedAccessException e)
            {
                DebugMessage("Unautorized exception: " + e.Message);
                res.alternative = 1;
                return;
            }
            catch (Exception ex)
            {
                DebugMessage("Exception: " + ex.Message);
                res.alternative = 2;
            }
            if (Verbose)
                runAlt.BeginOutputReadLine();
            runAlt.WaitForExit();
            res.alternative = 0;
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
                alternative(di, res);

                //Diff files
                diff(di, res);

                //Create folder and run cmake                
                Directory.CreateDirectory(di.FullName + "/Output/build");
                Directory.SetCurrentDirectory(di.FullName + "/Output/build");

                if (res.alternative == 0)
                    Cmake(res);
                if (res.cmakeCode == 0)
                    msbuild(di, res);
                if (kvp.Value.msbuildCode == 0)
                    compareOutput(di, res);
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("******************************************************************** TEST RESULTS ***************************************************************");
            Console.ResetColor();

            string[,] arr = new string[tests.Length + 1, 6];
            arr[0, 0] = "NAME";
            arr[0, 1] = "ALTERNATIVE";
            arr[0, 2] = "CMAKE CODE";
            arr[0, 3] = "MSBUILD CODE";
            arr[0, 4] = "FILE DIFFER";
            arr[0, 5] = "OUTPUT";
            int i = 1;
            foreach (string s in tests)
            {
                KeyValuePair<DirectoryInfo, TestResult> kvp = Tests.First(x => x.Key.Name == s);
                arr[i, 0] = kvp.Key.Name;
                arr[i, 1] = kvp.Value.alternative == 0 ? "SUCCESS" : "FAIL. Code: " + kvp.Value.alternative;
                arr[i, 2] = kvp.Value.cmakeCode == 0 ? "SUCCESS" : "FAIL. Code: " + kvp.Value.cmakeCode;
                arr[i, 3] = kvp.Value.msbuildCode == 0 ? "BUILD SUCCEEDED" : "FAIL. Code: " + kvp.Value.msbuildCode;
                arr[i, 4] = (kvp.Value.diffCode == 0 ? "No Differ" : (kvp.Value.diffCode == 1 ? "Differ" : "Error. Code: " + kvp.Value.diffCode));
                arr[i, 5] = (kvp.Value.output ? "OK" : "FAIL");

                i++;
            }
            ArrayPrinter.PrintToConsole(arr);
        }
    }
}
