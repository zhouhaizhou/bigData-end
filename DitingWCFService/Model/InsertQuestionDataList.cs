using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WcfSmcGridService.Model
{
    public class InsertQuestionDataList
    {
        public List<Test> dataArr;
        public string advices;
        public string account;
    }
    public class Test
    {
       public int index { get; set; }
       public string value { get; set; }
    }
}