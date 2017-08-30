using System;
using System.Management;
using System.Net;
using System.Web;

namespace InstagramPhotos.Utility.Net.Extensions
{
    public static class IPAddressExtensions
    {
        public static long ToLong(this IPAddress address)
        {
            if (address.GetAddressBytes().Length == 4)
                return BitConverter.ToUInt32(address.GetAddressBytes(), 0);
            if (address.GetAddressBytes().Length == 8)
                return (long) BitConverter.ToUInt64(address.GetAddressBytes(), 0);
            throw new InvalidOperationException("Dude, what the heck?");
        }

        public static IPAddress Current()
        {
            HttpContext context = HttpContext.Current;
            IPAddress ipAddress;
            if (context != null && context.Request.UserHostAddress != null)
                ipAddress = IPAddress.Parse(GetIP(context));
            else
                ipAddress = IPAddress.Any;
            return ipAddress;
        }

        /// <summary>
        ///     Get real IP address from client-side.
        ///     If client is using proxy, try to return real client IP, else, return proxy IP.
        /// </summary>
        /// <param name="context">current HttpContext.</param>
        /// <returns>IP address as string format.</returns>
        /// <remarks>Wayne added on 2012-8-28</remarks>
        private static string GetIP(HttpContext context)
        {
            try
            {
                if (context.Request.ServerVariables["HTTP_VIA"] != null ||
                    context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null) // using proxy
                {
                    return context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].Split(',')[0].Trim();
                        // Return real client IP.
                }
                // Request.UserHostAddress's time cost is 380times than Request.ServerVariables["REMOTE_ADDR"]
                return context.Request.ServerVariables["REMOTE_ADDR"];
                    //While it can't get the Client IP, it will return proxy IP.
            }
            catch
            {
                return "0.0.0.0";
            }
        }

        public static string GetHost_IPAddress()
        {
            string result;
            try
            {
                ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapterConfiguration WHERE IPEnabled = 'TRUE'");
                ManagementObjectCollection managementObjectCollection = managementObjectSearcher.Get();
                using (ManagementObjectCollection.ManagementObjectEnumerator enumerator = managementObjectCollection.GetEnumerator())
                {
                    if (enumerator.MoveNext())
                    {
                        ManagementObject managementObject = (ManagementObject)enumerator.Current;
                        string[] array = (string[])managementObject["IPAddress"];
                        result = array[0];
                        return result;
                    }
                }
                result = "";
            }
            catch (Exception)
            {
                result = "";
            }
            return result;
        }

        /// <summary>
        /// 获取客户端IP地址
        /// </summary>
        /// <returns>若失败则返回回送地址</returns>
        public static string GetClientIP(HttpRequest request=null)
        {
            try
            {
                request = (request ?? HttpContext.Current.Request);
                //如果客户端使用了代理服务器，则利用HTTP_X_FORWARDED_FOR找到客户端IP地址
                string userHostAddress = request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString().Split(',')[0].Trim();
                //否则直接读取REMOTE_ADDR获取客户端IP地址
                if (string.IsNullOrEmpty(userHostAddress))
                {
                    userHostAddress = request.ServerVariables["REMOTE_ADDR"];
                }
                //前两者均失败，则利用Request.UserHostAddress属性获取IP地址，但此时无法确定该IP是客户端IP还是代理IP
                if (string.IsNullOrEmpty(userHostAddress))
                {
                    userHostAddress = request.UserHostAddress;
                }
                //最后判断获取是否成功，并检查IP地址的格式（检查其格式非常重要）
                if (!string.IsNullOrEmpty(userHostAddress) && IsIP(userHostAddress))
                {
                    return userHostAddress;
                }
            }
            catch (Exception ex)
            {
                return "0.0.0.0";
            }
            return "127.0.0.1";
        }

        /// <summary>
        /// 获取客户端IP地址
        /// </summary>
        /// <returns>若失败则返回回送地址</returns>
        public static string GetClientIP(HttpRequestBase requestBase)
        {
            try
            {
                //如果客户端使用了代理服务器，则利用HTTP_X_FORWARDED_FOR找到客户端IP地址
                string userHostAddress = requestBase.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString().Split(',')[0].Trim();
                //否则直接读取REMOTE_ADDR获取客户端IP地址
                if (string.IsNullOrEmpty(userHostAddress))
                {
                    userHostAddress = requestBase.ServerVariables["REMOTE_ADDR"];
                }
                //前两者均失败，则利用Request.UserHostAddress属性获取IP地址，但此时无法确定该IP是客户端IP还是代理IP
                if (string.IsNullOrEmpty(userHostAddress))
                {
                    userHostAddress = requestBase.UserHostAddress;
                }
                //最后判断获取是否成功，并检查IP地址的格式（检查其格式非常重要）
                if (!string.IsNullOrEmpty(userHostAddress) && IsIP(userHostAddress))
                {
                    return userHostAddress;
                }
            }
            catch (Exception ex)
            {
                return "0.0.0.0";
            }
            return "127.0.0.1";
        }

        /// <summary>
        /// 检查IP地址格式
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool IsIP(string ip)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }

        //获取主板序列号
        public static string GetBIOSSerialNumber()
        {
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("Select * From Win32_BIOS");
                string sBIOSSerialNumber = "";
                foreach (ManagementObject mo in searcher.Get())
                {
                    sBIOSSerialNumber = mo["SerialNumber"].ToString().Trim();
                }
                return sBIOSSerialNumber;
            }
            catch
            {
                return "";
            }
        }
        //获取CPU序列号
        public static string GetCPUSerialNumber()
        {
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("Select * From Win32_Processor");
                string sCPUSerialNumber = "";
                foreach (ManagementObject mo in searcher.Get())
                {
                    sCPUSerialNumber = mo["ProcessorId"].ToString().Trim();
                }
                return sCPUSerialNumber;
            }
            catch
            {
                return "";
            }
        }
        //获取硬盘序列号
        public static string GetHardDiskSerialNumber()
        {
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMedia");
                string sHardDiskSerialNumber = "";
                foreach (ManagementObject mo in searcher.Get())
                {
                    sHardDiskSerialNumber = mo["SerialNumber"].ToString().Trim();
                    break;
                }
                return sHardDiskSerialNumber;
            }
            catch
            {
                return "";
            }
        }
        //获取网卡地址
        public static string GetNetCardMACAddress()
        {
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapter WHERE ((MACAddress Is Not NULL) AND (Manufacturer <> 'Microsoft'))");
                string NetCardMACAddress = "";
                foreach (ManagementObject mo in searcher.Get())
                {
                    NetCardMACAddress = mo["MACAddress"].ToString().Trim();
                }
                return NetCardMACAddress;
            }
            catch
            {
                return "";
            }
        }
    }
}