using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RegressionTest
{
    public class TestResult
    {
        public bool output;
        public bool fileDiff;
        public short cmakeCode;
        public short msbuildCode;

        public TestResult()
        {
            output = false;
            fileDiff = true;
            cmakeCode = -1;
            msbuildCode = -1;
        }
    }
}
