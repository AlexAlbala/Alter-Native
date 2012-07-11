using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Properties
{
    class MyClassB
    {
        private int _data = 3;
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
    }
}
