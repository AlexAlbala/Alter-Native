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
using Mono.Options;

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

#if !CORE
            if (args.Length > 0)
            {
                try
                {
                    Program p = new Program();
                    exitcode = p.ConsoleMain(args);

                }
                catch (Exception e)
                {
                    Utils.WriteToConsole("Exception: " + e.ToString());
                    exitcode = 1;
                }
            }
            else
            {
                ICSharpCode.ILSpy.App app = new ICSharpCode.ILSpy.App();
                app.InitializeComponent();
                app.Run();
                exitcode = 0;
            }
#else
            if (args.Length >= 0)
            {
                try
                {
                    Program p = new Program();
                    exitcode = p.ConsoleMain(args);

                }
                catch (Exception e)
                {
                    Utils.WriteToConsole("Exception: " + e.ToString());
                    exitcode = 1;
                }
            }
            else
            {
                exitcode = 1;
            }
#endif

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
        public int ConsoleMain(string[] args)
        {
            bool show_help = false;

            var opts = new OptionSet() {
            {"v", "Increase the verbosity of debug messages",
            v => Config.Verbose = v!= null},
            {"r", "Compiles in release mode",
            v => Config.Release = v!= null},
            {"R", "Recursive mode. The dependencies are decompiled",
            v => Config.RecursiveDependencies = v != null},
            { "h|help",  "show this message and exit", 
            v => show_help = v != null }
            };

            try
            {
                Config.Extra = opts.Parse(args);
            }
            catch (OptionException e)
            {
                Utils.WriteToConsole("alternative: ");
                Utils.WriteToConsole(e.Message);
                Utils.WriteToConsole("Try `alternative --help' for more information.");
                return -1;
            }

            if (show_help)
            {
                ShowHelp(opts);
                return -1;
            }

            if (Config.Extra.Count == 0)
            {
                Console.Write("alternative: ");
                Utils.WriteToConsole("Command not specified.");
                Utils.WriteToConsole("Try `alternative --help' for more information.");
                return -1;
            }
            Config.Command = Config.Extra[0];
            if (!Config.Command.Equals("new") && !Config.Command.Equals("make"))
            {
                Config.Command = "translate";
            }
            else
            {
                Config.Extra.RemoveAt(0);
            }

            int errorCode = 0;
            switch (Config.Command)
            {
                case "translate":
                    if (Config.Extra.Count == 2)
                    {
                        Config.OutputPath = Config.Extra[1];
                        errorCode = Run();
                    }
                    else
                    {
                        Console.Write("alternative: ");
                        Utils.WriteToConsole("Command 'translate' requires 2 parameters.");
                        Utils.WriteToConsole("Try `alternative --help' for more information.");
                        return -1;
                    }
                    break;
                case "new":
                    if (Config.Extra.Count == 1)
                    {
                        Config.OutputPath = Config.Extra[0];
                        errorCode = Run();
                    }
                    else
                    {
                        Console.Write("alternative: ");
                        Utils.WriteToConsole("Command 'new' requires 1 parameter.");
                        Utils.WriteToConsole("Try `alternative --help' for more information.");
                        return -1;
                    }
                    break;
                case "make":
                    if (Config.Extra.Count == 1)
                    {
                        Config.OutputPath = Config.Extra[0];
                        errorCode = Run();
                    }
                    else
                    {
                        Console.Write("alternative: ");
                        Utils.WriteToConsole("Command 'make' requires 1 parameter.");
                        Utils.WriteToConsole("Try `alternative --help' for more information.");
                        return -1;
                    }
                    break;
            }

            return errorCode;
        }

        private void DecompileAssembly(AssemblyDefinition adef, string outputDir, string location)
        {
            Utils.WriteToConsole("Decompiling assembly: " + adef.FullName);
            //Each visitor is responsible of changing the file if necessary (from here it is ipmossible to know the file names)
            ICSharpCode.Decompiler.ITextOutput textOutput = new ICSharpCode.ILSpy.FileTextOutput(outputDir);
            FileWritterManager.WorkingPath = outputDir;

            //CONFIGURE OUTPUT LANGUAGE
            ICSharpCode.ILSpy.Language lang = OutputLanguage("CXX");

            if (Config.RecursiveDependencies)
            {
                var resolver = new DefaultAssemblyResolver();
                Utils.WriteToConsole("Adding " + location + " to resolver search directories");
                resolver.AddSearchDirectory(location);
                foreach (AssemblyNameReference anref in adef.MainModule.AssemblyReferences)
                {
                    if (!Config.IgnoreReferences.Contains(anref.Name))
                    {
                        AssemblyDefinition assembly = resolver.Resolve(anref);

                        //TODO: Change directory ?
                        DecompileAssembly(assembly, outputDir, location);
                        if (assembly == null)
                        {
                            Utils.WriteToConsole("alternative: ");
                            Utils.WriteToConsole("ERROR - could not resolve assembly " + anref.FullName + " .");
                        }
                    }
                }
            }

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
        }

        public int Run()
        {
            String assemblyLocation = "";
            Utils.WriteToConsole("\n");
            Utils.WriteToConsole("Executing alternative command --> " + Config.Command);

            if (System.Environment.GetEnvironmentVariable("ALTERNATIVE_TOOLS_PATH") == null)
            {
                Utils.WriteToConsole("ALTERNATIVE_TOOLS_PATH not setted, please execute alternative-init command");
            }
            else
            {
                Config.AlterNativeTools = System.Environment.GetEnvironmentVariable("ALTERNATIVE_TOOLS_PATH");
            }            

            string outputDir = Utils.InitOutputPath(Config.OutputPath);

            AssemblyDefinition adef = null;
            if (Config.Command == "new")
            {
                adef = Commands.NewTemplate(new DirectoryInfo(outputDir));
                assemblyLocation = Config.AlterNativeTools + @"/Templates/Blank";
            }
            else if (Config.Command == "make")
            {
                int cmakeCode;
                DirectoryInfo buildDir = Commands.RunCMake(new DirectoryInfo(outputDir), out cmakeCode);
                int compileCode = Commands.Compile(buildDir);


                ConsoleColor current = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Green;
                Utils.WriteToConsole("alternative make done");
                Console.ForegroundColor = current;
                if (cmakeCode == 0 && compileCode == 0)
                    return 0;
                else if (cmakeCode == 0 && compileCode != 0)
                    return -1;
                else
                    return -2;
            }
            else
            {
                //LOAD TARGET ASSEMBLY
                adef = Commands.LoadAssembly(Config.Extra[0].Replace('\\', '/'));
                assemblyLocation = Config.Extra[0].Substring(0, Config.Extra[0].Replace('\\', '/').LastIndexOf('/')).Replace('\\', '/');
            }

            if (Config.Extra[0].EndsWith("dll"))
            {
                Config.targetType = TargetType.DynamicLinkLibrary;
            }
            else if (Config.Extra[0].EndsWith("exe"))
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

            try
            {
                DecompileAssembly(adef, outputDir, assemblyLocation);

                if (System.Environment.GetEnvironmentVariable("ALTERNATIVE_CPP_LIB_PATH") != null)
                {
                    Config.AlterNativeLib = System.Environment.GetEnvironmentVariable("ALTERNATIVE_CPP_LIB_PATH");
                }
                else
                {
                    Config.AlterNativeLib = @"../../../Lib/src";

                    Utils.WriteToConsole("ALTERNATIVE_CPP_LIB_PATH not defined, please execute alternative-init command");
                    Utils.WriteToConsole("Trying to locate the library at: " + Config.AlterNativeLib);
                }

                // Commands.CopyLibFiles(new DirectoryInfo(outputDir));
                //TRIM END .EXE : BUG If The name is File.exe, trim end ".exe" returns Fil !!!!
                string name = adef.MainModule.Name.Substring(0, adef.MainModule.Name.Length - 4);

                if (Config.Release)
                    CMakeGenerator.GenerateCMakeLists(name + "Proj", name, outputDir, FileWritterManager.GetSourceFiles(), true);
                else
                    CMakeGenerator.GenerateCMakeLists(name + "Proj", name, outputDir, FileWritterManager.GetSourceFiles());

#if !CORE
                Console.ForegroundColor = ConsoleColor.Green;
#endif
                Utils.WriteToConsole("Done");
#if !CORE
                Console.ResetColor();
#endif
                return 0;
            }
            catch (Exception e)
            {
                Utils.WriteToConsole("alternative: ");
                Utils.WriteToConsole(e.Message);
                Utils.WriteToConsole(e.StackTrace);
                return -1;
            }
        }
        #endregion

        static void ShowHelp(OptionSet p)
        {
            Utils.WriteToConsole("Usage: alternative <command> <parameters>");
            Utils.WriteToConsole("Translates a .NET assembly to C++11.");
            Utils.WriteToConsole("\n");
#if !CORE
            Utils.WriteToConsole("  alternative");
            Utils.WriteToConsole("    Opens the visual gui for alternative (only windows)");
#endif
            Utils.WriteToConsole("  alternative <INPUT-ASSEMBLY> <PROJECT-DIR>");
            Utils.WriteToConsole("      Translates a .NET assembly.");
            Utils.WriteToConsole("  alternative new <PROJECT-DIR>");
            Utils.WriteToConsole("      Creates a new empty project.");
            Utils.WriteToConsole("  alternative make <PROJECT-DIR>");
            Utils.WriteToConsole("      Builds the project.");
            Utils.WriteToConsole("\n");
            Utils.WriteToConsole("Options:");
            p.WriteOptionDescriptions(Console.Out);
        }

    }
}
