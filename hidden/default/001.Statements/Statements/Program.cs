using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Statements
{
    class Program
    {
        static void Main(string[] args)
        {
            Program p = new Program();
            p.For();
            p.While();
            p.DoWhile();
        }

        public void For()
        {
            Console.WriteLine("Testing for");
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine(i);
            }
        }

        public void While()
        {
            Console.WriteLine("Testing While");
            int i = 0;
            while (i < 5)
            {
                Console.WriteLine(i);
                i++;
            }
        }

        public void DoWhile()
        {
            Console.WriteLine("Testing dowhile");
            int i = 0;
            do
            {
                Console.WriteLine(i);
                i++;
            }
            while (i < 5);
        }
    }
}
