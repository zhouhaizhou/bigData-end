using Readearth.Data;
using Readearth.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;
using System.Web.Script.Serialization;
using WcfSmcGridService.Model;

namespace WcfSmcGridService.SYS.BigData
{
    
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“Kaifangtongji”。
    // 注意: 为了启动 WCF 测试客户端以测试此服务，请在解决方案资源管理器中选择 Kaifangtongji.svc 或 Kaifangtongji.svc.cs，然后开始调试。
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    
    //开放统计页面数据接口
    public class Kaifangtongji : IKaifangtongji
    {
       
        public string aaa() {
            return "aaaa";
        }

        /// <summary>
		/// 获取数据更新统计生成的div个数
        /// </summary>
        /// <returns></returns>
        public string DataUpdateDivNum()
        {
            try
            {
                Database DB = new Database("DBCONFIG116");
                string sql = "SELECT flag FROM [dbo].[T_DataAnalysis] WHERE flag is not Null GROUP BY flag ";
                DataTable dt = DB.GetDataTable(sql);
                int Num = dt.Rows.Count;
                return Num.ToString();
            }
            catch (Exception e)
            {

                return e.Message;
            }
        }

        /// <summary>
        /// 数据更新统计
        /// </summary>
        /// <returns></returns>
         public string DataUpdateAnalysis(string flagNum)
        {
            try
            {
                Database DB = new Database("DBCONFIG116");
                string sql = "SELECT * FROM [dbo].[T_DataAnalysis] WHERE flag="+flagNum+" ORDER BY date";
                string nameSql = "SELECT moduleCnName FROM [dbo].[T_DataAnalysis] WHERE flag=" + flagNum + " group by moduleCnName";
                DataTable dt = DB.GetDataTable(sql);
                DataTable dtName = DB.GetDataTable(nameSql);
                string resultNameDT = DtbTime2Json(dtName);                         
                string resultDT=DtbTime2Json(dt);
                
                string jsonString = string.Empty;
                jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(new {
                    dataValue = resultDT,
                    dataName = resultNameDT
                });
                return jsonString;
                //return DtbTime2Json(dt);
               

            }
            catch (Exception e)
            {

                return e.Message;
            }
           
        }
        /// <summary>
        /// 文件格式统计
        /// </summary>
        /// <returns></returns>
        public string FileFormat() {
            try
            {
                Database DB = new Database("DBCONFIG116");
                //string sql = string.Format("select COUNT([format]) as fcount,[format] from [dbo].[T_DownloadRank] where"
                    //+ "[format]is not NUll group by [format] order by [format]");
                string sql= "select COUNT([type]) as fcount,[type] as format from [dbo].[D_FileType] where[type] is not NUll group by [type] order by[type]";
                DataTable dt = DB.GetDataTable(sql);
                //加一列写百分比
                dt.Columns.Add("percent");
                double sum = Convert.ToInt32(dt.Compute("sum(fcount)", "TRUE"));
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["percent"] = Convert.ToInt32((Convert.ToInt32(dt.Rows[i]["fcount"]) / sum) * 100);
                }
                return DtbTime2Json(dt);
            }
            catch (Exception e)
            {

                return e.Message;
            }
           

        }
        /// <summary>
        /// 入库量、下载量
        /// </summary>
        /// <returns></returns>
        public string UpdateNum()
        {
            try
            {
                Database DB = new Database("DBCONFIG116");
                string sql = "SELECT [dataSize],[name] FROM [EMBDShare].[dbo].[T_OpenStat] WHERE [name]='总下载量' OR [name]='总入库量' OR [name]='总访问量' order by [name]";
                DataTable dt = DB.GetDataTable(sql);

                return DtbTime2Json(dt);
            }
            catch (Exception e)
            {

                return e.Message;
            }
            
        }

        /// <summary>
        /// 下载量排名
        /// </summary>
        /// <returns></returns>
        public string DownloadData()
        {
            try
            {
                Database DB = new Database("DBCONFIG116");
                string sql = string.Format("SELECT COUNT(b.Description) as countUserRoles,b.Description FROM"
                    + "[dbo].[T_DownloadRank] a LEFT JOIN [dbo].[T_Role] b ON a.roleId=b.Id "
                    + "WHERE b.Description is not NUll"
                    + " group by b.Description");
                DataTable dt = DB.GetDataTable(sql);
                return DtbTime2Json(dt);
            }
            catch (Exception e)
            {

                return e.Message;
            }
            
        }

        public string HotDataDownload() {
            try
            {
                Database DB = new Database("DBCONFIG116");
                string sql = string.Format("SELECT top 5 count([module_En])as downloadCount,[DataType] FROM [EMBDShare].[dbo].[T_DownloadRank] "
                             + "group by[module_En],[DataType] order by downloadCount desc");
                DataTable dt = DB.GetDataTable(sql);
                return DtbTime2Json(dt);
            }
            catch (Exception e)
            {

                return e.Message;
            }
           
        }

        /// <summary>
        /// 数据评分统计
        /// </summary>
        /// <returns></returns>
        public string RingData() {
            try
            {
                Database DB = new Database("DBCONFIG116");
                string sql = string.Format("SELECT count(value)as value,[value] as nameId FROM [EMBDShare].[dbo].[T_Questionnaire_Submit]"
                    + "group by [value] order by nameId");
                //string sql = string.Format("SELECT count(value)as value,[value] as name FROM [EMBDShare].[dbo].[T_Questionnaire_Submit]"
                //    + "group by [value] order by name");
                DataTable dt = DB.GetDataTable(sql);
                                            
                return DtbTime2Json(dt);
            }
            catch (Exception e)
            {

                return e.Message;
            }
            
        }

        /// <summary>
        /// 数据更新检测
        /// </summary>
        /// <returns></returns>
        public string CheckDataUpdate()
        {
            try
            {
                DateTime NowTime = DateTime.Now;
                string startTime = NowTime.ToString("yyyy-MM-dd 00:00");
                string endTime = NowTime.ToString("yyyy-MM-dd 23:59");
                Database DB = new Database("DBCONFIG116");
                string sql = "select moduleCnName,date,ifUpdate from [T_DataUpdateMonitor] WHERE date Between '" + startTime + "' AND '" + endTime + "'";
                DataTable dt = DB.GetDataTable(sql);
                //string sqlDis = "select moduleCnName from [T_DataUpdateMonitor] group by moduleCnName";//查一下c#选一列中不同值？？
                string sqlDis = "select moduleCnName from [T_DataUpdateMonitor] group by moduleCnName";//查一下c#选一列中不同值？？
                DataTable DT_Dis = DB.GetDataTable(sqlDis);
                List<KaifangtongjiClass> result = new List<KaifangtongjiClass>();

                for (int i = 0; i < DT_Dis.Rows.Count; i++)
                {
                    string name = DT_Dis.Rows[i][0].ToString();
                    KaifangtongjiClass k = new KaifangtongjiClass();
                    k.name = name;
                    List<Data> listD = new List<Data>();
                    for (DateTime time = DateTime.Parse(startTime); time < DateTime.Parse(endTime); time = time.AddHours(2))
                    {
                        DataRow[] dr = dt.Select("moduleCnName='" + name + "' and date>='" + time + "' AND date <='" + time.AddHours(2) + "'");
                        string f = "";
                         for (int j = 0; j < dr.Length; j++)
                        {
                            if (dr[j]["ifUpdate"].ToString() == "1")
                            {
                                f = "1";
                            } else if (dr[j]["ifUpdate"].ToString() == "2") {
                                f = "2";
                            }
                        }
                       
                        Data d = new Data();
                        d.time = time.ToString("HH");
                        
                        d.val = f;
                        listD.Add(d);
                       
                    }
                    Data cs = new Data();
                    
                    k.data=listD;
                    result.Add(k);

                }
                return JsonHelper.ToJSON(result);
            }
            catch (Exception e)
            {

                return e.Message;
            }


        }

        /// <summary>
        /// 访问量统计
        /// </summary>
        /// <returns></returns>
        public string VisitCount()
        {
            Database DB = new Database("DBCONFIG116");
            try
            {
                string startMouthTime = DateTime.Now.ToString("yyyy/MM/01 00:00");
                string startDayTime = DateTime.Now.ToString("yyyy/MM/dd 00:00");
                string endTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm");
                string sqlMouth = "SELECT count(EntryDate) FROM [T_loginLog] WHERE EntryDate BETWEEN '" + startMouthTime + "' AND '" + endTime + "'";
                string sqlDay = "SELECT count(EntryDate) FROM [T_loginLog] WHERE EntryDate BETWEEN '" + startDayTime + "' AND '" + endTime + "'";
                DataTable dtM = DB.GetDataTable(sqlMouth);
                DataTable dtD = DB.GetDataTable(sqlDay);
                //string result = [{dtM[0] },{ }];
                

                string result = "[{ \"MonthVisit\":\""+dtM.Rows[0][0]+"\" },{\"DayVisit\":\""+dtD.Rows[0][0]+"\" }]";
                
                //返回两个值
                string resultJSON=JsonHelper.ToJSON(result);

                return result;
            }
            catch (Exception e)
            {

                return e.Message;
            }
           
        }

        public static string DtbTime2Json(DataTable dtb)
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            System.Collections.ArrayList dic = new System.Collections.ArrayList();
            foreach (DataRow dr in dtb.Rows)
            {
                System.Collections.Generic.Dictionary<string, object> drow = new System.Collections.Generic.Dictionary<string, object>();
                foreach (DataColumn dc in dtb.Columns)
                {
                    drow.Add(dc.ColumnName, dr[dc.ColumnName]);
                }
                dic.Add(drow);
            }
            //序列化
            string jsonStr = jss.Serialize(dic);
            jsonStr = System.Text.RegularExpressions.Regex.Replace(jsonStr, @"\\/Date\((\d+)\)\\/", match =>
            {
                DateTime dt = new DateTime(1970, 1, 1);
                dt = dt.AddMilliseconds(long.Parse(match.Groups[1].Value));
                dt = dt.ToLocalTime();
                return dt.ToString("yyyy-MM-dd HH:mm");
            });
            return jsonStr;
        }
            
        
    }
}
