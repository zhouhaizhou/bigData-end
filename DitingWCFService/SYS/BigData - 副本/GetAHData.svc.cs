using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;

namespace WcfSmcGridService.SYS.BigData
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“GetAHData”。
    // 注意: 为了启动 WCF 测试客户端以测试此服务，请在解决方案资源管理器中选择 GetAHData.svc 或 GetAHData.svc.cs，然后开始调试。
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]

    public class GetAHData : IGetAHData
    {
        public string GetAHDataAVG()
        {

            return "0";
        }
    }
}
