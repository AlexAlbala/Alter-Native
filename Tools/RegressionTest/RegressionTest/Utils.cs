using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace RegressionTest
{
    public class Utils
    {
        private static List<string> ignoreFolders;
        public static string testPath = Environment.CurrentDirectory;
        public static string alternativePath = Environment.CurrentDirectory + "/../AlterNative.Core.bin/bin/Debug/AlterNative.Core.exe";
        public static string alternativeDirectory = Environment.CurrentDirectory + "/../AlterNative.Core.bin/bin/Debug";
        public static string cxxLibraryPath = testPath + "/../Lib/src";

        static Utils()
        {
             ignoreFolders = new List<string>() { "gc", "boost", "System", "build", "Microsoft.SPOT" };
             string alt_exe_var_tmp = Environment.GetEnvironmentVariable("ALTERNATIVE_BIN");
             string alt_var_tmp = Environment.GetEnvironmentVariable("ALTERNATIVE_BIN_PATH");
             string lib_var_tmp = Environment.GetEnvironmentVariable("ALTERNATIVE_CPP_LIB");

             if (alt_exe_var_tmp != null)
             {
		         alternativeDirectory = alt_var_tmp;
                 alternativePath = alt_exe_var_tmp;
	    } else
                 WarningMessage("Variable <ALTERNATIVE_BIN> should be setted. Default is: " + alternativePath);

            if (lib_var_tmp != null)
                cxxLibraryPath = lib_var_tmp;
            else
                WarningMessage("Variable <ALTERNATIVE_CPP_LIB_PATH> should be setted. Default is: " + cxxLibraryPath);
               

	    Console.WriteLine("testPath="+testPath);
	    Console.WriteLine("alternativePath="+alternativePath);
	    Console.WriteLine("alternativeDirectory="+alternativeDirectory);
	    Console.WriteLine("cxxLibraryPath="+cxxLibraryPath);
	    
        }

        public static ITest CreateTest(Platform platform)
        {
            switch (platform)
            {
                case Platform.Windows32:
                case Platform.Windows64:
                    return new Win32Test();
		        case Platform.MacOS: 
                case Platform.Linux: 
                    return new PosixTest();
                case Platform.Android: 
                    return new AndroidTest();
                default: 
					break;
            }
            return null;
        }

        public static string GetAltCompileArg(CompileMode comp)
        {
            string cmode = "";

            switch (comp)
            {
                case CompileMode.Debug:
                    cmode = "";
                    break;
                case CompileMode.Release:
                    cmode = " -r";
                    break;
            }

            return cmode;
        }

        public static string GetMsBuildCompileArg(CompileMode comp)
        {
            string cmode = "";

            switch (comp)
            {
                case CompileMode.Debug:
                    cmode = "";
                    break;
                case CompileMode.Release:
                    cmode = " /p:Configuration=Release";
                    break;
            }

            return cmode;
        }

        public static string GetOutputFolderName(CompileMode comp)
        {
            string cmode = "";

            switch (comp)
            {
                case CompileMode.Debug:
                    cmode = "Debug";
                    break;
                case CompileMode.Release:
                    cmode = "Release";
                    break;
            }

            return cmode;
        }

        public static void DebugMessage(string message)
        {
            if (Config.Debug)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("DEBUG >>> " + message);
                Console.ResetColor();
            }
        }

        public static void WarningMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("WARNING >>> " + message);
            Console.ResetColor();
        }

        public static void ErrorMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("ERROR >>> " + message);
            Console.ResetColor();
        }

        public static void CleanDirectory(DirectoryInfo d, bool cleanRoot = true, bool ignoreFolders = false)
        {
            try
            {
                if (d.Exists)
                {
                    foreach (DirectoryInfo di in d.GetDirectories())
                        if (!(ignoreFolders && Utils.ignoreFolders.Contains(di.Name)))
                            CleanDirectory(di);

                    foreach (FileInfo fi in d.GetFiles())
                        fi.Delete();

                    if (cleanRoot)
                        d.Delete();
                }
            }
            catch (IOException e)
            {
                Utils.DebugMessage("IOException: " + e.Message);
            }
        }

        public static bool ContainsDirectory(DirectoryInfo directory, string directoryName)
        {
            foreach (DirectoryInfo di in directory.GetDirectories())
            {
                if (di.Name == directoryName)
                    return true;
            }

            return false;
        }

        public static int diffDirectory(DirectoryInfo di1, DirectoryInfo di2)
        {
            if (!di1.Exists || !di2.Exists)
                return 1;

            Process diff = new Process();
            string diffArgs = "-q " +
                "-x CMakeLists.txt " +/*
                 "-x \"" + di.Name + "/Output/gc/*\" " +*/
                    di1.FullName + " " + di2.FullName;

            Utils.DebugMessage("DIFF COMMAND:");
            Utils.DebugMessage("diff " + diffArgs);


            diff.StartInfo = new ProcessStartInfo("diff", diffArgs);
            if (Config.Verbose)
            {
                diff.StartInfo.CreateNoWindow = true;
                diff.StartInfo.UseShellExecute = false;
                diff.StartInfo.RedirectStandardOutput = true;
                diff.OutputDataReceived += (sender, args) => Console.WriteLine(args.Data);
            }
            diff.Start();
            if (Config.Verbose)
                diff.BeginOutputReadLine();
            diff.WaitForExit();

            return diff.ExitCode;
        }

        public static void diff(DirectoryInfo di, TestResult res)
        {
            int exitCode = 0;
            Console.WriteLine("Diff output source with target source...");
            Directory.SetCurrentDirectory(testPath);
            Utils.DebugMessage("Current directory: " + Environment.CurrentDirectory);

            DirectoryInfo output = new DirectoryInfo(di.FullName + "/Output");
            DirectoryInfo target = new DirectoryInfo(di.FullName + "/Target");
            int rootDirectoryCode = diffDirectory(output, target);

            if (rootDirectoryCode == 0)
            {
                foreach (DirectoryInfo d in output.GetDirectories())
                {
                    if (ignoreFolders.Contains(d.Name))
                    {
                        Utils.DebugMessage("Ignoring " + d.Name + " folder");
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
            Utils.DebugMessage("Exit Code: " + res.diffCode);
        }

        public static long CountTotalDirectoryLines(DirectoryInfo di)
        {
            long lines = 0;
            foreach (FileInfo fi in di.GetFiles())
            {
                if (fi.Extension == ".cs" || fi.Extension == ".c" || fi.Extension == ".cpp" || fi.Extension == ".h")
                    lines += CountLinesInFile(fi);
            }
            foreach (DirectoryInfo d in di.GetDirectories())
            {
                if (!ignoreFolders.Contains(d.Name))
                    lines += CountTotalDirectoryLines(d);
            }

            return lines;
        }

        public static long CountLinesInFile(FileInfo fi)
        {
            long count = 0;
            using (StreamReader r = new StreamReader(fi.FullName))
            {
                string line;
                while ((line = r.ReadLine()) != null)
                {
                    count++;
                }
            }
            return count;
        }

        public static void CountLines(DirectoryInfo di, TestResult res)
        {
            Directory.SetCurrentDirectory(di.FullName);
            res.finalLines = CountTotalDirectoryLines(new DirectoryInfo(di.FullName + "/Output"));
            res.originalLines = CountTotalDirectoryLines(new DirectoryInfo(di.FullName + "/src"));
            res.linesDifference = res.finalLines - res.originalLines;
            res.relativeLines = ((float)100 * res.linesDifference / (float)res.originalLines);

            Utils.DebugMessage("Original Lines: " + res.originalLines);
            Utils.DebugMessage("Final Lines: " + res.finalLines);
        }

        public static void CopyAll(DirectoryInfo source, DirectoryInfo target)
        {
            // Copy each file into it’s new directory.
            foreach (FileInfo fi in source.GetFiles())
                fi.CopyTo(System.IO.Path.Combine(target.ToString(), fi.Name), true);


            // Copy each subdirectory using recursion.
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir =
                    target.CreateSubdirectory(diSourceSubDir.Name);
                CopyAll(diSourceSubDir, nextTargetSubDir);
            }
        }

        public static void OverwriteTarget(DirectoryInfo di)
        {
            Directory.SetCurrentDirectory(di.FullName);
            DirectoryInfo output = new DirectoryInfo(di.FullName + "/Output");
            DirectoryInfo target = new DirectoryInfo(di.FullName + "/Target");

            CleanDirectory(target, false, true);

            foreach (FileInfo f in output.GetFiles())
                File.Copy(f.FullName, Path.Combine(target.ToString(), f.Name));

            foreach (DirectoryInfo d in output.GetDirectories())
            {
                if (!ignoreFolders.Contains(d.Name))
                {
                    Directory.CreateDirectory(target.FullName + "/" + d.Name);
                    CopyAll(d, new DirectoryInfo(target.FullName + "/" + d.Name));
                    d.Delete(true);
                }
            }
        }
    }
}
