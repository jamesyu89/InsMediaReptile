using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace InstagramPhotos.Utility.Security.DES
{
    /// <summary>
    /// AES 加密类
    /// </summary>
    public static class AESUtil
    {
        // 默认密钥向量
        private static byte[] IV = { 0x20, 0x04, 0x26, 0x28, 0x90, 0xAC, 0xED, 0xE2, 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
        private static string KEY = "ygXwd2#c82p543vR";

        /// <summary>
        /// AES 加密
        /// </summary>
        /// <param name="plainText">明文</param>
        /// <returns></returns>
        public static string Encrypt(string plainText)
        {
            return Encrypt(plainText, KEY);
        }

        /// <summary>
        /// AES 解密
        /// </summary>
        /// <param name="cipherText">密文</param>
        /// <returns></returns>
        public static string Decrypt(string cipherText)
        {
            return Decrypt(cipherText, KEY);
        }

        /// <summary>
        /// AES 加密
        /// </summary>
        /// <param name="plainText">明文</param>
        /// <param name="key">密钥</param>
        /// <returns></returns>
        public static string Encrypt(string plainText, string key)
        {
            string cipherText = string.Empty;
            try
            {
                // Create an Aes object
                // with the specified key and IV.
                using (Aes aesAlg = Aes.Create())
                {
                    aesAlg.Key = Encoding.UTF8.GetBytes(key);
                    aesAlg.IV = IV;

                    // Create a decrytor to perform the stream transform.
                    ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                    // Create the streams used for encryption.
                    using (MemoryStream msEncrypt = new MemoryStream())
                    {
                        using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        {
                            using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                            {
                                // Write all data to the stream.
                                swEncrypt.Write(plainText);
                            }
                            cipherText = Convert.ToBase64String(msEncrypt.ToArray());
                        }
                    }
                }
            }
            catch
            {
                cipherText = string.Empty;
            }
            return cipherText;
        }

        /// <summary>
        /// AES 解密
        /// </summary>
        /// <param name="cipherText">密文</param>
        /// <param name="key">密钥</param>
        /// <returns></returns>
        public static string Decrypt(string cipherText, string key)
        {
            string plaintext = string.Empty;
            try
            {
                // Create an Aes object
                // with the specified key and IV.
                using (Aes aesAlg = Aes.Create())
                {
                    aesAlg.Key = Encoding.UTF8.GetBytes(key);
                    aesAlg.IV = IV;

                    // Create a decrytor to perform the stream transform.
                    ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                    // Create the streams used for decryption.
                    using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(cipherText)))
                    {
                        using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                            {
                                plaintext = srDecrypt.ReadToEnd();
                            }
                        }
                    }
                }
            }
            catch
            {
                plaintext = string.Empty;
            }
            return plaintext;
        }
    }
}
