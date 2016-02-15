using System;

namespace Mammothcode.Core.Data.DataConvert
{
    /// <summary>
    /// 时间操作的公共类
    /// 作者:孙佳杰
    /// 修改时间：2015.1.14
    /// 功能：ConvertDateTimeTimeStamp（时间戳）
    ///       ConvertIntTimeStamp（DateTime时间格式转换为Unix时间戳格式）
    /// </summary>
    public static class TimeUtil
    {

        #region 时间戳

        /// <summary>
        /// 时间戳转为C#格式时间
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        public static DateTime ConvertDateTimeTimeStamp(string timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime); return dtStart.Add(toNow);
        }

        /// <summary>
        /// DateTime时间格式转换为Unix时间戳格式
        /// </summary>
        /// <param name=”time”></param>
        /// <returns></returns>
        public static long ConvertIntTimeStamp(DateTime time)
        {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            return (int)(time - startTime).TotalSeconds;
        }

        #endregion

        /// <summary>
        /// 格式化显示时间为几个月,几天前,几小时前,几分钟前,或几秒前
        /// 创建人:孙佳杰  创建时间:2015.3.18
        /// </summary>
        /// <param name="dt">要格式化显示的时间</param>
        /// <returns>几个月,几天前,几小时前,几分钟前,或几秒前</returns>
        public static string GetDateStringFromNow(DateTime dt)
        {
            TimeSpan span = DateTime.Now - dt;
            if (span.TotalDays > 60)
            {
                return dt.ToShortDateString();
            }
            else if (span.TotalDays > 30)
            {
                return "1个月前";
            }
            else if (span.TotalDays > 14)
            {
                return "2周前";
            }
            else if (span.TotalDays > 7)
            {
                return "1周前";
            }
            else if (span.TotalDays > 1)
            {
                return string.Format("{0}天前", (int)Math.Floor(span.TotalDays));
            }
            else if (span.TotalHours > 1)
            {
                return string.Format("{0}小时前", (int)Math.Floor(span.TotalHours));
            }
            else if (span.TotalMinutes > 1)
            {
                return string.Format("{0}分钟前", (int)Math.Floor(span.TotalMinutes));
            }
            else if (span.TotalSeconds >= 1)
            {
                return string.Format("{0}秒前", (int)Math.Floor(span.TotalSeconds));
            }
            else
            {
                return "刚刚";
            }
        }

        /// <summary>
        /// 比较两个时间获取相隔的天数
        /// 创建人：孙佳杰  创建时间：2015.4.28
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static int GetDateDaysFromNow(DateTime dt)
        {
            TimeSpan span = DateTime.Now - dt;

            return span.TotalDays.ToInt();
        }
    }
}
