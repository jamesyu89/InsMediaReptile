using System.Security.Cryptography;
using System.Text;

namespace InstagramPhotos.Utility.Security
{
    /// <summary>
    /// 哈希帮助类
    /// </summary>
    public class HashUtil
    {
        /// <summary>
        /// 计算哈希 仿支付宝 请勿修改
        /// </summary>
        /// <param name="input"></param>
        /// <returns>32位长度的字符串</returns>
        public static string ComputeHash(string input)
        {
            if (input == null)
            {
                input = string.Empty;
            }
            var sb = new StringBuilder(256);
            using (MD5 md5 = new MD5CryptoServiceProvider())
            {
                byte[] byteArr = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
                for (int i = 0; i < byteArr.Length; i++)
                {
                    sb.Append(byteArr[i].ToString("x").PadLeft(2, '0'));
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// 计算带盐的哈希
        /// </summary>
        /// <param name="input"></param>
        /// <param name="salt"></param>
        /// <returns>32位长度的字符串</returns>
        public static string ComputeHash(string input, string salt)
        {
            if (input == null)
            {
                input = string.Empty;
            }
            input = string.Format("{0}{1}", input, salt);

            return ComputeHash(input);
        }

        /// <summary>
        /// 使用SHA256算法进行散列 防止哈希碰撞的可能
        /// </summary>
        /// <param name="input">输入参数</param>
        /// <returns>64位长度的字符串</returns>
        public static string ComputeSHA256Hash(string input)
        {
            if (input == null)
            {
                input = string.Empty;
            }
            var sb = new StringBuilder(256);
            using (var sha256 = new SHA256Managed())
            {
                byte[] byteArr = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                for (int i = 0; i < byteArr.Length; i++)
                {
                    sb.Append(byteArr[i].ToString("x").PadLeft(2, '0'));
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// 计算带盐的哈希  使用SHA256算法进行散列 防止哈希碰撞的可能
        /// </summary>
        /// <param name="input"></param>
        /// <param name="salt"></param>
        /// <returns>64位长度的字符串</returns>
        public static string ComputeSHA256Hash(string input, string salt)
        {
            if (input == null)
            {
                input = string.Empty;
            }
            input = string.Format("{0}{1}", input, salt);

            return ComputeSHA256Hash(input);
        }

        /// <summary>
        /// 使用SHA512算法进行散列 防止哈希碰撞的可能
        /// </summary>
        /// <param name="input">输入参数</param>
        /// <returns>128位长度的字符串</returns>
        public static string ComputeSHA512Hash(string input)
        {
            if (input == null)
            {
                input = string.Empty;
            }
            var sb = new StringBuilder(256);
            using (var sha512 = new SHA512Managed())
            {
                byte[] byteArr = sha512.ComputeHash(Encoding.UTF8.GetBytes(input));
                for (int i = 0; i < byteArr.Length; i++)
                {
                    sb.Append(byteArr[i].ToString("x").PadLeft(2, '0'));
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// 计算带盐的哈希  使用SHA512算法进行散列 防止哈希碰撞的可能
        /// </summary>
        /// <param name="input"></param>
        /// <param name="salt"></param>
        /// <returns>128位长度的字符串</returns>
        public static string ComputeSHA512Hash(string input, string salt)
        {
            if (input == null)
            {
                input = string.Empty;
            }
            input = string.Format("{0}{1}", input, salt);

            return ComputeSHA512Hash(input);
        }
    }
}
