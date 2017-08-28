using System;
using System.Configuration;
using System.Linq;

namespace InstagramPhotos.Utility.Configuration
{
    public class AppSettings
    {
        public static T GetValue<T>(string key)
        {
            return GetValue(key, default(T));
        }

        public static T GetValue<T>(string key, T defaultValue)
        {
            T value;

            if (ConfigurationManager.AppSettings.AllKeys.Contains(key))
                value = (T) Convert.ChangeType(ConfigurationManager.AppSettings[key], typeof (T));
            else
                value = defaultValue;

            return value;
        }
    }
}