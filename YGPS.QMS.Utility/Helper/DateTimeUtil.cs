using System;
using System.Collections.Generic;

namespace InstagramPhotos.Utility.Helper
{
    /// <summary>
    /// 时间相关帮助类
    /// </summary>
    public class DateTimeUtil
    {
        #region 方法

        /// <summary>
        /// 获取一年的月份列表 不足两位的以0为前缀补齐
        /// </summary>
        /// <returns></returns>
        public static IList<string> GetMonthList()
        {
            var list = new List<string>();
            var counter = 12;
            for (int i = 1; i <= counter; i++)
            {
                var strNum = i < 10 ? string.Format("0{0}", i) : i.ToString();
                list.Add(strNum);
            }
            return list;
        }

        /// <summary>
        /// 获取距今10年的年份列表
        /// </summary>
        /// <returns></returns>
        public static IList<string> GetYearList()
        {
            var startYear = DateTime.Now.Year;
            var list = new List<string>();
            var yearCount = 10;
            for (int i = 0; i <= yearCount; i++)
            {
                list.Add(startYear.ToString());
                startYear++;
            }

            return list;
        }

        /// <summary>
        /// 时间戳转为C#格式时间
        /// </summary>
        /// <param name="unixTimeStamp">Unix时间戳格式整数</param>
        /// <returns>C#格式时间</returns>
        public static DateTime GetTimeByUnixStamp(Int32 unixTimeStamp)
        {
            return GetTimeByUnixStamp(unixTimeStamp.ToString());
        }

        /// <summary>
        /// 时间戳转为C#格式时间
        /// </summary>
        /// <param name="unixTimeStamp">Unix时间戳格式字符串</param>
        /// <returns>C#格式时间</returns>
        public static DateTime GetTimeByUnixStamp(string unixTimeStamp)
        {
            var dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            var timeStamp = long.Parse(unixTimeStamp + "0000000");
            var toNow = new TimeSpan(timeStamp);
            return dtStart.Add(toNow);
        }

        /// <summary>
        /// DateTime时间格式转换为Unix时间戳格式
        /// </summary>
        /// <param name="time"> DateTime时间格式</param>
        /// <returns>Unix时间戳格式</returns>
        public static int GetUnixStamp(DateTime time)
        {
            var startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (int)(time - startTime).TotalSeconds;
        }

        #endregion

        #region 辅助方法


        #endregion
    }
}
