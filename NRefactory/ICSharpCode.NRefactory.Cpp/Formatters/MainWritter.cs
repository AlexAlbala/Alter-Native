using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using ICSharpCode.NRefactory.Cpp.Formatters;
using Antlr4.StringTemplate;

namespace ICSharpCode.NRefactory.Cpp
{
    public class MainWritter
    {
        public static void GenerateMain(string entryType, string entryNamespace, bool inputArgs)
        {
            StreamWriter writer = new StreamWriter(FileWritterManager.WorkingPath + "main.cpp");

            string txt;
            Template template;

            string altTools = Environment.GetEnvironmentVariable("ALTERNATIVE_TOOLS_PATH");

            if(altTools == null)
                altTools = Path.Combine(Environment.CurrentDirectory, (@"..\..\..\Tools").Replace('\\','/'));

            StreamReader sr = new StreamReader((altTools + @"\Templates\Code\main.stg").Replace('\\', '/'));
            txt = sr.ReadToEnd();
            template = new Template(txt);

            StreamReader fs = new StreamReader(Path.Combine(altTools, "Text/notice"));
            string notice = fs.ReadToEnd();

            template.Add("NOTICE", notice);
            template.Add("ENTRY_TYPE", entryType);
            template.Add("ENTRY_NAMESPACE_NEEDED", !String.IsNullOrEmpty(entryNamespace));
            template.Add("ENTRY_NAMESPACE", entryNamespace);
            template.Add("INPUT_ARGS", inputArgs);

            string output = template.Render();

            writer.Write(output);
            writer.Flush();
            writer.Close();

            FileWritterManager.AddSourceFile("main.cpp");
        }
    }
}
