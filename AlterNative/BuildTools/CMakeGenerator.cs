using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace AlterNative.BuildTools
{
    class CMakeGenerator
    {
        public static void GenerateCMakeLists(string projectName, string execName, string workingDir, string[] sourceFiles)
        {
            Utils.WriteToConsole("Generating CMakeLists.txt for project " + projectName + " and executable " + execName);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("CMAKE_MINIMUM_REQUIRED(VERSION 2.8)");
            sb.AppendLine("PROJECT(" + projectName + " CXX)");
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
            sb.AppendLine("TARGET_LINK_LIBRARIES(" + execName + " LIB)");
            sb.AppendLine("TARGET_LINK_LIBRARIES(" + execName + " gc-lib)");
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
    }
}
