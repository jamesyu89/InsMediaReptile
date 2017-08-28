using System;
using System.Text;

namespace InstagramPhotos.Utility.Helper
{
    /// <summary>
    /// 字符串替代类 可直接+而不用考虑性能问题
    /// </summary>
    public class StringBuffer
    {
        #region filds and properties

        public StringBuilder builder = new StringBuilder();

        private int defaultCapacity = 500;//StringBuilder的默认Capacity

        public int Length
        {
            get
            {
                return builder.Length;
            }
        }

        #endregion

        #region constructors

        public StringBuffer()
        {
            builder = new StringBuilder(defaultCapacity);
        }
        public StringBuffer(int capacity)
        {
            builder = new StringBuilder(capacity);
        }
        public StringBuffer(string value)
        {
            builder = new StringBuilder(value);
        }
        public StringBuffer(int capacity, int maxCapacity)
        {
            builder = new StringBuilder(capacity, maxCapacity);
        }
        public StringBuffer(string value, int capacity)
        {
            builder = new StringBuilder(value, capacity);
        }
        public StringBuffer(string value, int startIndex, int length, int capacity)
        {
            builder = new StringBuilder(value, startIndex, length, capacity);
        }
        public StringBuffer(StringBuilder innerBuilder)
        {
            builder = innerBuilder;
        }

        #endregion

        #region  methods

        public static StringBuffer Format(string format, object arg0)
        {
            StringBuffer sb = new StringBuffer();
            sb.builder.AppendFormat(format, arg0);
            return sb;
        }

        public static StringBuffer Format(string format, params object[] args)
        {
            StringBuffer sb = new StringBuffer();
            sb.builder.AppendFormat(format, args);
            return sb;
        }

        public static StringBuffer Format(IFormatProvider provider, string format, params object[] args)
        {
            StringBuffer sb = new StringBuffer();
            sb.builder.AppendFormat(provider, format, args);
            return sb;
        }

        public static StringBuffer Format(string format, object arg0, object arg1)
        {
            StringBuffer sb = new StringBuffer();
            sb.builder.AppendFormat(format, arg0, arg1);
            return sb;
        }

        public static StringBuffer Format(string format, object arg0, object arg1, object arg2)
        {
            StringBuffer sb = new StringBuffer();
            sb.builder.AppendFormat(format, arg0, arg1, arg2);
            return sb;
        }

        public void Remove(int startIndex, int length)
        {
            builder.Remove(startIndex, length);
        }

        public override string ToString()
        {
            return builder.ToString();
        }

        ///// <summary>
        ///// 将此实例中所有的指定字符替换为其他指定字符
        ///// </summary>
        ///// <param name="oldChar">要替换的字符</param>
        ///// <param name="newChar">替换 oldChar 的字符</param>
        ///// <returns></returns>
        //public void Replace(char oldChar, char newChar)
        //{
        //    this.builder.Replace(oldChar, newChar);
        //}

        ///// <summary>
        ///// 将此实例的子字符串中所有指定字符的匹配项替换为其他指定字符
        ///// </summary>
        ///// <param name="oldChar">要替换的字符</param>
        ///// <param name="newChar"> 替换 oldChar 的字符</param>
        ///// <param name="startIndex">此实例中子字符串开始的位置</param>
        ///// <param name="count">子字符串的长度</param>
        ///// <returns></returns>
        //public void Replace(char oldChar, char newChar, int startIndex, int count)
        //{
        //    this.builder.Replace(oldChar, newChar);
        //}

        ///// <summary>
        ///// 将此实例中所有指定字符串的匹配项替换为其他指定字符串
        ///// </summary>
        ///// <param name="oldStr">要替换的字符串</param>
        ///// <param name="newStr">替换 oldValue 的字符串或 null。</param>
        ///// <returns></returns>
        //public void Replace(string oldStr, string newStr)
        //{
        //    this.builder.Replace(oldStr, newStr);
        //}

        ///// <summary>
        ///// 将此实例的子字符串中所有指定字符串的匹配项替换为其他指定字符串。
        ///// </summary>
        ///// <param name="oldStr">要替换的字符串</param>
        ///// <param name="newStr"> 替换 oldValue 的字符串或 null。</param>
        ///// <param name="startIndex"> 此实例中子字符串开始的位置。</param>
        ///// <param name="count">  子字符串的长度。</param>
        ///// <returns> </returns>
        //public void Replace(string oldStr, string newStr, int startIndex, int count)
        //{
        //    this.builder.Replace(oldStr, newStr, startIndex, count);
        //}

        /// <summary>
        /// 将此实例中所有的指定字符替换为其他指定字符
        /// </summary>
        /// <param name="oldChar">要替换的字符</param>
        /// <param name="newChar">替换 oldChar 的字符</param>
        /// <returns></returns>
        public StringBuffer Replace(char oldChar, char newChar)
        {
            this.builder.Replace(oldChar, newChar);
            return this;
        }

        /// <summary>
        /// 将此实例的子字符串中所有指定字符的匹配项替换为其他指定字符
        /// </summary>
        /// <param name="oldChar">要替换的字符</param>
        /// <param name="newChar"> 替换 oldChar 的字符</param>
        /// <param name="startIndex">此实例中子字符串开始的位置</param>
        /// <param name="count">子字符串的长度</param>
        /// <returns></returns>
        public StringBuffer Replace(char oldChar, char newChar, int startIndex, int count)
        {
            this.builder.Replace(oldChar, newChar);
            return this;
        }

        /// <summary>
        /// 将此实例中所有指定字符串的匹配项替换为其他指定字符串
        /// </summary>
        /// <param name="oldStr">要替换的字符串</param>
        /// <param name="newStr">替换 oldValue 的字符串或 null。</param>
        /// <returns></returns>
        public StringBuffer Replace(string oldStr, string newStr)
        {
            this.builder.Replace(oldStr, newStr);
            return this;
        }

        /// <summary>
        /// 将此实例的子字符串中所有指定字符串的匹配项替换为其他指定字符串。
        /// </summary>
        /// <param name="oldStr">要替换的字符串</param>
        /// <param name="newStr"> 替换 oldValue 的字符串或 null。</param>
        /// <param name="startIndex"> 此实例中子字符串开始的位置。</param>
        /// <param name="count">  子字符串的长度。</param>
        /// <returns> </returns>
        public StringBuffer Replace(string oldStr, string newStr, int startIndex, int count)
        {
            this.builder.Replace(oldStr, newStr, startIndex, count);
            return this;
        }

        /// <summary>
        /// 清空已填充的字符串
        /// </summary>
        public void Clear()
        {
            this.builder.Length = 0;
        }

        public static implicit operator StringBuffer(string input)
        {
            StringBuffer sb = new StringBuffer(input);
            return sb;
        }
        public static implicit operator String(StringBuffer sb)
        {
            return sb.ToString();
        }

        public static StringBuffer operator +(StringBuffer sb, string value)
        {
            sb.builder.Append(value);

            return sb;
        }
        public static StringBuffer operator +(StringBuffer sb, object value)
        {
            sb.builder.Append(value);

            return sb;
        }

        public static StringBuffer operator +(StringBuffer sb, bool value)
        {
            sb.builder.Append(value);

            return sb;
        }
        public static StringBuffer operator +(StringBuffer sb, byte value)
        {
            sb.builder.Append(value);

            return sb;
        }
        public static StringBuffer operator +(StringBuffer sb, char value)
        {
            sb.builder.Append(value);

            return sb;
        }
        public static StringBuffer operator +(StringBuffer sb, char[] value)
        {
            sb.builder.Append(value);

            return sb;
        }
        public static StringBuffer operator +(StringBuffer sb, decimal value)
        {
            sb.builder.Append(value);

            return sb;
        }
        public static StringBuffer operator +(StringBuffer sb, double value)
        {
            sb.builder.Append(value);

            return sb;
        }
        public static StringBuffer operator +(StringBuffer sb, float value)
        {
            sb.builder.Append(value);

            return sb;
        }
        public static StringBuffer operator +(StringBuffer sb, int value)
        {
            sb.builder.Append(value);

            return sb;
        }
        public static StringBuffer operator +(StringBuffer sb, long value)
        {
            sb.builder.Append(value);

            return sb;
        }
        public static StringBuffer operator +(StringBuffer sb, sbyte value)
        {
            sb.builder.Append(value);

            return sb;
        }
        public static StringBuffer operator +(StringBuffer sb, short value)
        {
            sb.builder.Append(value);

            return sb;
        }
        public static StringBuffer operator +(StringBuffer sb, uint value)
        {
            sb.builder.Append(value);

            return sb;
        }
        public static StringBuffer operator +(StringBuffer sb, ulong value)
        {
            sb.builder.Append(value);

            return sb;
        }
        public static StringBuffer operator +(StringBuffer sb, ushort value)
        {
            sb.builder.Append(value);

            return sb;
        }

        #endregion

        #region Append method

        public StringBuffer Append(string input)
        {
            this.builder.Append(input);
            return this;
        }
        public StringBuffer Append(object input)
        {
            this.builder.Append(input);
            return this;
        }

        public StringBuffer Append(bool input)
        {
            this.builder.Append(input);
            return this;
        }
        public StringBuffer Append(byte input)
        {
            this.builder.Append(input);
            return this;
        }
        public StringBuffer Append(char input)
        {
            this.builder.Append(input);
            return this;
        }
        public StringBuffer Append(char[] input)
        {
            this.builder.Append(input);
            return this;
        }
        public StringBuffer Append(decimal input)
        {
            this.builder.Append(input);
            return this;
        }
        public StringBuffer Append(double input)
        {
            this.builder.Append(input);
            return this;
        }
        public StringBuffer Append(float input)
        {
            this.builder.Append(input);
            return this;
        }
        public StringBuffer Append(int input)
        {
            this.builder.Append(input);
            return this;
        }
        public StringBuffer Append(long input)
        {
            this.builder.Append(input);
            return this;
        }
        public StringBuffer Append(sbyte input)
        {
            this.builder.Append(input);
            return this;
        }
        public StringBuffer Append(short input)
        {
            this.builder.Append(input);
            return this;
        }
        public StringBuffer Append(ushort input)
        {
            this.builder.Append(input);
            return this;
        }
        public StringBuffer Append(uint input)
        {
            this.builder.Append(input);
            return this;
        }
        public StringBuffer Append(ulong input)
        {
            this.builder.Append(input);
            return this;
        }
        /// <summary>
        /// 在此实例的结尾追加指定子字符串的副本
        /// </summary>
        /// <param name="this"></param>
        /// <param name="input">input 中子字符串的开始位置</param>
        /// <param name="startIndex"></param>
        /// <param name="count">input 中要追加的字符数</param>
        /// <returns></returns>
        public StringBuffer Append(string input, int startIndex, int count)
        {
            this.builder.Append(input, startIndex, count);
            return this;
        }
        /// <summary>
        /// 在此实例的结尾追加 Unicode 字符的字符串表示形式指定数目的副本
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="input">要追加的字符</param>
        /// <param name="repeatCount">追加 value 的次数</param>
        /// <returns></returns>
        public StringBuffer Append(char input, int repeatCount)
        {
            this.builder.Append(input, repeatCount);
            return this;
        }
        /// <summary>
        ///  在此实例的结尾追加指定的 Unicode 字符子数组的字符串表示形式
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="input">字符数组</param>
        /// <param name="startIndex">input 中的起始位置</param>
        /// <param name="charCount">要追加的字符数</param>
        /// <returns></returns>
        public StringBuffer Append(char[] input, int startIndex, int charCount)
        {
            this.builder.Append(input, startIndex, charCount);
            return this;
        }

        #endregion

        #region AppendFormat method

        public StringBuffer AppendFormat(string format, object arg0)
        {
            this.builder.AppendFormat(format, arg0);
            return this;
        }
        public StringBuffer AppendFormat(string format, params object[] args)
        {
            this.builder.AppendFormat(format, args);
            return this;
        }
        public StringBuffer AppendFormat(IFormatProvider provider, string format, params object[] args)
        {
            this.builder.AppendFormat(provider, format, args);
            return this;
        }
        public StringBuffer AppendFormat(string format, object arg0, object arg1)
        {
            this.builder.AppendFormat(format, arg0, arg1);
            return this;
        }
        public StringBuffer AppendFormat(string format, object arg0, object arg1, object arg2)
        {
            this.builder.AppendFormat(format, arg0, arg1, arg2);
            return this;
        }

        #endregion

    }

    /// <summary>
    /// StringBuffer的扩展
    /// </summary>
    public static class StringBufferExtension
    {
        #region Append method

        public static StringBuffer Append(this StringBuffer sb, string input)
        {
            sb.builder.Append(input);
            //sb += input;
            return sb;
        }
        public static StringBuffer Append(this StringBuffer sb, object input)
        {
            sb.builder.Append(input);
            //sb += input;
            return sb;
        }

        public static StringBuffer Append(this StringBuffer sb, bool input)
        {
            sb.builder.Append(input);
            return sb;
        }
        public static StringBuffer Append(this StringBuffer sb, byte input)
        {
            sb.builder.Append(input);
            return sb;
        }
        public static StringBuffer Append(this StringBuffer sb, char input)
        {
            sb.builder.Append(input);
            return sb;
        }
        public static StringBuffer Append(this StringBuffer sb, char[] input)
        {
            sb.builder.Append(input);
            return sb;
        }
        public static StringBuffer Append(this StringBuffer sb, decimal input)
        {
            sb.builder.Append(input);
            return sb;
        }
        public static StringBuffer Append(this StringBuffer sb, double input)
        {
            sb.builder.Append(input);
            return sb;
        }
        public static StringBuffer Append(this StringBuffer sb, float input)
        {
            sb.builder.Append(input);
            return sb;
        }
        public static StringBuffer Append(this StringBuffer sb, int input)
        {
            sb.builder.Append(input);
            return sb;
        }
        public static StringBuffer Append(this StringBuffer sb, long input)
        {
            sb.builder.Append(input);
            return sb;
        }
        public static StringBuffer Append(this StringBuffer sb, sbyte input)
        {
            sb.builder.Append(input);
            return sb;
        }
        public static StringBuffer Append(this StringBuffer sb, short input)
        {
            sb.builder.Append(input);
            return sb;
        }
        public static StringBuffer Append(this StringBuffer sb, ushort input)
        {
            sb.builder.Append(input);
            return sb;
        }
        public static StringBuffer Append(this StringBuffer sb, uint input)
        {
            sb.builder.Append(input);
            return sb;
        }
        public static StringBuffer Append(this StringBuffer sb, ulong input)
        {
            sb.builder.Append(input);
            return sb;
        }
        /// <summary>
        /// 在此实例的结尾追加指定子字符串的副本
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="input">input 中子字符串的开始位置</param>
        /// <param name="startIndex"></param>
        /// <param name="count">input 中要追加的字符数</param>
        /// <returns></returns>
        public static StringBuffer Append(this StringBuffer sb, string input, int startIndex, int count)
        {
            sb.builder.Append(input, startIndex, count);
            return sb;
        }
        /// <summary>
        /// 在此实例的结尾追加 Unicode 字符的字符串表示形式指定数目的副本
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="input">要追加的字符</param>
        /// <param name="repeatCount">追加 value 的次数</param>
        /// <returns></returns>
        public static StringBuffer Append(this StringBuffer sb, char input, int repeatCount)
        {
            sb.builder.Append(input, repeatCount);
            return sb;
        }
        /// <summary>
        ///  在此实例的结尾追加指定的 Unicode 字符子数组的字符串表示形式
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="input">字符数组</param>
        /// <param name="startIndex">input 中的起始位置</param>
        /// <param name="charCount">要追加的字符数</param>
        /// <returns></returns>
        public static StringBuffer Append(this StringBuffer sb, char[] input, int startIndex, int charCount)
        {
            sb.builder.Append(input, startIndex, charCount);
            return sb;
        }

        #endregion

        #region AppendFormat method

        public static StringBuffer AppendFormat(this StringBuffer sb, string format, object arg0)
        {
            sb.builder.AppendFormat(format, arg0);
            return sb;
        }
        public static StringBuffer AppendFormat(this StringBuffer sb, string format, params object[] args)
        {
            sb.builder.AppendFormat(format, args);
            return sb;
        }
        public static StringBuffer AppendFormat(this StringBuffer sb, IFormatProvider provider, string format, params object[] args)
        {
            sb.builder.AppendFormat(provider, format, args);
            return sb;
        }
        public static StringBuffer AppendFormat(this StringBuffer sb, string format, object arg0, object arg1)
        {
            sb.builder.AppendFormat(format, arg0, arg1);
            return sb;
        }
        public static StringBuffer AppendFormat(this StringBuffer sb, string format, object arg0, object arg1, object arg2)
        {
            sb.builder.AppendFormat(format, arg0, arg1, arg2);
            return sb;
        }

        #endregion
    }
}