using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Re.Common
{
    public class TimeUtil
    {

        public static string FormatCommonTime(string time)
        {
            return ToLocalTimeString(DateTime.ParseExact(time, "yyyyMMddHHmmss", null));
        }

        public static string ToLocalTimeString(DateTime time){

            return time.ToString("yyyy-MM-dd HH:mm:ss");
        }
        public static DateTime ToTimeFromWebParam(string time)
        {
            return DateTime.ParseExact(time, "yyyyMMddHHmmss", null);
        }

        /**
		 *判断开始时间是否大于结束时间
		 **/
        public static  Boolean ComparisonDate(string strStartTime, string strEndTime)
        {
            DateTime startDate = Convert.ToDateTime(strStartTime);
            DateTime endDate = Convert.ToDateTime(strEndTime);
            if (DateTime.Compare(startDate, endDate) < 0)
            {
                return true;
            }
            return false;
        }
        public static  Boolean ComparisonIsQuere(string strStartTime, string strEndTime)
        {
            DateTime startDate = Convert.ToDateTime(strStartTime);
            DateTime endDate = Convert.ToDateTime(strEndTime);
            if (DateTime.Compare(startDate, endDate) == 0)
            {
                return true;
            }
            return false;
        }
        /**
         * 获得从开始时间到结束时间内的所有时间（以一个小时为间隔）
         **/
        public static  List<string> getAllTimeBetweenTimeAndTime(DateTime tmpStartDate, DateTime tmpEndDate, int minGap)
        {
            List<string> timesList = new List<string>(); //日期集合
            DateTime dStatand = new DateTime(1970, 1, 1);
            double numBegin = tmpStartDate.Subtract(dStatand).TotalMilliseconds;
            double numEnd = tmpEndDate.Subtract(dStatand).TotalMilliseconds;
            while (numBegin <= numEnd)
            {
                string day = IntToTime(numBegin.ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                numBegin += minGap * 60 * 1000;
                timesList.Add(day);
            }
            return timesList;
        }
        /// <summary>
        /// 整数转成1970年至今的时间，返回结果为DateTime格式的时间
        /// </summary>
        /// <param name="Int">10位的字符串</param>
        /// <returns>返回结果为DateTime格式的时间</returns>
        public static  DateTime IntToTime(string Int)
        {
            DateTime d1 = Convert.ToDateTime("1970-01-01 00:00:00");
            if (Int.Length < 10)
            {
                return d1;
            }
            else
            {
                DateTime dt;
                try
                {
                    double dTimes = Convert.ToDouble(Int);
                    dt = d1.AddSeconds(dTimes / 1000);
                }
                catch (Exception e)
                {
                    throw e;
                }
                return dt;
            }
        }
       
    }
}