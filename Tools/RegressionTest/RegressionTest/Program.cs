using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Mono.Options;

namespace RegressionTest
{
    class Program
    {
        ITest test;
        Dictionary<DirectoryInfo, TestResult> Tests = new Dictionary<DirectoryInfo, TestResult>();

        public static int Main(string[] args)
        {
            bool show_help = false;

            var opts = new OptionSet() { 
            { "d", "debug messages" ,
            v => Config.Debug = v!= null},
            { "v", "increase debug message verbosity.",
            v => Config.Verbose = v != null },
            { "r", "Compile in release mode.",
            v => Config.compileMode  = v != null ? CompileMode.Release :CompileMode.Debug },
            { "R", "Recursive translation.",
            v => Config.RecursiveDependencies  = v != null},
            { "f", "Fast mode. Only compares target/output files.",
            v => Config.fast = v != null },
            { "o", "Overwrite target files with output files.",
            v => Config.overwriteTarget = v != null },
            { "u", "Output messages are unlimited.",
            v => Config.Unlimited = v != null },
            { "p", "Performance tests are also included.",
            v => Config.performanceTests = v != null },
            { "platform=", "Platform to use: [win|win32|win64|linux|macos|android].",
            (String v) => Config.platform = (Platform)Enum.Parse(typeof(Platform), v) },
            { "h|help",  "show this message and exit", 
            v => show_help = v != null },
            };

            List<string> tests;
            try
            {
                tests = opts.Parse(args);
            }
            catch (OptionException e)
            {
                Console.Write("RegressionTest: ");
                Console.WriteLine(e.Message);
                Console.WriteLine("Try `RegressionTest --help' for more information.");
                return -1;
            }

            if (show_help)
            {
                ShowHelp(opts);
                return -1;
            }

            Program p = new Program();

            p.test = Utils.CreateTest(Config.platform);
            p.AvailableTests();


            foreach (string s in tests)
                Console.WriteLine(s);


            if (tests.Count == 0)
            {
                List<string> t = new List<string>();
                foreach (DirectoryInfo di in p.Tests.Keys)
                    t.Add(di.Name);
                p.RunTests(t.ToArray());
            }
            else
            {
                p.RunTests(tests.ToArray());
            }

            return 0;
        }

        static void ShowHelp(OptionSet p)
        {
            Console.WriteLine("Usage: RegressionTest [OPTIONS]+ <001.test> <002.test> ...");
            Console.WriteLine("Executes AlterNative regression tests.");
            Console.WriteLine();
            Console.WriteLine("Options:");
            p.WriteOptionDescriptions(Console.Out);
        }

        public void AvailableTests()
        {
            DirectoryInfo d = new DirectoryInfo(Environment.CurrentDirectory);

            foreach (DirectoryInfo di in d.GetDirectories())
            {
                if (Utils.ContainsDirectory(di, "Target") &&
                    Utils.ContainsDirectory(di, "src") &&
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
            foreach (string _s in tests)
            {
		//Removed final '/' if exists
		string s = _s.TrimEnd('/');
                Environment.CurrentDirectory = Utils.testPath;
                KeyValuePair<DirectoryInfo, TestResult> kvp = Tests.First(x => x.Key.Name == s);
                
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

                try
                {
                    Environment.CurrentDirectory = Utils.alternativeDirectory;
                    //Run alternative
                    test.Alternative(di, res);
                    Environment.CurrentDirectory = Utils.testPath;
                }
                catch (Exception e)
                {
                    Console.WriteLine("ERROR IN ALTERNATIVE: " + e.Message);
                    res.alternative = 1;
                }

                try
                {
                    //Diff files
                    Utils.diff(di, res);
                }
                catch (Exception e)
                {
                    Console.WriteLine("ERROR IN DIFF: " + e.Message);
                    res.diffCode = 1;
                }
                if (Config.fast)
                {
                    res.output = res.msbuildCode = res.cmakeCode = -10;
                    continue;
                }

                if (res.alternative == 0)
                {
                    try
                    {
                        test.Make(di, res);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("ERROR IN MAKE: " + e.Message);
                        res.cmakeCode = -2;
                        res.cmakeCode = -2;
                    }
                }
                else
                    res.output = res.msbuildCode = res.cmakeCode = -10;


                if (kvp.Value.msbuildCode == 0)
                {
                    try
                    {
                        test.CompareOutputs(di, res);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("ERROR COMPARING OUTPUTS: " + e.Message);
                        res.output = 1;
                    }

                }
                else
                    res.output = -10;

                try
                {
                    if (res.msbuildCode == 0 && Config.overwriteTarget)
                        Utils.OverwriteTarget(di);

                    //if (res.alternative == 0)
                      //  Utils.CountLines(di, res);
                }
                catch (Exception e)
                {
                    Console.WriteLine("ERROR OVERWRITING FILES OR COUNTING LINES: " + e.Message);
                }
            }

            string[,] arr = new string[tests.Length + 1, 7];
            arr[0, 0] = "NAME";
            arr[0, 1] = "X-LATE";
            arr[0, 2] = "DIFF";
            arr[0, 3] = "CMAKE";
            arr[0, 4] = "COMPILE"; // (" + Config.platform.ToString() + "|" + Config.compileMode.ToString() + ")";
            arr[0, 5] = "OUTPUT";
            arr[0, 6] = "TIME";
            int i = 1;
            foreach (string _s in tests)
            {
		string s = _s.TrimEnd('/');
                KeyValuePair<DirectoryInfo, TestResult> kvp = Tests.First(x => x.Key.Name == s);
                //KeyValuePair<DirectoryInfo, TestResult> kvp = SearchFirstWithName(s);
                arr[i, 0] = kvp.Value.name;
                arr[i, 1] = kvp.Value.alternative == 0 ? "#gSUCCESS" : (kvp.Value.alternative == -10 ? "#ySKIPPED" : "#rFAIL. Code: " + kvp.Value.alternative);
                arr[i, 2] = kvp.Value.diffCode == 0 ? "#gNo Differ" : (kvp.Value.diffCode == 1 ? "#rDiffer" : (kvp.Value.diffCode == -10 ? "#ySKIPPED" : "#rError. Code: " + kvp.Value.diffCode));
                arr[i, 3] = kvp.Value.cmakeCode == 0 ? "#gSUCCESS" : (kvp.Value.cmakeCode == -10 ? "#ySKIPPED" : "#rFAIL. Code: " + kvp.Value.cmakeCode);
                arr[i, 4] = kvp.Value.msbuildCode == 0 ? "#gSUCCEESS" : (kvp.Value.msbuildCode == -10 ? "#ySKIPPED" : "#rFAIL. Code: " + kvp.Value.msbuildCode);
                arr[i, 5] = kvp.Value.output == 0 ? "#gOK" : (kvp.Value.output == -10 ? "#ySKIPPED" : "#rFAIL");
                arr[i, 6] = (kvp.Value.msTimeSpan >= 0 ? (kvp.Value.msTimeSpan == 0 ? "#y" : "#r") : "#g") + kvp.Value.msTimeSpan.ToString() + " ms " + "(" + kvp.Value.relativeTime.ToString("N2") + "%)";

                i++;
            }
            ArrayPrinter.PrintToConsole(arr);
        }
    }
}
