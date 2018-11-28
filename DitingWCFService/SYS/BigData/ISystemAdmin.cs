using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WcfSmcGridService.SYS.BigData
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的接口名“ISystemAdmin”。
    [ServiceContract]
    public interface ISystemAdmin
    {
        [OperationContract]
        [WebInvoke(Method = "GET",
             RequestFormat = WebMessageFormat.Json,
             ResponseFormat = WebMessageFormat.Json,
             BodyStyle = WebMessageBodyStyle.Bare,
             UriTemplate = "ModifyPass?oldPass={oldPass}&newPass={newPass}"
        )]
        string ModifyPass(string oldPass,string newPass);


        [OperationContract]
        [WebInvoke(Method = "GET",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "GetRoleData"
        )]
        string GetRoleData();

        [WebInvoke(Method = "GET",
           RequestFormat = WebMessageFormat.Json,
           ResponseFormat = WebMessageFormat.Json,
           BodyStyle = WebMessageBodyStyle.Bare,
           UriTemplate = "GetRoleAuth?roleId={roleId}"
       )]
        string GetRoleAuth(string roleId);

        [WebInvoke(Method = "GET",
           RequestFormat = WebMessageFormat.Json,
           ResponseFormat = WebMessageFormat.Json,
           BodyStyle = WebMessageBodyStyle.Bare,
           UriTemplate = "GetRoleExistAuth?roleId={roleId}"
       )]
        string GetRoleExistAuth(string roleId);

        [WebInvoke(Method = "GET",
           RequestFormat = WebMessageFormat.Json,
           ResponseFormat = WebMessageFormat.Json,
           BodyStyle = WebMessageBodyStyle.Bare,
           UriTemplate = "SaveAuth?auth={auth}&roleId={roleId}"
       )]
        void SaveAuth(string auth,string roleId);

        [WebInvoke(Method = "GET",
          RequestFormat = WebMessageFormat.Json,
          ResponseFormat = WebMessageFormat.Json,
          BodyStyle = WebMessageBodyStyle.Bare,
          UriTemplate = "GetDynamicInfo"
      )]
        string GetDynamicInfo();

        [WebInvoke(Method = "GET",
          RequestFormat = WebMessageFormat.Json,
          ResponseFormat = WebMessageFormat.Json,
          BodyStyle = WebMessageBodyStyle.Bare,
          UriTemplate = "DelDynamicInfo?ids={ids}"
      )]
        void DelDynamicInfo(string ids);
        
        [WebInvoke(Method = "GET",
          RequestFormat = WebMessageFormat.Json,
          ResponseFormat = WebMessageFormat.Json,
          BodyStyle = WebMessageBodyStyle.Bare,
          UriTemplate = "GetUser"
      )]
        string GetUser();

        [WebInvoke(Method = "GET",
          RequestFormat = WebMessageFormat.Json,
          ResponseFormat = WebMessageFormat.Json,
          BodyStyle = WebMessageBodyStyle.Bare,
          UriTemplate = "UserManagerSave?values={values}&id={id}"
      )]
        void UserManagerSave(string values,string id);

        [WebInvoke(Method = "GET",
          RequestFormat = WebMessageFormat.Json,
          ResponseFormat = WebMessageFormat.Json,
          BodyStyle = WebMessageBodyStyle.Bare,
          UriTemplate = "GetRelateResult"
      )]
        string GetRelateResult();

        [WebInvoke(Method = "GET",
          RequestFormat = WebMessageFormat.Json,
          ResponseFormat = WebMessageFormat.Json,
          BodyStyle = WebMessageBodyStyle.Bare,
          UriTemplate = "DelRelateResult?ids={ids}"
      )]
        void DelRelateResult(string ids);

        [WebInvoke(Method = "GET",
          RequestFormat = WebMessageFormat.Json,
          ResponseFormat = WebMessageFormat.Json,
          BodyStyle = WebMessageBodyStyle.Bare,
          UriTemplate = "SaveRelateResult?type={type}&name={name}&url={url}&status={status}&id={id}"
      )]
        void SaveRelateResult(string type,string name,string url,string status,string id);

        [WebInvoke(Method = "GET",
          RequestFormat = WebMessageFormat.Json,
          ResponseFormat = WebMessageFormat.Json,
          BodyStyle = WebMessageBodyStyle.Bare,
          UriTemplate = "MyDownload?startTime={startTime}&endTime={endTime}"
      )]
        string MyDownload(string startTime,string endTime);

        [WebInvoke(Method = "GET",
          RequestFormat = WebMessageFormat.Json,
          ResponseFormat = WebMessageFormat.Json,
          BodyStyle = WebMessageBodyStyle.Bare,
          UriTemplate = "DelMyDownload?ids={ids}"
      )]
        void DelMyDownload(string ids);

        [WebInvoke(Method = "GET",
         RequestFormat = WebMessageFormat.Json,
         ResponseFormat = WebMessageFormat.Json,
         BodyStyle = WebMessageBodyStyle.Bare,
         UriTemplate = "GetMyCollect"
     )]
        string GetMyCollect();

        [WebInvoke(Method = "GET",
         RequestFormat = WebMessageFormat.Json,
         ResponseFormat = WebMessageFormat.Json,
         BodyStyle = WebMessageBodyStyle.Bare,
         UriTemplate = "DelMyCollect?ids={ids}"
     )]
        void DelMyCollect(string ids);

        [WebInvoke(Method = "GET",
         RequestFormat = WebMessageFormat.Json,
         ResponseFormat = WebMessageFormat.Json,
         BodyStyle = WebMessageBodyStyle.Bare,
         UriTemplate = "SaveMyDownload?id={id}"
     )]
        void SaveMyDownload(string id);
    }
}
