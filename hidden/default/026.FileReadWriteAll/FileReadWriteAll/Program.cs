using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace FileReadWriteAll
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
            File.WriteAllText("test.txt", "Hello I'm a pice of Text!And I'm a breaked line!");
            Console.WriteLine(File.ReadAllText("test.txt"));
        }
    }
}
