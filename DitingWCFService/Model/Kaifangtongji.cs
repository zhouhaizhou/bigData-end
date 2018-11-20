using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WcfSmcGridService.Model
{
    public class KaifangtongjiClass
    {
        public string name { get; set; }
        public List<Data> data { get; set; }
        

    }
    public class Data {
        public string time { get; set; }
        public string val { get; set; }
    }

    
}