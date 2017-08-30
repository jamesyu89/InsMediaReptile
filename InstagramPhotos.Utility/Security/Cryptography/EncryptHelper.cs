using System;
using System.Security.Cryptography;
using System.Text;

namespace InstagramPhotos.Utility.Security.Cryptography
{
    public class EncryptHelper
    {
        public static string DecryptString(string strText, string key)
        {
            try
            {
                byte[] buffer = new MD5CryptoServiceProvider().ComputeHash(Encoding.ASCII.GetBytes(key));
                var provider = new TripleDESCryptoServiceProvider();
                provider.Key = buffer;
                provider.Mode = CipherMode.ECB;
                byte[] inputBuffer = Convert.FromBase64String(strText);
                return
                    Encoding.ASCII.GetString(provider.CreateDecryptor()
                        .TransformFinalBlock(inputBuffer, 0, inputBuffer.Length));
            }
            catch (Exception ex)
            {
                throw new ApplicationException("解密字符串错误：" + key, ex);
            }
        }

        public static string DecryptUTF8String(string strText, string key)
        {
            try
            {
                byte[] buffer = new MD5CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(key));
                var provider = new TripleDESCryptoServiceProvider();
                provider.Key = buffer;
                provider.Mode = CipherMode.ECB;
                byte[] inputBuffer = Convert.FromBase64String(strText);
                return
                    Encoding.UTF8.GetString(provider.CreateDecryptor()
                        .TransformFinalBlock(inputBuffer, 0, inputBuffer.Length));
            }
            catch (Exception ex)
            {
                throw new ApplicationException("解密字符串错误：" + key, ex);
            }
        }

        public static string EncryptString(string strText, string key)
        {
            try
            {
                byte[] buffer = new MD5CryptoServiceProvider().ComputeHash(Encoding.ASCII.GetBytes(key));
                var provider = new TripleDESCryptoServiceProvider();
                provider.Key = buffer;
                provider.Mode = CipherMode.ECB;
                byte[] bytes = Encoding.ASCII.GetBytes(strText);
                string str =
                    Convert.ToBase64String(provider.CreateEncryptor().TransformFinalBlock(bytes, 0, bytes.Length));
                provider = null;
                return str;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("加密字符串错误：" + key, ex);
            }
        }

        public static string EncryptUTF8String(string strText, string key)
        {
            try
            {
                byte[] buffer = new MD5CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(key));
                var provider = new TripleDESCryptoServiceProvider();
                provider.Key = buffer;
                provider.Mode = CipherMode.ECB;
                byte[] bytes = Encoding.UTF8.GetBytes(strText);
                string str =
                    Convert.ToBase64String(provider.CreateEncryptor().TransformFinalBlock(bytes, 0, bytes.Length));
                provider = null;
                return str;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("加密字符串错误：" + key, ex);
            }
        }

        /// <summary>
        ///     AES 加密
        /// </summary>
        /// <param name="str"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string AesEncrypt(string str, string key)
        {
            if (string.IsNullOrEmpty(str)) return null;
            Byte[] toEncryptArray = Encoding.UTF8.GetBytes(str);

            var rm = new RijndaelManaged
            {
                Key = Encoding.UTF8.GetBytes(key),
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };

            ICryptoTransform cTransform = rm.CreateEncryptor();
            Byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        /// <summary>
        ///     AES 解密
        /// </summary>
        /// <param name="str"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string AesDecrypt(string str, string key)
        {
            if (string.IsNullOrEmpty(str)) return null;
            Byte[] toEncryptArray = Convert.FromBase64String(str);

            var rm = new RijndaelManaged
            {
                Key = Encoding.UTF8.GetBytes(key),
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };

            ICryptoTransform cTransform = rm.CreateDecryptor();
            Byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Encoding.UTF8.GetString(resultArray);
        }
    }
}