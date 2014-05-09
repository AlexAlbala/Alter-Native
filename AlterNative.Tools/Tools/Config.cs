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

        public static List<string> addedLibs
        {
            get;
            set;
        }
    }
}
