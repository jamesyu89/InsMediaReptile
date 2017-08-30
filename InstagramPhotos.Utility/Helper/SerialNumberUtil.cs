using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace InstagramPhotos.Utility.Helper
{
    /// <summary>
    /// 全局唯一序列号生成器
    /// </summary>
    public class SerialNumberUtil
    {
        #region 字段

        private static readonly Object syncObj = new Object();

        private static int incremental = 10;

        /// <summary>
        /// 服务器IP地址最后三位构成的字符串 不足三位以0左对齐补齐
        /// </summary>
        private static string serverIPLast3Number = string.Empty;

        #endregion

        /// <summary>
        /// 按照时间生成线程安全的几乎唯一的流水号
        /// </summary>
        /// <returns></returns>
        public static long Create()
        {
            long serialNum = 0;

            lock (syncObj)
            {
                incremental = incremental + 1;
                if (incremental > 99)
                {
                    incremental = 10;
                }

                var time = string.Format("{0}{1}", DateTime.Now.ToString("yyyyMMddHHmmssfff"), incremental);
                serialNum = Convert.ToInt64(time);
            }

            return serialNum;
        }

        /// <summary>
        /// 按照时间生成线程安全的几乎唯一的流水号字符串
        /// </summary>
        /// <returns></returns>
        public static string CreateLongTimeString()
        {
            var num = Create();
            return num.ToString();
        }

        /// <summary>
        /// 按照时间+ip地址+随机数生成线程安全的几乎唯一的流水号
        /// </summary>
        /// <returns></returns>
        public static string CreateNumString()
        {
            var strResult = string.Empty;
            var ipNum = GetServerIpLast3Number();

            lock (syncObj)
            {
                incremental = incremental + 1;
                if (incremental > 99)
                {
                    incremental = 10;
                }

                strResult = string.Format("{0}{1}{2}", DateTime.Now.ToString("yyyyMMddHHmmssfff"), ipNum, incremental);
            }

            return strResult;
        }

        /// <summary>
        /// 获取全局唯一的32位的guid
        /// </summary>
        /// <returns></returns>
        public static string GetGuid()
        {
            return Guid.NewGuid().ToString("n");
        }

        /// <summary>
        /// machineKey的生成算法
        /// </summary>
        /// <param name="len"></param>
        /// <returns></returns>
        public static string CreateMachineKey(int len)
        {
            if (len < 1)
            {
                return string.Empty;
            }

            var bytes = new byte[len];

            new RNGCryptoServiceProvider().GetBytes(bytes);

            var sb = new StringBuilder();

            for (int i = 0; i < bytes.Length; i++)
            {
                sb.Append(string.Format("{0:X2}", bytes[i]));
            }

            return sb.ToString();
        }


        #region 帮助方法

        /// <summary>
        /// 获取服务器IP地址的最后的数字 三位，不足以0补齐
        /// </summary>
        /// <returns></returns>
        private static string GetServerIpLast3Number()
        {
            if (string.IsNullOrWhiteSpace(serverIPLast3Number) == false)
            {
                return serverIPLast3Number;
            }

            try
            {
                var ipString = Dns.GetHostAddresses(string.Empty)
                    .First(x => x.IsIPv6LinkLocal == false && x.AddressFamily == AddressFamily.InterNetwork)
                    .ToString();

                serverIPLast3Number = ipString.Split('.').Last() ?? string.Empty;
            }
            catch
            {
                serverIPLast3Number = string.Empty;
            }

            if (IsNumber(serverIPLast3Number) == false) //非数字 构造三位数字
            {
                for (int i = 0; i < 3; i++)
                {
                    var rdm = new Random(Guid.NewGuid().GetHashCode());
                    serverIPLast3Number = string.Format("{0}{1}", serverIPLast3Number, rdm.Next(0, 9));

                }
            }

            if (serverIPLast3Number.Length < 3)
            {
                serverIPLast3Number = serverIPLast3Number.PadLeft(3, '0');
            }

            return serverIPLast3Number;
        }

        /// <summary>
        /// 验证输入字符串是否全部由数字构成
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static bool IsNumber(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }

            var regNum = new Regex(@"^\d+$", RegexOptions.CultureInvariant);

            return regNum.IsMatch(input);
        }

        #endregion
    }
}