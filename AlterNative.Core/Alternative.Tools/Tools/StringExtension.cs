using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlterNative.Tools
{
    public static class StringExtension
    {

        public static string TrimEnd( this String str, String trim)
        {
            if (trim.Length > str.Length)
                return str;

            if (str.Substring(str.Length - trim.Length) == trim)
            {
                return str.Substring(0, str.Length - trim.Length);
            }
            else
            {
                return str;
            }
        }
    }
}
