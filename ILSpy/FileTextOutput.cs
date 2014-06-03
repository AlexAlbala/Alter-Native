using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICSharpCode.Decompiler;
using System.IO;
using System.Diagnostics;

namespace ICSharpCode.ILSpy
{
    public class FileTextOutput : ITextOutput
    {
        StreamWriter output;
        int indent;
        bool needsIndent;
        int lineNumber;
        int lastLineStart;
        string WorkingDirectory = "";

        public FileTextOutput(string WorkingDirectory)
        {
            indent = 0;
            this.WorkingDirectory = WorkingDirectory;
        }       

        public void NewFile(string FileName)
        {
            indent = 0;
            if (output != null)
            {
                output.Flush();
                output.Close();
                output.Dispose();
            }            
            output = new StreamWriter(Path.Combine(WorkingDirectory, FileName));

            string toolsPath = Environment.GetEnvironmentVariable("ALTERNATIVE_TOOLS_PATH");

            StreamReader fs = new StreamReader(Path.Combine(toolsPath, "Text/notice"));
            output.Write(fs.ReadToEnd());
        }

        public void Close()
        {
            indent = 0;
            if (output != null)
            {
                output.Flush();
                output.Close();
                output.Dispose();
            }
            output = null;
        }

        public NRefactory.TextLocation Location
        {
            get { throw new NotImplementedException(); }
        }

        public void Indent()
        {
            indent++;
        }

        public void Unindent()
        {
            indent--;
        }

        void WriteIndent()
        {
            if (needsIndent)
            {
                needsIndent = false;
                for (int i = 0; i < indent; i++)
                {
                    output.Write('\t');
                }
            }
        }

        public void Write(char ch)
        {
            WriteIndent();
            output.Write(ch);
        }

        public void Write(string text)
        {
            WriteIndent();
            output.Write(text);
        }

        public void WriteLine()
        {
            output.WriteLine();
            needsIndent = true;
            lastLineStart = (int)output.BaseStream.Length;
            lineNumber++;

        }

        public void WriteDefinition(string text, object definition, bool isLocal = true)
        {
            Write(text);            
        }

        public void WriteReference(string text, object reference, bool isLocal = false)
        {
            Write(text);
        }

        public void AddDebuggerMemberMapping(MemberMapping memberMapping)
        {
            throw new NotImplementedException();
        }

        public void MarkFoldStart(string collapsedText = "...", bool defaultCollapsed = false)
        {
            throw new NotImplementedException();
        }

        public void MarkFoldEnd()
        {
            throw new NotImplementedException();
        }
    }
}
