using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;

namespace AlterNative.Tools
{
    public class Utils
    {
        public static void CopyAll(DirectoryInfo source, DirectoryInfo target)
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

        public static void CleanDirectory(DirectoryInfo d, bool cleanRoot = false)
        {
            try
            {
                if (d.Exists)
                {
                    foreach (DirectoryInfo di in d.GetDirectories())
                        CleanDirectory(di, true);

                    foreach (FileInfo fi in d.GetFiles())
                        fi.Delete();

                    if (cleanRoot)
                        d.Delete();
                }
            }
            catch (IOException e)
            {
                Utils.WriteToConsole("IOException: " + e.Message);
            }
        }

        public static string InitOutputPath(string path)
        {
            //CONFIGURE OUTPUT PATH           
            string outputDir = path.Replace('\\', '/');

            if (!outputDir.EndsWith("/"))
                outputDir += "/";

            return outputDir;
        }


        public static void WriteToConsole(string message)
        {
#if !CORE
            AttachConsole(-1);
#endif
            Console.Out.WriteLine(message);        
        }

#if !CORE
        [DllImport("Kernel32.dll")]
        public static extern bool AttachConsole(int processId);
#endif
    }
}
