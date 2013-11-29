using System;
using System.Linq;
using Mono.Cecil;
using System.IO;
using ICSharpCode.NRefactory.Cpp.Formatters;
#if !CONSOLE
using ICSharpCode.ILSpy;
#endif
using AlterNative.BuildTools;
using AlterNative.Tools;

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
        private AssemblyDefinition LoadAssembly(string path)
        {
            //LOAD TARGET ASSEMBLY            
            ReaderParameters readerParams = new ReaderParameters() { ReadSymbols = true };
            AssemblyDefinition adef = AssemblyDefinition.ReadAssembly(path, readerParams);
            Utils.WriteToConsole("Loaded Assembly " + adef.Name);
            return adef;
        }

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
            if (System.Environment.GetEnvironmentVariable("ALTERNATIVE_BIN_PATH") != null)
            {
                Utils.WriteToConsole("ALTERNATIVE_BIN_PATH not setted, please execute alternative-init command");
            }

            Utils.WriteToConsole("\n");

            AssemblyDefinition adef = null;
            if (args[0].ToLower() == "new")
            {
                Utils.WriteToConsole("Creating blank template...");
                if (System.Environment.GetEnvironmentVariable("ALTERNATIVE_BIN_PATH") != null)
                {
                    adef = LoadAssembly(Environment.GetEnvironmentVariable("ALTERNATIVE_BIN_PATH")
                                                    + @"../../../../Tools/Templates/Blank/Blank.exe");
                }
                else
                {
                    Utils.WriteToConsole("WARNING: ALTERNATIVE_BIN_PATH not setted");
#if CORE
                    adef = LoadAssembly(@"../../../../Tools/Templates/Blank/Blank.exe");
                    Utils.WriteToConsole("Trying to get templates from: " + @"../../../../Tools/Templates/Blank/Blank.exe");
#else
                    adef = LoadAssembly(@"../../../Tools/Templates/Blank/Blank.exe");
                    Utils.WriteToConsole("Trying to get templates from: " + @"../../../Tools/Templates/Blank/Blank.exe");
               
#endif
                }
            }
            else
            {
                //LOAD TARGET ASSEMBLY
                adef = LoadAssembly(args[0].Replace('\\', '/'));
            }

            //CONFIGURE OUTPUT PATH           
            string outputDir = args[1].Replace('\\', '/');

            if (!outputDir.EndsWith("/"))
                outputDir += "/";

            if (!Directory.Exists(outputDir))
            {
                Utils.WriteToConsole(outputDir + " does not exists. Created");
                Directory.CreateDirectory(outputDir);
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
                    lang.DecompileType(tdef, textOutput, new ICSharpCode.ILSpy.DecompilationOptions() { FullDecompilation = true });
                }
            }

            //DECOMPILE
            foreach (TypeDefinition tdef in adef.MainModule.Types)
            {
                if (!tdef.Name.Contains("<"))
                {
                    lang.DecompileType(tdef, textOutput, new ICSharpCode.ILSpy.DecompilationOptions() { FullDecompilation = true });
                    Utils.WriteToConsole("Decompiled: " + tdef.FullName);
                }
            }

            string libPath = "";
            if (System.Environment.GetEnvironmentVariable("ALTERNATIVE_CPP_LIB_PATH") != null)
            {
                libPath = System.Environment.GetEnvironmentVariable("ALTERNATIVE_CPP_LIB_PATH");
            }
            else
            {
#if CORE
                libPath = @"../../../../Lib/src";
#else
                libPath = @"../../../Lib/src";
#endif
                Console.WriteLine("ALTERNATIVE_CPP_LIB_PATH not defined, please execute alternative-init command");
                Console.WriteLine("Trying to locate the library at: " + libPath);
            }
            //COPY LIB FILES            
            CopyAll(new DirectoryInfo(libPath), new DirectoryInfo(outputDir));

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

        public void CopyAll(DirectoryInfo source, DirectoryInfo target)
        {
            // Copy each file into it’s new directory.
            foreach (FileInfo fi in source.GetFiles())
            {
                Utils.WriteToConsole(@"Copying: " + target.FullName + "/" + fi.Name);
                fi.CopyTo(System.IO.Path.Combine(target.ToString(), fi.Name), true);
            }

            // Copy each subdirectory using recursion.
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir =
                    target.CreateSubdirectory(diSourceSubDir.Name);


                CopyAll(diSourceSubDir, nextTargetSubDir);
            }
        }
        #endregion

    }
}