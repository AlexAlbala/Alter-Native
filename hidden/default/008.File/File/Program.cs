using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace File
{
    class Program
    {
        static void Main(string[] args)
        {
            Program p = new Program();
            p.Run();
        }

        private void Run()
        {
            StreamWriter sw = new StreamWriter("test.txt");
            sw.WriteLine("Hi! I'm a stream writer");
            sw.Flush();
            sw.Close();

            StreamReader sr = new StreamReader("test.txt");
            string text = sr.ReadToEnd();
            sr.Close();

            Console.WriteLine(text);
        }
    }
}
