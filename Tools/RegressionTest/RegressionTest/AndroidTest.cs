using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace RegressionTest
{
    class AndroidTest : ITest
    {
        private string NDK_CMAKE_PATH;
        public AndroidTest(string NDK_CMAKE_PATH)
        {
            this.NDK_CMAKE_PATH = NDK_CMAKE_PATH;
            Utils.DebugMessage("NDK_CMAKE_PATH: " + NDK_CMAKE_PATH);
        }

        public void Alternative(DirectoryInfo di, TestResult res)
        {
            DirectoryInfo outd = new DirectoryInfo(di.FullName + "/Output");
            Console.WriteLine("Cleanning directory: " + outd.Name);
            Utils.CleanDirectory(outd);
            Console.WriteLine("Running alternative...");

            Process runAlt = new Process();

            string altArgs = di.FullName + "/NETbin/" + di.Name.Split('.')[1] + ".exe" + " "
                                                    + di.FullName + "/Output/" + " "
                                                    + "CXX" + " "
                                                    + Utils.testPath + "/../Lib/"
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

            string cmakeArgs = "-DCMAKE_TOOLCHAIN_FILE=" + NDK_CMAKE_PATH + "/toolchain/android.toolchain.cmake ..";
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
            throw new NotImplementedException();
        }

        public void CompareOutputs(DirectoryInfo di, TestResult res)
        {
            //TODO: HOW WE RUN IN ANDROID ?

            //START AVD
            //PUSH FILE
            //RUN FILE
            throw new NotImplementedException();
        }
    }
}
