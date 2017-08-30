using System;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using InstagramPhotos.Utility.Log;
using InstagramPhotos.Utility.Security.Cryptography;
using InstagramPhotos.Utility.Security.DES;

namespace InstagramPhotos.Utility.Helper
{
    /// <summary>
    /// 上传文件加密帮助类
    /// </summary>
    public static class UploadFileCryptoHelper
    {
        private static readonly TimeSpan MAX_LIFE_TIME = TimeSpan.FromMinutes(30);
        private const string AES_KEY = "ygTg687#638xdVwP";
        private const string SIGN_KEY = "ba0c7db15a7b4a1f8430a055e92ff6c8";

        /// <summary>
        /// 生成加密的 Url
        /// </summary>
        /// <param name="url"></param>
        /// <param name="userId"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static string EncryptUrl(string url, string userId, string userName)
        {
            if (string.IsNullOrEmpty(url) || string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException("参数 url、userId 不能为空");
            }

            var query = HttpUtility.ParseQueryString(string.Empty);
            query.Add("UserId", userId);
            query.Add("UserName", userName);
            query.Add("MTime", DateTime.Now.ToString("yyyyMMddHHmmss"));

            string data = AESUtil.Encrypt(query.ToString(), AES_KEY);
            string sign = GenerateSign(query.ToString());

            if (url.Contains("?"))
            {
                url = url.TrimEnd('&') + "&";
            }
            else
            {
                url = url.TrimEnd('?') + "?";
            }

            string cipherUrl = string.Format(url + "data={0}&sign={1}&r={2}",
                HttpUtility.UrlEncode(data),
                HttpUtility.UrlEncode(sign),
                DateTime.Now.ToFileTime());
            return cipherUrl;
        }

        /// <summary>
        /// 获取查询字符串明文
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static NameValueCollection GetUserQueryString(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return null;
            }
            return GetUserQueryString(new Uri(url));
        }

        /// <summary>
        /// 获取查询字符串明文
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static NameValueCollection GetUserQueryString(Uri uri)
        {
            if (uri == null || string.IsNullOrEmpty(uri.Query))
            {
                return null;
            }

            var values = HttpUtility.ParseQueryString(uri.Query);
            string data = values["data"];
            string sign = values["sign"];
            if (string.IsNullOrEmpty(data) || string.IsNullOrEmpty(sign))
            {
                return null;
            }

            string qs = AESUtil.Encrypt(data, AES_KEY);
            if (string.IsNullOrEmpty(qs))
            {
                return null;
            }

            if (VerifySign(qs, sign))
            {
                var query = HttpUtility.ParseQueryString(qs);
                DateTime time = DateTime.ParseExact(query["MTime"], "yyyyMMddHHmmss", null);
                if (DateTime.Now.Subtract(time) > MAX_LIFE_TIME)
                {
                    Logger.Error("上传文件地址，已超过 30 分钟", new Exception(uri.ToString()));
                    return null;
                }
                return query;
            }
            else
            {
                Logger.Error("上传文件地址，验证签名失败", new Exception(uri.ToString()));
            }
            return null;
        }

        /// <summary>
        /// 生成签名
        /// </summary>
        /// <param name="queryString">查询字符串</param>
        /// <returns></returns>
        private static string GenerateSign(string queryString)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(queryString + SIGN_KEY);
            byte[] md5 = MD5.Create().ComputeHash(bytes);
            return BitConverter.ToString(md5).Replace("-", "").ToLower();
        }

        /// <summary>
        /// 根据查询字符串明文，验证签名
        /// </summary>
        /// <param name="qs"></param>
        /// <param name="sign"></param>
        /// <returns></returns>
        private static bool VerifySign(string qs, string sign)
        {
            if (string.IsNullOrEmpty(qs) || string.IsNullOrEmpty(sign))
            {
                return false;
            }

            string c = GenerateSign(qs).ToLower();
            return (sign.ToLower() == c);
        }
    }
}