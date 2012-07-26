using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RegressionTest
{
    public class TestResult
    {
        public bool output;
        public short diffCode;
        public short cmakeCode;
        public short msbuildCode;

        public TestResult()
        {
            output = false;
            diffCode = -1;
            cmakeCode = -1;
            msbuildCode = -1;
        }
    }
}
