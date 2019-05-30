using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Monster.Common
{
    public class AESEncrypt
    {
        /// <summary>
        /// 获取密钥
        /// </summary>
        private static string Key => @"(O[LE]6,DB}+efcaj{+OSEh9d8>B'e5M";

        /// <summary>
        /// 获取向量
        /// </summary>
        private static string IV => @"W+\~g8,Ir)b$=lar";

        #region 参数是byte[]类型
        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="data">被加密的明文</param>
        /// <param name="key">密钥</param>
        /// <param name="vector">向量</param>
        /// <returns>密文</returns>
        public static byte[] Encrypt(byte[] data, string key, string vector)
        {
            byte[] bKey = new byte[32];
            Array.Copy(Encoding.UTF8.GetBytes(key.PadRight(bKey.Length)), bKey, bKey.Length);
            byte[] bVector = new byte[16];
            Array.Copy(Encoding.UTF8.GetBytes(vector.PadRight(bVector.Length)), bVector, bVector.Length);
            byte[] cryptograph = null; // 加密后的密文
            Rijndael aes = Rijndael.Create();
            try
            {
                // 开辟一块内存流
                using (MemoryStream memory = new MemoryStream())
                {
                    // 把内存流对象包装成加密流对象
                    using (CryptoStream encryptor = new CryptoStream(memory,
                     aes.CreateEncryptor(bKey, bVector),
                     CryptoStreamMode.Write))
                    {
                        // 明文数据写入加密流
                        encryptor.Write(data, 0, data.Length);
                        encryptor.FlushFinalBlock();

                        cryptograph = memory.ToArray();
                    }
                }
            }
            catch
            {
                cryptograph = null;
            }
            return cryptograph;
        }

        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="data">被解密的密文</param>
        /// <param name="key">密钥</param>
        /// <param name="vector">向量</param>
        /// <returns>明文</returns>
        public static byte[] Decrypt(byte[] data, string key, string vector)
        {
            byte[] bKey = new byte[32];
            Array.Copy(Encoding.UTF8.GetBytes(key.PadRight(bKey.Length)), bKey, bKey.Length);
            byte[] bVector = new byte[16];
            Array.Copy(Encoding.UTF8.GetBytes(vector.PadRight(bVector.Length)), bVector, bVector.Length);

            byte[] original = null; // 解密后的明文

            Rijndael aes = Rijndael.Create();
            try
            {
                // 开辟一块内存流，存储密文
                using (MemoryStream memory = new MemoryStream(data))
                {
                    // 把内存流对象包装成加密流对象
                    using (CryptoStream decryptor = new CryptoStream(memory,
                    aes.CreateDecryptor(bKey, bVector),
                    CryptoStreamMode.Read))
                    {
                        // 明文存储区
                        using (MemoryStream originalMemory = new MemoryStream())
                        {
                            byte[] buffer = new byte[1024];
                            int readBytes = 0;
                            while ((readBytes = decryptor.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                originalMemory.Write(buffer, 0, readBytes);
                            }

                            original = originalMemory.ToArray();
                        }
                    }
                }
            }
            catch
            {
                original = null;
            }
            return original;
        }

        #endregion

        #region 参数是string类型



        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="plainStr">明文字符串</param>
        /// <returns>密文</returns>
        public static string Encrypt(string plainStr)
        {
            byte[] bKey = Encoding.UTF8.GetBytes(Key);
            byte[] bIV = Encoding.UTF8.GetBytes(IV);
            byte[] byteArray = Encoding.UTF8.GetBytes(plainStr);

            string encrypt = null;
            Rijndael aes = Rijndael.Create();
            using (MemoryStream mStream = new MemoryStream())
            {
                using (CryptoStream cStream = new CryptoStream(mStream, aes.CreateEncryptor(bKey, bIV), CryptoStreamMode.Write))
                {
                    cStream.Write(byteArray, 0, byteArray.Length);
                    cStream.FlushFinalBlock();
                    encrypt = Convert.ToBase64String(mStream.ToArray());
                }
            }
            aes.Clear();
            return encrypt;
        }

        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="plainStr">明文字符串</param>
        /// <param name="returnNull">加密失败时是否返回 null，false 返回 String.Empty</param>
        /// <returns>密文</returns>
        public static string Encrypt(string plainStr, bool returnNull)
        {
            string encrypt = Encrypt(plainStr);
            return returnNull ? encrypt : (encrypt ?? String.Empty);
        }

        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="encryptStr">密文字符串</param>
        /// <returns>明文</returns>
        public static string Decrypt(string encryptStr)
        {
            byte[] bKey = Encoding.UTF8.GetBytes(Key);
            byte[] bIV = Encoding.UTF8.GetBytes(IV);
            byte[] byteArray = Convert.FromBase64String(encryptStr);

            string decrypt = null;
            Rijndael aes = Rijndael.Create();
            using (MemoryStream mStream = new MemoryStream())
            {
                using (CryptoStream cStream = new CryptoStream(mStream, aes.CreateDecryptor(bKey, bIV), CryptoStreamMode.Write))
                {
                    cStream.Write(byteArray, 0, byteArray.Length);
                    cStream.FlushFinalBlock();
                    decrypt = Encoding.UTF8.GetString(mStream.ToArray());
                }
            }
            aes.Clear();
            return decrypt;
        }

        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="encryptStr">密文字符串</param>
        /// <param name="returnNull">解密失败时是否返回 null，false 返回 String.Empty</param>
        /// <returns>明文</returns>
        public static string Decrypt(string encryptStr, bool returnNull)
        {
            string decrypt = Decrypt(encryptStr);
            return returnNull ? decrypt : (decrypt ?? String.Empty);
        }


        #endregion

        #region 256位AES加密算法

        /// <summary>
        /// 256位AES加密
        /// </summary>
        /// <param name="toEncrypt"></param>
        /// <returns></returns>
        public static string Encrypt256(string toEncrypt)
        {
            // 256-AES key    
            byte[] keyArray = Encoding.UTF8.GetBytes(Key);
            byte[] toEncryptArray = Encoding.UTF8.GetBytes(toEncrypt);

            RijndaelManaged rDel = new RijndaelManaged
            {
                Key = keyArray,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };

            ICryptoTransform cTransform = rDel.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        /// <summary>
        /// 256位AES解密
        /// </summary>
        /// <param name="toDecrypt"></param>
        /// <returns></returns>
        public static string Decrypt256(string toDecrypt)
        {
            // 256-AES key    
            byte[] keyArray = Encoding.UTF8.GetBytes(Key);
            byte[] toEncryptArray = Convert.FromBase64String(toDecrypt);

            RijndaelManaged rDel = new RijndaelManaged
            {
                Key = keyArray,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };

            ICryptoTransform cTransform = rDel.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Encoding.UTF8.GetString(resultArray);
        }

        #endregion
    }
}
