using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web;

namespace InstagramPhotos.Utility.Helper
{
    public  class Functions
    {
        /// <summary>
        /// 检测数据并且添加
        /// </summary>
        /// <param name="htSouce"></param>
        /// <param name="keyName"></param>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public static Hashtable CheckValueToSelect(Hashtable htSouce, string keyName, object keyValue)
        {
            string value = keyValue == null ? "" : keyValue.ToString();
            if (!string.IsNullOrWhiteSpace(value) && value != "-999")
            {
                if (!htSouce.Contains(keyName))
                {

                    htSouce.Add(keyName, keyValue.ToString());
                }
                else
                {

                    htSouce[keyName] = keyValue.ToString();
                }
            }
            else
            {
                if (!htSouce.Contains(keyName))
                {

                    htSouce.Remove(keyName);
                }
            }
            return htSouce;
        }

        public static string SQLConnectDecrypt(string EncryptStr)
        {
            return YGCFDDecrypt.Crypto.DecryptString(EncryptStr);
        }
        /// <summary>
        /// 检测数据中修改人和修改时间
        /// </summary>
        /// <param name="htValue"></param>
        /// <param name="loginedUser"></param>
        /// <returns></returns>
        public static Hashtable ModifyByCheck(Hashtable htValue)
        {
            if (!htValue.Contains("Rec_ModifyTime"))
            {
                htValue.Add("Rec_ModifyTime", "getdate()");
            }
            if (!htValue.Contains("Rec_ModifyBy"))
            {
                htValue.Add("Rec_ModifyBy", "admin");
            }
            return htValue;
        }

        /// <summary>
        /// 检测数据中创建人和创建时间
        /// </summary>
        /// <param name="htValue"></param>
        /// <param name="loginedUser"></param>
        /// <returns></returns>
        public static Hashtable CreateByCheck(Hashtable htValue)
        {
            if (!htValue.Contains("Rec_CreateTime"))
            {
                htValue.Add("Rec_CreateTime", "getdate()");
            }
            if (!htValue.Contains("Rec_CreateBy"))
            {
                htValue.Add("Rec_CreateBy", HttpContext.Current == null ? "admin" :"admin");
            }
            return htValue;
        }
        /// <summary>
        /// 检测数据并且添加
        /// </summary>
        /// <param name="htSouce"></param>
        /// <param name="KeyName"></param>
        /// <param name="KeyValue"></param>
        /// <param name="isExceptNullOrEmpty">是否排除空值 当为空值时hashtable中删除此键值对</param>
        /// <returns></returns>
        public static Hashtable CheckValueToAdd(Hashtable htSouce, string KeyName, object KeyValue, bool isExceptNullOrEmpty = false)
        {
            if (isExceptNullOrEmpty)
            {
                if (KeyValue is Guid)
                {
                    if (Guid.Parse(KeyValue.ToString()) == Guid.Empty)
                    {
                        if (htSouce.Contains(KeyName))
                        {
                            htSouce.Remove(KeyName);
                        }
                        return htSouce;
                    }
                }
                else
                {
                    if (KeyValue == null)
                    {
                        if (htSouce.Contains(KeyName))
                        {
                            htSouce.Remove(KeyName);
                        }
                        return htSouce;
                    }
                }
            }
            if (KeyValue != null && KeyValue.ToString() != "" && KeyValue.ToString() != "$null")
            {
                if (!htSouce.Contains(KeyName))
                {
                    if (KeyValue.ToString() != "" && KeyValue.ToString() != "-999")
                    {
                        htSouce.Add(KeyName, KeyValue.ToString());
                    }
                }
                else
                {
                    if (KeyValue.ToString() != "" && KeyValue.ToString() != "-999")
                    {
                        htSouce[KeyName] = KeyValue.ToString();
                    }
                }
            }
            else
            {
                if (KeyValue == null)
                {
                    if (!htSouce.Contains(KeyName))
                    {

                        htSouce.Add(KeyName, null);
                    }
                    else
                    {
                        htSouce[KeyName] = null;
                    }
                }
                else
                {
                    if (!htSouce.Contains(KeyName))
                    {

                        htSouce.Add(KeyName, "");
                    }
                    else
                    {
                        htSouce[KeyName] = "";
                    }
                }
            }
            return htSouce;
        }

        /// <summary>
        /// 移除hashtable中无效数据的键值对
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public static Hashtable CheckValue(ref Hashtable ht)
        {
            Hashtable newHT = new Hashtable();
            if (ht != null)
            {
                foreach (DictionaryEntry kv in ht)
                {
                    if (kv.Value == null || string.IsNullOrWhiteSpace(kv.Value.ToString()))
                    {
                        continue;
                    }
                    CheckValueToAdd(newHT, kv.Key.ToString(), kv.Value);
                }
            }
            ht = newHT;
            return ht;
        }

        public static Hashtable CheckValueToAdd(Hashtable htSouce, string KeyName, object KeyValue, string DefaultValue)
        {
            if (KeyValue != null && KeyValue.ToString() != "")
            {
                if (!htSouce.Contains(KeyName))
                {
                    if (KeyValue.ToString() != "" && KeyValue.ToString() != "-999")
                    {
                        htSouce.Add(KeyName, KeyValue.ToString());
                    }
                }
                else
                {
                    if (KeyValue.ToString() != "" && KeyValue.ToString() != "-999")
                    {
                        htSouce[KeyName] = KeyValue.ToString();
                    }
                }
            }
            else
            {
                if (!htSouce.Contains(KeyName))
                {

                    htSouce.Add(KeyName, DefaultValue);
                }
                else
                {
                    htSouce[KeyName] = DefaultValue;
                }

            }
            return htSouce;
        }
        /// <summary>
        /// 获取表中某字段集合，返回List Guid
        /// </summary>
        /// <param name="data">表</param>
        /// <param name="colName">要获取数据的字段</param>
        /// <returns></returns>
        public static List<Guid> GetGuidColl(DataTable data, string colName)
        {
            var alOrderId = new List<Guid>();

            foreach (DataRow row in data.Rows)
            {
                alOrderId.Add(new Guid(row[colName].ToString()));
            }
            return alOrderId;
        }

        /// <summary>
        /// 获取表中某字段集合，返回List string
        /// </summary>
        /// <param name="data">表</param>
        /// <param name="colName">要获取数据的字段</param>
        /// <returns></returns>
        public static List<string> GetStrColl(DataTable data, string colName)
        {
            var alOrderId = new List<string>();

            foreach (DataRow row in data.Rows)
            {
                alOrderId.Add(row[colName].ToString());
            }
            return alOrderId;
        }

        public static Hashtable MergeHashtable(params Hashtable[] hts)
        {
            if (hts.Length == 0) return new Hashtable();
            Hashtable htFirst = hts[0];
            for (int i = 1; i < hts.Length; i++)
            {
                foreach (DictionaryEntry param in hts[i])
                {
                    if (htFirst.ContainsKey(param.Key))
                    {
                        htFirst[param.Key] = param.Value;
                        continue;
                    }
                    htFirst.Add(param.Key, param.Value);
                }
            }
            return htFirst;
        }
    }
}
