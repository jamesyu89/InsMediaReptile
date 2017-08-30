using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;

namespace InstagramPhotos.Utility.Utility
{
    /// <summary>
    /// General function library.
    /// </summary>
    public static class Checker
    {
        #region IsEmpty系列函数

        #region DataSet

        /// <summary>
        /// 断定DataSet为null或无数据。
        /// </summary>
        /// <param name="dataSet">要检查的DataSet</param>
        /// <returns>如果为null或无数据返回<c>true</c>; 否则返回 <c>false</c>.</returns>
        public static bool IsEmpty(DataSet dataSet)
        {
            return !(IsNotEmpty(dataSet));
        }

        #endregion

        #region DataTable

        /// <summary>
        /// 断定DataTable为null或无数据。
        /// </summary>
        /// <param name="dataTable">DataTable.</param>
        /// <returns>如果为null或无数据返回<c>true</c>; 否则返回 <c>false</c>.</returns>
        public static bool IsEmpty(DataTable dataTable)
        {
            return !(IsNotEmpty(dataTable));
        }

        #endregion

        #region String

        /// <summary>
        /// 断定输入字符串为null或String.Empty。
        /// </summary>
        /// <param name="str">要检查的字串</param>
        /// <param name="removeSpace">是否允许去除字符串中首尾的空格。</param>
        /// <returns>如果为null或String.Empty返回<c>true</c>; 否则返回 <c>false</c>.</returns>
        public static bool IsEmpty(string str, bool removeSpace)
        {
            if (str == null)
            {
                return true;
            }

            if (removeSpace)
            {
                return (str.Trim().Length == 0);
            }

            return (str.Length == 0);
        }

        /// <summary>
        /// 断定输入字符串是否为null或String.Empty，去除首尾的空格。
        /// </summary>
        /// <param name="str">要检查的字串</param>
        /// <returns>如果为null或String.Empty返回<c>true</c>; 否则返回 <c>false</c>.</returns>
        public static bool IsEmpty(string str)
        {
            return IsEmpty(str, true);
        }

        #endregion

        #region ICollection

        /// <summary>
        /// Determines whether the specified value is empty.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// 	<c>true</c> if the specified value is empty; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsEmpty(ICollection value)
        {
            return (value == null || value.Count == 0);
        }

        /// <summary>
        /// Determines whether the specified value is empty.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// 	<c>true</c> if the specified value is empty; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsEmpty<T>(ICollection<T> value)
        {
            return (value == null || value.Count == 0);
        }

        #endregion

        #region Object

        /// <summary>
        /// Determines whether the specified value is empty.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// 	<c>true</c> if the specified value is empty; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsEmpty(object value)
        {
            if (value == null)
            {
                return true;
            }

            if (value is string)
            {
                return IsEmpty((string)value);
            }

            if (value is ICollection)
            {
                return IsEmpty((ICollection)value);
            }

            if (value is DataSet)
            {
                return IsEmpty((DataSet)value);
            }

            if (value is DataTable)
            {
                return IsEmpty((DataTable)value);
            }

            return false;
        }

        #endregion

        #endregion

        #region IsNotEmpty系列函数

        #region DataSet

        /// <summary>
        /// 断定DataSet有数据。
        /// </summary>
        /// <param name="dataSet">要检查的DataSet</param>
        /// <returns>如果为null或无数据返回<c>false</c>; 否则返回 <c>true</c>.</returns>
        public static bool IsNotEmpty(DataSet dataSet)
        {
            if (dataSet == null)
            {
                return false;
            }
            for (int i = 0; i < dataSet.Tables.Count; i++)
            {
                if (IsNotEmpty(dataSet.Tables[i]))
                {
                    return true;
                }
            }
            return false;
        }

        #endregion

        #region DataTable

        /// <summary>
        /// 断定DataTable有数据。
        /// </summary>
        /// <param name="dataTable">DataTable.</param>
        /// <returns>如果为null或无数据返回<c>false</c>; 否则返回 <c>true</c>.</returns>
        public static bool IsNotEmpty(DataTable dataTable)
        {
            if (dataTable == null)
            {
                return false;
            }

            if (dataTable.Rows.Count > 0)
            {
                return true;
            }

            return false;
        }

        #endregion

        #region String

        /// <summary>
        /// 断定输入字符串不是null或String.Empty。
        /// </summary>
        /// <param name="str">要检查的字串</param>
        /// <param name="removeSpace">是否允许去除字符串中首尾的space。</param>
        /// <returns>如果为null或String.Empty返回<c>false</c>; 否则返回 <c>true</c>.</returns>
        public static bool IsNotEmpty(string str, bool removeSpace)
        {
            return (!IsEmpty(str, removeSpace));
        }

        /// <summary>
        /// 断定输入字符串不是null或String.Empty，去除首尾的空格。
        /// </summary>
        /// <param name="str">要检查的字串</param>
        /// <returns>如果为null或String.Empty返回<c>false</c>; 否则返回 <c>true</c>.</returns>
        public static bool IsNotEmpty(string str)
        {
            return IsNotEmpty(str, true);
        }

        #endregion

        #region ICollection

        /// <summary>
        /// Determines whether [is not empty] [the specified value].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// 	<c>true</c> if [is not empty] [the specified value]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNotEmpty(ICollection value)
        {
            return !IsEmpty(value);
        }

        /// <summary>
        /// Determines whether [is not empty] [the specified value].
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <returns>
        /// 	<c>true</c> if [is not empty] [the specified value]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNotEmpty<T>(ICollection<T> value)
        {
            return !IsEmpty(value);
        }

        #endregion

        #region Object

        /// <summary>
        /// Determines whether [is not empty] [the specified value].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// 	<c>true</c> if [is not empty] [the specified value]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNotEmpty(object value)
        {
            return !IsEmpty(value);
        }

        #endregion

        #endregion

        #region HasEmpty

        /// <summary>
        /// Determines whether the specified args has empty.
        /// </summary>
        /// <param name="args">The args.</param>
        /// <returns>
        /// 	<c>true</c> if the specified args has empty; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasEmpty(params object[] args)
        {
            foreach (object arg in args)
            {
                if (IsEmpty(arg))
                {
                    return true;
                }
            }
            return false;
        }

        #endregion

        #region AllAreEmpty

        /// <summary>
        /// Alls the are empty.
        /// </summary>
        /// <param name="args">The args.</param>
        /// <returns></returns>
        public static bool AllAreEmpty(params object[] args)
        {
            foreach (object arg in args)
            {
                if (IsNotEmpty(arg))
                {
                    return false;
                }
            }
            return true;
        }

        #endregion

        #region AllAreNotEmpty

        /// <summary>
        /// Alls the are not empty.
        /// </summary>
        /// <param name="args">The args.</param>
        /// <returns></returns>
        public static bool AllAreNotEmpty(params object[] args)
        {
            foreach (object arg in args)
            {
                if (IsEmpty(arg))
                {
                    return false;
                }
            }
            return true;
        }

        #endregion

        #region 数字检查系列

        #region IsUInt

        /// <summary>
        /// 是否十进制无符号整数字符串。
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <returns></returns>
        public static bool IsUInt(string input)
        {
            Regex regUInt = new Regex("^[0-9]+$");

            if (!IsNotEmpty(input))
            {
                return false;
            }

            Match match = regUInt.Match(input);
            return match.Success;
        }

        #endregion

        #region IsInt

        /// <summary>
        /// 是否十进制整数字符串。
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <returns></returns>
        public static bool IsInt(string input)
        {
            Regex regInt = new Regex("^[+-]?[0-9]+$");

            if (!IsNotEmpty(input))
            {
                return false;
            }

            Match match = regInt.Match(input);
            return match.Success;
        }

        #endregion

        #region IsUDecimal

        /// <summary>
        /// 是否是十进制无符号数字，包括小数。
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <returns></returns>
        public static bool IsUDecimal(string input)
        {
            Regex regUDecimal = new Regex("^[0-9]+[.]?[0-9]*$");

            if (!IsNotEmpty(input))
            {
                return false;
            }

            Match match = regUDecimal.Match(input);
            return match.Success;
        }

        #endregion

        #region IsDecimal

        /// <summary>
        /// 是否是十进制数字，包括小数。
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <returns></returns>
        public static bool IsDecimal(string input)
        {
            Regex regDecimal = new Regex("^[+-]?[0-9]+[.]?[0-9]*$"); //等价于^[+-]?\d+[.]?\d+$

            if (!IsNotEmpty(input))
            {
                return false;
            }

            Match match = regDecimal.Match(input);
            return match.Success;
        }

        #endregion

        #endregion

        #region 中文检测

        /// <summary>
        /// 检测是否有中文字
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <returns></returns>
        public static bool HasChinese(string input)
        {
            Regex regChinese = new Regex("[\u4e00-\u9fa5]");

            if (!IsNotEmpty(input))
            {
                return false;
            }

            Match match = regChinese.Match(input);
            return match.Success;
        }

        #endregion

        #region 邮件地址

        /// <summary>
        /// 是否是浮点数 可带正负号
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <returns></returns>
        public static bool IsEmail(string input)
        {
            Regex regEmail = new Regex("^[\\w-]+@[\\w-]+\\.(com|net|org|edu|mil|tv|biz|info)$");

            if (!IsNotEmpty(input))
            {
                return false;
            }

            Match match = regEmail.Match(input);
            return match.Success;
        }

        #endregion

    }
}