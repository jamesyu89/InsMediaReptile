using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;
using InstagramPhotos.Utility.FastReflection;

namespace InstagramPhotos.Utility.Helper
{
    /// <summary>
    /// 反射帮助类
    /// </summary>
    public class ReflectUtil
    {
        /// <summary>
        /// 转换单个对象为另外一种类型对象
        /// </summary>
        /// <typeparam name="TSource">待转换的源对象类型</typeparam>
        /// <typeparam name="TResult">转换的目标对象类型</typeparam>
        /// <param name="source">待转换的源对象</param>
        /// <returns>转换的目标对象</returns>
        public static TResult CloneObject<TSource, TResult>(TSource source) where TResult : new()
        {
            if (source == null)
            {
                return default(TResult);
            }
            TResult result = new TResult();
            CloneObject(source, result);
            return result;
        }

        /// <summary>
        /// 克隆对象
        /// </summary>
        /// <param name="source">原始对象 (需初始化)</param>
        /// <param name="dest">目标对象  (需初始化)</param>
        public static void CloneObject(object source, object dest)
        {
            var properties = source.GetType().GetProperties();
            var destType = dest.GetType();
            foreach (var item in properties)
            {
                var property = destType.GetProperty(item.Name);
                if (property == null)
                {
                    continue;
                }
                try
                {
                    property.FastSetValue(dest, item.FastGetValue(source));
                }
                catch
                {
                    object realValue = null;
                    try
                    {
                        var strValue = item.FastGetValue(source);
                        realValue = Convert.ChangeType(strValue, item.PropertyType);
                        property.FastSetValue(dest, realValue);
                    }
                    catch
                    {
                    }
                }
            }
        }

        public static string CreateObjString<TSource>(TSource source) where TSource : new()
        {
            if (source == null)
            {
                return string.Empty;
            }

            var sb = new StringBuilder(512);
            var properties = source.GetType().GetProperties();
            foreach (var item in properties)
            {
                try
                {
                    sb.AppendFormat("{0}:{1} ", item.Name, item.FastGetValue(source));
                }
                catch
                {
                }
            }

            return sb.ToString();
        }

        public static TResult GenerateObject<TResult>(IDictionary<string, string> dict, Encoding encoder)
            where TResult : new()
        {
            var response = default(TResult);
            if (dict == null)
            {
                return response;
            }

            response = new TResult();

            var propArr = response.GetType().GetProperties();

            var keyArr = dict.Keys;
            foreach (var item in keyArr)
            {

                var prop = propArr.Where(x => string.Compare(x.Name, item, true) == 0).FirstOrDefault();
                if (prop == null)
                {
                    continue;
                }

                var strValue = HttpUtility.UrlDecode(dict[item], encoder) ?? string.Empty;
                try
                {
                    prop.FastSetValue(response, strValue);
                }
                catch
                {
                    object realValue = null;
                    try
                    {
                        realValue = Convert.ChangeType(strValue, prop.PropertyType);
                        prop.FastSetValue(response, realValue);
                    }
                    catch
                    {
                    }
                }

            }

            return response;
        }

        public static TResult GenerateObject<TResult>(NameValueCollection collection, Encoding encoder)
            where TResult : new()
        {
            var response = default(TResult);
            if (collection == null)
            {
                return response;
            }

            response = new TResult();

            var propArr = response.GetType().GetProperties();

            var keyArr = collection.AllKeys;
            foreach (var item in keyArr)
            {

                var prop = propArr.Where(x => string.Compare(x.Name, item, true) == 0).FirstOrDefault();
                if (prop == null)
                {
                    continue;
                }

                var strValue = HttpUtility.UrlDecode(collection[item], encoder) ?? string.Empty;
                try
                {
                    prop.FastSetValue(response, strValue);
                }
                catch
                {
                    object realValue = null;
                    try
                    {
                        realValue = Convert.ChangeType(strValue, prop.PropertyType);
                        prop.FastSetValue(response, realValue);
                    }
                    catch
                    {
                    }
                }

            }

            return response;
        }
    }
}