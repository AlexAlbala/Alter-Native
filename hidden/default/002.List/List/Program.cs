using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace List
{
    class Program
    {
        MyList list;

        static void Main(string[] args)
        {
            Program p = new Program();
            p.Run();
            p.printList();
            p.Sort();
            p.printList();
        }

        void Run()
        {
            list = new MyList();

            for (int i = 0; i < 100; i++)
                list.Add(new Node());
        }

        void Sort()
        {
            list.BubbleSort();
        }

        void printList()
        {
            Console.WriteLine("****************");
            for (int i = 0; i < list.Length(); i++)
            {
                Console.Write("Node ");
                Console.Write(i);
                Console.Write(": ");
                Console.WriteLine(list.getElementAt(i).value);                
            }
        }


    }
}
