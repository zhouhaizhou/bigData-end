using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WcfSmcGridService.Model
{
    public class QuestionItem
    {
        //调查问卷表中的各列
        public string taskNumber { get; set; }
        public string question { get; set;}
        public string value { get; set; }
        public string startTime { get; set; }
        public string endTime { get; set; }
        public string createTime { get; set; }
        public string condition { get; set; }
        public int enable { get; set; }
    }
}