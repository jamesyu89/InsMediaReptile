using System;
using System.Collections.Generic;

namespace InstagramPhotos.Utility.Utility
{
    public class DateUtility
    {
        /// <summary>
        /// 时间按周月季度返回
        /// </summary>
        /// <param name="dt">时间</param>
        /// <param name="type">0周1月2季度</param>
        /// <returns></returns>
        public static Tuple<DateTime, DateTime> GetDate(DateTime dt, int type)
        {
            DateTime sdate;
            DateTime edate;
            switch (type)
            {

                case 0:
                    sdate = dt.AddDays(1 - Convert.ToInt32(dt.DayOfWeek.ToString("d"))).Date; //本周周一
                    edate = sdate.AddDays(7); //本周周日
                    return new Tuple<DateTime, DateTime>(sdate, edate);
                case 1:
                    sdate = dt.AddDays(1 - dt.Day).Date; //本月月初
                    edate = sdate.AddMonths(1).Date; //本月月末
                    return new Tuple<DateTime, DateTime>(sdate, edate);
                case 2:
                    sdate = dt.AddMonths(0 - (dt.Month - 1)%3).AddDays(1 - dt.Day).Date; //本季度初
                    edate = sdate.AddMonths(3).Date; //本季度末
                    return new Tuple<DateTime, DateTime>(sdate, edate);
                default:
                    sdate = dt.AddDays(1 - Convert.ToInt32(dt.DayOfWeek.ToString("d"))); //本周周一
                    edate = sdate.AddDays(7); //本周周日
                    return new Tuple<DateTime, DateTime>(sdate, edate);
            }
        }

        public static DateTime unixTimestampZeroPoint = new DateTime(1970, 01, 01, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// 获取小时列表
        /// </summary>
        /// <param name="is_12hour"></param>
        /// <returns></returns>
        public static List<string> GetHourList(Boolean is_12hour)
        {
            var result = new List<string>();
            for (Int32 i = 0; i < ((is_12hour) ? 12 : 24); i++)
            {
                result.Add(i.ToString());
            }
            return result;
        }

        /// <summary>
        /// 获取日期名称列表
        /// </summary>
        /// <returns></returns>
        public static List<string> GetDateNameList()
        {
            return new List<string> {"星期一", "星期二", "星期三", "星期四", "星期五", "星期六", "星期日"};
        }

        /// <summary>
        /// 获取该月份的日期列表
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static List<string> GetMonthDayList(DateTime date)
        {
            
            var result = new List<string>();
            for (Int32 i = 0; i <= DateTime.DaysInMonth(date.Year, date.Month); i++)
            {
                result.Add(i.ToString());
            }
            return result;
        }
    }
}
