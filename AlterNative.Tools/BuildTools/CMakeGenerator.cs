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
            StreamReader sr = new StreamReader((altTools + @"\Templates\CMake\CMakeLists.stg").Replace('\\','/'));
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
            else
            {
                List<string> libToLink = new List<string>();

                foreach (FileInfo f in di.GetFiles())
                    libToLink.Add(f.FullName.Replace('\\', '/'));

                if (Config.AdditionalLibraries.Any())
                    libToLink.AddRange(Config.AdditionalLibraries);

                template.Add("LINK_LIBS", libToLink);
            }

            string output = template.Render();

            StreamWriter sw = new StreamWriter(workingDir + "CMakeLists.txt");
            sw.Write(output);
            sw.Flush();
            sw.Close();
        }
    }
}
