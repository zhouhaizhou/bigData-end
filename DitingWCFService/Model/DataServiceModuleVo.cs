using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WcfSmcGridService.Model
{
    public class DataServiceModuleVo
    {
        //编号
        public int id { get; set; }
        //父节点英文名
        public string parentModule { get; set; }
        //父节点中文名
        public string parentModuleCnName { get; set; }
        //模块英文名
        public string moduleEnName { get; set; }
        //模块中文名
        public string moduleCnName { get; set; }
        //是否可用
        public int enable { get; set; }
        //权限ID
        public int roleId { get; set; }
        //鼠标悬浮内容
        public string content { get; set; }
        //图片路径
        public string imgUrl { get; set; }
        //查看次数
        public int viewCount { get; set; }
        //回复次数
        public int commentCount { get; set; }
        //点赞次数
        public int likesCount { get; set; }
        //排序
        public int OrderIndex { get; set; }
        //是否有收藏
        public string likeActive { get; set; }
        //弹出页面子标题
        //public string subTitle { get; set; }
        ////数据起始时间
        //public string startTime { get; set; }
        ////数据终止时间
        //public string endTime { get; set; }
        ////更新频率
        //public string UpdateInter { get; set; }
        ////数据源
        //public string dataSource { get; set; }
        ////共享级别
        //public string shareClassID { get; set; }

    }
}