using System;
using System.Collections.Generic;
using System.Reflection;

namespace InstagramPhotos.Utility.Utility
{
    /// <summary>
    /// 类型转换辅助类
    /// </summary>
    public class ConvertHelper
    {
        public static T1 ConvertObject<T1, T2>(T2 source)
        {
            T1 obj = (T1)EmitHelper.GetInstanceCreator(typeof(T1)).Invoke();
            foreach (PropertyInfo p in typeof(T2).GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                PropertyInfo newP = typeof(T1).GetProperty(p.Name);
                newP.SetValue(obj, p.GetValue(source, null), null);
            }
            return obj;
        }

        /// <summary>
        /// 转换xml字符串中的特殊字符
        /// </summary>
        /// <returns></returns>
        public static string ParseXML(string xmlStr)
        {
            xmlStr = xmlStr.Replace("&", "&amp;");
            return xmlStr;
        }

        /// <summary>
        /// 将输入对象转化为指定的类型。
        /// </summary>
        /// <typeparam name="T">要转化为的类型</typeparam>
        /// <param name="input">输入对象</param>
        /// <returns></returns>
        /// <remarks>支持类型：Boolean、Char、SByte、Byte、Int16、Int32、Int64、
        /// UInt16、UInt32、UInt64、Single、Double、Decimal、DateTime 和 String。包括值类型的Nullable类型</remarks>
        public static T ConvertType<T>(object input)
        {
            //input是否为DBNull
            if (Convert.IsDBNull(input))
            {
                //转化DBNull为null
                input = null;
            }
            //input为空直接返回input
            if (input == null)
            {
                return (T)(input);
            }
            Type type = typeof(T);
            //判断类型是否为Nullable结构定义的类型，如果是则取得原有类型
            if (type.IsGenericType)
            {
                type = type.GetGenericArguments()[0];
            }
            if (type.IsEnum)
            {
                return ParseEnum<T>(input.ToString());
            }
            return (T)(Convert.ChangeType(input, type));
        }

        /// <summary>
        /// 将一个或多个枚举常数的名称或数字值的字符串表示转换成等效的枚举对象。此运算不区分大小写。
        /// </summary>
        /// <param name="value">包含要转换的值或名称的字符串。</param>
        /// <returns></returns>
        /// <typeparam name="T">枚举的 Type。</typeparam>
        public static T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        /// <summary>  
        ///   
        /// 将对象属性转换为key-value对  
        /// </summary>  
        /// <param name="o"></param>  
        /// <returns></returns>  
        public static Dictionary<String, Object> ToDicMap(Object o)
        {
            Dictionary<String, Object> map = new Dictionary<string, object>();

            Type t = o.GetType();

            PropertyInfo[] pi = t.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo p in pi)
            {
                MethodInfo mi = p.GetGetMethod();

                if (mi != null && mi.IsPublic)
                {
                    map.Add(p.Name, mi.Invoke(o, new Object[] { }));
                }
            }

            return map;
        }
    }
}
