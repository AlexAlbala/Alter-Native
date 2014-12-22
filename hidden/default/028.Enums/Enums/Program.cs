using System;

namespace Enums
{
    public enum MyEnum : int
    {
        NONE = 0,
        ONE = 1
    }

    public enum MyDisorderedEnum : int
    {
        ONE = 1,
        NONE = 0
    }

    public enum MyIncompleteEnum : int
    {
        SEVEN = 7,
        EIGHT = 8
    }

    public class Program
    {

        public static void Main()
        {
            MyEnum e = MyEnum.NONE;

            if (e == MyEnum.NONE)
            {
                Console.WriteLine("Uquality working");
            }

            e = (MyEnum)1;
            Console.WriteLine("case 1 -> should print \"ONE\": ");
            Console.WriteLine(e);

            int result = (int)e;
            Console.WriteLine("case 2 -> should print \"1\": ");
            Console.WriteLine(result);

            MyIncompleteEnum ie = MyIncompleteEnum.SEVEN;

            if (ie == MyIncompleteEnum.SEVEN)
            {
                Console.WriteLine("Uquality working");
            }

            ie = (MyIncompleteEnum)7;
            Console.WriteLine("case 3 -> should print \"SEVEN\": ");
            Console.WriteLine(ie);

            int result2 = (int)ie;
            Console.WriteLine("case 4 -> should print \"7\": ");
            Console.WriteLine(result2);

            MyDisorderedEnum de = MyDisorderedEnum.ONE;

            if (de == MyDisorderedEnum.ONE)
            {
                Console.WriteLine("Uquality working");
            }

            de = (MyDisorderedEnum)1;
            Console.WriteLine("case 5 -> should print \"ONE\": ");
            Console.WriteLine(de);

            int result3 = (int)de;
            Console.WriteLine("case 6 -> should print \"1\": ");
            Console.WriteLine(result3);
        }
    }
}