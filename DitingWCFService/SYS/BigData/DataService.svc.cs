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
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“DataService”。
    // 注意: 为了启动 WCF 测试客户端以测试此服务，请在解决方案资源管理器中选择 DataService.svc 或 DataService.svc.cs，然后开始调试。
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class DataService : IDataService
    {
        DataServiceImpl dsImpl = new DataServiceImpl();
        /// <summary>
        /// 获得省信息
        /// </summary>
        /// <returns></returns>
        public string GetProvince(string DataType)
        {
            try
            {
                return JsonHelper.ToJSON(dsImpl.GetProvince(DataType));
            }
            catch (Exception ex)
            {
                string recordInfo = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " DataService_GetProvince " + ex.Message;
                LogManager.WriteLogRecord(DateTime.Now.ToString("yyyy年MM月") + "污染天气大数据平台", recordInfo);
                return "ERROR";
            }
        }

        /// <summary>
        /// 获得要素信息
        /// </summary>
        /// <returns></returns>
        /// 
        public string GetStationByDataType(string DataType, string Provinces)
        {
            try
            {
                return JsonHelper.ToJSON(dsImpl.GetStationByDataType(DataType, Provinces));
            }
            catch (Exception ex)
            {
                string recordInfo = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " DataService_GetStationByDataType " + ex.Message;
                LogManager.WriteLogRecord(DateTime.Now.ToString("yyyy年MM月") + "污染天气大数据平台", recordInfo);
                return "";
            }
        }
       
        /// <summary>
        /// 获得要素信息
        /// </summary>
        /// <returns></returns>
        public string GetElement(string DataType)
        {
            try
            {
                return JsonHelper.ToJSON(dsImpl.GetElement(DataType));
            }
            catch (Exception ex)
            {
                string recordInfo = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " DataService_GetElement " + ex.Message;
                LogManager.WriteLogRecord(DateTime.Now.ToString("yyyy年MM月") + "污染天气大数据平台", recordInfo);
                return "ERROR";
            }
        }
         /// <summary>
        /// 获得子标题信息
        /// </summary>
        /// <returns></returns>
        public string getSubTitle(string moduleEnName, string account)
        {
            try
            {
                return JsonHelper.ToJSON(dsImpl.getSubTitle(moduleEnName,account));
            }
            catch (Exception ex)
            {
                string recordInfo = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " DataService_getSubTitle " + ex.Message;
                LogManager.WriteLogRecord(DateTime.Now.ToString("yyyy年MM月") + "污染天气大数据平台", recordInfo);
                return "ERROR";
            }
        }

        /// <summary>
        /// 根据父节点获得数据服务信息
        /// </summary>
        /// <param name="parentModule">父节点名称</param>
        /// <param name="roldId">备用参数，现在不考虑</param>
        /// <returns></returns>
        public string GetModuleByParentModule(string parentModule, string roldId, string account)
        {
            try
            {
                return JsonHelper.ToJSON(dsImpl.GetModuleByParentModule(parentModule,roldId,account));
            }
            catch (Exception ex)
            {
                string recordInfo = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " DataService_GetModuleByParentModule " + ex.Message;
                LogManager.WriteLogRecord(DateTime.Now.ToString("yyyy年MM月") + "污染天气大数据平台", recordInfo);
                return "ERROR";
            }
        }
       
        /// <summary>
        /// 查看次数-更新
        /// </summary>
        /// <param name="moduleEnName">小模块英文名</param>
        /// <returns></returns>
        public string updateViewCount(string moduleEnName)
        {
            try
            {
                return JsonHelper.ToJSON(dsImpl.updateViewCount(moduleEnName));
            }
            catch (Exception ex)
            {
                string recordInfo = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " DataService_updateViewCount " + ex.Message;
                LogManager.WriteLogRecord(DateTime.Now.ToString("yyyy年MM月") + "污染天气大数据平台", recordInfo);
                return "ERROR";
            }
        }
        /// <summary>
        /// 评论次数-更新
        /// </summary>
        /// <param name="moduleEnName">小模块英文名</param>
        /// <returns></returns>
        public string updateCommentCount(string moduleEnName)
        {
            try
            {
                return JsonHelper.ToJSON(dsImpl.updateCommentCount(moduleEnName));
            }
            catch (Exception ex)
            {
                string recordInfo = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " DataService_updateCommentCount " + ex.Message;
                LogManager.WriteLogRecord(DateTime.Now.ToString("yyyy年MM月") + "污染天气大数据平台", recordInfo);
                return "ERROR";
            }
        }

        /// <summary>
        /// 点赞次数-更新
        /// </summary>
        /// <param name="moduleEnName">小模块英文名</param>
        /// <returns></returns>
        public string updateLikesCount(string moduleEnName)
        {
            try
            {
                return JsonHelper.ToJSON(dsImpl.updateLikesCount(moduleEnName));
            }
            catch (Exception ex)
            {
                string recordInfo = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " DataService_updateLikesCount " + ex.Message;
                LogManager.WriteLogRecord(DateTime.Now.ToString("yyyy年MM月") + "污染天气大数据平台", recordInfo);
                return "ERROR";
            }
        }

        /// <summary>
        /// 下载清单查询
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns></returns>
        public string GetDownloadList(string userName, string startTime, string endTime)
        {
            try
            {
                return JsonHelper.ToJSON(dsImpl.GetDownloadList(userName, startTime, endTime));
            }
            catch (Exception ex)
            {
                string recordInfo = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " DataService_GetProvince " + ex.Message;
                LogManager.WriteLogRecord(DateTime.Now.ToString("yyyy年MM月") + "污染天气大数据平台", recordInfo);
                return "ERROR";
            }
        }

        /// <summary>
        /// 获得活动的列表清单
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public string  GetActiveList(string userName)
        {
            try
            {
                return JsonHelper.ToJSON(dsImpl.GetActiveList(userName));
            }
            catch (Exception ex)
            {
                string recordInfo = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " DataService_GetActiveList " + ex.Message;
                LogManager.WriteLogRecord(DateTime.Now.ToString("yyyy年MM月") + "污染天气大数据平台", recordInfo);
                return "ERROR";
            }
        }
        /// <summary>
        /// 删除下载清单
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
         public string  deleteDownList(string userName,string ids)
        {
            try
            {
                return JsonHelper.ToJSON(dsImpl.deleteDownList(userName,ids));
            }
            catch (Exception ex)
            {
                string recordInfo = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " DataService_deleteDownList " + ex.Message;
                LogManager.WriteLogRecord(DateTime.Now.ToString("yyyy年MM月") + "污染天气大数据平台", recordInfo);
                return "ERROR";
            }
        }

         /// <summary>
        /// 根据用户名，id 批量下载
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
         public string BatchDownload(string userName,string ids)
         {
             try
             {
                 return JsonHelper.ToJSON(dsImpl.BatchDownload(userName, ids));
             }
             catch (Exception ex)
             {
                 string recordInfo = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " DataService_BatchDownload " + ex.Message;
                 LogManager.WriteLogRecord(DateTime.Now.ToString("yyyy年MM月") + "污染天气大数据平台", recordInfo);
                 return "ERROR";
             }
         }

          /// <summary>
        /// 直接下载，或者加入下载清单
        /// </summary>
        /// <param name="funParams"></param>
        /// <returns></returns>
         public string insertDownList(string funParams)
         {
             try
             {
                 return JsonHelper.ToJSON(dsImpl.insertDownList(funParams));
             }
             catch (Exception ex)
             {
                 string recordInfo = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " DataService_insertDonwList " + ex.Message;
                 LogManager.WriteLogRecord(DateTime.Now.ToString("yyyy年MM月") + "污染天气大数据平台", recordInfo);
                 return "ERROR";
             }
         }


        /// <summary>
        /// 获得要素信息
        /// </summary>
        /// <returns></returns>
        /// 
        public string GetSitesInfo()
        {
            try
            {
                return JsonHelper.ToJSON(dsImpl.GetSiteInfo());
            }
            catch (Exception ex)
            {
                string recordInfo = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " GetSiteInfo " + ex.Message;
                LogManager.WriteLogRecord(DateTime.Now.ToString("yyyy年MM月") + "污染天气大数据平台", recordInfo);
                return "ERROR";
            }
        }

        /// <summary>
        /// 获得空气国控站点
        /// </summary>
        /// <returns></returns>
        /// 
        public string GetAirSitesInfo()
        {
            try
            {
                return JsonHelper.ToJSON(dsImpl.GetAirSiteInfo());
            }
            catch (Exception ex)
            {
                string recordInfo = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " GetAirSiteInfo " + ex.Message;
                LogManager.WriteLogRecord(DateTime.Now.ToString("yyyy年MM月") + "污染天气大数据平台", recordInfo);
                return "ERROR";
            }
        }

        public string GetMaxTime()
        {
            try
            {
                return dsImpl.GetMaxTime();
            }
            catch (Exception ex)
            {
                string recordInfo = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " GetMaxTime " + ex.Message;
                LogManager.WriteLogRecord(DateTime.Now.ToString("yyyy年MM月") + "污染天气大数据平台", recordInfo);
                return "ERROR";
            }
        }

        /// <summary>
        /// 获得站点或者所有站点的传入时间点或最新时间点数据(空气，气象)
        /// </summary>
        /// <returns></returns>
        /// 
        public string GetDatas(string stationID, string timePoint, string element, string duration, string isHistory)
        {
            try
            {
                return JsonHelper.ToJSON(dsImpl.GetDatas(stationID,  timePoint,  
                                                         element,  duration,  isHistory));
            }
            catch (Exception ex)
            {
                string recordInfo = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " GetDatas " + ex.Message;
                LogManager.WriteLogRecord(DateTime.Now.ToString("yyyy年MM月") + "污染天气大数据平台", recordInfo);
                return "ERROR";
            }
        }
        /// <summary>
        /// 我的收藏 插入
        /// </summary>
        /// <param name="moduleEnName"></param>
        /// <param name="account"></param>
        /// <param name="insertTime"></param>
        /// <returns></returns>
        public string insertLikeData(string moduleEnName, string account, string insertTime)
        {
            try
            {
                return JsonHelper.ToJSON(dsImpl.insertLikeData(moduleEnName, account, insertTime));
            }
            catch (Exception ex)
            {

                return ex.Message;
            }
        }
        /// <summary>
        /// 我的收藏 取消收藏
        /// </summary>
        /// <param name="moduleEnName"></param>
        /// <param name="account"></param>
        /// <param name="insertTime"></param>
        /// <returns></returns>
        public string deleteLikeData(string moduleEnName, string account)
        {
            try
            {
                return JsonHelper.ToJSON(dsImpl.deleteLikeData(moduleEnName, account));
            }
            catch (Exception ex)
            {

                return ex.Message;
            }
        }

        public string GetSitebyName(string ModuleName)
        {
            try
            {
                return JsonHelper.ToJSON(dsImpl.GetSitebyName(ModuleName));
            }
            catch (Exception ex)
            {
                string recordInfo = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " GetSitebyName " + ex.Message;
                LogManager.WriteLogRecord(DateTime.Now.ToString("yyyy年MM月") + "污染天气大数据平台", recordInfo);
                return "ERROR";
            }
        }

        public string GetAirCityInfo(string city)
        {
            try
            {
                return dsImpl.GetAirCityInfo(city);
            }
            catch (Exception ex)
            {
                string recordInfo = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " GetAirCityInfo " + ex.Message;
                LogManager.WriteLogRecord(DateTime.Now.ToString("yyyy年MM月") + "污染天气大数据平台", recordInfo);
                return "ERROR";
            }
        }

        public string GetDataList()
        {
            try
            {
                return JsonHelper.ToJSON(dsImpl.GetDataList());
            }
            catch (Exception ex)
            {
                string recordInfo = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " GetDataList " + ex.Message;
                LogManager.WriteLogRecord(DateTime.Now.ToString("yyyy年MM月") + "污染天气大数据平台", recordInfo);
                return "ERROR";
            }
        }

        public string GetDataListDetail(string moduleEnName)
        {
            try
            {
                return JsonHelper.ToJSON(dsImpl.GetDataListDetail(moduleEnName));
            }
            catch (Exception ex)
            {
                string recordInfo = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " GetDataListDetail " + ex.Message;
                LogManager.WriteLogRecord(DateTime.Now.ToString("yyyy年MM月") + "污染天气大数据平台", recordInfo);
                return "ERROR";
            }
        }

        public string GetColumnDetail(string moduleEnName)
        {
            try
            {
                return JsonHelper.ToJSON(dsImpl.GetColumnDetail(moduleEnName));
            }
            catch (Exception ex)
            {
                string recordInfo = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " GetColumnDetail " + ex.Message;
                LogManager.WriteLogRecord(DateTime.Now.ToString("yyyy年MM月") + "污染天气大数据平台", recordInfo);
                return "ERROR";
            }
        }

        public string Login(string userName, string Pwd)
        {
            try
            {
                return JsonHelper.ToJSON(dsImpl.Login(userName,Pwd));
            }
            catch (Exception ex)
            {
                string recordInfo = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " Login " + ex.Message;
                LogManager.WriteLogRecord(DateTime.Now.ToString("yyyy年MM月") + "污染天气大数据平台", recordInfo);
                return "ERROR";
            }
        }

        public string GetIntefaceInfo(string moduleName)
        {
            try
            {
                return JsonHelper.ToJSON(dsImpl.GetIntefaceInfo(moduleName));
            }
            catch (Exception ex)
            {
                string recordInfo = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " GetIntefaceInfo " + ex.Message;
                LogManager.WriteLogRecord(DateTime.Now.ToString("yyyy年MM月") + "污染天气大数据平台", recordInfo);
                return "ERROR";
            }
        }

        public string GetDataIntelTime(string dataType)
        {
            try
            {
                return JsonHelper.ToJSON(dsImpl.GetDataIntelTime(dataType));
            }
            catch (Exception ex)
            {
                string recordInfo = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " GetDataIntelTime " + ex.Message;
                LogManager.WriteLogRecord(DateTime.Now.ToString("yyyy年MM月") + "污染天气大数据平台", recordInfo);
                return "ERROR";
            }
        }

        public string GetDataLastTime(string dataType)
        {
            try
            {
                return JsonHelper.ToJSON(dsImpl.GetDataLastTime(dataType));
            }
            catch (Exception ex)
            {
                string recordInfo = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " GetDataLastTime " + ex.Message;
                LogManager.WriteLogRecord(DateTime.Now.ToString("yyyy年MM月") + "污染天气大数据平台", recordInfo);
                return "ERROR";
            }
        }

        public string UpdateVisit(string access, string ip, string location)
        {

            try
            {
                return JsonHelper.ToJSON(dsImpl.UpdateLoginTime(access, ip, location));
            }
            catch (Exception ex)
            {
                string recordInfo = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " GetDataLastTime " + ex.Message;
                LogManager.WriteLogRecord(DateTime.Now.ToString("yyyy年MM月") + "污染天气大数据平台", recordInfo);
                return "ERROR";
            }
        }

        public string GetAWSSite()
        {
            try
            {
                return JsonHelper.ToJSON(dsImpl.GetAWSSite());
            }
            catch (Exception ex)
            {
                string recordInfo = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " GetAWSSite " + ex.Message;
                LogManager.WriteLogRecord(DateTime.Now.ToString("yyyy年MM月") + "污染天气大数据平台", recordInfo);
                return "ERROR";
            }
        }

        public string GetAWSData(string beginTime, string endTime, string siteNo, string type,string element)
        {
            try
            {
                return JsonHelper.ToJSON(dsImpl.GetAWSData(beginTime,  endTime,  siteNo,  type, element));
            }
            catch (Exception ex)
            {
                string recordInfo = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " GetAWSData " + ex.Message;
                LogManager.WriteLogRecord(DateTime.Now.ToString("yyyy年MM月") + "污染天气大数据平台", recordInfo);
                return "ERROR";
            }
        }
    }
}
