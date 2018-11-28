using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WcfSmcGridService.SYS.BigData
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的接口名“IKaifangtongji”。
    [ServiceContract]
    public interface IKaifangtongji
    {
        [OperationContract]
        [WebInvoke(Method = "GET",
         RequestFormat = WebMessageFormat.Json,
         ResponseFormat = WebMessageFormat.Json,
         BodyStyle = WebMessageBodyStyle.Bare,
         UriTemplate = "aaa"
       )]
        string aaa();

        [OperationContract]
        [WebInvoke(Method = "GET",
         RequestFormat = WebMessageFormat.Json,
         ResponseFormat = WebMessageFormat.Json,
         BodyStyle = WebMessageBodyStyle.Bare,
         UriTemplate = "DataUpdateAnalysis?flagNum={flagNum}"
       )]
        string DataUpdateAnalysis(string flagNum);

        [OperationContract]
        [WebInvoke(Method = "GET",
         RequestFormat = WebMessageFormat.Json,
         ResponseFormat = WebMessageFormat.Json,
         BodyStyle = WebMessageBodyStyle.Bare,
         UriTemplate = "FileFormat"
       )]
        string FileFormat();

        [OperationContract]
        [WebInvoke(Method = "GET",
         RequestFormat = WebMessageFormat.Json,
         ResponseFormat = WebMessageFormat.Json,
         BodyStyle = WebMessageBodyStyle.Bare,
         UriTemplate = "UpdateNum"
       )]
        string UpdateNum();

        [OperationContract]
        [WebInvoke(Method = "GET",
         RequestFormat = WebMessageFormat.Json,
         ResponseFormat = WebMessageFormat.Json,
         BodyStyle = WebMessageBodyStyle.Bare,
         UriTemplate = "DownloadData"
       )]
        string DownloadData();

        [OperationContract]
        [WebInvoke(Method = "GET",
         RequestFormat = WebMessageFormat.Json,
         ResponseFormat = WebMessageFormat.Json,
         BodyStyle = WebMessageBodyStyle.Bare,
         UriTemplate = "HotDataDownload"
       )]
        string HotDataDownload();

        [OperationContract]
        [WebInvoke(Method = "GET",
         RequestFormat = WebMessageFormat.Json,
         ResponseFormat = WebMessageFormat.Json,
         BodyStyle = WebMessageBodyStyle.Bare,
         UriTemplate = "RingData"
       )]
        string RingData();


        [OperationContract]
        [WebInvoke(Method = "GET",
         RequestFormat = WebMessageFormat.Json,
         ResponseFormat = WebMessageFormat.Json,
         BodyStyle = WebMessageBodyStyle.Bare,
         UriTemplate = "CheckDataUpdate"
       )]
        string CheckDataUpdate();

        [OperationContract]
        [WebInvoke(Method = "GET",
         RequestFormat = WebMessageFormat.Json,
         ResponseFormat = WebMessageFormat.Json,
         BodyStyle = WebMessageBodyStyle.Bare,
         UriTemplate = "VisitCount"
       )]
        string VisitCount();

        [OperationContract]
        [WebInvoke(Method = "GET",
        RequestFormat = WebMessageFormat.Json,
        ResponseFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.Bare,
        UriTemplate = "DataUpdateDivNum"
      )]
        string DataUpdateDivNum();

    }
}
