using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace RegressionTest
{
    class AndroidTest : ITest
    {
        private string CMAKE_ANDROID_TOOLCHAIN = Utils.testPath + "/../Tools/Android/android_cmake/android.toolchain.cmake";
        public AndroidTest()
        {            

            string cmake_var_tmp = Environment.GetEnvironmentVariable("CMAKE_ANDROID_TOOLCHAIN");

            if (cmake_var_tmp != null)
                CMAKE_ANDROID_TOOLCHAIN = cmake_var_tmp;
            else
                Utils.WarningMessage("Variable <CMAKE_ANDROID_TOOLCHAIN> should be setted. Default is: " + CMAKE_ANDROID_TOOLCHAIN);
        }

        public void Alternative(DirectoryInfo di, TestResult res)
        {
            DirectoryInfo outd = new DirectoryInfo(di.FullName + "/Output");
            Console.WriteLine("Cleaning directory: " + outd.Name);
            Utils.CleanDirectory(outd);
            Console.WriteLine("Running alternative...");

            Process runAlt = new Process();

            string altArgs = di.FullName + "/NETbin/" + di.Name.Split('.')[1] + ".exe" + " "
                                                    + di.FullName + "/Output/" + " "                                                    
                                                    + Utils.GetAltCompileArg(Config.compileMode)
                                                    + " -android";

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

            string cmakeArgs = "-G \"Unix Makefiles\" -DCMAKE_TOOLCHAIN_FILE=" + this.CMAKE_ANDROID_TOOLCHAIN + " ..";
            //Run cmake
            Process runCmake = new Process();
            runCmake.StartInfo = new ProcessStartInfo("cmake", cmakeArgs);

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
            //DO MAKE
            //Create folder and run cmake                
            Directory.CreateDirectory(di.FullName + "/Output/build");
            Directory.SetCurrentDirectory(di.FullName + "/Output/build");

            Console.WriteLine("Building code...");

            //string cmakeArgs = "-G \"Unix Makefiles\" -DCMAKE_TOOLCHAIN_FILE=" + NDK_CMAKE_PATH.TrimEnd("/".ToCharArray()) + "/android.toolchain.cmake ..";
            //Run cmake
            Process runMake = new Process();
            runMake.StartInfo = new ProcessStartInfo("make");

            runMake.StartInfo.CreateNoWindow = true;
            runMake.StartInfo.UseShellExecute = false;
            if (Config.Verbose)
            {
                runMake.StartInfo.RedirectStandardOutput = true;
                runMake.OutputDataReceived += (sender, args) => Console.WriteLine(args.Data);
            }
            runMake.Start();
            if (Config.Verbose)
                runMake.BeginOutputReadLine();
            runMake.WaitForExit();

            res.cmakeCode = (short)runMake.ExitCode;
            Utils.DebugMessage("Exit Code: " + res.cmakeCode);
        }

        public void CompareOutputs(DirectoryInfo di, TestResult res)
        {
            //TODO: HOW WE RUN IN ANDROID ?

            //START AVD
            //PUSH MONO
            //PUSH FILE
            //RUN FILE
            throw new NotImplementedException();
        }
    }
}
