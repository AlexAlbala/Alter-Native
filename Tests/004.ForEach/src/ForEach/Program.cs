using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace ForEach
{
    class Program
    {
        static void Main(string[] args)
        {
            List<float> myList = new List<float>();
            myList.Add(5.6f);
            myList.Add(5.7f);
            myList.Add(5.2f);
            myList.Add(5.9f);
            myList.Add(3.6f);
            myList.Add(52.6f);
            myList.Add(523.6f);

            foreach (float f in myList)
            {
                Console.WriteLine(f);
            }
        }
    }
}
