using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using System.IO;
using System.Runtime.InteropServices;
using ICSharpCode.NRefactory.Cpp.Formatters;
using ICSharpCode.ILSpy;
using AlterNative.BuildTools;

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
            ICSharpCode.ILSpy.App app = new ICSharpCode.ILSpy.App();
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
                app.InitializeComponent();
                app.Run();
                exitcode = 0;
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

        private Language OutputLanguage(string language)
        {
            //CONFIGURE OUTPUT LANGUAGE
            Language lang;
            switch (language)
            {
                case "CXX":
                    lang = new ICSharpCode.ILSpy.Cpp.CppLanguage();
                    break;
                case "C#":
                    lang = new CSharpLanguage();
                    break;
                case "VB":
                    lang = new ICSharpCode.ILSpy.VB.VBLanguage();
                    break;
                case "IL":
                    lang = new ILLanguage(true);
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
            Utils.WriteToConsole("\n");

            //LOAD TARGET ASSEMBLY
            AssemblyDefinition adef = LoadAssembly(args[0].Replace('\\', '/'));

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
            ICSharpCode.Decompiler.ITextOutput textOutput = new FileTextOutput(outputDir);
            FileWritterManager.WorkingPath = outputDir;

            //CONFIGURE OUTPUT LANGUAGE
            Language lang = OutputLanguage(args[2]);

            //DECOMPILE FIRST TIME AND FILL THE TABLES
            foreach (TypeDefinition tdef in adef.MainModule.Types)
            {
                if (!tdef.Name.Contains("<"))
                {
                    lang.DecompileType(tdef, textOutput, new DecompilationOptions() { FullDecompilation = true });
                }
            }

            //DECOMPILE
            foreach (TypeDefinition tdef in adef.MainModule.Types)
            {
                if (!tdef.Name.Contains("<"))
                {
                    lang.DecompileType(tdef, textOutput, new DecompilationOptions() { FullDecompilation = true });
                    Utils.WriteToConsole("Decompiled: " + tdef.FullName);
                }
            }

            //COPY LIB FILES IF NECESSARY
            if (args.Length >= 4 && args[3] != "")
                CopyAll(new DirectoryInfo(args[3].Replace('\\', '/')), new DirectoryInfo(outputDir));

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
