using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlterNative
{
    public enum TargetType
    {
        Executable,
        DynamicLinkLibrary
    }

    public class Config
    {
        private static List<string> ignoreReferences = new List<string>(){"mscorlib", "System", "System.Core"};
        public static List<string> IgnoreReferences
        {
            get{ return ignoreReferences; }
        }

        public static TargetType targetType
        {
            get;
            set;
        }
        public static string AlterNativeTools
        {
            get;
            set;
        }
        public static string AlterNativeLib
        {
            get;
            set;
        }

        public static string Command
        {
            get;
            set;
        }

        public static List<string> Extra
        {
            get;
            set;
        }

        public static string OutputPath
        {
            get;
            set;
        }        

        public static bool Verbose
        {
            get;
            set;
        }

        public static bool Release
        {
            get;
            set;
        }

        public static List<string> AdditionalLibraries
        {
            get;
            set;
        }

        public static bool RecursiveDependencies
        {
            get;
            set;
        }
    }
}