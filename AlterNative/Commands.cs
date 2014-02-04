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
        public static DirectoryInfo RunCMake(DirectoryInfo di)
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

            p.StartInfo.RedirectStandardOutput = true;
            p.OutputDataReceived += (sender, args) => Utils.WriteToConsole(args.Data);

            p.Start();
            p.BeginOutputReadLine();
            p.WaitForExit();

            DirectoryInfo buildDir = new DirectoryInfo(di.FullName + "build");
            return buildDir;
        }

        public static void Compile(DirectoryInfo di)
        {
            Environment.CurrentDirectory = di.FullName;
#if CORE
            Process make = new Process();
            make.StartInfo = new ProcessStartInfo("make");
            make.StartInfo.UseShellExecute = false;
            make.StartInfo.CreateNoWindow = true;    
            make.StartInfo.RedirectStandardOutput = true;
            make.OutputDataReceived += (sender, args) => Utils.WriteToConsole(args.Data);
            make.Start();
            make.BeginOutputReadLine();
            make.WaitForExit();
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
                msbuild.StartInfo.RedirectStandardOutput = true;
                msbuild.OutputDataReceived += (sender, args) => Utils.WriteToConsole(args.Data);
                msbuild.Start();
                msbuild.BeginOutputReadLine();
                msbuild.WaitForExit();
            }
            else
            {
                Utils.WriteToConsole("No .sln file found in " + di.FullName);
            }
#endif

        }

        public static AssemblyDefinition NewTemplate(DirectoryInfo output)
        {
            Utils.WriteToConsole("Creating blank template...");
            if (Config.AlterNativeHome != null)
            {
                Config.targetType = TargetType.Executable;
                return LoadAssembly(Config.AlterNativeHome + @"/Tools/Templates/Blank/Blank.exe");
            }
            else
            {
                Utils.WriteToConsole("WARNING: ALTERNATIVE_HOME not setted");

                Utils.WriteToConsole("Trying to get templates from: " + @"../../../Tools/Templates/Blank/Blank.exe");
                return LoadAssembly(@"../../../Tools/Templates/Blank/Blank.exe");
            }
        }

        public static AssemblyDefinition LoadAssembly(string path)
        {
            //LOAD TARGET ASSEMBLY
            var resolver = new DefaultAssemblyResolver();
            resolver.AddSearchDirectory(path.Substring(0, path.Replace('\\', '/').LastIndexOf("/")));

            ReaderParameters readerParams = new ReaderParameters()
            {
                ReadSymbols = true,
                AssemblyResolver = resolver
            };
            AssemblyDefinition adef = AssemblyDefinition.ReadAssembly(path, readerParams);
            Utils.WriteToConsole("Loaded Assembly " + adef.Name);
            return adef;
        }

        public static void SetLinkedLibraries(string[] args)
        {
            Config.addedLibs = new List<string>();

            for (int i = 0; i < args.Length; i++)
            {
                string arg = args[i];
                if (arg.ToLower().Equals("-l"))
                {
                    Config.addedLibs.Add(args[++i]);
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
