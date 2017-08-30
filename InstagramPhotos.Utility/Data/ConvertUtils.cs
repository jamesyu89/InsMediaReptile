using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml;

namespace InstagramPhotos.Utility.Data
{
    public static class ConvertUtils
    {
        public static Guid ToGuid(this string str)
        {
            if (str == null)
            {
                return Guid.Empty;
            }
            return new Guid(str);
        }

        public static string ToGuidStr(this string str)
        {
            if (str == null)
            {
                return null;
            }
            return str.ToUpper();
        }

        public static string ToGuidStr(this Guid guid)
        {
            return guid.ToString().ToUpper();
        }

        public static string ToGuidStr(this Guid? guid)
        {
            if (guid == null)
            {
                return null;
            }
            return guid.Value.ToString().ToUpper();
        }

        public static bool ToBool(this short s)
        {
            return s != 0;
        }

        public static short ToShort(this bool b)
        {
            return b ? (short) 1 : (short) 0;
        }

        public static bool ToBool(this short? s)
        {
            return s != 0;
        }

        #region Functions

        public static object IDFromString(String id)
        {
            return !string.IsNullOrEmpty(id) ? (object) Int64.Parse(id) : DBNull.Value;
        }

        public static string CleanSearchString(string searchString)
        {
            if (string.IsNullOrEmpty(searchString))
                return null;

            // Do wild card replacements
            searchString = searchString.Replace("*", "%");

            // Strip any markup characters
            //searchString = Transforms.StripHtmlXmlTags(searchString);

            // Remove known bad SQL characters
            searchString = Regex.Replace(searchString, "--|;|'|\"", " ", RegexOptions.Compiled | RegexOptions.Multiline);

            // Finally remove any extra spaces from the string
            searchString = Regex.Replace(searchString, " {1,}", " ",
                RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Multiline);

            return searchString;
        }

        public static List<int> PopulateReadersToIds(IDataReader dr, string key)
        {
            return PopulateReadersToIds<int>(dr, key);
        }

        public static List<T> PopulateReadersToIds<T>(IDataReader dr, string key)
        {
            var ids = new List<T>();
            //Dictionary<T, bool> existsIds = new Dictionary<T, bool>();
            while (dr.Read())
            {
                var id = (T) dr[key];
                //if (!existsIds.ContainsKey(id))
                //{
                //existsIds.Add(id, true);
                ids.Add(id);
                //}
            }
            return ids;
        }

        public static List<T> PopulateReadersToIds<T>(IDataReader dr)
        {
            var ids = new List<T>();
            //Dictionary<T, bool> existsIds = new Dictionary<T, bool>();
            while (dr.Read())
            {
                var id = (T) dr[0];
                //if (!existsIds.ContainsKey(id))
                //{
                //existsIds.Add(id, true);
                ids.Add(id);
                //}
            }
            return ids;
        }

        public static Dictionary<string, DateTime> PopulateReadersToDic(IDataReader dr, string key, string value)
        {
            return PopulateReadersToDic<string, DateTime>(dr, key, value);
        }

        public static Dictionary<T1, T2> PopulateReadersToDic<T1, T2>(IDataReader dr, string key, string value)
        {
            var result = new Dictionary<T1, T2>();

            while (dr.Read())
            {
                var name = (T1) dr[key];
                var date = (T2) dr[value];

                result.Add(name, date);
            }
            return result;
        }

        public static object ConvertIdsToXML<T>(string itemName, T[] ids)
        {
            string rootName = itemName + "s";
            const string idName = "i";
            return ConvertIdsToXML(rootName, itemName, idName, ids);
        }

        public static object ConvertModelListToXML<T>(string itemName, List<T> modelList)
        {
            if (modelList == null)
            {
                return DBNull.Value;
            }

            var sw = new StringWriter();
            var writer = new XmlTextWriter(sw);
            writer.WriteStartElement(itemName + "s");

            foreach (T model in modelList)
            {
                writer.WriteStartElement(itemName);
                foreach (PropertyInfo p in typeof (T).GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    try
                    {
                        writer.WriteAttributeString(p.Name,
                            p.GetValue(model, null) == null ? "" : p.GetValue(model, null).ToString());
                    }
                    catch
                    {
                        writer.WriteAttributeString(p.Name, DBNull.Value.ToString());
                    }
                }
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
            writer.Close();
            return sw.ToString();
        }

        public static object ConvertDicToXML<T, K>(string rootName, Dictionary<T, K> dictionary)
        {
            if (dictionary == null)
            {
                return DBNull.Value;
            }

            var sw = new StringWriter();
            var writer = new XmlTextWriter(sw);
            writer.WriteStartElement(rootName + "s");
            foreach (var item in dictionary)
            {
                writer.WriteStartElement(rootName);
                writer.WriteAttributeString("k", item.Key.ToString());
                writer.WriteAttributeString("v", item.Value.ToString());
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
            writer.Close();
            return sw.ToString();
        }

        public static object ConvertIdsToXML<T>(string rootName, string itemName, string idName, T[] ids)
        {
            if (ids == null)
                return DBNull.Value;

            var sw = new StringWriter();
            var writer = new XmlTextWriter(sw);
            writer.WriteStartElement(rootName);
            foreach (T id in ids)
            {
                writer.WriteStartElement(itemName);
                writer.WriteAttributeString(idName, id.ToString());
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
            writer.Close();
            return sw.ToString();
        }

        #endregion
    }
}