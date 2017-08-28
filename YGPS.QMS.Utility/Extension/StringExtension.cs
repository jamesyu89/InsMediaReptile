using System;
using System.Text.RegularExpressions;
using System.Web;

namespace InstagramPhotos.Utility.Extension
{
    public static class StringExtension
    {
        public static string NullSafe(this string target)
        {
            return (target ?? string.Empty).Trim();
        }

        public static Guid ToGuid(this string target)
        {
            if ((!string.IsNullOrEmpty(target)) && (target.Trim().Length == 22))
            {
                string encoded = string.Concat(target.Trim().Replace("-", "+").Replace("_", "/"), "==");

                byte[] base64 = Convert.FromBase64String(encoded);

                return new Guid(base64);
            }

            return Guid.Empty;
        }


        public static T ToEnum<T>(this string target, T defaultValue) where T : IComparable, IFormattable
        {
            T convertedValue = defaultValue;

            if (!string.IsNullOrEmpty(target))
            {
                try
                {
                    convertedValue = (T)Enum.Parse(typeof(T), target.Trim(), true);
                }
                catch (ArgumentException)
                {
                }
            }

            return convertedValue;
        }

        public static string CleanQuoteTag(this string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            var options = RegexOptions.IgnoreCase;

            for (int i = 0; i < 20; i++)
                text = Regex.Replace(text, @"\[quote(?:\s*)user=""((.|\n)*?)""\]((.|\n)*?)\[/quote(\s*)\]", "", options);
            for (int i = 0; i < 20; i++)
                text = Regex.Replace(text, @"\[quote\]([^>]+?|.+?)\[\/quote\]", "", options);
            return text;
        }

        public static int TryToInt(this string text)
        {
            int i;
            int.TryParse(text, out i);
            return i;
        }

        /// <summary> string to int 如果异常则返回默认值
        /// </summary>
        /// <param name="text"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int ToInt(this string text, int defaultValue = 0)
        {
            int i;
            return int.TryParse(text, out i) ? i : defaultValue;
        }

        public static DateTime TryToDatetime(this string text, DateTime defaultValue)
        {
            DateTime result;
            try
            {
                result = Convert.ToDateTime(text);
            }
            catch (Exception)
            {
                result = defaultValue;
            }
            return result;
        }

        /// <summary> string to decimal 如果异常则返回默认值 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static decimal ToDecimal(this string text, decimal defaultValue = (decimal)0)
        {
            decimal d;

            return decimal.TryParse(text, out d) ? d : defaultValue;
        }

        public static string HtmlEncode(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return string.Empty;
            }
            return HttpUtility.HtmlEncode(str);
        }

        public static string HtmlDecode(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return string.Empty;
            }
            return HttpUtility.HtmlDecode(str);
        }

        public static string UnDash(this object value)
        {
            return ((value as string) ?? string.Empty).UnDash();
        }
        /// <summary>
        /// 移除字符串中“-”符号
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string UnDash(this string value)
        {
            return (value ?? string.Empty).Replace("-", string.Empty);
        }
    }
}