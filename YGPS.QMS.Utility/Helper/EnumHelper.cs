using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using SystemEnum = System.Enum;

namespace InstagramPhotos.Utility.Helper
{
    /// <summary>
    /// 枚举帮助类
    /// </summary>
    public class EnumHelper
    {
        /// <summary>
        /// 根据枚举的int值类型转换为枚举类型
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="currentValue">枚举的int值</param>
        /// <returns>枚举类型</returns>
        public static T GetEnumByValue<T>(int currentValue)
        {
            return (T)SystemEnum.Parse(typeof(T), currentValue.ToString());
        }

        /// <summary>
        /// 根据枚举值获取枚举类型
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="currentValue">枚举的值</param>
        /// <returns>枚举值</returns>
        public static T GetEnumByEnumKey<T>(string currentValue)
        {
            return (T)SystemEnum.Parse(typeof(T), currentValue);
        }

        /// <summary>
        /// 根据枚举的枚举项获取枚举值的详细说明
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="currentValue">枚举的枚举项 如：Info、Degug、Error、Fatal</param>
        /// <returns>枚举值说明</returns>
        public static string GetDescriptionByEnumKey<T>(string currentValue)
        {
            var result = string.Empty;
            try
            {
                T currentType = GetEnumByEnumKey<T>(currentValue);
                result = GetEnumDescription<T>(currentType);
            }
            catch
            {
            }
            return result;
        }

        /// <summary>
        /// 根据枚举的int值获取枚举值的详细说明
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="currentValue">枚举的Int值</param>
        /// <returns>枚举值说明</returns>
        public static string GetDescriptionByValue<T>(int currentValue)
        {
            var result = string.Empty;
            try
            {
                T currentType = GetEnumByValue<T>(currentValue);
                result = GetEnumDescription<T>(currentType);
            }
            catch
            {
            }
            return result;
        }

        /// <summary>
        /// 获取枚举值的说明
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="enumType">枚举类型值</param>
        /// <returns>枚举值说明</returns>
        public static string GetEnumDescription<T>(T enumType)
        {
            var description = enumType.ToString();
            Type currentType = enumType.GetType();
            //获取字段信息   
            FieldInfo[] fields = currentType.GetFields();

            if (fields == null || fields.Length < 1)
            {
                return description;
            }

            var fieldArr = fields.Where(x => enumType.ToString().Equals(x.Name)).ToArray();

            var isFind = false; //是否已找到说明
            for (int i = 0; i < fieldArr.Length; i++)
            {
                if (isFind == true)
                {
                    break;
                }
                FieldInfo field = fieldArr[i];

                //反射自定义属性   
                object[] attributes = field.GetCustomAttributes(typeof(DescriptionAttribute), false);

                foreach (Attribute item in attributes)
                {
                    //类型转换找到一个Description，用Description作为成员名称   
                    var currentAttribute = item as DescriptionAttribute;

                    if (currentAttribute != null)
                    {
                        description = currentAttribute.Description ?? string.Empty;
                        isFind = true;
                        break;
                    }
                }
            }

            return description;
        }

        /// <summary>
        /// 根据枚举类型返回类型中的所有值，说明
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <returns>枚举类型字典集</returns>
        public static Dictionary<string, string> GetDictEnumDescription(Type currentType)
        {
            var dict = new Dictionary<string, string>();

            //获取字段信息   
            FieldInfo[] fields = currentType.GetFields();
            if (fields == null || fields.Length < 1)
            {
                return dict;
            }

            for (int i = 0; i < fields.Length; i++)
            {
                FieldInfo field = fields[i];

                string currentKey = ((int)SystemEnum.Parse(currentType, field.Name)).ToString();
                string currentValue = field.Name;

                //反射自定义属性   
                object[] attributes = field.GetCustomAttributes(typeof(DescriptionAttribute), false);

                foreach (Attribute item in attributes)
                {
                    //类型转换找到一个Description，用Description作为成员名称   
                    var currentAttribute = item as DescriptionAttribute;

                    if (currentAttribute != null)
                    {
                        currentValue = currentAttribute.Description;
                    }
                }

                dict[currentKey] = currentValue;
            }

            return dict;
        }
    }
}
