using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using ServiceStack;
using InstagramPhotos.Utility.Log;

namespace InstagramPhotos.Utility.Utility
{
    public static class SerializeHelper
    {
        private static readonly JsonSerializerSettings settings;

        static SerializeHelper()
        {
            settings = new JsonSerializerSettings
            {
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                DateFormatString = "yyyy-MM-dd HH:mm:ss.fff"
            };
        }

        public static string ToJson(object model)
        {
            return JsonConvert.SerializeObject(model, settings);
        }


        public static T DeserializeObject<T>(string json) where T : class
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static T Deserialize<T>(string json)
        {
            if (json == null)
                return default(T);

            if ((typeof(T) == typeof(string)) || (typeof(T) == typeof(int)) || (typeof(T) == typeof(DateTime)))
                return json.To<T>();

            return JsonConvert.DeserializeObject<T>(json);
        }

        public static string ToJson(object jsonObject, string dateFormate)
        {
            var timeConverter = new IsoDateTimeConverter();
            timeConverter.DateTimeFormat = dateFormate;
            return JsonConvert.SerializeObject(jsonObject, Formatting.Indented, timeConverter);
        }

        /// <summary>
        ///     序列化XML文件
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        public static string XmlSerialize<T>(T obj)
        {
            var Stream = new MemoryStream();
            //创建序列化对象 
            var xml = new XmlSerializer(typeof(T));
            try
            {
                //序列化对象 
                xml.Serialize(Stream, obj);
            }
            catch (Exception ex)
            {
                //Logger.Exception(ex);
            }
            Stream.Position = 0;
            var sr = new StreamReader(Stream);
            var str = sr.ReadToEnd();
            return str;
        }

        public static Dictionary<string, string> GetSpecifiedUnit(string source)
        {
            var dict = new Dictionary<string, string>();
            var regex = new Regex(@"^({)([^}]+)(})");
            if (!regex.IsMatch(source, 0)) return null;
            var matchs = regex.Matches(source, 0);
            if (matchs.Count != 1) return null;
            var sourceChips = matchs[0].ToString().Substring(1, matchs[0].ToString().Length - 2).Split(',');
            foreach (var str in sourceChips)
            {
                var s = str.IndexOf(':');
                dict.Add(str.Substring(0, s).Replace("\"", ""), str.Substring(s + 1).Replace("\"", ""));
            }
            return dict;
        }

        public static SortedDictionary<string, string> GetSpecifiedSortedUnit(string source)
        {
            var dict = new SortedDictionary<string, string>();
            var regex = new Regex(@"^({)([^}]+)(})");
            if (!regex.IsMatch(source, 0)) return null;
            var matchs = regex.Matches(source, 0);
            if (matchs.Count != 1) return null;
            var sourceChips = matchs[0].ToString().Substring(1, matchs[0].ToString().Length - 2).Split(',');
            foreach (var str in sourceChips)
            {
                var s = str.IndexOf(':');
                dict.Add(str.Substring(0, s).Replace("\"", ""), str.Substring(s + 1).Replace("\"", ""));
            }
            return dict;
        }
    }
}