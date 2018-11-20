using Newtonsoft.Json;
using Readearth.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using WcfSmcGridService.Model;

namespace WcfSmcGridService.BLL
{
    /// <summary>
    /// 获得首页数据
    /// </summary>
    public class HomeDataImpl
    {
        private Database home_DB = new Database("DBCONFIG116");
        /// <summary>
        /// 今日下载排行
        /// </summary>
        /// <param name="time">今天日期</param>
        /// <returns></returns>
        public DataTable GetDownloadRank(string time)
        {

            //如果日期不填写，则全局统计

            string strAllSQL = @"SELECT top 5 DataType  ,count(*) as downCount FROM T_DownloadRank group by DataType  order by downCount desc";
            return home_DB.GetDataTable(strAllSQL);

            ////根据当前的日期统计
            //time = TimeUtil.FormatCommonTime(time);
            //string strSQL = @"SELECT  DataType,count(*) as downCount FROM T_DownloadRank  where Date = '{0}' group by DataType  order by downCount desc";
            //strSQL = string.Format(strSQL, time);
            //return home_DB.GetDataTable(strSQL);

        }
        /// <summary>
        /// 动态资讯
        /// </summary>
        /// <returns></returns>
        public DataTable GetDynamicInfo()
        {
            ArrayList result = new ArrayList();
            string strSql = @"SELECT id ,Item ,CONVERT(varchar(100), Date, 23) as Date ,orderIndex FROM T_DynamicInfo order by date desc;";
            return home_DB.GetDataTable(strSql);
        }
        ///// <summary>
        /////获取用户反馈的信息插入数据库
        ///// </summary>
        ///// <returns></returns>
        //public string GetUserFormInfo(string title, string advice, string name, string phone, string email, string userName, string commiteTime)
        //{
        //    string strSql = @"insert into [dbo].[T_Form] ([title],[advice],[name],[phone],[email],[userName],[commiteTime]) values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}')";
        //    strSql = string.Format(strSql,title, advice, name, phone, email, userName, commiteTime);
        //    int count=home_DB.Execute(strSql);
        //    if (count > 0)
        //    {
        //        return "插入成功！";
        //    }
        //    else {
        //        return "插入失败！";
        //    }

        //}


        /// <summary>
        ///获取用户反馈的信息插入数据库
        /// </summary>
        /// <returns></returns>
        public string GetUserFormInfo(string funParams)
        {
            JsonSerializerSettings jSetting = new JsonSerializerSettings();
            jSetting.NullValueHandling = NullValueHandling.Ignore;
            GetUserFormInfoList getUserFormInfoList = JsonConvert.DeserializeObject<GetUserFormInfoList>(funParams, jSetting);//解析前台传过来的json字符串
            string strSql = @"insert into [dbo].[T_Form] ([title],[advice],[name],[phone],[email],[userName],[commiteTime]) values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}')";
            strSql = string.Format(strSql, getUserFormInfoList.title, getUserFormInfoList.advice, getUserFormInfoList.name, getUserFormInfoList.phone, getUserFormInfoList.email, getUserFormInfoList.userName, getUserFormInfoList.commiteTime);
            int count = home_DB.Execute(strSql);
            if (count > 0)
            {
                return "提交成功！";
            }
            else
            {
                return "提交失败！";
            }

        }
        /// <summary>
        /// 相关成果
        /// </summary>
        /// <returns></returns>
        public List<RelateResultsGroup> GetRelateResults() {
            List<RelateResultsGroup> result = new List<RelateResultsGroup>();
            
            //获取文章
            string strSql1 = @"select * from [dbo].[T_RelateResults] where type='article'";
            strSql1 = string.Format(strSql1);
            DataTable dt1 = home_DB.GetDataTable(strSql1);
            List<Articles> articleList = new List<Articles>();//定义一个list集合
            //遍历datatable，为list添加多条记录
            if (dt1.Rows.Count > 0) {
                for (int i = 0; i < dt1.Rows.Count;i++ ) {
                    DataRow dr = dt1.Rows[i] as DataRow;
                    Articles article = new Articles();//实例化对象，实例化list集合中的每一条
                    article.articleName = dr.IsNull("content") ? "" : Convert.ToString(dr["content"]);
                    article.articleUrl = dr.IsNull("url") ? "" : Convert.ToString(dr["url"]);
                    articleList.Add(article);//将实例化的对象记录一一添加到list集合中
                }
            }
            //获取专著
            string strSql2 = @"select * from [dbo].[T_RelateResults] where type='book'";
            strSql2 = string.Format(strSql2);
            DataTable dt2 = home_DB.GetDataTable(strSql2);
            List<Books> bookList = new List<Books>();
            if(dt2.Rows.Count>0){
                for (int i = 0; i < dt2.Rows.Count;i++ ) {
                    DataRow dr = dt2.Rows[i] as DataRow;
                    Books book = new Books();
                    book.bookName = dr.IsNull("content") ? "" : Convert.ToString(dr["content"]);
                    book.bookUrl = dr.IsNull("url") ? "" : Convert.ToString(dr["url"]);
                    bookList.Add(book);//将实例化的对象记录一一添加到list集合中
                }
            }
            //获取软件著作权
            string strSql3 = @"select * from [dbo].[T_RelateResults] where type='copyright'";
            strSql3 = string.Format(strSql3);
            DataTable dt3 = home_DB.GetDataTable(strSql3);
            List<Copyrights> copyrightList = new List<Copyrights>();
            if(dt3.Rows.Count>0){
                for (int i = 0; i < dt3.Rows.Count;i++ ) {
                    DataRow dr = dt3.Rows[i] as DataRow;
                    Copyrights copyright = new Copyrights();
                    copyright.copyrightName = dr.IsNull("content") ? "" : Convert.ToString(dr["content"]);
                    copyright.copyrightUrl = dr.IsNull("url") ? "" : Convert.ToString(dr["url"]);
                    copyrightList.Add(copyright);//将实例化的对象记录一一添加到list集合中
                }
            }
            RelateResultsGroup rrg = new RelateResultsGroup();
            rrg.articleLists = articleList;
            rrg.bookLists = bookList;
            rrg.copyrightLists = copyrightList;
            result.Add(rrg);

            return result;
        }

        /// <summary>
        /// 获取调查问卷的模板数据  业务逻辑层   向数据库中查询数据
        /// </summary>
        /// <returns></returns>
        public DataTable GetQuestionData(string starLevel)
        {
            string strSql = @"select [question],[value] from [dbo].[D_Questionnaire_Module] where startTime<CONVERT(varchar(100), GETDATE(), 20) and endTime>CONVERT(varchar(100), GETDATE(), 20) and condition>='"+starLevel+"' and enable='1' ";
            return home_DB.GetDataTable(strSql);
        }
        /// <summary>
        /// 是否显示调查问卷的模板数据
        /// </summary>
        /// <returns></returns>
        public DataTable ShowQuestionnaire(string starLevel,string account)
        {
            string strSql = @"select [question] from [dbo].[D_Questionnaire_Module] a left join [dbo].[T_Questionnaire_Submit] b on a.taskNumber=b.taskNumber where 
startTime<CONVERT(varchar(100), GETDATE(), 20) and endTime>CONVERT(varchar(100), GETDATE(), 20) and condition>='" + starLevel + "' and enable='1' and b.account='" + account + "' ";
            return home_DB.GetDataTable(strSql);
        }
        /// <summary>
        /// 开放统计
        /// </summary>
        /// <returns></returns>
        public DataTable GetOpenStat()
        {
            string strSql = @"SELECT id ,name ,dataSize ,CONVERT(varchar(100), date, 23) as date  FROM T_openStat where type='1' order by orderIndex asc";
            return home_DB.GetDataTable(strSql);
        }


        /// <summary>
        /// 调查问卷提交的数据
        /// </summary>
        /// <param name="submitData"></param>
        /// <returns></returns>
        public string InsertQuestionData(string submitData, string starLevel)
        {
            //解析前台传过来的json字符串
            JsonSerializerSettings jSetting = new JsonSerializerSettings();
            jSetting.NullValueHandling = NullValueHandling.Ignore;
            InsertQuestionDataList InsertQuestionDataList = JsonConvert.DeserializeObject<InsertQuestionDataList>(submitData, jSetting);//解析前台传过来的json字符串

            //利用抽象类获取解析的json字符串中的值
            List<Test> dataArr = InsertQuestionDataList.dataArr;
            string advices=InsertQuestionDataList.advices;
            string account = InsertQuestionDataList.account;

            //获取此次调查问卷的taskNumber
            string strSqlTaskNum = @" SELECT DISTINCT taskNumber FROM [EMBDShare].[dbo].[D_Questionnaire_Module] where 
startTime<CONVERT(varchar(100), GETDATE(), 20) and endTime>CONVERT(varchar(100), GETDATE(), 20) and condition>='" + starLevel + "' and enable='1'  ";
            string taskNum = home_DB.GetFirstValue(strSqlTaskNum);

            int count = 1;//
            //遍历数组，将数组中每个对象的相应值插入表中
            for (int i = 0; i < dataArr.Count; i++) {
                int index = dataArr[i].index;
                string value = dataArr[i].value;
                string strSql = @"insert into [dbo].[T_Questionnaire_Submit] ([taskNumber],[questionId],[value],[advices],[account]) values ({0},'{1}','{2}','{3}','{4}')";
                strSql = string.Format(strSql, taskNum,index, value, advices, account);
                count= home_DB.Execute(strSql);
            }
            if (count > 0)
            {
                return "提交成功！";
            }
            else
            {
                return "提交失败！";
            }


        }
    }
}