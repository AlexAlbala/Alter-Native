using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using System.IO;
using System.Runtime.InteropServices;
using ICSharpCode.NRefactory.Cpp.Formatters;
using ICSharpCode.ILSpy;

namespace AlterNative
{
    class Program
    {
        /// <summary>
        /// Application Entry Point.
        /// </summary>
        [System.STAThreadAttribute()]
        public static void Main(string[] args)
        {
            ICSharpCode.ILSpy.App app = new ICSharpCode.ILSpy.App();
            if (args.Length > 1 && !args[0].Equals("/separate"))
            {
                Program p = new Program();
                p.ConsoleMain(args);
            }
            else
            {
                app.InitializeComponent();
                app.Run();
            }
        }

        #region Console
        private AssemblyDefinition LoadAssembly(string path)
        {
            //LOAD TARGET ASSEMBLY            
            ReaderParameters readerParams = new ReaderParameters() { ReadSymbols = true };
            AssemblyDefinition adef = AssemblyDefinition.ReadAssembly(path, readerParams);
            WriteToConsole("Loaded Assembly " + adef.Name);
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

            WriteToConsole("Output language is " + lang.Name);
            return lang;
        }

        /// <summary>
        ///The entry point when ILSPY is called from console 
        /// </summary>
        /// <param name="args">{ assembly, destinationPath, language, Params } (In CPP: Params is the path of the library)</param>
        public void ConsoleMain(string[] args)
        {
            WriteToConsole("\n");

            //LOAD TARGET ASSEMBLY
            AssemblyDefinition adef = LoadAssembly(args[0].Replace('\\', '/'));

            //CONFIGURE OUTPUT PATH           
            string outputDir = args[1].Replace('\\', '/');

            if (!outputDir.EndsWith("/"))
                outputDir += "/";

            if (!Directory.Exists(outputDir))
            {
                WriteToConsole(outputDir + " does not exists. Created");
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
                if (tdef.Name != "<Module>")
                {
                    lang.DecompileType(tdef, textOutput, new DecompilationOptions() { FullDecompilation = true });
                }
            }

            //DECOMPILE
            foreach (TypeDefinition tdef in adef.MainModule.Types)
            {
                if (tdef.Name != "<Module>")
                {
                    lang.DecompileType(tdef, textOutput, new DecompilationOptions() { FullDecompilation = true });
                    WriteToConsole("Decompiled: " + tdef.FullName);
                }
            }

            //COPY LIB FILES IF NECESSARY
            if (args.Length >= 4 && args[3] != "")
                CopyAll(new DirectoryInfo(args[3].Replace('\\', '/')), new DirectoryInfo(outputDir));

            string name = adef.MainModule.Name.TrimEnd(new char[] { '.', 'e', 'x', 'e' });
            GenerateCMakeLists(name + "Proj",name, outputDir, FileWritterManager.GetSourceFiles());

            Console.ForegroundColor = ConsoleColor.Green;
            WriteToConsole("Done");
            Console.ResetColor();
        }

        public void CopyAll(DirectoryInfo source, DirectoryInfo target)
        {
            // Copy each file into it’s new directory.
            foreach (FileInfo fi in source.GetFiles())
            {
                WriteToConsole(@"Copying: " + target.FullName + "/" + fi.Name);
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

        private void GenerateCMakeLists(string projectName, string execName, string workingDir, string[] sourceFiles)
        {
            WriteToConsole("Generating CMakeLists.txt for project " + projectName + " and executable " + execName);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("CMAKE_MINIMUM_REQUIRED(VERSION 2.8)");
            sb.AppendLine("PROJECT(" + projectName + " CXX)");
            sb.AppendLine("ADD_SUBDIRECTORY(System)");
            sb.Append("SET(EXECPATH");

            foreach (string s in sourceFiles)
            {
                WriteToConsole("Source file: " + s + " added");
                sb.Append(" " + s);
            }

            sb.AppendLine(")");
            sb.AppendLine("ADD_EXECUTABLE(" + execName + " ${EXECPATH})");
            sb.AppendLine("TARGET_LINK_LIBRARIES(" + execName + " LIB)");
            sb.AppendLine("IF(UNIX)");
            sb.AppendLine("IF(!ANDROID)");
            sb.AppendLine("TARGET_LINK_LIBRARIES(" + execName + " pthread)");
            sb.AppendLine("ENDIF()");
            sb.AppendLine("ENDIF(UNIX)");

            StreamWriter sw = new StreamWriter(workingDir + "CMakeLists.txt");
            sw.Write(sb.ToString());
            sw.Flush();
            sw.Close();
        }

        public void WriteToConsole(string message)
        {
            AttachConsole(-1);
            Console.WriteLine(message);
        }

        [DllImport("Kernel32.dll")]
        public static extern bool AttachConsole(int processId);

        #endregion

    }
}
