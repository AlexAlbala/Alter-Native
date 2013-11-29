using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows;
using AlterNative.Tools;

namespace AlterNative.BuildTools
{
    public class CMakeGenerator
    {
        public static void GenerateCMakeLists(string projectName, string execName, string workingDir, string[] sourceFiles, bool release = false)
        {
            Utils.WriteToConsole("Generating CMakeLists.txt for project " + projectName + " and executable " + execName);           

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("CMAKE_MINIMUM_REQUIRED(VERSION 2.8)");
           

            /*FileInfo v120Cmake = new FileInfo(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\..\..\..\Tools\Code\CMAKE-vs120");
            StreamReader srv120 = new StreamReader(v120Cmake.FullName);
            sb.AppendLine(srv120.ReadToEnd());*/

            sb.AppendLine("PROJECT(" + projectName + " CXX)");
            
            sb.AppendLine("SET_PROPERTY(GLOBAL PROPERTY GL_IS_RELEASE " + (release ? "1" : "0") + ")");

            sb.AppendLine("ADD_SUBDIRECTORY(System)");
            sb.AppendLine("ADD_SUBDIRECTORY(gc)");
            sb.Append("SET(EXECPATH");

            foreach (string s in sourceFiles)
            {
                Utils.WriteToConsole("Source file: " + s + " added");
                sb.Append(" " + s);
            }

            sb.AppendLine(")");
            sb.AppendLine("ADD_EXECUTABLE(" + execName + " ${EXECPATH})");
            sb.AppendLine("TARGET_LINK_LIBRARIES(" + execName + " System)");
            sb.AppendLine("TARGET_LINK_LIBRARIES(" + execName + " gc-lib)");
            sb.AppendLine("IF(UNIX)");
            sb.AppendLine("IF(!ANDROID)");
            sb.AppendLine("TARGET_LINK_LIBRARIES(" + execName + " pthread)");
            sb.AppendLine("ENDIF()");
            sb.AppendLine("ENDIF(UNIX)");

            if (release)
                sb.AppendLine("SET(CMAKE_BUILD_TYPE Release)");
            
#if CORE
            FileInfo boostCmake = new FileInfo(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"/../../../../Tools/Code/CMAKE-BOOST");
#else
            FileInfo boostCmake = new FileInfo(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"/../../../Tools/Code/CMAKE-BOOST");
#endif
            StreamReader sr = new StreamReader(boostCmake.FullName);
            sb.AppendLine("SET(PROJ_NAME " + execName + ")");
            sb.Append(sr.ReadToEnd());

            StreamWriter sw = new StreamWriter(workingDir + "CMakeLists.txt");
            sw.Write(sb.ToString());
            sw.Flush();
            sw.Close();
        }
    }
}
