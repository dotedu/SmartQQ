using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SmartQQLib.API.Tool
{


    internal class UniversalTool
    {

        internal List<string> Base64Encrypt(List<string> input)
        {
            if (input.Count > 0 && input != null)
            {

                for (int i = 0; i < input.Count; i++)
                {
                    input[i] = Base64Encrypt(input[i]);
                }
                return input;
            }
            return null;

        }

        /// <summary>
        /// Base64加密
        /// </summary>
        /// <param name="input">需要加密的字符串</param>
        /// <returns></returns>
        internal string Base64Encrypt(string input)
        {
            return Base64Encrypt(input, new UTF8Encoding());
        }

        /// <summary>
        /// Base64加密
        /// </summary>
        /// <param name="input">需要加密的字符串</param>
        /// <param name="encode">字符编码</param>
        /// <returns></returns>
        internal string Base64Encrypt(string input, Encoding encode)
        {
            return Convert.ToBase64String(encode.GetBytes(input));
        }

        /// <summary>
        /// 获取1970-01-01至dateTime的毫秒数
        /// </summary>
        internal long GetTimeStamp(DateTime dateTime)
        {
            DateTime dt1970 = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return (dateTime.Ticks - dt1970.Ticks) / 10000;
        }


        /// <summary>  
        /// 取得hash值 
        /// </summary> 
        internal String hash(long x, String K)
        {
            int[] N = { 0, 0, 0, 0 };
            char[] k = K.ToCharArray();

            for (int T = 0; T < K.Length; T++)
            {
                N[T % 4] ^= (int)k[T];
            }


            String[] U = { "EC", "OK" };
            long[] V = { 0, 0, 0, 0 };
            V[0] = x >> 24 & 255 ^ (int)U[0].ToCharArray()[0];
            V[1] = x >> 16 & 255 ^ (int)U[0].ToCharArray()[1];
            V[2] = x >> 8 & 255 ^ (int)U[1].ToCharArray()[0];
            V[3] = x & 255 ^ (int)U[1].ToCharArray()[1];


            long[] U1 = { 0, 0, 0, 0, 0, 0, 0, 0 };

            for (int T = 0; T < 8; T++)
            {
                U1[T] = T % 2 == 0 ? N[T >> 1] : V[T >> 1];
            }


            char[] N1 = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };
            String V1 = "";

            for (int T = 0; T < U1.Count(); T++)
            {
                V1 += N1[((U1[T] >> 4) & 15)];
                V1 += N1[(U1[T] & 15)];
            }

            return V1;
        }


        /// <summary>  
        /// 取得bkn值 
        /// </summary> 
        internal int GetBkn(String e)
        {
            char[] E = e.ToCharArray();
            int hash = 5381;
            for (int i = 0, len = e.Length; i < len; ++i)
                hash += (hash << 5) + E[i];
            int t = 2147483647 & hash;
            return t;
        }

        internal string unicode(List<string> type)
        {

            if (type.Count > 0&& type!=null)
            {
                StringBuilder data = new StringBuilder(string.Empty);

                for (int i = 0; i < type.Count; i++)
                {
                    if (i == 0)
                    {
                        data.AppendFormat("{0}", type[i]);

                    }
                    else
                    {
                        data.AppendFormat("-{0}", type[i]);

                    }

                }
                return data.ToString();
            }
            return "";
        }


        /// <summary>
        /// 获取目录文件夹下的所有子目录
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="filePattern"></param>
        /// <returns></returns>
        public static List<string> FindSubDirectories(string directory)
        {
            return Directory.GetDirectories(directory, "*", SearchOption.AllDirectories).ToList<string>();
        }


        /// <summary>
        /// 获取目录名称
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
        public static string GetDirectoryName(string directory)
        {
            if (!Directory.Exists(directory))
            {
                return string.Empty;// DirectoryHelper.CreateDirectory(directory);
            }
            return new DirectoryInfo(directory).Name;
        }
    }
}
