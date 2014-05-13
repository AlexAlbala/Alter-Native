using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows;
using AlterNative.Tools;
using Antlr4.StringTemplate;

namespace AlterNative.BuildTools
{
    public class CMakeGenerator
    {
        public static void GenerateCMakeLists(string projectName, string execName, string workingDir, string[] sourceFiles, bool release = false)
        {
            string txt;
            Template template;

            Utils.WriteToConsole("Generating CMakeLists.txt for project " + projectName + " and executable " + execName);

            string altTools = Config.AlterNativeTools;
            StreamReader sr = new StreamReader(altTools + @"\Templates\CMake\CMakeLists.stg");
            txt = sr.ReadToEnd();

            template = new Template(txt);

            template.Add("RELEASE", release ? "1" : "0");
            template.Add("PROJECT_NAME", projectName);
            template.Add("EXEC", sourceFiles);
            template.Add("TARGET_NAME", execName);
            template.Add("IS_DLL", Config.targetType == TargetType.DynamicLinkLibrary ?  true : false);

            string libPub = (Environment.GetEnvironmentVariable("ALTERNATIVE_CPP_LIB_PATH") + @"\src\public").Replace('\\', '/');
            string libPrv = (Environment.GetEnvironmentVariable("ALTERNATIVE_CPP_LIB_PATH") + @"\src\private").Replace('\\', '/');
            string libDir = (Environment.GetEnvironmentVariable("ALTERNATIVE_CPP_LIB_PATH") + @"\build\libfiles");

            List<string> includes = new List<string>() { libPrv, libPub };
            template.Add("INCLUDE_DIRS", includes);
            

            DirectoryInfo di = new DirectoryInfo(libDir.Replace('\\', '/'));
            if (!di.Exists)
            {
                Utils.WriteToConsole("Library files not found. Make sure to execute script Lib/alternative-lib-compile");
            }
            List<string> libToLink = new List<string>();

            foreach (FileInfo f in di.GetFiles())
                libToLink.Add(f.FullName.Replace('\\', '/'));

            template.Add("LINK_LIBS", libToLink);

            string output = template.Render();

            StreamWriter sw = new StreamWriter(workingDir + "CMakeLists.txt");
            sw.Write(output);
            sw.Flush();
            sw.Close();


            /*
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("CMAKE_MINIMUM_REQUIRED(VERSION 2.8)");
            sb.AppendLine("PROJECT(" + projectName + " CXX)");

            FileInfo cxx11Cmake = new FileInfo((altTools + @"\Code\CMAKE-CXX11").Replace('\\','/'));
            StreamReader sr_cxx11 = new StreamReader(cxx11Cmake.FullName);
            sb.AppendLine(sr_cxx11.ReadToEnd());

                       

            sb.AppendLine("SET_PROPERTY(GLOBAL PROPERTY GL_IS_RELEASE " + (release ? "1" : "0") + ")");

            //sb.AppendLine("ADD_SUBDIRECTORY(System)");
            //sb.AppendLine("ADD_SUBDIRECTORY(gc)");
            sb.Append("SET(EXECPATH");

            foreach (string s in sourceFiles)
            {
                Utils.WriteToConsole("Source file: " + s + " added");
                sb.Append(" " + s);
            }

            sb.AppendLine(")");
            if (Config.targetType == TargetType.Executable)
                sb.AppendLine("ADD_EXECUTABLE(" + execName + " ${EXECPATH})");
            else if (Config.targetType == TargetType.DynamicLinkLibrary)
                sb.AppendLine("ADD_LIBRARY(" + execName + " ${EXECPATH})");
            else
                sb.AppendLine("ERROR");

            string libPub = (Environment.GetEnvironmentVariable("ALTERNATIVE_CPP_LIB_PATH") + @"\src\public").Replace('\\','/');
            string libPrv = (Environment.GetEnvironmentVariable("ALTERNATIVE_CPP_LIB_PATH") + @"\src\private").Replace('\\','/');
            string libDir = (Environment.GetEnvironmentVariable("ALTERNATIVE_CPP_LIB_PATH") + @"\build\libfiles");

            sb.AppendLine("INCLUDE_DIRECTORIES(" + libPub + ")");
            sb.AppendLine("INCLUDE_DIRECTORIES(" + libPrv + ")");

            DirectoryInfo di = new DirectoryInfo(libDir.Replace('\\', '/'));
            if (!di.Exists)
            {
                Utils.WriteToConsole("Library files not found. Make sure to execute script Lib/alternative-lib-compile");
            }
            List<string> libToLink = new List<string>();

            foreach (FileInfo f in di.GetFiles())
                libToLink.Add(f.FullName.Replace('\\','/'));

            foreach (string lib in libToLink)            
                sb.AppendLine("TARGET_LINK_LIBRARIES(" + execName + " " + lib +")");


            sb.AppendLine("IF(UNIX)");
            sb.AppendLine("IF(NOT ANDROID)");
            sb.AppendLine("TARGET_LINK_LIBRARIES(" + execName + " pthread)");
            sb.AppendLine("ENDIF()");
            sb.AppendLine("ENDIF(UNIX)");

            if (release)
                sb.AppendLine("SET(CMAKE_BUILD_TYPE Release)");            

            FileInfo boostCmake = new FileInfo((altTools + @"\Code\CMAKE-BOOST").Replace('\\','/'));

            StreamReader sr = new StreamReader(boostCmake.FullName);
            sb.AppendLine("SET(PROJ_NAME " + execName + ")");
            sb.Append(sr.ReadToEnd());

            StreamWriter sw = new StreamWriter(workingDir + "CMakeLists.txt");
            sw.Write(sb.ToString());
            sw.Flush();
            sw.Close();*/
        }
    }
}
