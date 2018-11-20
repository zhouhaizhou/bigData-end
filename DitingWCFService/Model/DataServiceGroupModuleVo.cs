using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WcfSmcGridService.Model
{
    public class DataServiceGroupModuleVo
    {
        public string parentModule { get; set; }
        public string parentModuleCnName { get; set; }
        public List<DataServiceModuleVo> listData { get; set; }
    }
}