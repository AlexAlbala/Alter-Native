using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RegressionTest
{
    public interface ITest
    {
        void Alternative(DirectoryInfo di, TestResult res);
        void Make(DirectoryInfo di, TestResult res);
        void CompareOutputs(DirectoryInfo di, TestResult res);        
    }
}
