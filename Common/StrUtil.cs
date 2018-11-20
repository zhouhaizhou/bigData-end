using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
namespace DitingWFCService.Re.Common
{
    public class StrUtil
    {
        /// <summary>
        /// 判断字符串是否是int/double
        /// </summary>
        public static bool IsIntOrDouble(string strNumber)
        {
            Regex objNotNumberPattern = new Regex("[^0-9.-]");
            Regex objTwoDotPattern = new Regex("[0-9]*[.][0-9]*[.][0-9]*");
            Regex objTwoMinusPattern = new Regex("[0-9]*[-][0-9]*[-][0-9]*");
            const string strValidRealPattern = "^([-]|[.]|[-.]|[0-9])[0-9]*[.]*[0-9]+$";
            const string strValidIntegerPattern = "^([-]|[0-9])[0-9]*$";
            Regex objNumberPattern = new Regex("(" + strValidRealPattern + ")|(" + strValidIntegerPattern + ")");
            return !objNotNumberPattern.IsMatch(strNumber) &&
             !objTwoDotPattern.IsMatch(strNumber) &&
             !objTwoMinusPattern.IsMatch(strNumber) &&
             objNumberPattern.IsMatch(strNumber);
        }
        //数据库中in字符串的处理
        public static string SqlINStringDeal(string strContent)
        {
            string result = "";
            string[] contentArr = strContent.Split(',');
            int index = 0;
            foreach (string strOneContent in contentArr)
            {
                if (index == 0)
                {
                    result = "'" + strOneContent + "'";
                    index++;
                }
                else
                {
                    result = result + "," + "'" + strOneContent + "'";
                }
            }
            return result;
        }
        public static string SqlInDealRegionName(string regionName)
        {
            string strRegion = StrUtil.SqlINStringDeal(regionName);
            if (strRegion== "'ALL'" || strRegion == "''")
            {
                strRegion = "";
               // strRegion = "'浦东新区','浦东区','黄浦区','静安区','徐汇区','长宁区','普陀区','闸北区','虹口区','杨浦区','宝山区','闵行区','嘉定区','金山区','松江区','青浦区','奉贤区','崇明区'";
            }
            else 
            {
                if (strRegion.Contains("静安区") == true && strRegion.Contains("闸北区") == false)
                {
                    strRegion = strRegion + ",'闸北区'";
                }
                if(strRegion.Contains("浦东新区") == true)
                {
                    strRegion = strRegion + ",'浦东区'";
                }
            }
            return strRegion;
        }
    }
}