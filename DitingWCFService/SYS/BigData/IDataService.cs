using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WcfSmcGridService.SYS.BigData {
  // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的接口名“IDataService”。
  [ServiceContract]
  public interface IDataService {
    /// <summary>
    ///  获得省信息
    /// </summary>
    /// <returns></returns>
    [OperationContract]
    [WebInvoke (Method = "GET",
      RequestFormat = WebMessageFormat.Json,
      ResponseFormat = WebMessageFormat.Json,
      BodyStyle = WebMessageBodyStyle.Bare,
      UriTemplate = "GetProvince?DataType={DataType}"
    )]
    string GetProvince (string DataType);

    /// <summary>
    ///  获得站点信息
    /// </summary>
    /// <returns></returns>
    [OperationContract]
    [WebInvoke (Method = "GET",
      RequestFormat = WebMessageFormat.Json,
      ResponseFormat = WebMessageFormat.Json,
      BodyStyle = WebMessageBodyStyle.Bare,
      UriTemplate = "GetSitesInfo"
    )]
    string GetSitesInfo ();

    /// <summary>
    ///  获得AWS站点信息
    /// </summary>
    /// <returns></returns>
    [OperationContract]
    [WebInvoke (Method = "GET",
      RequestFormat = WebMessageFormat.Json,
      ResponseFormat = WebMessageFormat.Json,
      BodyStyle = WebMessageBodyStyle.Bare,
      UriTemplate = "GetAWSSite"
    )]
    string GetAWSSite ();

    /// <summary>
    ///  获得AWS数据
    /// </summary>
    /// <returns></returns>
    [OperationContract]
    [WebInvoke (Method = "GET",
      RequestFormat = WebMessageFormat.Json,
      ResponseFormat = WebMessageFormat.Json,
      BodyStyle = WebMessageBodyStyle.Bare,
      UriTemplate = "GetAWSData?beginTime={beginTime}&endTime={endTime}&siteNo={siteNo}&type={type}&element={element}"
    )]
    string GetAWSData (string beginTime, string endTime, string siteNo, string type, string element);

    /// <summary>
    ///  获得资料清单
    /// </summary>
    /// <returns></returns>
    [OperationContract]
    [WebInvoke (Method = "GET",
      RequestFormat = WebMessageFormat.Json,
      ResponseFormat = WebMessageFormat.Json,
      BodyStyle = WebMessageBodyStyle.Bare,
      UriTemplate = "GetDataList"
    )]
    string GetDataList ();

    /// <summary>
    ///  获得资料清单详细信息
    /// </summary>
    /// <returns></returns>
    [OperationContract]
    [WebInvoke (Method = "GET",
      RequestFormat = WebMessageFormat.Json,
      ResponseFormat = WebMessageFormat.Json,
      BodyStyle = WebMessageBodyStyle.Bare,
      UriTemplate = "GetDataListDetail?moduleEnName={moduleEnName}"
    )]
    string GetDataListDetail (string moduleEnName);

    /// <summary>
    ///  获得资料字段说明
    /// </summary>
    /// <returns></returns>
    [OperationContract]
    [WebInvoke (Method = "GET",
      RequestFormat = WebMessageFormat.Json,
      ResponseFormat = WebMessageFormat.Json,
      BodyStyle = WebMessageBodyStyle.Bare,
      UriTemplate = "GetColumnDetail?moduleEnName={moduleEnName}"
    )]
    string GetColumnDetail (string moduleEnName);

    /// <summary>
    ///  获得天气情况
    /// </summary>
    /// <returns></returns>
    [OperationContract]
    [WebInvoke (Method = "GET",
      RequestFormat = WebMessageFormat.Json,
      ResponseFormat = WebMessageFormat.Json,
      BodyStyle = WebMessageBodyStyle.Bare,
      UriTemplate = "GetAirCityInfo?city={city}"
    )]
    string GetAirCityInfo (string city);

    /// <summary>
    ///  得到资料对应的间隔时间
    /// </summary>
    /// <returns></returns>
    [OperationContract]
    [WebInvoke (Method = "GET",
      RequestFormat = WebMessageFormat.Json,
      ResponseFormat = WebMessageFormat.Json,
      BodyStyle = WebMessageBodyStyle.Bare,
      UriTemplate = "GetDataIntelTime?dataType={dataType}"
    )]
    string GetDataIntelTime (string dataType);

    /// <summary>
    ///  获得模块的数据接口里面的详细资料信息
    /// </summary>
    /// <returns></returns>
    [OperationContract]
    [WebInvoke (Method = "GET",
      RequestFormat = WebMessageFormat.Json,
      ResponseFormat = WebMessageFormat.Json,
      BodyStyle = WebMessageBodyStyle.Bare,
      UriTemplate = "GetIntefaceInfo?moduleName={moduleName}"
    )]
    string GetIntefaceInfo (string moduleName);

    /// <summary>
    ///  获得数据的最新时间
    /// </summary>
    /// <returns></returns>
    [OperationContract]
    [WebInvoke (Method = "GET",
      RequestFormat = WebMessageFormat.Json,
      ResponseFormat = WebMessageFormat.Json,
      BodyStyle = WebMessageBodyStyle.Bare,
      UriTemplate = "GetDataLastTime?dataType={dataType}"
    )]
    string GetDataLastTime (string dataType);

    /// <summary>
    ///  登录验证
    /// </summary>
    /// <returns></returns>
    [OperationContract]
    [WebInvoke (Method = "GET",
      RequestFormat = WebMessageFormat.Json,
      ResponseFormat = WebMessageFormat.Json,
      BodyStyle = WebMessageBodyStyle.Bare,
      UriTemplate = "Login?userName={userName}&Pwd={Pwd}"
    )]
    string Login (string userName, string Pwd);

    /// <summary>
    ///  获得站点信息
    /// </summary>
    /// <returns></returns>
    [OperationContract]
    [WebInvoke (Method = "GET",
      RequestFormat = WebMessageFormat.Json,
      ResponseFormat = WebMessageFormat.Json,
      BodyStyle = WebMessageBodyStyle.Bare,
      UriTemplate = "GetMaxTime"
    )]
    string GetMaxTime ();

    /// <summary>
    ///  获得站点信息
    /// </summary>
    /// <returns></returns>
    [OperationContract]
    [WebInvoke (Method = "GET",
      RequestFormat = WebMessageFormat.Json,
      ResponseFormat = WebMessageFormat.Json,
      BodyStyle = WebMessageBodyStyle.Bare,
      UriTemplate = "GetSitebyName?ModuleName={ModuleName}"
    )]
    string GetSitebyName (string ModuleName);

    /// <summary>
    ///  获得国控站点信息
    /// </summary>
    /// <returns></returns>
    [OperationContract]
    [WebInvoke (Method = "GET",
      RequestFormat = WebMessageFormat.Json,
      ResponseFormat = WebMessageFormat.Json,
      BodyStyle = WebMessageBodyStyle.Bare,
      UriTemplate = "GetAirSitesInfo"
    )]
    string GetAirSitesInfo ();

    /// <summary>
    /// 根据省市获得城市站点信息
    /// </summary>
    /// <param name="DataType"></param>
    /// <returns></returns>
    [OperationContract]
    [WebInvoke (Method = "GET",
      RequestFormat = WebMessageFormat.Json,
      ResponseFormat = WebMessageFormat.Json,
      BodyStyle = WebMessageBodyStyle.Bare,
      UriTemplate = "GetStationByDataType?DataType={DataType}&Provinces={Provinces}"
    )]
    string GetStationByDataType (string DataType, string Provinces);

    /// <summary>
    /// 获得站点或者所有站点的传入时间点或最新时间点数据(空气，气象)
    /// </summary>
    /// <param name="stationID"></param>
    /// <param name="timePoint"></param>
    /// <param name="element"></param>
    /// <param name="duration"></param>
    /// <param name="isHistory"></param>
    /// <returns></returns>
    [OperationContract]
    [WebInvoke (Method = "GET",
      RequestFormat = WebMessageFormat.Json,
      ResponseFormat = WebMessageFormat.Json,
      BodyStyle = WebMessageBodyStyle.Bare,
      UriTemplate = "GetDatas?stationID={stationID}&timePoint={timePoint}&element={element}&duration={duration}&isHistory={isHistory}"
    )]
    string GetDatas (string stationID, string timePoint, string element, string duration, string isHistory);
    /// <summary>
    /// 获得要素信息
    /// </summary>
    /// <returns></returns>
    [OperationContract]
    [WebInvoke (Method = "GET",
      RequestFormat = WebMessageFormat.Json,
      ResponseFormat = WebMessageFormat.Json,
      BodyStyle = WebMessageBodyStyle.Bare,
      UriTemplate = "GetElement?DataType={DataType}"
    )]
    string GetElement (string DataType);

    /// <summary>
    /// 获得子标题信息
    /// </summary>
    /// <returns></returns>
    [OperationContract]
    [WebInvoke (Method = "GET",
      RequestFormat = WebMessageFormat.Json,
      ResponseFormat = WebMessageFormat.Json,
      BodyStyle = WebMessageBodyStyle.Bare,
      UriTemplate = "getSubTitle?moduleEnName={moduleEnName}&account={account}"
    )]
    string getSubTitle (string moduleEnName, string account);

    /// <summary>
    /// 根据父节点获得数据服务信息
    /// </summary>
    /// <param name="parentModule">父节点名称</param>
    /// <param name="roldId">备用参数，现在不考虑</param>
    /// <returns></returns>
    [OperationContract]
    [WebInvoke (Method = "GET",
      RequestFormat = WebMessageFormat.Json,
      ResponseFormat = WebMessageFormat.Json,
      BodyStyle = WebMessageBodyStyle.Bare,
      UriTemplate = "GetModuleByParentModule?parentModule={parentModule}&roldId={roldId}&account={account}"
    )]
    string GetModuleByParentModule (string parentModule, string roldId, string account);

    /// <summary>
    /// 查看次数-更新
    /// </summary>
    /// <param name="moduleEnName">小模块英文名</param>
    /// <returns></returns>
    [OperationContract]
    [WebInvoke (Method = "GET",
      RequestFormat = WebMessageFormat.Json,
      ResponseFormat = WebMessageFormat.Json,
      BodyStyle = WebMessageBodyStyle.Bare,
      UriTemplate = "updateViewCount?moduleEnName={moduleEnName}"
    )]
    string updateViewCount (string moduleEnName);
    /// <summary>
    /// 评论次数-更新
    /// </summary>
    /// <param name="moduleEnName">小模块英文名</param>
    /// <returns></returns>
    [OperationContract]
    [WebInvoke (Method = "GET",
      RequestFormat = WebMessageFormat.Json,
      ResponseFormat = WebMessageFormat.Json,
      BodyStyle = WebMessageBodyStyle.Bare,
      UriTemplate = "updateCommentCount?moduleEnName={moduleEnName}"
    )]
    string updateCommentCount (string moduleEnName);

    /// <summary>
    /// 点赞次数-更新
    /// </summary>
    /// <param name="moduleEnName">小模块英文名</param>
    /// <returns></returns>
    [OperationContract]
    [WebInvoke (Method = "GET",
      RequestFormat = WebMessageFormat.Json,
      ResponseFormat = WebMessageFormat.Json,
      BodyStyle = WebMessageBodyStyle.Bare,
      UriTemplate = "updateLikesCount?moduleEnName={moduleEnName}"
    )]
    string updateLikesCount (string moduleEnName);

    /// <summary>
    /// 下载清单查询
    /// </summary>
    /// <param name="userName">用户名</param>
    /// <param name="startTime">开始时间</param>
    /// <param name="endTime">结束时间</param>
    /// <returns></returns>
    [OperationContract]
    [WebInvoke (Method = "GET",
      RequestFormat = WebMessageFormat.Json,
      ResponseFormat = WebMessageFormat.Json,
      BodyStyle = WebMessageBodyStyle.Bare,
      UriTemplate = "GetDownloadList?userName={userName}&startTime={startTime}&endTime={endTime}"
    )]
    string GetDownloadList (string userName, string startTime, string endTime);

    /// <summary>
    /// 获得活动的列表清单
    /// </summary>
    /// <param name="userName"></param>
    /// <returns></returns>
    [OperationContract]
    [WebInvoke (Method = "GET",
      RequestFormat = WebMessageFormat.Json,
      ResponseFormat = WebMessageFormat.Json,
      BodyStyle = WebMessageBodyStyle.Bare,
      UriTemplate = "GetActiveList?userName={userName}"
    )]
    string GetActiveList (string userName);

    /// <summary>
    /// 删除下载清单
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="ids"></param>
    /// <returns></returns>
    [OperationContract]
    [WebInvoke (Method = "GET",
      RequestFormat = WebMessageFormat.Json,
      ResponseFormat = WebMessageFormat.Json,
      BodyStyle = WebMessageBodyStyle.Bare,
      UriTemplate = "deleteDownList?userName={userName}&ids={ids}"
    )]
    string deleteDownList (string userName, string ids);

    /// <summary>
    /// 根据用户名，id 批量下载
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="ids"></param>
    /// <returns></returns>
    [OperationContract]
    [WebInvoke (Method = "GET",
      RequestFormat = WebMessageFormat.Json,
      ResponseFormat = WebMessageFormat.Json,
      BodyStyle = WebMessageBodyStyle.Bare,
      UriTemplate = "BatchDownload?userName={userName}&ids={ids}"
    )]
    string BatchDownload (string userName, string ids);

    /// <summary>
    /// 直接下载，或者加入下载清单
    /// </summary>
    /// <param name="funParams"></param>
    /// <returns></returns>
    [OperationContract]
    [WebInvoke (Method = "GET",
      RequestFormat = WebMessageFormat.Json,
      ResponseFormat = WebMessageFormat.Json,
      BodyStyle = WebMessageBodyStyle.Bare,
      UriTemplate = "insertDownList?funParams={funParams}"
    )]
    string insertDownList (string funParams);

    /// <summary>
    /// 更新访问信息 2018-09-17 by 孙明宇
    /// </summary>
    /// <param name="id"></param>
    /// <param name="access"></param>
    /// <param name="ip"></param>
    /// <returns></returns>
    [OperationContract]
    [WebInvoke (Method = "GET",
      RequestFormat = WebMessageFormat.Json,
      ResponseFormat = WebMessageFormat.Json,
      BodyStyle = WebMessageBodyStyle.Bare,
      UriTemplate = "UpdateVisit?access={access}&ip={ip}&location={location}"
    )]
    string UpdateVisit (string access, string ip, string location);

    /// <summary>
    /// 我的收藏 插入
    /// </summary>
    /// <param name="funParams"></param>
    /// <returns></returns>
    [OperationContract]
    [WebInvoke (Method = "GET",
      RequestFormat = WebMessageFormat.Json,
      ResponseFormat = WebMessageFormat.Json,
      BodyStyle = WebMessageBodyStyle.Bare,
      UriTemplate = "insertLikeData?moduleEnName={moduleEnName}&account={account}&insertTime={insertTime}"
    )]
    string insertLikeData (string moduleEnName, string account, string insertTime);
    /// <summary>
    /// 我的收藏 取消收藏
    /// </summary>
    /// <param name="funParams"></param>
    /// <returns></returns>
    [OperationContract]
    [WebInvoke (Method = "GET",
      RequestFormat = WebMessageFormat.Json,
      ResponseFormat = WebMessageFormat.Json,
      BodyStyle = WebMessageBodyStyle.Bare,
      UriTemplate = "deleteLikeData?moduleEnName={moduleEnName}&account={account}"
    )]
    string deleteLikeData (string moduleEnName, string account);

    [OperationContract]
    [WebInvoke (Method = "GET",
      RequestFormat = WebMessageFormat.Json,
      ResponseFormat = WebMessageFormat.Json,
      BodyStyle = WebMessageBodyStyle.Bare,
      UriTemplate = "GetAllComment?moduleId={moduleId}"
    )]
    string GetAllComment (string moduleId);

    [OperationContract]
    [WebInvoke (Method = "GET",
      RequestFormat = WebMessageFormat.Json,
      ResponseFormat = WebMessageFormat.Json,
      BodyStyle = WebMessageBodyStyle.Bare,
      UriTemplate = "ClickComment?moduleId={moduleId}&id={id}&inputComment={inputComment}"
    )]
    void ClickComment (string moduleId, string id, string inputComment);

  }
}