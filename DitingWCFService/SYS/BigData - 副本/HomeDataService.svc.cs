using Re.Common;
using Readearth.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;
using WcfSmcGridService.BLL;

namespace WcfSmcGridService.SYS.BigData
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“HomeDataService”。
    // 注意: 为了启动 WCF 测试客户端以测试此服务，请在解决方案资源管理器中选择 HomeDataService.svc 或 HomeDataService.svc.cs，然后开始调试。
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class HomeDataService : IHomeDataService
    {
        HomeDataImpl homeDataImpl = new HomeDataImpl();
        /// <summary>
        /// 今日下载排行
        /// </summary>
        /// <returns></returns>
        public string  GetDownloadRank(string time)
        {
            try
            {
                return JsonHelper.ToJSON(homeDataImpl.GetDownloadRank(time));
            }catch(Exception ex)
            {
                string recordInfo = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " HomeDataService_GetDownloadRank " + ex.Message;
                LogManager.WriteLogRecord(DateTime.Now.ToString("yyyy年MM月") + "污染天气大数据平台", recordInfo);
                return "ERROR";
            }
        }
        /// <summary>
        /// 动态资讯
        /// </summary>
        /// <returns></returns>
        public string  GetDynamicInfo()
        {
            try
            {
                return JsonHelper.ToJSON(homeDataImpl.GetDynamicInfo());
            }
            catch (Exception ex)
            {
                string recordInfo = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " HomeDataService_GetDynamicInfo " + ex.Message;
                LogManager.WriteLogRecord(DateTime.Now.ToString("yyyy年MM月") + "污染天气大数据平台", recordInfo);
                return "ERROR";
            }
        }
        /// <summary>
        /// 开放统计
        /// </summary>
        /// <returns></returns>
        public string  GetOpenStat()
        {
            try
            {
                return JsonHelper.ToJSON(homeDataImpl.GetOpenStat());
            }
            catch (Exception ex)
            {
                string recordInfo = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " HomeDataService_GetOpenStat " + ex.Message;
                LogManager.WriteLogRecord(DateTime.Now.ToString("yyyy年MM月") + "污染天气大数据平台", recordInfo);
                return "ERROR";
            }
        }

        ///// <summary>
        ///// 获取用户反馈的信息插入数据库
        ///// </summary>
        ///// <returns></returns>
        //public string GetUserFormInfo(string title, string advice, string name,string phone,string email,string userName,string commiteTime)
        //{
        //    try
        //    {
        //        //return JsonHelper.ToJSON(homeDataImpl.GetDynamicInfo());
        //        return JsonHelper.ToJSON(homeDataImpl.GetUserFormInfo(title, advice, name, phone, email, userName, commiteTime));//返回的为字符串
        //    }
        //    catch (Exception ex)
        //    {
        //        string recordInfo = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " HomeDataService_GetDynamicInfo " + ex.Message;
        //        LogManager.WriteLogRecord(DateTime.Now.ToString("yyyy年MM月") + "污染天气大数据平台", recordInfo);
        //        return "ERROR";
        //    }
        //}

        /// <summary>
        /// 获取用户反馈的信息插入数据库
        /// </summary>
        /// <returns></returns>
        public string GetUserFormInfo(string funParams)
        {
            try
            {
                //return JsonHelper.ToJSON(homeDataImpl.GetDynamicInfo());
                return JsonHelper.ToJSON(homeDataImpl.GetUserFormInfo(funParams));//返回的为字符串
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        /// <summary>
        /// 调查问卷提交的数据
        /// </summary>
        /// <param name="submitData"></param>
        /// <returns></returns>
        public string InsertQuestionData(string submitData) {
            try
            {
                //return JsonHelper.ToJSON(homeDataImpl.GetDynamicInfo());
                return JsonHelper.ToJSON(homeDataImpl.InsertQuestionData(submitData));//返回的为字符串
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// 相关成果
        /// </summary>
        /// <returns></returns>
        public string GetRelateResults()
        {
            try
            {
                return JsonHelper.ToJSON(homeDataImpl.GetRelateResults());

            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        /// <summary>
        /// 获取调查问卷的模板数据
        /// </summary>
        /// <returns></returns>
        public string GetQuestionData()
        {
            try
            {
                return JsonHelper.ToJSON(homeDataImpl.GetQuestionData());

            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
