using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace InstagramPhotos.Utility.Utility
{
    /// <summary>
    /// 枚举类型帮助类
    /// </summary>
    public static class EnumHelper
    {
        /// <summary>
        /// 返回枚举项的描述信息。
        /// </summary>
        /// <param name="value">要获取描述信息的枚举项。</param>
        /// <returns>枚举想的描述信息。</returns>
        //public static string GetDescription(Enum value)
        //{
        //    Type enumType = value.GetType();
        //    // 获取枚举常数名称。
        //    string name = Enum.GetName(enumType, value);
        //    if (name != null)
        //    {
        //        // 获取枚举字段。
        //        FieldInfo fieldInfo = enumType.GetField(name);
        //        if (fieldInfo != null)
        //        {
        //            // 获取描述的属性。
        //            DescriptionAttribute attr = Attribute.GetCustomAttribute(fieldInfo,
        //                typeof(DescriptionAttribute), false) as DescriptionAttribute;
        //            if (attr != null)
        //            {
        //                return attr.Description;
        //            }
        //        }
        //    }
        //    return null;
        //}

        public static string GetDescription(this Enum value)
        {
            Type enumType = value.GetType();
            // 获取枚举常数名称。
            string name = Enum.GetName(enumType, value);
            if (name != null)
            {
                // 获取枚举字段。
                FieldInfo fieldInfo = enumType.GetField(name);
                if (fieldInfo != null)
                {
                    // 获取描述的属性。
                    DescriptionAttribute attr = Attribute.GetCustomAttribute(fieldInfo,
                        typeof(DescriptionAttribute), false) as DescriptionAttribute;
                    if (attr != null)
                    {
                        return attr.Description;
                    }
                }
            }
            return null;
            /* how to use
                MyEnum x = MyEnum.NeedMoreCoffee;
                string description = x.GetDescription();
            */

        }

        /// <summary>
        /// 获取枚举描述列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<string> GetListOfDescription<T>() where T : struct
        {
            Type t = typeof(T);
            return !t.IsEnum ? null : Enum.GetValues(t).Cast<Enum>().Select(x => x.GetDescription()).ToList();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<string> GetListOfName<T>() where T : struct
        {
            return Enum.GetNames(typeof(T)).ToList();
        }
        /// <summary>
        /// 获取枚举描述和值键值对
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Dictionary<string, int> GetDescValueList<T>() where T : struct
        {
            
            var type = typeof (T);
            if(!type.IsEnum) return new Dictionary<string, int>();
            try
            {
                Dictionary<string, int> dics = new Dictionary<string, int>();
                var enumNamesList = Enum.GetValues(type);
                foreach (var enumName in enumNamesList)
                {
                    T value = ((T)enumName);
                    var desc = GetDescription(value as Enum);
                    dics.Add(desc, value.GetHashCode());
                }
                return dics;
            }
            catch (Exception)
            {
                return new Dictionary<string, int>();
            }
        }
    }
}
