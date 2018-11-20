using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WcfSmcGridService.Model
{
    public class ParentRoleAuth
    {
        public string moduleId { get; set; }
        public string moduleNameFlag { get; set; }
        public string moduleName { get; set; }
        public string parentModuleId { get; set; }
        public List<ParentRoleAuth> children { get; set; }
    }
    public class SaveAuth {
        public string moduleId { get; set; }
    }
}