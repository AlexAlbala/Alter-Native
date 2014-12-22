using System;

namespace HelloWorld
{
    class Hello 
    {
        public static void Main() 
        {
            Console.WriteLine("Hello World!");

            //Keep the console window open in debug mode
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
}