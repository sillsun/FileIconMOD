using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace FileIconConsole
{

    internal class MD5Helper
    {
        /// <summary>
        /// 获取一个新的Guid并返回他的MD5码
        /// </summary>
        /// <returns></returns>
        public static string NewGuid()
        {
            return CalcMD5L(System.Guid.NewGuid().ToByteArray());
        }

        /// <summary>
        /// 计算流的MD5码
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="BuffSize">缓冲区大小（单位：MB）</param>
        /// <returns></returns>
        public static string CalcMD5L(Stream stream)
        {
            using (var md5 = MD5.Create())
            {
                var hash = md5.ComputeHash(stream);
                return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
            }
        }

        /// <summary>
        /// 计算文件的MD5码
        /// </summary>
        /// <param name="file"></param>
        /// <param name="BuffSize">缓冲区大小（单位：MB）</param>
        /// <returns></returns>
        public static string CalcMD5L(FileInfo file)
        {
            using (var stream = File.OpenRead(file.FullName))
            {
                return CalcMD5L(stream);
            }
        }

        /// <summary>
        /// 计算字节数组的 MD5 值
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static string CalcMD5L(byte[] buffer)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] md5Bytes = md5.ComputeHash(buffer);
                return BitConverter.ToString(md5Bytes).Replace("-", "").ToLowerInvariant();
            }
        }

        /// <summary>
        /// 计算流的MD5码
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="BuffSize">缓冲区大小（单位：MB）</param>
        /// <returns></returns>
        public static string CalcMD5U(Stream stream)
        {
            using (var md5 = MD5.Create())
            {
                var hash = md5.ComputeHash(stream);
                return BitConverter.ToString(hash).Replace("-", "").ToUpperInvariant();
            }
        }

        /// <summary>
        /// 计算文件的MD5码
        /// </summary>
        /// <param name="file"></param>
        /// <param name="BuffSize">缓冲区大小（单位：MB）</param>
        /// <returns></returns>
        public static string CalcMD5U(FileInfo file)
        {
            using (var stream = File.OpenRead(file.FullName))
            {
                return CalcMD5U(stream);
            }
        }

        /// <summary>
        /// 计算字节数组的 MD5 值
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static string CalcMD5U(byte[] buffer)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] md5Bytes = md5.ComputeHash(buffer);
                return BitConverter.ToString(md5Bytes).Replace("-", "").ToUpperInvariant();

            }
        }




        /// <summary>
        /// 计算流的MD5码
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="BuffSize">缓冲区大小（单位：MB）</param>
        /// <returns></returns>
        public static string CalcMD5B64(Stream stream)
        {
            using (var md5 = MD5.Create())
            {
                var hash = md5.ComputeHash(stream);
                return Convert.ToBase64String(hash);
            }
        }

        /// <summary>
        /// 计算文件的MD5码
        /// </summary>
        /// <param name="file"></param>
        /// <param name="BuffSize">缓冲区大小（单位：MB）</param>
        /// <returns></returns>
        public static string CalcMD5B64(FileInfo file)
        {
            using (var stream = File.OpenRead(file.FullName))
            {
                return CalcMD5B64(stream);
            }
        }

        /// <summary>
        /// 计算字节数组的 MD5 值
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static string CalcMD5B64(byte[] buffer)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] md5Bytes = md5.ComputeHash(buffer);
                return Convert.ToBase64String(md5Bytes);
            }
        }



        /// <summary>
        /// 计算字符串的 MD5 值
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string CalcMD5L(string str)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(str);
            return CalcMD5L(buffer);
        }

        public static string CalcMD5U(string str)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(str);
            return CalcMD5U(buffer);
        }
        public static string CalcMD5B64(string str)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(str);
            return CalcMD5B64(buffer);

        }

    }
}
