using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace RegressionTest
{
    class JUnitConverter
    {
        public static void Export(List<TestResult> tests)
        {
            //Create file
            XmlTextWriter xml = new XmlTextWriter(Utils.testPath + "/junit.Output" + String.Format("{HH:mm:ss}", DateTime.Now) + ".xml", ASCIIEncoding.UTF8);
            
        }
    }
}
