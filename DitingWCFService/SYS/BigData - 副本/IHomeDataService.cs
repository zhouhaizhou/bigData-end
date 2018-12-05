using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WcfSmcGridService.SYS.BigData
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的接口名“IHomeDataService”。
    [ServiceContract]
    public interface IHomeDataService
    {

        /// <summary>
        /// 今日下载排行
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "GET",
          RequestFormat = WebMessageFormat.Json,
          ResponseFormat = WebMessageFormat.Json,
          BodyStyle = WebMessageBodyStyle.Bare,
          UriTemplate = "GetDownloadRank?time={time}"
        )]
        string GetDownloadRank(string time);
        /// <summary>
        /// 动态资讯
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "GET",
          RequestFormat = WebMessageFormat.Json,
          ResponseFormat = WebMessageFormat.Json,
          BodyStyle = WebMessageBodyStyle.Bare,
          UriTemplate = "GetDynamicInfo"
        )]
        string GetDynamicInfo();

        ///// <summary>
        ///// 获取用户反馈的信息插入数据库
        ///// </summary>
        ///// <returns></returns>
        //[OperationContract]
        //[WebInvoke(Method = "GET",
        //  RequestFormat = WebMessageFormat.Json,
        //  ResponseFormat = WebMessageFormat.Json,
        //  BodyStyle = WebMessageBodyStyle.Bare,
        //  UriTemplate = "GetUserFormInfo?title={title}&advice={advice}&name={name}&phone={phone}&email={email}&userName={userName}&commiteTime={commiteTime}" 
        //)]
        //string GetUserFormInfo(string title, string advice, string name, string phone, string email, string userName, string commiteTime);


        /// <summary>
        /// 获取用户反馈的信息插入数据库
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "GET",
          RequestFormat = WebMessageFormat.Json,
          ResponseFormat = WebMessageFormat.Json,
          BodyStyle = WebMessageBodyStyle.Bare,
          UriTemplate = "GetUserFormInfo?funParams={funParams}"
        )]
        string GetUserFormInfo(string funParams);

        [OperationContract]
        [WebInvoke(Method = "GET",
          RequestFormat = WebMessageFormat.Json,
          ResponseFormat = WebMessageFormat.Json,
          BodyStyle = WebMessageBodyStyle.Bare,
          UriTemplate = "InsertQuestionData?submitData={submitData}"
        )]
        string InsertQuestionData(string submitData);

        /// <summary>
        /// 相关成果
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "GET",
          RequestFormat = WebMessageFormat.Json,
          ResponseFormat = WebMessageFormat.Json,
          BodyStyle = WebMessageBodyStyle.Bare,
          UriTemplate = "GetRelateResults"
        )]
        string GetRelateResults();

        /// <summary>
        /// 获取调查问卷的模板数据的接口
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "GET",
          RequestFormat = WebMessageFormat.Json,
          ResponseFormat = WebMessageFormat.Json,
          BodyStyle = WebMessageBodyStyle.Bare,
          UriTemplate = "GetQuestionData"
        )]
        string GetQuestionData();

        /// <summary>
        /// 开放统计
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "GET",
          RequestFormat = WebMessageFormat.Json,
          ResponseFormat = WebMessageFormat.Json,
          BodyStyle = WebMessageBodyStyle.Bare,
          UriTemplate = "GetOpenStat"
        )]
        string GetOpenStat();

    }
}
