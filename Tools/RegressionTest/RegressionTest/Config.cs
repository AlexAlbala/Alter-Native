using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RegressionTest
{
    public enum Platform
    {
        Windows32,
        Windows64,
        Linux,
        MacOS,
        Android
    }

    public enum CompileMode
    { 
        Debug,
        Release
    }

    public class Config
    {
        /*******************CONFIGURATION****************/
        public static Platform platform = Platform.Windows32;
        public static bool Debug = false;
        public static bool Verbose = false;
        public static bool Unlimited = false;
        public static bool fast = false;
        public static bool overwriteTarget = false;
        public static CompileMode compileMode = CompileMode.Debug;
        /*******************CONFIGURATION****************/
    }
}
