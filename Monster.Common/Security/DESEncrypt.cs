using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Monster.Common
{
    public class DESEncrypt
    {
        private static string Key => "121djkfjdf88##$#%$^mmjj&gt;";

        #region ========加密========

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string Encrypt(string text)
        {
            return Encrypt(text, Key);
        }

        /// <summary>
        /// 双重加密
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string DoubleEncrypt(string text)
        {
            text = Encrypt(text);
            text = Encrypt(text);
            return text;
        }

        /// <summary> 
        /// 加密数据 
        /// </summary> 
        /// <param name="text"></param> 
        /// <param name="sKey"></param> 
        /// <returns></returns> 
        public static string Encrypt(string text, string sKey)
        {
            if (string.IsNullOrEmpty(text))
                return "";
            using (DESCryptoServiceProvider sa = new DESCryptoServiceProvider { Key = Encoding.UTF8.GetBytes(Md5Hash(sKey, 8)), IV = Encoding.UTF8.GetBytes(Md5Hash(sKey, 8)) })
            {
                using (ICryptoTransform ct = sa.CreateEncryptor())
                {
                    byte[] by = Encoding.UTF8.GetBytes(text);
                    using (var ms = new MemoryStream())
                    {
                        using (var cs = new CryptoStream(ms, ct, CryptoStreamMode.Write))
                        {
                            cs.Write(by, 0, by.Length);
                            cs.FlushFinalBlock();
                        }
                        return Convert.ToBase64String(ms.ToArray());
                    }
                }
            }
        }
        #endregion

        #region ========解密========

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string Decrypt(string text)
        {
            return Decrypt(text, Key);
        }
        /// <summary> 
        /// 解密数据 
        /// </summary> 
        /// <param name="text"></param> 
        /// <param name="sKey"></param> 
        /// <returns></returns> 
        public static string Decrypt(string text, string sKey)
        {
            if (string.IsNullOrEmpty(text))
                return "";
            using (DESCryptoServiceProvider sa = new DESCryptoServiceProvider
            {
                Key = Encoding.UTF8.GetBytes(Md5Hash(sKey, 8)),
                IV = Encoding.UTF8.GetBytes(Md5Hash(sKey, 8))
            })
            {
                using (ICryptoTransform ct = sa.CreateDecryptor())
                {
                    byte[] byt = Convert.FromBase64String(text);

                    using (var ms = new MemoryStream())
                    {
                        using (var cs = new CryptoStream(ms, ct, CryptoStreamMode.Write))
                        {
                            cs.Write(byt, 0, byt.Length);
                            cs.FlushFinalBlock();
                        }
                        return Encoding.UTF8.GetString(ms.ToArray());
                    }
                }
            }
        }
        private static string Md5Hash(string input, int length)
        {
            MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString().Substring(0, length);
        }
        #endregion
    }
}
