using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace RegressionTest
{
    class Win32Test : ITest
    {
        public void Alternative(DirectoryInfo di, TestResult res)
        {
            DirectoryInfo outd = new DirectoryInfo(di.FullName + "/Output");
            Console.WriteLine("Cleanning directory: " + outd.Name);
            Utils.CleanDirectory(outd);
            Console.WriteLine("Running alternative...");

            Process runAlt = new Process();           

            string altArgs = di.FullName + "/NETbin/" + di.Name.Split('.')[1] + ".exe" + " "
                                                    + di.FullName + "/Output/" + " "
                                                    + Utils.GetAltCompileArg(Config.compileMode);

            Utils.DebugMessage("ALTERNATIVE COMMAND:");
            Utils.DebugMessage(Utils.alternativePath + " " + altArgs);
            
            runAlt.StartInfo = new ProcessStartInfo(Utils.alternativePath, altArgs);
            runAlt.StartInfo.CreateNoWindow = true;
            runAlt.StartInfo.UseShellExecute = false;
            if (Config.Verbose)
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
                Utils.DebugMessage("Unautorized exception: " + e.Message);
                res.alternative = 1;
                return;
            }
            catch (Exception ex)
            {
                Utils.DebugMessage("Exception: " + ex.Message);
                res.alternative = 2;
            }
            if (Config.Verbose)
                runAlt.BeginOutputReadLine();
            runAlt.WaitForExit();
            res.alternative = (short)runAlt.ExitCode;
        }

        public void Cmake(DirectoryInfo di, TestResult res)
        {
            //Create folder and run cmake                
            Directory.CreateDirectory(di.FullName + "/Output/build");
            Directory.SetCurrentDirectory(di.FullName + "/Output/build");

            Console.WriteLine("Configuring native source project...");
            //Run cmake
            Process runCmake = new Process();
            runCmake.StartInfo = new ProcessStartInfo("cmake", "..");

            runCmake.StartInfo.CreateNoWindow = true;
            runCmake.StartInfo.UseShellExecute = false;
            if (Config.Verbose)
            {
                runCmake.StartInfo.RedirectStandardOutput = true;
                runCmake.OutputDataReceived += (sender, args) => Console.WriteLine(args.Data);
            }
            runCmake.Start();
            if (Config.Verbose)
                runCmake.BeginOutputReadLine();
            runCmake.WaitForExit();

            res.cmakeCode = (short)runCmake.ExitCode;
            Utils.DebugMessage("Exit Code: " + res.cmakeCode);
        }

        public void Compile(DirectoryInfo di, TestResult res)
        {
            Console.WriteLine("Building native code...");
            //Compile the code
            string msbuildPath = System.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeDirectory();
            msbuildPath += @"msbuild.exe";

            string targetFile = di.Name.Split('.')[1] + "Proj.sln";
            string msbuildArgs = targetFile + Utils.GetMsBuildCompileArg(Config.compileMode) + " /p:PlatformToolset=v120_CTP_Nov2012";
            Utils.DebugMessage("MSBUILD COMMAND:");
            Utils.DebugMessage("msbuild " + msbuildArgs);
            //Run msbuild
            Process msbuild = new Process();
            msbuild.StartInfo = new ProcessStartInfo(msbuildPath, msbuildArgs);
            msbuild.StartInfo.UseShellExecute = false;
            msbuild.StartInfo.CreateNoWindow = true;
            if (Config.Verbose)
            {
                msbuild.StartInfo.RedirectStandardOutput = true;
                msbuild.OutputDataReceived += (sender, args) => Console.WriteLine(args.Data);
            }
            msbuild.Start();
            if (Config.Verbose)
                msbuild.BeginOutputReadLine();
            msbuild.WaitForExit();

            res.msbuildCode = (short)msbuild.ExitCode;
            Utils.DebugMessage("Exit Code: " + res.msbuildCode);
        }       

        public void CompareOutputs(DirectoryInfo di, TestResult res)
        {
            Console.WriteLine("Comparing outputs...");
            //Run original app
            Process orig = new Process();
            orig.StartInfo = new ProcessStartInfo(di.FullName + @"/NETbin/" + di.Name.Split('.')[1] + ".exe");

            orig.StartInfo.RedirectStandardOutput = true;
            orig.StartInfo.CreateNoWindow = true;
            orig.StartInfo.UseShellExecute = false;
            DateTime orig_di = DateTime.Now;
            orig.Start();
            orig.WaitForExit();
            DateTime orig_df = DateTime.Now;
            String originalOutput = orig.StandardOutput.ReadToEnd();

            Process final = new Process();
            final.StartInfo = new ProcessStartInfo(di.FullName + @"/Output/build/" + Utils.GetOutputFolderName(Config.compileMode) + "/"  + di.Name.Split('.')[1] + ".exe");
            final.StartInfo.RedirectStandardOutput = true;
            final.StartInfo.CreateNoWindow = true;
            final.StartInfo.UseShellExecute = false;
            DateTime final_di = DateTime.Now;
            final.Start();
            final.WaitForExit();
            DateTime final_df = DateTime.Now;
            String finalOutput = final.StandardOutput.ReadToEnd();

            res.output = (short)string.Compare(originalOutput, finalOutput);

            int maxLengthMsg;
            if (Config.Unlimited)
                maxLengthMsg = int.MaxValue;
            else
                maxLengthMsg = 100;

            //Timespan original
            TimeSpan tso = orig_df - orig_di;
            long ot = (long)tso.TotalMilliseconds;
            res.originalTime = ot;

            //Timespan final
            TimeSpan tsf = final_df - final_di;
            long ft = (long)tsf.TotalMilliseconds;
            res.finalTime = ft;

            Utils.DebugMessage("ORIGINAL");
            Utils.DebugMessage(originalOutput.Length > maxLengthMsg ? originalOutput.Substring(0, maxLengthMsg) + " [......] " : originalOutput);
            Utils.DebugMessage("TimeSpan: " + ot);

            Utils.DebugMessage("FINAL");
            Utils.DebugMessage(finalOutput.Length > maxLengthMsg ? finalOutput.Substring(0, maxLengthMsg) + " [......] " : finalOutput);
            Utils.DebugMessage("TimeSpan: " + ft);

            res.msTimeSpan = ft - ot;
            res.relativeTime = 100 * ((float)res.msTimeSpan / (float)ot);
        }
    }
}
