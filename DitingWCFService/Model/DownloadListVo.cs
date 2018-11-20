using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WcfSmcGridService.Model
{
    public class DownloadListVo
    {
        public string userName;
        public string downTime;
        public string updateInterValue;
        public string moduleEnName;
        public string date;
        public string province;
        public string provinceData;
        public string citySite;
        public string citySiteDetail;
        public string elementEn;
        public string elementCn;
        public string famat;
        public string timeInterval;
        public string insertTime;
        public int downState;
        public int isDown; //1下载，0加入下载清单
    }
}