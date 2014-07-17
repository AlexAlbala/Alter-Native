using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using AlterNative.Tools;
using Mono.Cecil;

namespace AlterNative
{
    class Commands
    {
        public static DirectoryInfo RunCMake(DirectoryInfo di, out int cmakeCode)
        {
            di.CreateSubdirectory("build");
            Environment.CurrentDirectory = di.FullName + "build";
            Process p = new Process();
#if CORE
            p.StartInfo = new ProcessStartInfo("cmake", "-G \"Unix Makefiles\" ..");
#else
            p.StartInfo = new ProcessStartInfo("cmake", "-G \"Visual Studio 11\" ..");
#endif
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.UseShellExecute = false;

            if (Config.Verbose)
            {
                p.StartInfo.RedirectStandardOutput = true;
                p.OutputDataReceived += (sender, args) => Utils.WriteToConsole(args.Data);
            }

            p.Start();
            if (Config.Verbose)
                p.BeginOutputReadLine();

            p.WaitForExit();

            cmakeCode = p.ExitCode;
            DirectoryInfo buildDir = new DirectoryInfo(di.FullName + "build");
            return buildDir;
        }

        public static int Compile(DirectoryInfo di)
        {
            int exitCode = 0;
            Environment.CurrentDirectory = di.FullName;
#if CORE
            Process make = new Process();
            make.StartInfo = new ProcessStartInfo("make");
            make.StartInfo.UseShellExecute = false;
            make.StartInfo.CreateNoWindow = true;    
            if(Config.Verbose){
                make.StartInfo.RedirectStandardOutput = true;
                make.OutputDataReceived += (sender, args) => Utils.WriteToConsole(args.Data);
            }
            make.Start();
            if(Config.Verbose)
                make.BeginOutputReadLine();

            make.WaitForExit();
            exitCode = make.ExitCode;
#else
            string msbuildPath = System.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeDirectory();
            msbuildPath += @"msbuild.exe";

            //Compile the code
            FileInfo[] finfos = di.GetFiles("*.sln");

            if (finfos.Count() > 0)
            {
                string targetFile = finfos[0].FullName;
                string msbuildArgs = targetFile + " /p:PlatformToolset=v120_CTP_Nov2012";
                //string msbuildArgs = targetFile + " /p:PlatformToolset=CTP_Nov2013";

                //Run msbuild
                Process msbuild = new Process();
                msbuild.StartInfo = new ProcessStartInfo(msbuildPath, msbuildArgs);
                msbuild.StartInfo.UseShellExecute = false;
                msbuild.StartInfo.CreateNoWindow = true;

                if (Config.Verbose)
                {
                    msbuild.StartInfo.RedirectStandardOutput = true;
                    msbuild.OutputDataReceived += (sender, args) => Utils.WriteToConsole(args.Data);
                }

                msbuild.Start();

                if (Config.Verbose)
                    msbuild.BeginOutputReadLine();

                msbuild.WaitForExit();
                exitCode = msbuild.ExitCode;
            }
            else
            {
                Utils.WriteToConsole("No .sln file found in " + di.FullName);
                exitCode = 1;
            }
#endif
            return exitCode;
        }

        public static void CompileNetCode(DirectoryInfo di)
        {
            string cscPath = System.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeDirectory();
            cscPath += @"csc.exe";

            Process p = new Process();
            string pargs = "";
            foreach (FileInfo f in di.GetFiles())
            {
                pargs += f.FullName;
                pargs += "";
            }
#if CORE
            p.StartInfo = new ProcessStartInfo(cscPath, args);
#else
            p.StartInfo = new ProcessStartInfo("csc", pargs);
#endif
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.UseShellExecute = false;

            if (Config.Verbose)
            {
                p.StartInfo.RedirectStandardOutput = true;
                p.OutputDataReceived += (sender, args) => Utils.WriteToConsole(args.Data);
            }

            p.Start();
            if (Config.Verbose)
                p.BeginOutputReadLine();

            p.WaitForExit();
        }

        public static void CompileNetCode(FileInfo fi)
        {
            string cscPath = System.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeDirectory();
            cscPath += @"csc.exe";

            Process p = new Process();
#if CORE
            p.StartInfo = new ProcessStartInfo(cscPath, fi.FullName);
#else
            p.StartInfo = new ProcessStartInfo("csc", fi.FullName);
#endif
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.UseShellExecute = false;

            if (Config.Verbose)
            {
                p.StartInfo.RedirectStandardOutput = true;
                p.OutputDataReceived += (sender, args) => Utils.WriteToConsole(args.Data);
            }

            p.Start();
            if (Config.Verbose)
                p.BeginOutputReadLine();

            p.WaitForExit();
        }

        public static AssemblyDefinition NewTemplate(DirectoryInfo output)
        {
            Utils.WriteToConsole("Creating blank template...");
            if (Config.AlterNativeTools != null)
            {
                Config.targetType = TargetType.Executable;
                return LoadAssembly(Config.AlterNativeTools + @"/Templates/Blank/Blank.exe");
            }
            else
            {
                Utils.WriteToConsole("WARNING: ALTERNATIVE_TOOLS_PATH not setted");

                Utils.WriteToConsole("Trying to get templates from: " + @"../../../Tools/Templates/Blank/Blank.exe");
                return LoadAssembly(@"../../../Tools/Templates/Blank/Blank.exe");
            }
        }

        public static AssemblyDefinition LoadAssembly(string path)
        {
            string directoryPath = "";
            
            if(path.Replace('\\','/').Contains('/'))
                directoryPath = path.Substring(0, path.Replace('\\', '/').LastIndexOf("/"));
            else
                directoryPath = Environment.CurrentDirectory;

            bool existsSymbols = File.Exists(Path.Combine(directoryPath, Path.GetFileNameWithoutExtension(path) + ".pdb"));
#if CORE		
            if (existsSymbols &&
                !File.Exists(Path.Combine(directoryPath,Path.GetFileNameWithoutExtension(path) + ".mdb")))
            {
                Utils.WriteToConsole("Executing pdb2mdb process for decompilation in mono environment");
                Process p = new Process();
                p.StartInfo = new ProcessStartInfo("bash", "-c 'pdb2mdb \"" + path + "\"'");
                p.Start();
                p.WaitForExit();
            }
            existsSymbols = File.Exists(Path.Combine(directoryPath, Path.GetFileNameWithoutExtension(path) + ".mdb"));
#endif
            //LOAD TARGET ASSEMBLY
            var resolver = new DefaultAssemblyResolver();
            resolver.AddSearchDirectory(directoryPath);

            ReaderParameters readerParams = new ReaderParameters()
            {
                ReadSymbols = existsSymbols,
                AssemblyResolver = resolver
            };            
            
            AssemblyDefinition adef = AssemblyDefinition.ReadAssembly(path, readerParams);
            Utils.WriteToConsole("Loaded Assembly " + adef.Name);
            return adef;
        }

        public static void SetLinkedLibraries(string[] args)
        {
            Config.AdditionalLibraries = new List<string>();

            for (int i = 0; i < args.Length; i++)
            {
                string arg = args[i];
                if (arg.ToLower().Equals("-l"))
                {
                    Config.AdditionalLibraries.Add(args[++i]);
                }
            }
        }

        public static void CopyLibFiles(DirectoryInfo dst)
        {
            //COPY LIB FILES            
            Utils.CopyAll(new DirectoryInfo(Config.AlterNativeLib), dst);
        }
    }
}
