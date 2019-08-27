using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.IO.Compression;

namespace GAdminLib
{
   internal class Utils
    {
       public static byte[] Compress(byte[] raw)
        {
            byte[] cp = null;
            using (MemoryStream memory = new MemoryStream())
            {
                using (GZipStream gzip = new GZipStream(memory, CompressionMode.Compress, true))
                {
                    gzip.Write(raw, 0, raw.Length);
                }
                cp = memory.ToArray();
            }
            return cp;
        }
        public static byte[] DeCompress(byte[] gzip)
        {
            byte[] cp = null;

            using (GZipStream stream = new GZipStream(new MemoryStream(gzip), CompressionMode.Decompress))
            {
                const int size = 4096;
                byte[] buffer = new byte[size];
                using (MemoryStream memory = new MemoryStream())
                {
                    int count = 0;
                    do
                    {
                        count = stream.Read(buffer, 0, size);
                        if (count > 0)
                        {
                            memory.Write(buffer, 0, count);
                        }
                    }
                    while (count > 0);
                    cp = memory.ToArray();
                }

            }
            return cp;

        }
        internal static string ToHex(string data)
        {
            string hex = "";
            foreach (byte b in Encoding.Default.GetBytes(data))
                hex += b.ToString("X2") + "-";
            return hex.Remove(hex.Length - 1, 1);
        }

        internal static string FromHex(string hex)
        {
            string data = "";
            foreach (string hexval in hex.Split('-'))
                data += Encoding.Default.GetString(new byte[1] { Convert.ToByte(hexval, 16) });

            return data;
        }
       public static string MD5Hash(string input)
       {
           // Use input string to calculate MD5 hash
           MD5 md5 = System.Security.Cryptography.MD5.Create();
           byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
           byte[] hashBytes = md5.ComputeHash(inputBytes);

           // Convert the byte array to hexadecimal string
           StringBuilder sb = new StringBuilder();
           for (int i = 0; i < hashBytes.Length; i++)
           {
               sb.Append(hashBytes[i].ToString("x2"));
               // To force the hex string to lower-case letters instead of
               // upper-case, use he following line instead:
               // sb.Append(hashBytes[i].ToString("x2")); 
           }
           return sb.ToString();
       }
    }
}
