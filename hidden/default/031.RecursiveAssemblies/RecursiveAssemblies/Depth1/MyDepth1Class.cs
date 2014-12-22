using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Depth2;

namespace Depth1
{
    public class MyDepth1Class
    {
        MyDepth2Class c = new MyDepth2Class();

        public string Ping(string message)
        {
            return c.Ping(message);
        }
    }
}
