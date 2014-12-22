using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ForwardDeclaration
{
    class ClassB
    {        
        ClassA a;

        public ClassB(ClassA a)
        {
            this.a = a;
        }

        public string SayHello()
        {
            return "Hello I am B";
        }
    }
}
