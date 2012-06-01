using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ICSharpCode.NRefactory.Cpp.Formatters
{
    //TODO: THIS CLASS SHOULD NOT EXIST !!!
    //HOW TO SHARE THIS INFO FROM COMMAND LINE TO VISITORS ?
    //IF THIS CLASS EXISTS, IN EACH LANGUAGE IT IS NECESSARY OTHER LIKE THIS
    public class FileWritterManager
    {
        public static string WorkingPath { get; set; }
        private static List<string> Files;

        static FileWritterManager()
        {
            Files = new List<string>();
        }

        public static void AddSourceFile(string file)
        {
            Files.Add(file);
        }

        public static string[] GetSourceFiles()
        {
            return Files.ToArray();
        }

    }
}
