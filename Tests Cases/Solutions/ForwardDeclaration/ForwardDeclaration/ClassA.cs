using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ForwardDeclaration
{
    class ClassA
    {
        ClassB b;
        public ClassA()
        {
            b = new ClassB(this);
        }

        public string SayHello()
        {
            return b.SayHello();
        }
    }
}
