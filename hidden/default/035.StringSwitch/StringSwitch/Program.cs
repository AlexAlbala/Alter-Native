using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringSwitch
{
    class Program
    {
        static void Main(string[] args)
        {
            Program p = new Program();
            p.LessThan6CasesSwitch("case1");
            p.LessThan6CasesSwitch("case3");
            p.LessThan6CasesSwitch("case5");

            p.MoreThan6CasesSwitch("case2");
            p.MoreThan6CasesSwitch("case4");
            p.MoreThan6CasesSwitch("case6");
        }

        public void LessThan6CasesSwitch(string s)
        {
            switch (s)
            {
                case "case1":
                    Console.WriteLine("This is the case 1");
                    break;
                case "case2":
                    Console.WriteLine("This is the case 2");
                    break;
                case "case3":
                    Console.WriteLine("This is the case 3");
                    break;
                case "case4":
                    Console.WriteLine("This is the case 4");
                    break;
                default:
                    Console.WriteLine("Default case");
                    break;
            }
        }

        public void MoreThan6CasesSwitch(string s)
        {

            switch (s)
            {
                case "case1":
                    Console.WriteLine("This is the case 1");
                    break;
                case "case2":
                    Console.WriteLine("This is the case 2");
                    break;
                case "case3":
                    Console.WriteLine("This is the case 3");
                    break;
                case "case4":
                    Console.WriteLine("This is the case 4");
                    break;
                case "case5":
                    Console.WriteLine("This is the case 5");
                    break;
                case "case6":
                    Console.WriteLine("This is the case 6");
                    break;
                case "case7":
                    Console.WriteLine("This is the case 7");
                    break;
                default:
                    Console.WriteLine("Default case");
                    break;
            }
        }
    }
}
