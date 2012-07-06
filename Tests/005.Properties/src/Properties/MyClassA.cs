using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Properties
{
    class MyClassA
    {
        private int _data = 6;
        public int data
        {
            get
            {
                return _data;
            }
            set
            {
                _data = value;
            }
        }

        public void CallProperties()
        {            
            data = 6;
            int value = data;
            Console.WriteLine(value);

            MyClassB b = new MyClassB();
            b.data = 5;
            int valueB = b.data;
            Console.WriteLine(valueB);
        }
    }
}
