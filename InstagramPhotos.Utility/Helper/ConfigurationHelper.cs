using System;
using System.Configuration;
using System.Web.Configuration;
using System.Web.WebPages;

namespace InstagramPhotos.Utility.Helper
{
    /// <summary>
    /// 获取配置文件内容的帮助类
    /// </summary>
    public class ConfigurationHelper
    {
        /// <summary>
        /// 获取AppSetting节点值
        /// </summary>
        /// <param name="key">AppSetting节点中的Key</param>
        /// <returns></returns>
        public static string GetValueAppSetting(string key)
        {
            string value = ConfigurationManager.AppSettings[key].Trim();
            return value;
        }
    }
    /// <summary>
    /// 对配置节点的读取和写入
    /// </summary>
    public static class Setting
    {
        private const string Prefix = "config";
        public static readonly string CurrentTheme = GetValue<string>("CurrentTheme");
        public static readonly bool EnableTiles = GetValue<bool>("EnableTiles");
        public static T GetValue<T>(string key, string prefix = Prefix)
        {
            string entry = string.Format("{0}:{1}", prefix, key);
            if (string.IsNullOrWhiteSpace(entry))
            {
                return default(T);
            }
            string value = ConfigurationManager.AppSettings[entry];
            if (string.IsNullOrWhiteSpace(value))
            {
                return default(T);
            }
            if (typeof(T).IsEnum)
            {
                return (T)Enum.Parse(typeof(T), value, true);
            }

            if (typeof(T) == typeof(bool) && value.Is<int>())
            {
                return (T)Convert.ChangeType(value.As<int>(), typeof(T));
            }
            return (T)Convert.ChangeType(value, typeof(T));
        }

        public static T SetValue<T>(string key, string value)
        {
            System.Configuration.Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
            config.AppSettings.Settings[key].Value = value;
            ConfigurationManager.RefreshSection("appSettings");
            return default(T);
        }
    }
}
