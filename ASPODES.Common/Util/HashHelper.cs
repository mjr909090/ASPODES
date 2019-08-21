using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ASPODES.Common.Util
{
    public class HashHelper
    {
        public static int LoginDuration = 40;

        /// <summary>
        /// 添加字符串"ASPODES"后再进行MD5加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string IntoMd5(string str)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            string source = "ASPODES" + str;
            byte[] message;
            message = Encoding.Default.GetBytes(source);
            md5.ComputeHash(message);
            string hashpwd = Convert.ToBase64String(md5.Hash);
            return hashpwd;
        }

        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string MD5( string str )
        {
            StringBuilder password = new StringBuilder();
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            var byteBuffer = Encoding.Default.GetBytes(str);
            foreach( var b in byteBuffer )
            {
                password.Append(b.ToString("X2"));
            }
            return password.ToString().Substring(0, 8);
        }
        /// <summary>
        /// 进行DES加密。
        /// </summary>
        /// <param name="pToEncrypt">要加密的字符串。</param>
        /// <param name="sKey">密钥，且必须为8位。</param>
        /// <returns>以Base64格式返回的加密字符串。</returns>
        public static string Encrypt(string pToEncrypt, string sKey)
        {
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                byte[] inputByteArray = Encoding.UTF8.GetBytes(pToEncrypt);
                des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
                des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();
                    cs.Close();
                }
                string str = Convert.ToBase64String(ms.ToArray());
                ms.Close();
                return str;
            }
        }

        /// <summary>
        /// 用"ASPODES1"为秘钥进行DES加密
        /// </summary>
        /// <param name="pToEncrypt"></param>
        /// <returns></returns>
        public static string Encrypt(string pToEncrypt)
        {
            return Encrypt(pToEncrypt, "ASPODES1");
        }

        /// <summary>
        /// 进行DES解密。
        /// </summary>
        /// <param name="pToDecrypt">要解密的以Base64</param>
        /// <param name="sKey">密钥，且必须为8位。</param>
        /// <returns>已解密的字符串。</returns>
        public static string Decrypt(string pToDecrypt, string sKey)
        {
            byte[] inputByteArray = Convert.FromBase64String(pToDecrypt);
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
                des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();
                    cs.Close();
                }
                string str = Encoding.UTF8.GetString(ms.ToArray());
                ms.Close();
                return str;
            }
        }

        /// <summary>
        /// 用"ASPODES1"为秘钥进行DES解密
        /// </summary>
        /// <param name="pToDecrypt"></param>
        /// <returns></returns>
        public static string Decrypt(string pToDecrypt)
        {
            return Decrypt(pToDecrypt, "ASPODES1");
        }

        /// <summary>
        /// 获取当前的时间戳
        /// </summary>
        /// <returns></returns>
        public static double GetTimestamp()
        {
            return GetTimestamp(DateTime.Now);
        }
        
        /// <summary>
        /// 获取某一时间至2000年1月1日0时0分0秒的时间戳
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static double GetTimestamp(DateTime dt)
        {
            TimeSpan ts = new TimeSpan(dt.Ticks - new DateTime(2000, 1, 1, 0, 0, 0).Ticks);
            return Convert.ToDouble(ts.TotalMilliseconds.ToString("f0"));
        }
    }
}
