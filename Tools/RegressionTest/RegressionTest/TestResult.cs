using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RegressionTest
{
    public class TestResult
    {
        public string name { get; set; }
        public short alternative;
        public short output;
        public short diffCode;
        public short cmakeCode;
        public short msbuildCode;

        //TIMMING
        public long msTimeSpan;
        public float relativeTime;        
        public float originalTime;
        public float finalTime;
        //
        //LINES
        public long originalLines;
        public long finalLines;
        public long linesDifference;
        public float relativeLines;
        //

        public TestResult()
        {
            output = -1;
            diffCode = -1;
            cmakeCode = -1;
            msbuildCode = -1;
            alternative = -1;

            msTimeSpan = 0;
            relativeTime = 0;
            originalTime = -1;
            finalTime = -1;

            originalLines = -1;
            finalLines = -1;
            linesDifference = 0;
            relativeTime = 0;
            
        }

        public bool AllSuccess()
        {
            return output == 0 &&  cmakeCode == 0 && msbuildCode == 0 && alternative == 0;
        }
    }
}
