using System.Management;
using System.Net;
using System.Text.RegularExpressions;

namespace InstagramPhotos.Utility.Utility
{
    public static class SysInfoHelper
    {
        #region 获得IP地址
        public static string GetIPAddress()
        {

            IPAddress[] ips = Dns.GetHostAddresses(Dns.GetHostName());

            string ip = string.Empty;

            for (int i = 0; i < ips.Length; i++)
            {
                ip += ips[i].ToString() + ",";
            }

            if (ip != string.Empty)
            {
                ip = ip.Substring(0, ip.Length - 1);
            }

            return ip;

            /*
            string ip = string.Empty;
            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc = mc.GetInstances();

            foreach (ManagementObject mo in moc)
            {
                if ((bool)mo["IPEnabled"] == true)
                {
                    string[] ipaddresses = (string[])mo["IPAddress"];

                    for (int i = 0; i < ipaddresses.Length; i++)
                    {
                        ip += ipaddresses[i].ToString() + ",";
                    }
                }
            }

            if (ip != string.Empty)
            {
                ip = ip.Substring(0, ip.Length - 1);
            }

            return ip;
           */

        }
        #endregion

        #region 获得MAC
        public static string GetAllMacAddress()
        {
            ManagementClass adapters = new ManagementClass("Win32_NetworkAdapterConfiguration");
            string MACAddress = string.Empty;
            foreach (ManagementObject adapter in adapters.GetInstances())
            {
                if ((bool)adapter["IPEnabled"] == true)
                {
                    MACAddress = adapter.Properties["MACAddress"].Value.ToString();
                    MACAddress += ",";
                }
            }

            //去除最后一个,号
            if (MACAddress != string.Empty)
            {
                MACAddress = MACAddress.Substring(0, MACAddress.Length - 1);
            }

            return MACAddress;
        }
        #endregion

        public static bool IsValidateOfMac(string mac)
        {
            bool flag = false;
            string s = @"^([0-9a-fA-F]{2})(([/\s:-][0-9a-fA-F]{2}){5})$";

            Regex r = new Regex(s, RegexOptions.IgnoreCase);

            Match m = r.Match(mac);
            if (m.Success)
            {
                if (Regex.IsMatch(mac, "[a-z]"))
                {
                    flag = false;
                }
                else
                {
                    flag = true;
                }
            }
            else
            {
                flag = false;
            }
            return flag;
        }

        public static string GetMacAddress()
        {
            var adapters = new ManagementClass("Win32_NetworkAdapterConfiguration");
            foreach (ManagementObject adapter in adapters.GetInstances())
            {
                if (adapter.Properties["MACAddress"].Value != null)
                {
                    return adapter.Properties["MACAddress"].Value.ToString();
                }
            }
            return string.Empty;
        }

    }



}
