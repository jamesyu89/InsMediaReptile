using System;
using InstagramPhotos.Utility.Utility;

namespace InstagramPhotos.Utility.Extension
{
    public static class DateTimeExtension
    {
        public static readonly DateTime MinDate = new DateTime(1900, 1, 1);
        public static readonly DateTime MaxDate = new DateTime(2099, 12, 31, 23, 59, 59, 999);

        public static bool IsValid(this DateTime target)
        {
            return (target >= MinDate) && (target <= MaxDate);
        }

        public static int ToTicket(this DateTime target)
        {
            var baseTime = new DateTime(1970, 1, 1);
            TimeSpan ts = target.ToUniversalTime() - baseTime.ToUniversalTime();
            return Convert.ToInt32(ts.TotalSeconds);
        }

        /// <summary>
        ///     �жϡ�DateTime?���Ƿ������Чֵ;
        ///     ����ֵ true:��null����DateTime.Year>2000
        /// </summary>
        /// <param name="dateTimeParams">���ڶ���</param>
        /// <returns>����ֵ true:��null����DateTime.Year>2000</returns>
        public static bool HasValidValues(this DateTime? dateTimeParams)
        {
            if (dateTimeParams != null && dateTimeParams.Value.Year > 2000)
                return true;
            return false;
        }

        #region RebuildDateScope

        /// <summary>
        ///     �ؽ�����ʱ�䣬��ʼʱ�佫��Ϊ�������00:00:00������ʱ�佫��Ϊ�������23:59:59.999
        /// </summary>
        /// <param name="beginDate">The begin date.</param>
        /// <param name="endDate">The end date.</param>
        public static void RebuildDateScope(ref DateTime beginDate, ref DateTime endDate)
        {
            beginDate = beginDate.Date;
            endDate = endDate.Date.AddDays(1).AddMilliseconds(-1);
        }

        /// <summary>
        ///     �ؽ�����ʱ�䣬��ʼʱ�佫��Ϊ�������00:00:00������ʱ�佫��Ϊ�������23:59:59.999
        /// </summary>
        /// <param name="beginDate">The begin date.</param>
        /// <param name="endDate">The end date.</param>
        public static void RebuildDateScope(ref DateTime? beginDate, ref DateTime? endDate)
        {
            if (Checker.AllAreNotEmpty(beginDate, endDate))
            {
                DateTime tempBeginDate = beginDate.Value;
                DateTime tempEndDate = endDate.Value;

                RebuildDateScope(ref tempBeginDate, ref tempEndDate);

                beginDate = tempBeginDate;
                endDate = tempEndDate;
            }
        }

        #endregion
    }
}