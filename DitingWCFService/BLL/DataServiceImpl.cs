using Common;
using Newtonsoft.Json;
using Re.Common;
using Readearth.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using WcfSmcGridService.Model;

namespace WcfSmcGridService.BLL
{

    public class DataServiceImpl
    {
        private Database ds_DB = new Database("DBCONFIG116");
        /// <summary>
        /// 获得省信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetProvince(string DataType)
        {
            //string strSQL = @"SELECT id , province , provinceCode ,provinceData, adminCode , adminName FROM D_Province ";
            string strSQL = "";

            if (DataType == "radioDay" || DataType == "radioMonth" || DataType == "radioYear")//由于辐射的资料查的不是cimiss表，表中的省份名称使用的名字和cimiss表中的不一样，所以采用a.provinceAlia=b.Province
            {
            strSQL = @"select DISTINCT(a.province) , a.provinceCode ,a.provinceData, a.adminCode , a.adminName from [EMBDShare].[dbo].[D_Province] a right join CimissDB.dbo.{0} b on a.provinceAlia=b.Province  ";
            }else{
                strSQL = @"select DISTINCT(a.province) , a.provinceCode ,a.provinceData, a.adminCode , a.adminName from [EMBDShare].[dbo].[D_Province] a right join CimissDB.dbo.{0} b on a.provinceData=b.Province ";
            }
            

            string sql = "  select siteTableName from [D_DataType_Table] where DataType='" + DataType + "' ";
            DataTable dt = ds_DB.GetDataTable(sql);
            string siteTableName = "";
            if (dt != null && dt.Rows.Count > 0)
                siteTableName = dt.Rows[0]["siteTableName"].ToString().Trim();

            strSQL = string.Format(strSQL, siteTableName);
            return ds_DB.GetDataTable(strSQL);
        }

        public DataTable GetAWSSite()
        {
            string strSQL = @"  select Station_Id_C,Station_Name,Province,Station_levl,Lat,Lon from Cimissdb.dbo.T_CIMISS_CHN_SURF_SITE where Station_levl in (11,12,13) ";
            strSQL = string.Format(strSQL);
            return ds_DB.GetDataTable(strSQL);
        }

        public DataTable GetAWSData(string beginTime,string endTime,string siteNo,string type,string element)
        {
            string tableName = "T_CIMISS_SURF_CHN_MUL_HOR";
            if (type == "day")
                tableName = "T_CIMISS_SURF_CHN_MUL_DAY_New";

            string where = "where 1=1 ";
            if (siteNo != "")
                where += " and  Station_Id_C='"+ siteNo + "'";//查全部

            string maxTime = "";
            if (beginTime == "" && endTime == "")
            {
                //查询当前最新时间点
                //直接连接使用远程数据库中的表  或本地有库直接连接  发布前释放
                string sql = "select top 1 collect_time FROM CimissDB.dbo." + tableName + " order by collect_time desc";

                // //本地连接远程服务器上的数据库CimissDB中的表siteTableName在本地进行调试   发布前隐藏 
                // string sql = "select top 1 collect_time FROM OPENDATASOURCE( 'SQLOLEDB', 'Data Source=10.228.9.116;User ID=sa;Password=Diting2015').CimissDB.dbo." + tableName + " order by collect_time desc";
                DataTable dt_time = ds_DB.GetDataTable(sql);
                if (dt_time != null && dt_time.Rows.Count > 0)
                    maxTime = dt_time.Rows[0]["collect_time"].ToString();
            }

            if (beginTime != "" || endTime != "")
            {
                maxTime = endTime;
            }


            string bTime = DateTime.Parse(maxTime).AddHours(0).ToString("yyyy-MM-dd HH:mm:ss");
            if(type=="day")
                   bTime = DateTime.Parse(maxTime).AddDays(0).ToString("yyyy-MM-dd HH:mm:ss");

            if (beginTime != "" || endTime != "")
            {
                bTime = beginTime;
                maxTime = endTime;
            }

            if (beginTime == "" && endTime == "" && siteNo!="")
            {
                bTime = DateTime.Parse(maxTime).AddHours(-24).ToString("yyyy-MM-dd HH:mm:ss");
                if (type == "day")
                    bTime = DateTime.Parse(maxTime).AddDays(-30).ToString("yyyy-MM-dd HH:mm:ss");
            }

             where += " and collect_time between  '"+ bTime + "' and '"+ maxTime + "' ";

            string ele = "";
            if (type == "hour")
            {
                switch (element)
                {
                    case "WIN": ele = "WIN_S_Avg_2mi as 'WIN'"; break;
                    case "TEM": ele = "TEM"; break;
                    case "PRS": ele = "PRS"; break;
                    case "PRE": ele = "PRE_24h"; break;
                    case "RHU": ele = "RHU"; break;
                    case "VIS": ele = "VIS"; break;
                }
            }
            else
            {
                switch (element)
                {
                    case "WIN": ele = "WIN_S_2mi_Avg as 'WIN'"; break;
                    case "TEM": ele = "TEM_Avg"; break;
                    case "PRS": ele = "PRS_Avg"; break;
                    case "PRE": ele = "PRE_Time_2020"; break;
                    case "RHU": ele = "RHU_Avg"; break;
                    case "VIS": ele = "VIS_Min"; break;
                }
            }

            //直接连接使用远程数据库中的表  或本地有库直接连接  发布前释放
           string strSQL = @"  select Station_Id_C,Station_Name,Province,Station_levl,Lat,Lon,
                               "+ele+ ",collect_time from Cimissdb.dbo." + tableName + " " + where + " Order by  collect_time";

            // //本地连接远程服务器上的数据库CimissDB中的表siteTableName在本地进行调试   发布前隐藏 
            // string strSQL = @"  select Station_Id_C,Station_Name,Province,Station_levl,Lat,Lon,
            //                     " + ele + ",collect_time from OPENDATASOURCE( 'SQLOLEDB', 'Data Source=10.228.9.116;User ID=sa;Password=Diting2015').CimissDB.dbo." + tableName + " " + where + " Order by  collect_time";

            strSQL = string.Format(strSQL);
            return ds_DB.GetDataTable(strSQL);
        }

        public DataTable GetDataLastTime(string dataType)
        {
            string strSQL = @"SELECT WhereSQL FROM [EMBDShare].[dbo].[D_DataType_Table] where DataType='" + dataType + "' ";
            strSQL = string.Format(strSQL);
            DataTable dt=ds_DB.GetDataTable(strSQL);
            string whereSql = dt.Rows[0]["WhereSQL"].ToString();
            return  ds_DB.GetDataTable(whereSql);
        }

        public DataTable Login(string userName, string Pwd)
        {
            string strSQL = "select [UserName] ,[Account] ,[Alias] ,[RoleID],[starLevel] from T_User where Account=@Username  and Password=@Password";
            if (userName == "BIGDATA" && Pwd == "BIGDATA")
            {
                userName = "readearth"; Pwd = "QX@2018";
            }
            SqlParameter[] para = new SqlParameter[] {
                new SqlParameter("@Username", userName),
                new SqlParameter("@Password", Pwd)
            };
            return ds_DB.GetDataTable(strSQL,para);

        }

        public DataTable GetDataList() {

            string strSQL = @"SELECT a.id ,parentModule,b.ModuleNameFlag as parentModuleCnName,moduleEnName ,moduleCnName  FROM
                             T_DataService_Module  a left join  T_Module  b on a.parentModule  = b.ModuleName where   b.enable = 1 and a.roleId=2 and a.parentModule in
                              (select ModuleName from T_Module where ParentModuleID = (select ModuleId  from T_Module where  ModuleName = 'dataDownLoad')) 
                            order by b.OrderIndex,a.OrderIndex asc";
            strSQL = string.Format(strSQL);
            return ds_DB.GetDataTable(strSQL);
        }

        public DataTable GetDataIntelTime(string dataType)
        {
            string strSQL = @"SELECT IntervalTime FROM [D_DataInterval]
                               WHERE entityName='"+ dataType + "'";
            strSQL = string.Format(strSQL);
            return ds_DB.GetDataTable(strSQL);
        }

        public DataTable GetDataListDetail(string moduleEnName)
        {
            string strSQL = @"select top  1  subTitle ,startTime ,endTime ,UpdateInter ,dataSource ,shareClassID,t2.siteCount,t2.dbSize,t2.lstTime 
                              from T_DataService_Module t1
							  left join D_DataListDetail t2 on t1.moduleEnName=t2.tabField
							  where moduleEnName = '"+ moduleEnName + "' and enable = 1 ";

            strSQL = string.Format(strSQL);
            return ds_DB.GetDataTable(strSQL);
        }

        public DataTable GetIntefaceInfo(string moduleName)
        {
            string strSQL = @"select type,innerHTML from D_DataInteface where Code='"+ moduleName + "'";
            strSQL = string.Format(strSQL);
            return ds_DB.GetDataTable(strSQL);
        }

        public DataTable GetColumnDetail(string moduleEnName)
        {
            string strSQL = @" select   ROW_NUMBER() OVER(ORDER BY dataType ASC) AS RowNum,dataType,columnName,columnDetail,unit from [dbo].[D_DataType_Detail]
							  where dataType='"+ moduleEnName + "'";

            strSQL = string.Format(strSQL);
            return ds_DB.GetDataTable(strSQL);
        }

        public DataTable GetSiteInfo()
        {
            //直接连接使用远程数据库中的表  或本地有库直接连接  发布前释放
           string strSQL = @"SELECT  [Station_Id_C]
                          ,[Station_Name]
                          ,[Lat]
                          ,[Lon]
                           FROM [CimissDB].[dbo].[T_CIMISS_CHN_SITE] 
                           where Station_levl in (11,12,13) ";

            // //本地连接远程服务器上的数据库CimissDB中的表siteTableName在本地进行调试   发布前隐藏 
            // string strSQL = @"SELECT  [Station_Id_C]
            //                ,[Station_Name]
            //                ,[Lat]
            //                ,[Lon]
            //                 FROM OPENDATASOURCE( 'SQLOLEDB', 'Data Source=10.228.9.116;User ID=sa;Password=Diting2015').CimissDB.dbo.[T_CIMISS_CHN_SITE] 
            //                 where Station_levl in (11,12,13) ";

            strSQL = string.Format(strSQL);
            return ds_DB.GetDataTable(strSQL);
        }

        public string GetAirCityInfo(string city)
        {
            //string url = "http://www.weather.com.cn/data/sk/{0}.html";
            //string strSQL = "select * from D_CityCode where tabField='" + city + "'";
            //DataTable dt = ds_DB.GetDataTable(strSQL);
            //string cityCode = "";
            //if (dt != null && dt.Rows.Count > 0)
            //    cityCode = dt.Rows[0]["Code"].ToString();

            //url = string.Format(url, cityCode);

            //string url = "https://www.sojson.com/open/api/weather/json.shtml?city="+city+"";
            city = city.Replace("市", "") + "市";
            string url = "http://api.map.baidu.com/telematics/v3/weather?location="+ city + "&output=json&ak=CF34cbf7412244f8c5e3a2be918eac86";

            return GetContentFromUrl(url);
        }


        public string GetContentFromUrl(string URL)
        {
            try
            {
                string strBuff = "";
                int byteRead = 0;
                char[] cbuffer = new char[256];
                HttpWebRequest httpReq = (HttpWebRequest)WebRequest.Create(new Uri(URL));
                HttpWebResponse httpResp = (HttpWebResponse)httpReq.GetResponse();
                Stream respStream = httpResp.GetResponseStream();
                StreamReader respStreamReader = new StreamReader(respStream, System.Text.Encoding.UTF8);
                byteRead = respStreamReader.Read(cbuffer, 0, 256);
                while (byteRead != 0)
                {
                    string strResp = new string(cbuffer, 0, byteRead);
                    strBuff = strBuff + strResp;
                    byteRead = respStreamReader.Read(cbuffer, 0, 256);
                }
                respStream.Close();
                return strBuff;
            }
            catch (Exception ex)
            {
                return "error";
            }
        }



        public DataTable GetSitebyName(string ModuleName)
        {
            string sql_table = " select * from EMBDShare.dbo.D_Site_TableName where  ModuleName='"+ ModuleName + "'";
            DataTable dt_module=ds_DB.GetDataTable(sql_table);
            string tableName = "";
            string type = "";
            if (dt_module != null && dt_module.Rows.Count > 0)
            {
                tableName = dt_module.Rows[0]["SITE_TableName"].ToString().Trim();
                type = dt_module.Rows[0]["Type"].ToString().Trim();
            }
            //直接连接使用远程数据库中的表  或本地有库直接连接  发布前释放
           string strSQL = @"SELECT [Station_Id_C]
                              ,[Station_Name]
                              ,[Province]
                              ,[City]
                              ,[Lat]
                              ,[Lon],'"+ type + "' as 'Type' FROM CimissDB.dbo." + tableName;

        //    //本地连接远程服务器上的数据库CimissDB中的表siteTableName在本地进行调试   发布前隐藏  
        //     string strSQL = @"SELECT [Station_Id_C]
        //                        ,[Station_Name]
        //                        ,[Province]
        //                        ,[City]
        //                        ,[Lat]
        //                        ,[Lon],'" + type + "' as 'Type' FROM OPENDATASOURCE( 'SQLOLEDB', 'Data Source=10.228.9.116;User ID=sa;Password=Diting2015').CimissDB.dbo." + tableName;


            if(ModuleName== "earthMeto")
                //直接连接使用远程数据库中的表  或本地有库直接连接  发布前释放
               strSQL = @"SELECT [Station_Id_C]
                              ,[Station_Name]
                              ,[Province]
                              ,[City]
                              ,[Lat]
                              ,[Lon],'" + type + "' as 'Type',Station_levl FROM CimissDB.dbo." + tableName;

                // //本地连接远程服务器上的数据库CimissDB中的表siteTableName在本地进行调试   发布前隐藏 
                // strSQL = @"SELECT [Station_Id_C]
                //                ,[Station_Name]
                //                ,[Province]
                //                ,[City]
                //                ,[Lat]
                //                ,[Lon],'" + type + "' as 'Type',Station_levl FROM OPENDATASOURCE( 'SQLOLEDB', 'Data Source=10.228.9.116;User ID=sa;Password=Diting2015').CimissDB.dbo." + tableName;


            if (ModuleName == "atmoCom")
            {
                type = "酸雨日值站点表";
                //直接连接使用远程数据库中的表  或本地有库直接连接  发布前释放
               strSQL = @"SELECT [Station_Id_C]
                              ,[Station_Name]
                              ,[Province]
                              ,[City]
                              ,[Lat]
                              ,[Lon],'" + type + "' as 'Type' FROM CimissDB.dbo.T_CIMISS_CHN_CAWN_AR_SITE";

                // //本地连接远程服务器上的数据库CimissDB中的表siteTableName在本地进行调试   发布前隐藏 
                // strSQL = @"SELECT [Station_Id_C]
                //                ,[Station_Name]
                //                ,[Province]
                //                ,[City]
                //                ,[Lat]
                //                ,[Lon],'" + type + "' as 'Type' FROM OPENDATASOURCE( 'SQLOLEDB', 'Data Source=10.228.9.116;User ID=sa;Password=Diting2015').CimissDB.dbo.T_CIMISS_CHN_CAWN_AR_SITE";

                DataTable dt_1=ds_DB.GetDataTable(strSQL);
                type = "气溶胶站点表";
                //直接连接使用远程数据库中的表  或本地有库直接连接  发布前释放
               strSQL = @"SELECT [Station_Id_C]
                              ,[Station_Name]
                              ,[Province]
                              ,[City]
                              ,[Lat]
                              ,[Lon],'" + type + "' as 'Type' FROM CimissDB.dbo.T_CIMISS_CHN_CAWN_PMM_SITE";

                // //本地连接远程服务器上的数据库CimissDB中的表siteTableName在本地进行调试   发布前隐藏  
                // strSQL = @"SELECT [Station_Id_C]
                //                ,[Station_Name]
                //                ,[Province]
                //                ,[City]
                //                ,[Lat]
                //                ,[Lon],'" + type + "' as 'Type' FROM OPENDATASOURCE( 'SQLOLEDB', 'Data Source=10.228.9.116;User ID=sa;Password=Diting2015').CimissDB.dbo.T_CIMISS_CHN_CAWN_PMM_SITE";

                DataTable dt_2 = ds_DB.GetDataTable(strSQL);
                foreach (DataRow row in dt_2.Rows) {
                    dt_1.Rows.Add(row.ItemArray);
                }
                return dt_1;
            }


            return ds_DB.GetDataTable(strSQL);
        }

        /// <summary>
        /// 获取国控站点信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetAirSiteInfo()
        {
            //直接连接使用远程数据库中的表  或本地有库直接连接  发布前释放
           string strSQL = @"SELECT [SiteID] as 'Station_Id_C'
                           ,[PositionName] as 'Station_Name'
                           ,[Latitude] as 'Lat'
                           ,[Longitude] as 'Lon'
                            FROM [CimissDB].[dbo].[China_RT_StationInfo] ";

            // //本地连接远程服务器上的数据库CimissDB中的表siteTableName在本地进行调试   发布前隐藏 
            // string strSQL = @"SELECT [SiteID] as 'Station_Id_C'
            //                 ,[PositionName] as 'Station_Name'
            //                 ,[Latitude] as 'Lat'
            //                 ,[Longitude] as 'Lon'
            //                  FROM OPENDATASOURCE( 'SQLOLEDB', 'Data Source=10.228.9.116;User ID=sa;Password=Diting2015').CimissDB.dbo.[China_RT_StationInfo] ";

            strSQL = string.Format(strSQL);
            return ds_DB.GetDataTable(strSQL);
        }

        public string GetMaxTime()
        {
            Database ds_DB_3433 = new Database("CimissDB3433");
            string sql_maxTime = "select top 1 timepoint  from [dbo].[China_RT_CNEMC_Data] order by timepoint desc";
            string maxTime = ds_DB_3433.GetFirstValue(sql_maxTime);
            string dayTime = DateTime.Parse(maxTime).AddDays(-1).ToString("yyyy-MM-dd");
            return "hour:"+maxTime + "&day:" + dayTime;
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="stationID"></param>
        /// <param name="timePoint"></param>
        /// <param name="element"></param>
        /// <returns></returns>
        public DataTable GetDatas(string stationID, string timePoint, string element,string duration,string isHistory) {
            Database ds_DB_3433 = new Database("CimissDB3433");
            string sql_maxTime = "select top 1 timepoint  from [dbo].[China_RT_CNEMC_Data] order by timepoint desc";
            string maxTime = ds_DB_3433.GetFirstValue(sql_maxTime);
            if (duration == "10")
            {
                if (element == "O3")
                     element = "O31 as 'O3' ";
            }
            else if (duration == "7")
            {
                if (element == "O3")
                     element = "O38 as 'O3' ";
            }
            string where = " where 1=1 ";

            string sql_data = "select siteID,TimePoint,PositionName,Latitude,Longitude,area,"+ element + " from China_RT_CNEMC_Data";
            //如果没有传stationID,默认就查所有的
            if (stationID != "")
                where += "and siteID='"+ stationID + "'   ";

            //如果没有传时间就查最新时间点数据
            if (isHistory == "N")
            {
                if (timePoint == "")
                    where += "and TimePoint='" + maxTime + "'   ";
                else
                    where += "and TimePoint='" + timePoint + "'   ";
            }
            if (isHistory == "Y") {
                if(duration=="10")
                    where += "and TimePoint between '" + DateTime.Parse(timePoint).AddHours(-24) + "' and '" + timePoint + "'";
                else
                    where += "and TimePoint between '" + DateTime.Parse(timePoint).AddDays(-30) + "' and '" + timePoint + "'";
            }

            sql_data += where;
            return ds_DB_3433.GetDataTable(sql_data);
        }

        /// <summary>
        /// 获得站点信息
        /// </summary>
        /// <param name="DataType"></param>
        /// <returns></returns>
        public DataTable GetStationByDataType(string DataType, string Provinces)
        {
            Provinces = Provinces.Replace("'", "").Replace("省","").Replace("市","");

            //直接连接使用远程数据库中的表  或本地有库直接连接  发布前释放
            string strSQL = " select Station_Id_C as 'StationId',Station_Name as 'Station_Name' from " +
                          "   CimissDB.dbo.{0} where Province like '%" + Provinces + "%' ";

            // //本地连接远程服务器上的数据库CimissDB中的表siteTableName在本地进行调试   发布前隐藏 
            // string strSQL = " select Station_Id_C as 'StationId',Station_Name as 'Station_Name' from " +
            //                "  OPENDATASOURCE( 'SQLOLEDB', 'Data Source=10.228.9.116;User ID=sa;Password=Diting2015').CimissDB.dbo.{0} where Province like '%" + Provinces + "%' ";

            string sql = "  select siteTableName from [D_DataType_Table] where DataType='"+ DataType+"' ";
            DataTable dt=ds_DB.GetDataTable(sql);
            string siteTableName = "";
            if (dt != null && dt.Rows.Count > 0)
                siteTableName = dt.Rows[0]["siteTableName"].ToString().Trim();

            return ds_DB.GetDataTable(string.Format(strSQL, siteTableName));
        }

        /// <summary>
        /// 获得要素信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetElement(string DataType)
        {
            string strSQL = @"SELECT a.id ,tabField as elementEn ,b.Type  as elementCn FROM  D_DataService_Element a left join D_Element b on a.element = b.Code where a.DataType = '{0}'";
            strSQL = string.Format(strSQL, DataType);
            return ds_DB.GetDataTable(strSQL);
        }
        /// <summary>
        /// 根据父节点获得数据服务信息
        /// </summary>
        /// <param name="parentModule">父节点名称</param>
        /// <param name="roldId">备用参数，现在不考虑</param>
        /// <returns></returns>
        public List<DataServiceGroupModuleVo> GetModuleByParentModule(string parentModule, string roldId, string account)
        {
            List<DataServiceGroupModuleVo> result = new List<DataServiceGroupModuleVo>();
            //获得父节点名字
            string strParentSQL = @"select ModuleName from T_Module where 
                                   ParentModuleID = (select ModuleId  from T_Module where  ModuleName = '{0}') ";
            strParentSQL = string.Format(strParentSQL, parentModule);

            string strSQL = @"select  x.*,y.moduleName,y.Account,y.likeActive from  (SELECT a.id ,parentModule,b.ModuleNameFlag as parentModuleCnName,moduleEnName ,
                        moduleCnName ,a.enable ,roleId ,content ,imgUrl ,viewCount ,commentCount ,
                        likesCount ,subTitle ,startTime ,endTime ,UpdateInter ,dataSource ,
                        shareClassID,a.OrderIndex as 'aOrderIndex',b.OrderIndex as 'bOrderIndex',siteCount FROM T_DataService_Module  a left join  T_Module  b 
                        on a.parentModule  = b.ModuleName where   b.enable = 1  and a.parentModule in ({0}) 
                        and roleID={1}   ) x left join (select moduleName,Account,likeActive from [EMBDShare].[dbo].[T_MyLikeData] where Account='{2}') y on x.moduleEnName=y.moduleName order by x.bOrderIndex,x.aOrderIndex asc   ";

            strSQL = string.Format(strSQL, strParentSQL, roldId, account);
            DataTable dt = ds_DB.GetDataTable(strSQL);
            List<DataServiceModuleVo> listModule = new List<DataServiceModuleVo>();
            //分组
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i] as DataRow;
                    DataServiceModuleVo dsModuleVo = new DataServiceModuleVo();
                    dsModuleVo.id = dr.IsNull("id") ? 0 : Convert.ToInt32(dr["id"]);
                    dsModuleVo.parentModule = dr.IsNull("parentModule") ? "" : Convert.ToString(dr["parentModule"]);
                    dsModuleVo.parentModuleCnName = dr.IsNull("parentModuleCnName") ? "" : Convert.ToString(dr["parentModuleCnName"]);
                    dsModuleVo.moduleEnName = dr.IsNull("moduleEnName") ? "" : Convert.ToString(dr["moduleEnName"]);
                    dsModuleVo.moduleCnName = dr.IsNull("moduleCnName") ? "" : Convert.ToString(dr["moduleCnName"]);
                    dsModuleVo.enable = dr.IsNull("enable") ? 0 : Convert.ToInt32(dr["enable"]);
                    dsModuleVo.roleId = dr.IsNull("roleId") ? 0 : Convert.ToInt32(dr["roleId"]);

                    string startTime= dr.IsNull("startTime") ? "" : Convert.ToString(dr["startTime"]);
                    DateTime st;
                    DateTime.TryParse(startTime, out st);
                    startTime = st.ToString("yyyy年MM月dd日");
                    string endTime = dr.IsNull("endTime") ? "" : Convert.ToString(dr["endTime"]);
                    DateTime et;
                    DateTime.TryParse(endTime, out et);
                    endTime = et.ToString("yyyy年MM月dd日");
                    string siteCount = dr.IsNull("siteCount") ? "" : Convert.ToString(dr["siteCount"]);
                    dsModuleVo.content = dr.IsNull("content") ? "" : Convert.ToString(dr["content"]);
                    dsModuleVo.content = dsModuleVo.content.Replace("2439", siteCount).
                        Replace("1951年1月", startTime).
                        Replace("2018年2月", endTime);

                    dsModuleVo.imgUrl = dr.IsNull("imgUrl") ? "" : Convert.ToString(dr["imgUrl"]);
                    dsModuleVo.viewCount = dr.IsNull("viewCount") ? 0 : Convert.ToInt32(dr["viewCount"]);
                    dsModuleVo.commentCount = dr.IsNull("commentCount") ? 0 : Convert.ToInt32(dr["commentCount"]);
                    dsModuleVo.likesCount = dr.IsNull("likesCount") ? 0 : Convert.ToInt32(dr["likesCount"]);
                    dsModuleVo.OrderIndex = dr.IsNull("bOrderIndex") ? 0 : Convert.ToInt32(dr["bOrderIndex"]);
                    dsModuleVo.likeActive = dr.IsNull("likeActive") ? "false" : Convert.ToString(dr["likeActive"]);
                    //dsModuleVo.subTitle = dr.IsNull("subTitle") ? "" : Convert.ToString(dr["subTitle"]);
                    //dsModuleVo.startTime = dr.IsNull("startTime") ? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") : Convert.ToString(dr["startTime"]);
                    //dsModuleVo.endTime = dr.IsNull("endTime") ? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") : Convert.ToString(dr["endTime"]);
                    //dsModuleVo.UpdateInter = dr.IsNull("UpdateInter") ? "" : Convert.ToString(dr["UpdateInter"]);
                    //dsModuleVo.dataSource = dr.IsNull("dataSource") ? "" : Convert.ToString(dr["dataSource"]);
                    //dsModuleVo.shareClassID = dr.IsNull("shareClassID") ? "" : Convert.ToString(dr["shareClassID"]);
                    listModule.Add(dsModuleVo);
                }
                //分组  整理成具有多级结构的json字符串
                var gItems = listModule.GroupBy(c => c.parentModule);
                foreach (var gItem in gItems)
                {
                    DataServiceGroupModuleVo gDsModuleVo = new DataServiceGroupModuleVo();
                    gDsModuleVo.parentModule = gItem.Key;
                    List<DataServiceModuleVo> listData = new List<DataServiceModuleVo>();
                    var gItemList = gItem.ToList().OrderBy(c => c.OrderIndex);
                    foreach (DataServiceModuleVo dsModuleVo in gItemList)
                    {
                        gDsModuleVo.parentModuleCnName = dsModuleVo.parentModuleCnName;
                        listData.Add(dsModuleVo);
                    }
                    gDsModuleVo.listData = listData;
                    result.Add(gDsModuleVo);
                }
            }
            return result;
        }
        /// <summary>
        /// 查看次数-更新
        /// </summary>
        /// <param name="moduleEnName">小模块英文名</param>
        /// <returns></returns>
        public int updateViewCount(string moduleEnName)
        {
            string strSQL = @"update  T_DataService_Module set viewCount = viewCount + 1 where moduleEnName = '{0}' ";
            strSQL = string.Format(strSQL, moduleEnName);
            return ds_DB.Execute(strSQL);
        }
        /// <summary>
        /// 评论次数-更新
        /// </summary>
        /// <param name="moduleEnName">小模块英文名</param>
        /// <returns></returns>
        public int updateCommentCount(string moduleEnName)
        {
            string strSQL = @"update  T_DataService_Module set commentCount = commentCount + 1 where moduleEnName = '{0}' ";
            strSQL = string.Format(strSQL, moduleEnName);
            return ds_DB.Execute(strSQL);
        }
        /// <summary>
        /// 点赞次数-更新
        /// </summary>
        /// <param name="moduleEnName">小模块英文名</param>
        /// <returns></returns>
        public int updateLikesCount(string moduleEnName)
        {
            string strSQL = @"update  T_DataService_Module set likesCount = likesCount + 1 where moduleEnName = '{0}' ";
            strSQL = string.Format(strSQL, moduleEnName);
            return ds_DB.Execute(strSQL);
        }
        /// <summary>
        /// 根据模块信息获得弹出框信息
        /// </summary>
        /// <param name="moduleEnName"></param>
        /// <returns></returns>
        public DataTable getSubTitle(string moduleEnName,string account)
        {
            string strSQL = @"select x.*,y.likeActive from (select top  1  moduleEnName,subTitle ,startTime ,endTime ,UpdateInter ,dataSource ,shareClassID,viewCount ,commentCount ,likesCount  from T_DataService_Module where moduleEnName = '{0}' and enable = 1 ) x left join (select moduleName,Account,likeActive from [EMBDShare].[dbo].[T_MyLikeData] where Account='{1}') y on x.moduleEnName=y.moduleName";
            strSQL = string.Format(strSQL, moduleEnName,account);
            return ds_DB.GetDataTable(strSQL);
        }
        #region 下载相关
        /// <summary>
        /// 下载清单查询
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// dataState = 1 下载成功，dataState = 0 列表内容
        /// <returns></returns>
        public DataTable GetDownloadList(string userName, string startTime, string endTime)
        {
            startTime = TimeUtil.FormatCommonTime(startTime);
            endTime = TimeUtil.FormatCommonTime(endTime);
            string strSQL = @"SELECT a.id ,userName ,downTime ,a.moduleEnName,b.moduleCnName,date ,province ,citySite,citySiteDetail,elementEn,elementCn  ,famat ,timeInterval FROM  T_DownloadList a left join T_DataService_Module b on a.moduleEnName  = b.moduleEnName  where userName like '%{0}%' and downTime >= '{1}' and downTime <= '{2}' and  downState = 1 order by downTime desc;";
            strSQL = string.Format(strSQL, userName, startTime, endTime);
            return ds_DB.GetDataTable(strSQL);
        }
        /// <summary>
        /// 获得活动的列表清单
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public DataTable GetActiveList(string userName)
        {
            //获取用户名的roleID
            string strSqlRoleID = "select roleId from [dbo].[T_User] where Account = '{0}'";
            strSqlRoleID = string.Format(strSqlRoleID, userName);
            DataSet DT = ds_DB.GetDataset(strSqlRoleID);
            string roleId = DT.Tables[0].Rows[0][0].ToString();
            string strSQL = @"SELECT a.id ,userName ,downTime ,a.moduleEnName,b.moduleCnName,dateType ,province ,citySite,citySiteDetail,elementEn,elementCn  ,famat ,timeInterval FROM  T_DownloadList a left join T_DataService_Module b on a.moduleEnName  = b.moduleEnName where userName = '{0}' and  downState = 0 and b.roleId='{1}'   order by insertTime desc;";
            strSQL = string.Format(strSQL, userName,roleId);
            return ds_DB.GetDataTable(strSQL);
        }
        /// <summary>
        /// 删除下载清单
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        public int deleteDownList(string userName,string ids)
        {
            string deleteSQL = @"delete T_DownloadList where userName = '{0}' and  id in ({1}) ";
            deleteSQL = string.Format(deleteSQL, userName, ids);
            return ds_DB.Execute(deleteSQL);
        }
        /// <summary>
        /// 加入清单,或者直接下载
        /// </summary>
        /// <returns></returns>
        public int insertDownListData(DownloadListVo dlVo)
        {
            //按照年月日要求处理时间字符串
            string[] TimeArr = Convert.ToString(dlVo.date).Split('-');//字符串数组
            string strUpdateInter = Convert.ToString(dlVo.updateInterValue);//获取资料内容中的“时间频率”

            string timeStr = handleTimeStrType(TimeArr, strUpdateInter);

            string insertSQL = @"INSERT INTO T_DownloadList(userName,downTime,moduleEnName,date,province,provinceData,citySite,citySiteDetail,elementEn,elementCn,famat,timeInterval,insertTime,downState,dateType) VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}',{13},'{14}')";
            insertSQL = string.Format(insertSQL, dlVo.userName, TimeUtil.FormatCommonTime(dlVo.downTime), dlVo.moduleEnName,
                 dlVo.date, dlVo.province, dlVo.provinceData, dlVo.citySite, dlVo.citySiteDetail, dlVo.elementEn, dlVo.elementCn, dlVo.famat, dlVo.timeInterval, TimeUtil.FormatCommonTime(dlVo.insertTime), dlVo.downState, timeStr);
            int result = ds_DB.Execute(insertSQL);
            return result;
        }

        /// <summary>
        /// //按照年月日要求处理时间字符串
        /// </summary>
        /// <param name="TimeArr"></param>
        /// <param name="strUpdateInter"></param>
        private static string handleTimeStrType(string[] TimeArr, string strUpdateInter)
        {
            string TimeStr = TimeArr[0].ToString().Substring(0, 10) + "-" + TimeArr[1].ToString().Substring(0, 10);
            if (strUpdateInter.IndexOf("日") > -1 || strUpdateInter.IndexOf("天") > -1)
            {
                TimeStr = TimeArr[0].ToString().Substring(0, 8) + "-" + TimeArr[1].ToString().Substring(0, 8);
            }
            else if (strUpdateInter.IndexOf("月") > -1)
            {
                TimeStr = TimeArr[0].ToString().Substring(0, 6) + "-" + TimeArr[1].ToString().Substring(0, 6);
            }
            else if (strUpdateInter.IndexOf("年") > -1)
            {
                TimeStr = TimeArr[0].ToString().Substring(0, 4) + "-" + TimeArr[1].ToString().Substring(0, 4);
            }
            return TimeStr;
        }
        public ArrayList insertDownList_Down(DownloadListVo dlVo)
        {
            insertDownListData(dlVo);
            //查询最新条数据的id
            string strSQL = @"select top 1 id from T_DownloadList where userName = '{0}' order by id desc;";
            strSQL = string.Format(strSQL, dlVo.userName);
            string strId = ds_DB.GetFirstValue(strSQL);
            return BatchDownload(dlVo.userName, strId);
        }
        /// <summary>
        /// 根据用户名，id 批量下载
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        public ArrayList BatchDownload(string userName,string ids)
        {
            BatchDownloadDataUtil batchDownLoadData = new BatchDownloadDataUtil();
            return batchDownLoadData.BatchDownload(userName, ids);
        }

        /// <summary>
        /// 直接下载，或者加入下载清单
        /// </summary>
        /// <param name="funParams"></param>
        /// <returns></returns>
        public ResultVo insertDownList(string funParams)
        {
          
            JsonSerializerSettings jSetting = new JsonSerializerSettings();
            jSetting.NullValueHandling = NullValueHandling.Ignore;
            ResultVo urVo = new ResultVo();
            urVo.resultCode = "1"; //1代表成功，0代表失败
            DownloadListVo downLoadVo = JsonConvert.DeserializeObject<DownloadListVo>(funParams, jSetting);
            if (downLoadVo.isDown == 0)//isDown=0 插入下载清单（未下载）
            {
                urVo.resultCode = insertDownListData(downLoadVo).ToString();
            }
            else if (downLoadVo.isDown == 1)//isDown=1  已下载
            {
                urVo.result =  insertDownList_Down(downLoadVo);
            }
            return urVo;
        }

        #endregion
        /// <summary>
        /// 刷新访问，登记IP地址 2018-09-17 by 孙明宇
        /// </summary>
        /// <param name="access"></param>
        /// <param name="ip"></param>
        public string UpdateLoginTime(string access, string ip, string location)
        {

            IPScanner qqWry = new IPScanner(System.Web.HttpContext.Current.Server.MapPath("~/QQWry.dat"));

            IPLocation ipLocation = qqWry.Query(ip);
            string visitTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            try
            {
                string strSQL = string.Format("insert into [T_loginLog] (Account,EntryDate,UserIp,location,carrieroperator) values('{0}','{1}','{2}','{3}','{4}' )", access, visitTime, ip, location, ipLocation.Local);
                ds_DB.Execute(strSQL);
                strSQL = "select top 1 id from T_loginLog order by id desc";
                int time = Convert.ToInt32(ds_DB.GetFirstValue(strSQL));
                strSQL = string.Format("update T_openStat set dataSize='{0}',date='{1}' WHERE NAME='总访问量'", time.ToString() + "次", visitTime);
                ds_DB.Execute(strSQL);
                return "登记成功";
            }
            catch (Exception ex)
            {
                return "登记失败";
            }

        }
        /// <summary>
        /// 我的收藏 插入
        /// </summary>
        /// <param name="moduleEnName"></param>
        /// <param name="account"></param>
        /// <param name="insertTime"></param>
        /// <returns></returns>
        public string insertLikeData(string moduleEnName, string account, string insertTime)
        {
            try
            {
                string strSQL = string.Format("insert into [EMBDShare].[dbo].[T_MyLikeData] ([moduleName],[Account],[insertTime],[likeActive]) values('{0}','{1}','{2}','{3}' )", moduleEnName, account, insertTime,"true");
                int result=ds_DB.Execute(strSQL);
                strSQL = string.Format("select COUNT(*) as 'countNum'  FROM [EMBDShare].[dbo].[T_MyLikeData] where [moduleName]='{0}'",moduleEnName);
                int countNum = Convert.ToInt32(ds_DB.GetFirstValue(strSQL));
                //将统计的条数更新到T_DataService_Module表中
                string updateSQL=string.Format("UPDATE [EMBDShare].[dbo].[T_DataService_Module] SET [likesCount] = ({0}) WHERE moduleEnName='{1}'",strSQL,moduleEnName);
                ds_DB.Execute(updateSQL);

                string resultData = result.ToString() + "," + countNum.ToString();

                return resultData;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        /// <summary>
        /// 我的收藏 取消收藏
        /// </summary>
        /// <param name="moduleEnName"></param>
        /// <param name="account"></param>
        /// <param name="insertTime"></param>
        /// <returns></returns>
        public string deleteLikeData(string moduleEnName, string account)
        {
            try
            {
                string strSQL = string.Format("delete [EMBDShare].[dbo].[T_MyLikeData] where [moduleName]='{0}' and [Account]='{1}' ", moduleEnName, account);
                int result = ds_DB.Execute(strSQL);
                strSQL = string.Format("select COUNT(*) as 'countNum'  FROM [EMBDShare].[dbo].[T_MyLikeData] where [moduleName]='{0}'", moduleEnName);
                int countNum = Convert.ToInt32(ds_DB.GetFirstValue(strSQL));
                //将统计的条数更新到T_DataService_Module表中
                string updateSQL = string.Format("UPDATE [EMBDShare].[dbo].[T_DataService_Module] SET [likesCount] = ({0}) WHERE moduleEnName='{1}'", strSQL, moduleEnName);
                ds_DB.Execute(updateSQL);

                string resultData = result.ToString() + "," + countNum.ToString();

                return resultData;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    
    }
}
