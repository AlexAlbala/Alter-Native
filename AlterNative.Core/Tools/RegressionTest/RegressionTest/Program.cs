using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace RegressionTest
{
    class Program
    {
        ITest test;
        Dictionary<DirectoryInfo, TestResult> Tests = new Dictionary<DirectoryInfo, TestResult>();
        
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
                if (s.Contains("="))
                {
                    if (s.Split("=".ToCharArray())[0] == "-platform")
                    {
                        string tmp = s.Split("=".ToCharArray())[1].ToLowerInvariant();

                        if (tmp == "win" || tmp == "win32")
                            Config.platform = Platform.Windows32;
                        if (tmp == "win64")
                            Config.platform = Platform.Windows64;
                        if (tmp == "linux")
                            Config.platform = Platform.Linux;
                        if (tmp == "macos")
                            Config.platform = Platform.MacOS;
                        if (tmp == "android")
                            Config.platform = Platform.Android;
                    }
                }
                else
                {

                    if (s.ToLowerInvariant().Contains("d"))
                        Config.Debug = true;
                    if (s.ToLowerInvariant().Contains("v"))
                        Config.Verbose = true;
                    if (s.ToLowerInvariant().Contains("u"))
                        Config.Unlimited = true;
                    if (s.ToLowerInvariant().Contains("f"))
                        Config.fast = true;
                    if (s.ToLowerInvariant().Contains("o"))
                        Config.overwriteTarget = true;
                    if (s.ToLowerInvariant().Contains("r"))
                        Config.compileMode = CompileMode.Release;
                    if (s.ToLowerInvariant().Contains("p"))
                        Config.performanceTests = true;
                }
            }

            p.test = Utils.CreateTest(Config.platform);

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
                if (Utils.ContainsDirectory(di, "Target") &&
                    Utils.ContainsDirectory(di, "src") &&
                    /*ContainsDirectory(di, "Output") &&*/
                    Utils.ContainsDirectory(di, "NETbin"))
                {
                    Tests.Add(di, new TestResult() { name = di.Name });
                    Utils.DebugMessage("Found test " + di.Name);
                }
            }
            Console.WriteLine();
            Utils.DebugMessage("Found " + Tests.Count + " Tests");
            Console.WriteLine();
        }

        //private  KeyValuePair<DirectoryInfo, TestResult> SearchFirstWithName(string name)
        //{
        //    foreach (KeyValuePair<DirectoryInfo, TestResult> kvp in Tests)
        //    {
        //        if (kvp.Key.Name == name)
        //            return kvp;
        //    }
        //    throw new KeyNotFoundException("name");
        //}

        public void RunTests(string[] tests)
        {
            Utils.DebugMessage("************* CONFIGURATION ***************");
            Utils.DebugMessage("TEST PLATFORM: " + Config.platform.ToString());
            Utils.DebugMessage("");
            Utils.DebugMessage("VERBOSE: " + Config.Verbose.ToString());
            Utils.DebugMessage("OUTPUT UNLIMITED: " + Config.Unlimited.ToString());
            Utils.DebugMessage("FAST MODE: " + Config.fast.ToString());
            Utils.DebugMessage("OVERWRITE TARGET CODE: " + Config.overwriteTarget.ToString());
            Utils.DebugMessage("PERFORMANCE TESTS: " + Config.performanceTests.ToString());
            Utils.DebugMessage("COMPILATION MODE: " + Config.compileMode.ToString());
            Utils.DebugMessage("*******************************************");
            Console.WriteLine();
            Console.WriteLine();
            foreach (string s in tests)
            {
                

                KeyValuePair<DirectoryInfo, TestResult> kvp = Tests.First(x => x.Key.Name == s);

               
                //KeyValuePair<DirectoryInfo, TestResult> kvp = SearchFirstWithName(s);
                DirectoryInfo di = kvp.Key;
                TestResult res = kvp.Value;

                if (s.StartsWith("PT") && !Config.performanceTests)
                {
                    res.cmakeCode = -10;
                    res.alternative = -10;
                    res.diffCode = -10;
                    res.msbuildCode = -10;
                    res.output = -10;
                    continue;
                }

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Running test " + di.Name);
                Console.ResetColor();

                //Run alternative
                test.Alternative(di, res);

                //Diff files
                Utils.diff(di, res);
                if (Config.fast)
                {
                    res.output = res.msbuildCode = res.cmakeCode = -10;
                    continue;
                }                

                if (res.alternative == 0)
                   test.Cmake(di, res);
                else
                    res.output = res.msbuildCode = res.cmakeCode = -10;
                if (res.cmakeCode == 0)
                {
                    if (Config.platform == Platform.Windows32 || Config.platform == Platform.Windows64)
                        test.Compile(di, res);
                }
                else
                    res.output = res.msbuildCode = -10;
                if (kvp.Value.msbuildCode == 0)
                    test.CompareOutputs(di, res);
                else
                    res.output = -10;


                if (res.AllSuccess() && Config.overwriteTarget)
                    Utils.OverwriteTarget(di);

                if (res.alternative == 0)
                    Utils.CountLines(di, res);
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("******************************************************************** TEST RESULTS ***************************************************************");
            Console.ResetColor();

            string[,] arr = new string[tests.Length + 1, 8];
            arr[0, 0] = "NAME";
            arr[0, 1] = "ALTERNATIVE";
            arr[0, 2] = "FILE DIFFER";
            arr[0, 3] = "CMAKE CODE";
            arr[0, 4] = "COMPILE (" + Config.platform.ToString() + "|" + Config.compileMode.ToString() + ")";
            arr[0, 5] = "OUTPUT";
            arr[0, 6] = "TIME DIFFERENCE";
            arr[0, 7] = "LINES DIFFERENCE";
            int i = 1;
            foreach (string s in tests)
            {
                KeyValuePair<DirectoryInfo, TestResult> kvp = Tests.First(x => x.Key.Name == s);
                //KeyValuePair<DirectoryInfo, TestResult> kvp = SearchFirstWithName(s);
                arr[i, 0] = kvp.Value.name;
                arr[i, 1] = kvp.Value.alternative == 0 ? "#gSUCCESS" : (kvp.Value.alternative == -10 ? "#ySKIPPED": "#rFAIL. Code: " + kvp.Value.alternative);
                arr[i, 2] = kvp.Value.diffCode == 0 ? "#gNo Differ" : (kvp.Value.diffCode == 1 ? "#rDiffer" : (kvp.Value.diffCode == -10 ? "#ySKIPPED" : "#rError. Code: " + kvp.Value.diffCode));
                arr[i, 3] = kvp.Value.cmakeCode == 0 ? "#gSUCCESS" : (kvp.Value.cmakeCode == -10 ? "#ySKIPPED" : "#rFAIL. Code: " + kvp.Value.cmakeCode);
                arr[i, 4] = kvp.Value.msbuildCode == 0 ? "#gBUILD SUCCEEDED" : (kvp.Value.msbuildCode == -10 ? "#ySKIPPED" : "#rFAIL. Code: " + kvp.Value.msbuildCode);
                arr[i, 5] = kvp.Value.output == 0 ? "#gOK" : (kvp.Value.output == -10 ? "#ySKIPPED" : "#rFAIL");
                arr[i, 6] = (kvp.Value.msTimeSpan >= 0 ? (kvp.Value.msTimeSpan == 0 ? "#y" : "#r") : "#g") + kvp.Value.msTimeSpan.ToString() + " ms " + "(" + kvp.Value.relativeTime.ToString("N2") + "%)";
                arr[i, 7] = (kvp.Value.linesDifference >= 0 ? (kvp.Value.linesDifference == 0 ? "#y" : "#r") : "#g") + kvp.Value.linesDifference.ToString() + " lines " + "(" + kvp.Value.relativeLines.ToString("N2") + "%)";

                i++;
            }
            ArrayPrinter.PrintToConsole(arr);
        }
    }
}
