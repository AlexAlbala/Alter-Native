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

        public static List<string> addedLibs
        {
            get;
            set;
        }
    }
}
