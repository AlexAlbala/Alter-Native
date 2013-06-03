using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICSharpCode.ILSpy.TextView
{
    class DecompilerTextView
    {

        /// <summary>
        /// Cleans up a node name for use as a file name.
        /// </summary>
        internal static string CleanUpName(string text)
        {
            int pos = text.IndexOf(':');
            if (pos > 0)
                text = text.Substring(0, pos);
            pos = text.IndexOf('`');
            if (pos > 0)
                text = text.Substring(0, pos);
            text = text.Trim();
            foreach (char c in Path.GetInvalidFileNameChars())
                text = text.Replace(c, '-');
            return text;
        }
    }
}
