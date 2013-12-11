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
        public static void RunCMake(DirectoryInfo di)
        {
            di.CreateSubdirectory("build");
            Environment.CurrentDirectory = di.FullName + "build";
            Process p = new Process();
#if CORE
            p.StartInfo = new ProcessStartInfo("cmake", "-G \"Unix Makefiles\" ..");
#else
            p.StartInfo = new ProcessStartInfo("cmake", "..");
#endif
            p.Start();
            p.WaitForExit();
            return;
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
#if CORE
                Utils.WriteToConsole("Trying to get templates from: " + @"../../../../Tools/Templates/Blank/Blank.exe");
                return LoadAssembly(@"../../../../Tools/Templates/Blank/Blank.exe");
#else
                Utils.WriteToConsole("Trying to get templates from: " + @"../../../Tools/Templates/Blank/Blank.exe");
                return LoadAssembly(@"../../../Tools/Templates/Blank/Blank.exe");
                
#endif
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
