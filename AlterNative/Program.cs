using System;
using System.Linq;
using Mono.Cecil;
using System.IO;
using ICSharpCode.NRefactory.Cpp.Formatters;
using ICSharpCode.ILSpy;
using AlterNative.BuildTools;
using AlterNative.Tools;
using System.Collections.Generic;
using System.Diagnostics;

namespace AlterNative
{
    class Program
    {
        /// <summary>
        /// Application Entry Point.
        /// </summary>
        [System.STAThreadAttribute()]
        public static int Main(string[] args)
        {
            int exitcode;
            if (args.Length > 1 && !args[0].Equals("/separate"))
            {
                try
                {
                    Program p = new Program();
                    p.ConsoleMain(args);
                    exitcode = 0;
                }
                catch (Exception e)
                {
                    Utils.WriteToConsole("Exception: " + e.ToString());
                    exitcode = 1;
                }
            }
            else
            {
#if !CORE
                ICSharpCode.ILSpy.App app = new ICSharpCode.ILSpy.App();
                app.InitializeComponent();
                app.Run();
                exitcode = 0;
#else
                exitcode = 1;
#endif
            }
            return exitcode;
        }

        #region Console
        

        private ICSharpCode.ILSpy.Language OutputLanguage(string language)
        {
            //CONFIGURE OUTPUT LANGUAGE
            ICSharpCode.ILSpy.Language lang;
            switch (language)
            {
                case "CXX":
                    lang = new ICSharpCode.ILSpy.Cpp.CppLanguage();
                    break;
                case "C#":
                    lang = new ICSharpCode.ILSpy.CSharpLanguage();
                    break;
                    //Why we can't add VB ??
#if !CORE
                case "VB":
                    lang = new ICSharpCode.ILSpy.VB.VBLanguage();
                    break;
#endif
                case "IL":
                    lang = new ICSharpCode.ILSpy.ILLanguage(true);
                    break;
                default:
                    throw new InvalidOperationException("");
            }

            Utils.WriteToConsole("Output language is " + lang.Name);
            return lang;
        }

        /// <summary>
        ///The entry point when ILSPY is called from console 
        /// </summary>
        /// <param name="args">{ assembly, destinationPath, language, Params } (In CPP: Params is the path of the library)</param>
        public void ConsoleMain(string[] args)
        {
            Commands.SetLinkedLibraries(args);

            if (System.Environment.GetEnvironmentVariable("ALTERNATIVE_HOME") == null)
            {
                Utils.WriteToConsole("ALTERNATIVE_HOME not setted, please execute alternative-init command");
            }
            else
            {
                Config.AlterNativeHome = System.Environment.GetEnvironmentVariable("ALTERNATIVE_HOME");
            }

            Utils.WriteToConsole("\n");

            string outputDir = Utils.InitOutputPath(args[1]);

            AssemblyDefinition adef = null;
            if (args[0].ToLower() == "new")
            {
                adef = Commands.NewTemplate(new DirectoryInfo(outputDir));
            }
            else if (args[0].ToLower() == "make")
            {
                DirectoryInfo buildDir = Commands.RunCMake(new DirectoryInfo(outputDir));
                Commands.Compile(buildDir);

                Console.ForegroundColor = ConsoleColor.Green;
                Utils.WriteToConsole("alternative make done");
                Console.ResetColor();
                return;
            }
            else
            {
                //LOAD TARGET ASSEMBLY
                adef = Commands.LoadAssembly(args[0].Replace('\\', '/'));
            }           
            
            if (args[0].EndsWith("dll"))
            {
                Config.targetType = TargetType.DynamicLinkLibrary;
            }
            else if (args[0].EndsWith("exe"))
            {
                Config.targetType = TargetType.Executable;
            }

            if (!Directory.Exists(outputDir))
            {
                Utils.WriteToConsole(outputDir + " does not exists. Created");
                Directory.CreateDirectory(outputDir);
            }
            else
            {
                Utils.CleanDirectory(new DirectoryInfo(outputDir));
            }

            //Each visitor is responsible of changing the file if necessary (from here it is ipmossible to know the file names)
            ICSharpCode.Decompiler.ITextOutput textOutput = new ICSharpCode.ILSpy.FileTextOutput(outputDir);
            FileWritterManager.WorkingPath = outputDir;

            //CONFIGURE OUTPUT LANGUAGE
            ICSharpCode.ILSpy.Language lang = OutputLanguage("CXX");
            

            //DECOMPILE FIRST TIME AND FILL THE TABLES
            foreach (TypeDefinition tdef in adef.MainModule.Types)
            {
                if (!tdef.Name.Contains("<"))
                {
                    lang.DecompileType(tdef, textOutput, new ICSharpCode.ILSpy.DecompilationOptions() { FullDecompilation = false });
                }
            }

            //DECOMPILE
            foreach (TypeDefinition tdef in adef.MainModule.Types)
            {
                if (!tdef.Name.Contains("<"))
                {
                    lang.DecompileType(tdef, textOutput, new ICSharpCode.ILSpy.DecompilationOptions() { FullDecompilation = false });
                    Utils.WriteToConsole("Decompiled: " + tdef.FullName);
                }
            }

            
            if (System.Environment.GetEnvironmentVariable("ALTERNATIVE_CPP_LIB_PATH") != null)
            {
                Config.AlterNativeLib = System.Environment.GetEnvironmentVariable("ALTERNATIVE_CPP_LIB_PATH");
            }
            else
            {
                Config.AlterNativeLib = @"../../../Lib/src";

                Console.WriteLine("ALTERNATIVE_CPP_LIB_PATH not defined, please execute alternative-init command");
                Console.WriteLine("Trying to locate the library at: " + Config.AlterNativeLib);
            }

            Commands.CopyLibFiles(new DirectoryInfo(outputDir));

            //TRIM END .EXE : BUG If The name is File.exe, trim end ".exe" returns Fil !!!!
            string name = adef.MainModule.Name.Substring(0, adef.MainModule.Name.Length - 4);
            if (args.Contains("-r") || args.Contains("-R"))
                CMakeGenerator.GenerateCMakeLists(name + "Proj", name, outputDir, FileWritterManager.GetSourceFiles(), true);
            else
                CMakeGenerator.GenerateCMakeLists(name + "Proj", name, outputDir, FileWritterManager.GetSourceFiles());

            Console.ForegroundColor = ConsoleColor.Green;
            Utils.WriteToConsole("Done");
            Console.ResetColor();
        }        
        #endregion

    }
}