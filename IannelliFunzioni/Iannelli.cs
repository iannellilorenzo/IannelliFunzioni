using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Threading;
using Microsoft.SqlServer.Server;
using System.Reflection;

namespace IannelliFunzioni
{
    public class Iannelli
    {
        public static string file1 = @"iannelli1.csv";
        public static string file2 = @"iannelli.csv";
        public static int id = 42, min = 10, max = 20;
        static Random rand = new Random();

        public static void Azione1()
        {
            string str;
            int index = 0, val;

            StreamWriter sw = new StreamWriter(file1, false);

            StreamReader sr = new StreamReader(file2);
            str = sr.ReadLine();

            while (str != null)
            {
                if (index == 0)
                {
                    sw.WriteLine($"{str};MIOVALORE;CANCELLAZIONE LOGICA;CAMPO UNIVOCO");
                }
                else
                {
                    val = rand.Next(min, max + 1);
                    sw.WriteLine($"{str};{val};false;{index}");
                }

                index++;
                str = sr.ReadLine();
            }

            sw.Close();
        }

        public static int Azione2()
        {
            StreamReader sr = new StreamReader(file1);
            string str = sr.ReadLine();
            sr.Close();

            int cont = str.Split(';').Length;

            return cont;
        }

        public static int Azione3()
        {
            StreamReader sr = new StreamReader(file1);
            int lungStr = 0, lungMax = 0, index = 0;
            string str = sr.ReadLine();

            while (str != null)
            {
                lungStr = str.Length;

                if (index != 0)
                {
                    if (lungMax < lungStr)
                    {
                        lungMax = str.Length;
                    }
                }

                str = sr.ReadLine();
                index++;
            }

            sr.Close();
            return lungMax;
        }

        public static int[] Azione3Avanzato()
        {
            StreamReader sr = new StreamReader(file1);
            string str = sr.ReadLine();

            int funz = Azione2();

            int[] lungMax = new int[funz];
            int cont = 0;

            str = sr.ReadLine();

            while (str != null)
            {
                string[] split = str.Split(';');
                string[] arr = new string[funz];

                for (int i = 0; i < funz; i++)
                {
                    sr.DiscardBufferedData();
                    sr.BaseStream.Seek(0, SeekOrigin.Begin);

                    str = sr.ReadLine();
                    cont = 0;

                    while (str != null)
                    {
                        string[] split2 = str.Split(';');
                        if (cont != 0)
                        {
                            if (lungMax[i] < split2[i].Length)
                            {
                                lungMax[i] = split2[i].Length;
                            }
                        }

                        str = sr.ReadLine();
                        cont++;
                    }
                }
            }

            sr.Close();
            return lungMax;
        }

        public static void Azione4()
        {
            string str;
            int index = 0;

            StreamReader sr = new StreamReader(file1);
            StreamWriter sw = new StreamWriter("temp.csv");

            str = sr.ReadLine();

            while (str != null)
            {
                sw.WriteLine(str.PadRight(200));

                str = sr.ReadLine();
                index++;
            }

            sr.Close();
            sw.Close();

            File.Replace("temp.csv", file1, "backup.csv");
        }

        public static bool Azione5(string NomeZona, string Attuazione, string Data, string TipoSosta)
        {
            id++;

            int num;
            bool success = int.TryParse(Data, out num), ret;

            if (string.IsNullOrWhiteSpace(NomeZona) || string.IsNullOrWhiteSpace(Attuazione) || string.IsNullOrWhiteSpace(Data) || !success || string.IsNullOrWhiteSpace(TipoSosta))
            {
                ret = false;
            }
            else
            {
                int r = rand.Next(min, max + 1);

                StreamReader sr = new StreamReader(file1);
                string str = sr.ReadLine();
                int index = 0;

                while (str != null)
                {
                    index++;
                    str = sr.ReadLine();
                }

                sr.Close();

                var oStream = new FileStream(file1, FileMode.Append, FileAccess.Write, FileShare.Read);
                BinaryWriter bw = new BinaryWriter(oStream);
                string record = $"{id};{NomeZona[0].ToString().ToUpper()}{NomeZona.Substring(1)};{Attuazione.ToUpper()};{Data};{TipoSosta.ToUpper()};{r};true;{index}".PadRight(200);
                byte[] b = Encoding.ASCII.GetBytes(record);
                bw.Write(b);

                bw.Close();
                oStream.Close();

                ret = true;
            }

            return ret;
        }

        public static string[] Azione6()
        {
            string str;
            int index = 0;
            string[] print = new string[id];

            StreamReader sr = new StreamReader(file1);
            str = sr.ReadLine();

            while (str != null)
            {
                String[] split = str.Split(';');

                if (split[6] == "false")
                {
                    print[index] += $"{split[1]};{split[2]};{split[4]}";
                }

                index++;
                str = sr.ReadLine();
            }

            sr.Close();
            return print;
        }

        public static int Azione7(string Ricerca)
        {
            int ret = -2, index = 0;
            string s;

            int num;
            bool success = int.TryParse(Ricerca, out num);

            if (success || string.IsNullOrWhiteSpace(Ricerca))
            {
                StreamReader sr = new StreamReader(file1);

                while ((s = sr.ReadLine()) != null)
                {
                    string[] split = s.Split(';');

                    if (index != 0)
                    {
                        if (split[0] == Ricerca)
                        {
                            ret = index;
                            break;
                        }

                        ret = -1;
                    }

                    index++;
                }

                sr.Close();
            }

            return ret;
        }

        public static int Azione8(string NomeZona, string Attuazione, string Data, string TipoSosta, string Ricerca)
        {
            int count = Azione7(Ricerca);
            int ret = 0;

            if (count == -2)
            {
                ret = -2;
            }
            else if (count == -1)
            {
                ret = -1;
            }
            else
            {
                ret = count;

                int r = rand.Next(min, max + 1);

                var oStream = new FileStream(file1, FileMode.Open, FileAccess.Write, FileShare.Read);
                BinaryWriter bw = new BinaryWriter(oStream);

                oStream.Seek(0, SeekOrigin.Begin);
                oStream.Seek((200 * count), SeekOrigin.Current);

                string record = $"{id};{NomeZona[0].ToString().ToUpper()}{NomeZona.Substring(1)};{Attuazione.ToUpper()};{Data};{TipoSosta.ToUpper()};{r};true;{count}".PadRight(200);
                byte[] b = Encoding.ASCII.GetBytes(record);
                bw.Write(b);

                bw.Close();
            }

            return ret;
        }

        public static void Azione9(string Ricerca)
        {
            int count = Azione7(Ricerca);
            int index = 0;

            var rStream = new FileStream(file1, FileMode.Open, FileAccess.Read, FileShare.Read);
            BinaryReader br = new BinaryReader(rStream);

            rStream.Seek(0, SeekOrigin.Begin);
            rStream.Seek((200 * count), SeekOrigin.Current);

            byte[] b = new byte[200];
            rStream.Read(b, 0, 200);
            string str = Encoding.ASCII.GetString(b);
            rStream.Close();
            br.Close();

            String[] split = str.Split(';');

            var wStream = new FileStream(file1, FileMode.Open, FileAccess.Write, FileShare.Write);
            BinaryWriter bw = new BinaryWriter(wStream);

            wStream.Seek(0, SeekOrigin.Begin);
            wStream.Seek((200 * count), SeekOrigin.Current);

            string record = $"{split[0]};{split[1]};{split[2]};{split[3]};{split[4]};{split[5]};true;{split[7]}".PadRight(200);
            byte[] b2 = Encoding.ASCII.GetBytes(record);
            bw.Write(b2);

            wStream.Close();
            bw.Close();
        }
    }
}
