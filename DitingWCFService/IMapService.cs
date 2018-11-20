using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.IO;

using WcfSmcGridService.Model;

namespace WcfSmcGridService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IMapService" in both code and config file together.
    [ServiceContract]
    public interface IMapService
    {
        [OperationContract]
        [WebInvoke(Method = "GET",
          RequestFormat = WebMessageFormat.Json,
          ResponseFormat = WebMessageFormat.Json,
          BodyStyle = WebMessageBodyStyle.Bare,
          UriTemplate = "mapservice?label={label}"
        )]
        ModelMap GetTreeview(string labels);
    }
}
