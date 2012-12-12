using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exceptions
{
    class Program
    {
        static void Main(string[] args)
        {
            Program p = new Program();
            p.catchException();
        }

        public void ThrowException()
        {
            throw new NotImplementedException("I'm not implemented");
        }

        public void catchException()
        {
            try
            {
                ThrowException();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
