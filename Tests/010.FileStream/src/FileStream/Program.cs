using System;
using System.IO;
using System.Text;

class Test
{

    public static void Main()
    {
        string path = "MyTest.txt";

        // Delete the file if it exists.
        if (File.Exists(path))
        {
            File.Delete(path);
        }

        //Create the file.
        FileStream fs = File.Create(path);

        AddText(fs, "This is some text");
        AddText(fs, "This is some more text,");
        AddText(fs, "\r\nand this is on a new line");
        AddText(fs, "\r\n\r\nThe following is a subset of characters:\r\n");

        for (int i = 100; i < 220; i++)
        {
            AddText(fs, Convert.ToChar(i).ToString());

            //Split the output at every 10th character.
            if (i % 10 == 0)
            {
                AddText(fs, "\r\n");
            }
        }
        fs.Close();
        //Open the stream and read it back.
        fs = File.OpenRead(path);

        byte[] b = new byte[1024];
        UTF8Encoding temp = new UTF8Encoding();
        while (fs.Read(b, 0, b.Length) > 0)
        {
            String s = temp.GetString(b);
            Console.WriteLine(s);
        }

    }

    private static void AddText(FileStream fs, string value)
    {
        byte[] info = new UTF8Encoding().GetBytes(value);
        fs.Write(info, 0, info.Length);
    }
}