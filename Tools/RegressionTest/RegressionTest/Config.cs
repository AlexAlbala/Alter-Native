using System;
using System.Collections.Generic;
using System.Text;

namespace RegressionTest
{
    public enum Platform
    {
        win,
        win32,
        win64,
        linux,
        macos,
        android
    }

    public enum CompileMode
    {
        Debug,
        Release
    }

    public class Config
    {
        /*******************CONFIGURATION****************/
        public static Platform platform = Platform.win32;
        public static bool Debug = false;
        public static bool Verbose = false;
        public static bool Unlimited = false;
        public static bool fast = false;
        public static bool overwriteTarget = false;
        public static CompileMode compileMode = CompileMode.Debug;
        public static bool performanceTests = false;
        public static bool featuresTest = true;
        public static bool RecursiveDependencies = false;
        /*******************CONFIGURATION****************/
    }
}
