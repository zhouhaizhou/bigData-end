using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WcfSmcGridService.Model
{
    public class RegisterItem
    {
        public string label { get; set; }
        public string infoKey { get; set; }
        public string value { get; set; }
        public string type { get; set; }
        public string ordex { get; set; }
        public string require { get; set; }
        public List<RegisterItemOptions> options { get; set; }
        public string sizeRestrict { get; set; }
        public string description { get; set; }
        public string accept { get; set; }
        public string regular { get; set; }
		public string infoType { get; set; }
        public string isEdit { get; set; }

    }
    public class RegisterItemOptions {
        public string DM { get; set; }
        public string code { get; set; }
        public string MC { get; set; }
    }
    public class RegisterItemFileName {
        public string key { get; set; }
        public string fileName { get; set; }
    }
}