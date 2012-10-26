using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace RegressionTest
{
    public interface ITest
    {
        void Alternative(DirectoryInfo di, TestResult res);
        void Cmake(DirectoryInfo di, TestResult res);
        void Compile(DirectoryInfo di, TestResult res);
        void CompareOutputs(DirectoryInfo di, TestResult res);        
    }
}
