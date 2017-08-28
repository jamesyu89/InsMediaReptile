using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using InstagramPhotos.Utility.Log;

namespace InstagramPhotos.Utility.Configuration
{
    public class FileAppSettings
    {
        private const string APPSETTINGS_SECTIONNAME = "appSettings";

        private static Dictionary<string, object> settings;

        private static readonly object locker = new object();

        static FileAppSettings()
        {
            LoadConfig();

            try
            {
                string configSource =
                    (ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None)
                        .AppSettings.SectionInformation.ConfigSource);
                var fsw = new FileSystemWatcher(Path.GetDirectoryName(configSource), Path.GetFileName(configSource));
                fsw.BeginInit();
                fsw.Changed += (sender, e) => LoadConfig();
                fsw.NotifyFilter = NotifyFilters.LastWrite;
                fsw.EnableRaisingEvents = true;
                fsw.EndInit();
            }
            catch (Exception e)
            {
                Logger.Exception("FileSystemWatcher failed, pls check if you have ADMIN permission", e);
                throw;
            }
        }

        private static void LoadConfig()
        {
            lock (locker)
            {
                ConfigurationManager.RefreshSection(APPSETTINGS_SECTIONNAME);

                settings = new Dictionary<string, object>();
                foreach (string key in ConfigurationManager.AppSettings.AllKeys)
                {
                    settings.Add(key, ConfigurationManager.AppSettings[key]);
                }
            }
        }

        #region Public Methods.

        public static T GetValue<T>(string key)
        {
            return GetValue(key, default(T));
        }

        public static T GetValue<T>(string key, T defaultValue)
        {
            return GetValue(key, default(T), false);
        }

        public static T GetValue<T>(string key, T defaultValue, bool SetDefaultValue)
        {
            T value = defaultValue;

            if (settings.ContainsKey(key))
                value = (T) Convert.ChangeType(settings[key], typeof (T));
            else if (SetDefaultValue)
                SetValue(key, defaultValue.ToString());

            return value;
        }

        public static void SetValue(string key, object value)
        {
            System.Configuration.Configuration config =
                ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            if (settings.ContainsKey(key))
                config.AppSettings.Settings.Remove(key);

            config.AppSettings.Settings.Add(key, value.ToString());

            config.Save(ConfigurationSaveMode.Modified);
            LoadConfig();
        }

        #endregion
    }
}