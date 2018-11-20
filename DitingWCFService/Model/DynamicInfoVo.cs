using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WcfSmcGridService.Model
{
    public class DynamicInfoVo
    {
        public int id;
        public string Item;
        public string Date;
        public int orderIndex;
    }
    public class SaveDynamicInfo {
        public string data;
        public string date;
        public string title;
    }
}