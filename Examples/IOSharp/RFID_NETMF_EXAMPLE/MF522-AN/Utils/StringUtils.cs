using System;
using Microsoft.SPOT;
using System.Diagnostics;

namespace IOSharp.Utils
{
    static class StringUtils
    {
        public static void PrintConsole(String message)
        {
#if MF
            Debug.Print(message);
#else
            Console.WriteLine(message);
#endif
        }

        public static void PrintConsole(byte input)
        {
            PrintConsole(ByteToString(input)+"");
        }

        public static void PrintConsole(byte[] input)
        {
            PrintConsole(ByteArrayToString(input)+"");
        }

        private static String ByteToString(byte input)
        {
            return input.ToString();
        }

        public static String ByteArrayToString(byte[] input)
        {
            String output = "";

            if (input != null)
                output = ByteArrayToHexString(input);

            return output;
        }

        /*public static String ToHexString(this byte buff)
        {
            return ByteToHexString(buff);
        }*/

        public static String ByteToHexString(byte b)
        {
            char[] c = new char[2];
            byte aux;

            aux = (byte)(b >> 4);
            c[0] = (char)(aux > 9 ? aux + 0x37 : aux + 0x30);
            aux = ((byte)(b & 0xF));
            c[1] = (char)(aux > 9 ? aux + 0x37 : aux + 0x30);
            
            return new String(c);
        }

        /*public static String ToHexString(this byte[] buff)
        {
            return ByteArrayToHexString(buff);
        }*/

        public static String ByteArrayToHexString(byte[] p)
        {
            char[] c = new char[p.Length * 4 + (int)p.Length/8];
            byte b;

            for (int y = 0, x = 0; y < p.Length; ++y, ++x)
            {
                b = ((byte)(p[y] >> 4));
                c[x] = (char)(b > 9 ? b + 0x37 : b + 0x30);
                b = ((byte)(p[y] & 0xF));
                c[++x] = (char)(b > 9 ? b + 0x37 : b + 0x30);
                c[++x] = ' ';

                if((y+1) % 8 == 0)
                {
                    c[++x] = '\n';
                }
            }
            return new String(c);
        }

    }
}
