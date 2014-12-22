using System;
using System.Collections.Generic;

namespace Structs
{
    struct Coordinate
    {
        public int x;
        public int y;

        public Coordinate(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

    class Program
    {
        public static void f(Coordinate c)
        {
            c.x = 3;
            c.y = 4;
            Console.WriteLine("c: " + c.x + "  " + c.y);
        }

        public static void Main(string[] args)
        {
            Coordinate defaultValue = new Coordinate();

            Coordinate c = new Coordinate(1, 2);
            //c.x = 1;
            //c.y = 2;
            f(c);
            Console.WriteLine("c: " + c.x + "  " + c.y);

            List<Object> myList = new List<object>();
            myList.Add(new Coordinate(0, 0));
            myList.Add(new Coordinate(1, 1));
        }
    }
}
