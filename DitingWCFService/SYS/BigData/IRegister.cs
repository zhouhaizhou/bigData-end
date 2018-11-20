using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WcfSmcGridService.SYS.BigData
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的接口名“IRegister”。
    [ServiceContract]
    public interface IRegister
    {
        /// <summary>
        /// 根据不同的角色获取注册信息
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "GET",
          RequestFormat = WebMessageFormat.Json,
          ResponseFormat = WebMessageFormat.Json,
          BodyStyle = WebMessageBodyStyle.Bare,
          UriTemplate = "GetRegisterItem?roleId={roleId}"
        )]
        string GetRegisterItem(string roleId);
        /// <summary>
        /// 根据不同的角色获取注册信息
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "GET",
          RequestFormat = WebMessageFormat.Json,
          ResponseFormat = WebMessageFormat.Json,
          BodyStyle = WebMessageBodyStyle.Bare,
          UriTemplate = "GetInfo?roleId={roleId}"
        )]
        string GetInfo(string roleId);
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        //[OperationContract]
        //[WebInvoke(Method = "GET",
        //  RequestFormat = WebMessageFormat.Json,
        //  ResponseFormat = WebMessageFormat.Json,
        //  BodyStyle = WebMessageBodyStyle.Bare,
        //  UriTemplate = "ClickRegister?streams={streams}"
        //)]
        //string ClickRegister(string streams);

        [OperationContract]
        [WebInvoke(UriTemplate = "ClickRegister", Method = "POST", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare,
                    ResponseFormat = WebMessageFormat.Json)]
        string ClickRegister(Stream stream);

        /// <summary>
        /// 账号验证
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "GET",
         RequestFormat = WebMessageFormat.Json,
         ResponseFormat = WebMessageFormat.Json,
         BodyStyle = WebMessageBodyStyle.Bare,
         UriTemplate = "CheckAccount?account={account}"
       )]
        string CheckAccount(string account);
        /// <summary>
        /// 获取注册项的正则
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "GET",
         RequestFormat = WebMessageFormat.Json,
         ResponseFormat = WebMessageFormat.Json,
         BodyStyle = WebMessageBodyStyle.Bare,
         UriTemplate = "GetcheckItem?roleId={roleId}"
       )]
        string GetcheckItem(string roleId);
        /// <summary>
        /// 获取默认路由
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "GET",
         RequestFormat = WebMessageFormat.Json,
         ResponseFormat = WebMessageFormat.Json,
         BodyStyle = WebMessageBodyStyle.Bare,
         UriTemplate = "GetDefault?rootName={rootName}"
       )]
        string GetDefault(string rootName);
    }
}
