using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ConsoleApplication1
{

    class Program
    {
        static void Main(string[] args)
        {
            if (args[0].Split('.')[1] == "arc")
            {
                dearchive(args[0]);
                Console.WriteLine("extracted");
            }
            else
            {
                archive(args[0]);
                Console.WriteLine("archieved");
            }
            
        }
        static void archive(string path)
        {
            FileStream reader = File.OpenRead(path);
            byte[] raw = new byte[reader.Length];
            byte[] arc = new byte[reader.Length];
            int iter = 0;
            reader.Read(raw, 0, (int)reader.Length);
            for (int i = 0; i < reader.Length; )
            {
                   int c = 1;
                   for (int j = 1; true && !(i + j >= reader.Length); j++)
                   {
                        if (raw[i] == raw[i + j])
                        {
                            c++;
                        }
                   }
                   arc[iter] = (byte)c;
                   arc[iter + 1] = raw[i];
                   iter += 2;
                   i += c;
            }
            byte[] e = new byte[iter];
            for (int i = 0; i < iter; i++)
            {
                e[i] = arc[i];
            }
            reader.Close();
            FileStream writer = File.OpenWrite(path.Split(new string[]{"."}, StringSplitOptions.None)[0] + ".arc");

            byte[] header = new byte[] { (byte)'A', (byte)'R', (byte)'C' };
            writer.Write(header, 0, 3);

            string type = path.Split('.')[1];
            byte[] typea = new byte[type.Length + 1];
            for (int i = 0; i < type.Length; i++)
            {
                typea[i] = (byte)type[i];
            }
            typea[type.Length] = 0;
            writer.Write(typea, 0, type.Length + 1);
            writer.Write(e, 0, iter);
            writer.Close();
        }
        static void dearchive(string path)
        {
            FileStream reader = File.OpenRead(path);
            byte[] raw = new byte[reader.Length];
            byte[] extracted = new byte[reader.Length * 8];
            reader.Read(raw, 0, (int)reader.Length);
            string type = "";
            for (int i = 3; raw[i] != (byte)0; i++)
            {
                type += (char)raw[i];
            }
            int of = type.Length + 3;
            int len = 0;
            for (int i = 1; i < reader.Length;)
            {
                int c = 1;
                if (i + of >= reader.Length)
                    break;
                for (int j = 0; j < raw[of + i]; j++)
                {
                    extracted[len + j] = raw[of + i + 1];
                    c++;
                }
                len += c-1;
                i += 2;
            }
            reader.Close();
            byte[] e = new byte[len];
            for(int i = 0; i < len; i++){
                e[i] = extracted[i];
            }
            FileStream writer = File.OpenWrite(path.Split('.')[0] + "." +  type);
            writer.Write(e, 0, len);
            writer.Close();
        }
    } 
}
