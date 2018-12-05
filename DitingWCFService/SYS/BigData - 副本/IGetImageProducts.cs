using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WcfSmcGridService.SYS.BigData
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的接口名“ILoadMenuService”。
    [ServiceContract]
    public interface IGetImageProducts
    {
        [OperationContract]
        [WebInvoke(Method = "GET",
                   RequestFormat = WebMessageFormat.Json,
                  ResponseFormat = WebMessageFormat.Json,
                       BodyStyle = WebMessageBodyStyle.Bare,
                     UriTemplate = "GetImageProducts?EntityName={EntityName}&Station={Station}&type={type}&bTime={bTime}&eTime={eTime}&interTime={interTime}"
        )]
        string GetImageProducts(string EntityName, string Station, string type,
            string bTime, string eTime, string interTime);


        [OperationContract]
        [WebInvoke(Method = "GET",
            RequestFormat = WebMessageFormat.Json,
           ResponseFormat = WebMessageFormat.Json,
                BodyStyle = WebMessageBodyStyle.Bare,
              UriTemplate = "Hello"
        )]
        string Hello();

        [OperationContract]
        [WebInvoke(Method = "GET",
                   RequestFormat = WebMessageFormat.Json,
                  ResponseFormat = WebMessageFormat.Json,
                       BodyStyle = WebMessageBodyStyle.Bare,
                     UriTemplate = "GetModules?token={token}&moduleName={moduleName}"
        )]
        string GetModules(string token, string moduleName);
    }

}
