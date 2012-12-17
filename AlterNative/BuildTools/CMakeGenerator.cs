using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows;

namespace AlterNative.BuildTools
{
    class CMakeGenerator
    {
        public static void GenerateCMakeLists(string projectName, string execName, string workingDir, string[] sourceFiles, bool release = false)
        {
            Utils.WriteToConsole("Generating CMakeLists.txt for project " + projectName + " and executable " + execName);

            DirectoryInfo di = new DirectoryInfo(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\..\..\..\Tools\External Libraries");

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("CMAKE_MINIMUM_REQUIRED(VERSION 2.8)");
            sb.AppendLine("PROJECT(" + projectName + " CXX)");
            sb.AppendLine("SET_PROPERTY(GLOBAL PROPERTY EXT_LIB_PATH \"" + di.FullName.Replace('\\', '/') + "\")");

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

            //if (ICSharpCode.NRefactory.Cpp.Resolver.boostLink)
            //{
                FileInfo boostCmake = new FileInfo(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\..\..\..\Tools\Code\CMAKE-BOOST");
                StreamReader sr = new StreamReader(boostCmake.FullName);
                sb.AppendLine("SET(PROJ_NAME " + execName + ")");
                sb.Append(sr.ReadToEnd());

            //}

            StreamWriter sw = new StreamWriter(workingDir + "CMakeLists.txt");
            sw.Write(sb.ToString());
            sw.Flush();
            sw.Close();
        }
    }
}
