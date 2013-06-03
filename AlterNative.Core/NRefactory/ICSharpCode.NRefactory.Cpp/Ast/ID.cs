using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ICSharpCode.NRefactory.Cpp
{
    public static class ID<T>
    {
        static long id = 0;
        static public long generateNewId()
        {
            return System.Threading.Interlocked.Increment(ref id);
        }
    }
}
