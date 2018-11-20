using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;
using Readearth.Data;
using System.Data;
using System.Linq.Expressions;
using System.Web;
using System.Collections.Specialized;

namespace WcfSmcGridService.SYS.BigData
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“LoadMenuService”。
    // 注意: 为了启动 WCF 测试客户端以测试此服务，请在解决方案资源管理器中选择 LoadMenuService.svc 或 LoadMenuService.svc.cs，然后开始调试。
    public class GetImageProducts : IGetImageProducts
    {


        //private string GetType(string type) {
        //    string t = "";
        //    switch (type) {
        //        case "": t = ""; break;
        //        case "": t = ""; break;
        //        case "": t = ""; break;
        //        case "": t = ""; break;
        //        case "": t = ""; break;
        //        case "": t = ""; break;
        //    }
        //    return t;
        //}


        string IGetImageProducts.GetImageProducts(string EntityName, string Station, string type,
            string bTime, string eTime, string interTime)
        {
            try
            {
                //type = GetType(type);
                EntityName = EntityName.ToLower();
                Database DB = new Database("DBCONFIG116");
                string sql_entity = "select condition,TableName,OperatorType from T_Entity  where EntityName='" + EntityName + "'";
                string sql_layer = "select Code from [dbo].[D_Mi_Layer] where DM='{0}'";
                string sql_type = "select * from [dbo].[D_Mi_Type] where DP='{0}'";
                string sql_typeII = "select * from [dbo].[D_Mi_Type] where DP like '%{0}%' order by dm";
                string sql_station = "select * from [dbo].[D_Mi_Station] where DP is null";
                string sql_product = "select * from {0} where {1}  and isNULL(Station,'')='{2}' and isnull(Type,'')='{3}' and ForecastDate between '{4}' and '{5}' order by ForecastDate asc";
                string sql_product_max = "select max(forecastDate) as 'forecastDate' from {0} where {1}  and isNULL(Station,'')='{2}' and isNull(Type,'')='{3}' order by ForecastDate asc";

                //if(Stations!="")
                //    sql_station = "select * from [dbo].[D_Mi_Station] where DP ='"+Stations+"'";

                DataTable dt_stations = DB.GetDataTable(string.Format(sql_station));

                //如果是高空的要特殊处理
                if (EntityName == "qiya")
                {
                    sql_station = " select * from D_GK_Station order by code  ";
                    dt_stations = DB.GetDataTable(string.Format(sql_station));
                }

                DataTable dt_fildInfo = DB.GetDataTable(sql_entity);
                if (dt_fildInfo == null || dt_fildInfo.Rows.Count <= 0)
                    return "no [condtion] data!";


                string condition = dt_fildInfo.Rows[0]["condition"].ToString();
                if (condition.IndexOf("in") >= 0)
                {
                    condition = condition.Trim().Replace("'", "").Replace("Layer in ", "");
                    sql_layer = "select Code from [dbo].[D_Mi_Layer] where DM in {0}";
                    sql_type = "select * from [dbo].[D_Mi_Layer] where Code in ('{0}')";

                }
                else
                    condition = condition.Trim().Replace("'", "").Replace("Layer = ", "");


                DataTable dt_layer = DB.GetDataTable(string.Format(sql_layer, condition));
                if (dt_layer == null || dt_layer.Rows.Count <= 0)
                    return "no [dt_layer] data!";

                //得到所有的Types
                string types = "";
                string type_dm = "";
                string type_dm_first = "";
                string typeII = "";
                DataTable dt_type = null;
                foreach (DataRow row_code in dt_layer.Rows)
                {
                    string code = row_code["Code"].ToString();
                    dt_type = DB.GetDataTable(string.Format(sql_type, code));
                    DataTable dt_typeII = DB.GetDataTable(string.Format(sql_typeII, code));
                    if (dt_type == null || dt_type.Rows.Count <= 0)
                        continue;

                    if (dt_typeII != null && dt_typeII.Rows.Count > 0 && typeII == "")
                    {
                        typeII = dt_typeII.Rows[0]["DM"].ToString().Trim();
                    }

                    foreach (DataRow row_type in dt_type.Rows)
                    {
                        string rowTYPE = row_type["MC"].ToString();
                        string rowTYPEs = row_type["DM"].ToString();

                        if (type_dm_first == "")
                            type_dm_first = rowTYPEs;

                        if (rowTYPE == type || type == "")
                        {
                            types += "\"" + rowTYPE + "\",";
                            type_dm = rowTYPEs;
                        }
                    }
                }

                if (EntityName == "wrfdust" && type == "高空沙尘")
                    typeII = "07";

                if (EntityName == "qirongjiao" && type == "PM25")
                    condition = "Layer ='35'";

                if (EntityName == "qirongjiao" && type == "")
                    condition = "Layer ='35'";

                if (EntityName == "pm1" && type == "PM1")
                    condition = "Layer ='34'";

                if (EntityName == "pm1" && type == "")
                    condition = "Layer ='34'";

                types = types.TrimEnd(',');

                //得到所有的Stations
                string Stations = "";
                string sta_dm = "";
                if (EntityName == "qiya")
                {
                    foreach (DataRow row_sta in dt_stations.Rows)
                    {
                        string rowSta = row_sta["DP"].ToString();
                        string rowStas = row_sta["MC"].ToString();
                        if (rowSta == Station || Station == "")
                        {
                            Stations += "\"" + rowSta + "\",";
                            sta_dm = rowStas;
                        }
                    }
                }
                else {
                    foreach (DataRow row_sta in dt_stations.Rows)
                    {
                        string rowSta = row_sta["MC"].ToString();
                        string rowStas = row_sta["DM"].ToString();
                        if (rowSta == Station || Station == "")
                        {
                            Stations += "\"" + rowSta + "\",";
                            sta_dm = rowStas;
                        }
                    }
                }
                Stations = Stations.TrimEnd(',');
                string area = dt_fildInfo.Rows[0]["OperatorType"].ToString().Trim();
                string tableName = dt_fildInfo.Rows[0]["TableName"].ToString();
                if (EntityName != "qirongjiao" && EntityName != "pm1")
                    condition = dt_fildInfo.Rows[0]["condition"].ToString();

                if (Station == "")//如果没有传station就默认取第一个
                {
                    Station = dt_stations.Rows[0]["DM"].ToString();
                    if (EntityName == "qiya")
                        Station = dt_stations.Rows[0]["MC"].ToString();

                    if (EntityName == "downloadaod")
                    {
                        area = "京津冀";
                        Stations = "\"京津冀\",\"长三角\"";
                        Station = "04";
                    }
                }
                else
                {
                    //if (EntityName != "qiya")
                    //{
                        
                    // }

                    if (EntityName == "downloadaod")
                    {
                        area = "京津冀";
                        if (Station == "京津冀")
                        {
                            Stations = "\"京津冀\",\"长三角\"";
                            Station = "04";
                        }
                        if (Station == "长三角")
                        {
                            Stations = "\"京津冀\",\"长三角\"";
                            Station = "03";
                        }
                        if (Station == "") {
                            Stations = "\"京津冀\",\"长三角\"";
                            Station = "04";
                        }

                    }
                    else {
                        Station = sta_dm;
                    }

                        //Stations = "";

                        //if (EntityName == "qiya")
                        //    Stations = "\"宝山站\","; ;
                }

                if (type == "")//如果没有type就默认取第一个
                    type = type_dm_first;
                else
                {
                    type = type_dm;
                    types = "";
                }

                //得到最新的开始时间
                string startTime = bTime;
                //得到最新的结束时间
                string endTime = eTime;
                DataTable dt_max = null;
              
                if (eTime == "")
                {
                   
                    if (area == "华东")
                    {
                        Station = "";
                        dt_max = DB.GetDataTable(string.Format(sql_product_max, tableName, "Layer='" + type + "'", "", typeII));
                        endTime = dt_max.Rows[0]["ForecastDate"].ToString();
                    }
                    else
                    {
                        if(condition== "Layer = '29'" || condition == "Layer = '30'" ||
                            condition == "Layer = '31'" || condition == "Layer = '32'" || condition == "Layer = '33'")
                        {
                            type = "13";
                        }
                        if (condition == "Layer ='34'" || condition == "Layer ='35'")
                        {
                            type = "14";
                        }
                        if (condition == "Layer = '28'")
                        {
                            type = "";
                        }

                        dt_max = DB.GetDataTable(string.Format(sql_product_max, tableName, condition, Station, type));
                        endTime = dt_max.Rows[0]["ForecastDate"].ToString();
                    }

                    if (area == "长三角" || area== "京津冀")
                    {
                        //Station = "";
                        type = "";
                        typeII = "";
                        dt_max = DB.GetDataTable(string.Format(sql_product_max, tableName, "Layer='28'", Station, type));
                        endTime = dt_max.Rows[0]["ForecastDate"].ToString();
                    }
                }
                if (bTime == "")
                {
                    if (dt_max == null || dt_max.Rows.Count <= 0)
                        return "no [ForecastDate] data!";


                    startTime = DateTime.Parse(endTime).AddDays(-1).ToString("yyyy-MM-dd HH:mm:ss");// 最近一天
                    if (EntityName == "wrfatmos" || EntityName == "wrfdust"
                        || EntityName == "wrfhaze" || EntityName == "wrfiaqi"
                        || EntityName == "wrfmete")
                    {
                        startTime = DateTime.Parse(endTime).AddDays(0).ToString("yyyy-MM-dd HH:mm:ss");// 最近一天
                    }
                    
                }

                DataTable dt_product = null;

                if (area != "华东")
                {
                    if (condition == "Layer = '29'" || condition == "Layer = '30'" ||
                      condition == "Layer = '31'" || condition == "Layer = '32'" || condition == "Layer = '33'")
                    {
                        type = "13";
                    }
                    if (condition == "Layer ='34'" || condition == "Layer ='35'")
                    {
                        type = "14";
                    }
                    if (condition == "Layer = '28'" )
                    {
                        type = "";
                    }
          
                    if (Station == "宝山站")
                        Station = "58362";

                    dt_product = DB.GetDataTable(string.Format(sql_product, tableName, condition, Station, type, startTime, endTime));

                }
                else
                {
                    dt_product = DB.GetDataTable(string.Format(sql_product, tableName, "Layer='" + type + "'", "", typeII, startTime, startTime));
                    endTime = "WRF";
                }
                //if (area == "长三角")
                //{
                //    type = "";
                //    //Station = "";
                //    dt_product = DB.GetDataTable(string.Format(sql_product, tableName, condition, Station, type, startTime, endTime));

                //}

                string lstURL = "";
                if (dt_product != null && dt_product.Rows.Count > 0)
                {
                    //{url:"图片地址",time:"2018-08-14 00:00:00"}
                    foreach (DataRow row in dt_product.Rows)
                    {
                        string url = row["Folder"].ToString() + "/" + row["Name"].ToString();
                        int period = 0;
                        int.TryParse(row["Period"].ToString(), out period);
                        string time = DateTime.Parse(row["ForecastDate"].ToString()).AddHours(period).ToString("yyyy-MM-dd HH:mm");
                        lstURL += "{url:\"" + url + "\",time:\"" + time + "\"},";
                    }
                    lstURL = lstURL.TrimEnd(',');
                }
                //得到间隔时间
                string intervalOpt = "{\"key\":\"1\",\"val\":\"1小时\"},{\"key\":\"12\",\"val\":\"12小时\"}";
                if (area == "长三角")
                {
                    Stations = "\"京津冀\",\"长三角\"";
                }
                    return Combin(Stations, types, startTime, endTime, intervalOpt, lstURL);
            }
            catch (Exception ex)
            {
                return ex.Message + ex.StackTrace;
            }
        }

        string Combin(string area, string type, string
                      startTime, string endTime, string intervalOpt, string times)
        {
            string json = "{" +
                       "area:[" + area + "]," +
                       "type:[" + type + "]," +
                       "startTime:\"" + startTime + "\"," +
                       "endTime:\"" + endTime + "\", " +
                       "intervalOpt:[" + intervalOpt + "]," +
                       "times:[" + times + "]" +
                       "}";
            return json;

        }

        string IGetImageProducts.Hello()
        {
            return "Hello";
        }


        public string GetModules(string token, string moduleName)
        {
            try
            {
               string account = HttpContext.Current.Request.Headers["Authorization"];
                //string a = nvc["Authorization"];
                Database DB = new Database("DBCONFIG116");
                string sql_qx = "select t3.ModuleID,t3.ModuleName,t1.RoleID from [dbo].[T_User] t1 inner join " +
                                "  [dbo].[T_RoleAuthority] t2 on t1.RoleID=t2.RoleID" +
                                "  inner join T_Module t3 on t2.ModuleID=t3.ModuleID where Account='" + token + "'" +
                                 " and  ModuleName='" + moduleName + "'";
                string sql_mod = @"select t1.*,t2.RoleID from [dbo].[T_Module] t1 left join[T_RoleAuthority] t2 on 
                                   t1.ModuleID = t2.ModuleID where ParentModuleID = {0}  and RoleID = '{1}' order by OrderIndex asc";
                DataTable dt_qx = DB.GetDataTable(sql_qx);// 加载权限表
                if (dt_qx == null || dt_qx.Rows.Count <= 0)
                    return "权限获取失败，请联系管理员赋值权限!";

                string moduleID = dt_qx.Rows[0]["ModuleID"].ToString();
                string roleID = dt_qx.Rows[0]["RoleID"].ToString();
                string moduleNames = dt_qx.Rows[0]["ModuleName"].ToString();
                DataTable dt_mod = DB.GetDataTable(string.Format(sql_mod, moduleID,roleID));// 加载权限表
                if (dt_mod == null || dt_mod.Rows.Count <= 0)
                    return "没有配置权限给您!";

                //根据moduleID查询所有配置的权限
                return GetModuleParent(moduleID, dt_mod, moduleNames);
            }
            catch (Exception ex)
            {
                return ex.Message + ex.StackTrace;
            }
        }

        public string GetModuleParent(string moduleID, DataTable dt_module, string moudleName)
        {
            string header = "[";
            string header1 = "{";
            string jsons =
                          "\"path\": \"@path\"," +
                          "\"component\": \"@component\"," +
                          "\"name\": \"@moduleName\"," +
                          "\"meta\": {" +
                              "\"name\": \"@name\"," +
                              "\"entityName\": \"@moduleName\"," +
                              "\"parentEntityName\":\"@parentEntityName\"" +
                                  "}";
            string floot = "]";
            string floot1 = "}";
            string json = "";
            foreach (DataRow row_module in dt_module.Rows)
            {
                string mid = row_module["ModuleID"].ToString();
                string path = row_module["ModuleName"].ToString();
                string component = row_module["URL"].ToString();
                string moduleName = row_module["ModuleName"].ToString();
                string name = row_module["ModuleNameFlag"].ToString();
                string RoleID = row_module["RoleID"].ToString();
                string parentEntityName = moudleName;

                string jsonx = jsons.Replace("@path", path).
                             Replace("@component", component).
                             Replace("@moduleName", moduleName).
                             Replace("@name", name).
                             Replace("@parentEntityName", parentEntityName);

                string child = GetModuleChild(mid, moduleName, RoleID);
                if (child == "")
                    json += header1 + jsonx + floot1 + ",";
                else
                    json += header1 + jsonx + "," + child + floot1 + ",";

            }
            return header + json.TrimEnd(',') + floot;
        }

        public string GetModuleChild(string moduleID, string parentEntit,string roleId)
        {
            string header = "\"children\": [";
            string jsons = "{" +
                       "\"path\": \"@path\"," +
                       "\"component\": \"@component\"," +
                       "\"name\": \"@moduleName\"," +
                       "\"meta\": {" +
                           "\"name\": \"@name\"," +
                           "\"entityName\": \"@moduleName\"," +
                           "\"parentEntityName\":\"@parentEntityName\"" +
                               "}" +
                      "}";
            string floot = "]";

            Database DB = new Database("DBCONFIG116");
            //string sql_module = "select * from [dbo].[T_Module] where ParentModuleID=" + moduleID + " order by OrderIndex asc";
            string sql_module = @"select t1.* from [dbo].[T_Module] t1 left join[T_RoleAuthority] t2 on 
                                   t1.ModuleID = t2.ModuleID where ParentModuleID = "+ moduleID + " and RoleID = '"+ roleId + "' order by OrderIndex asc";


            DataTable dt_module = DB.GetDataTable(sql_module);
            if (dt_module == null || dt_module.Rows.Count <= 0)
                return "";

            string json = "";
            foreach (DataRow row_module in dt_module.Rows)
            {
                string mid = row_module["ModuleID"].ToString();
                string path = row_module["ModuleName"].ToString();
                string component = row_module["URL"].ToString();
                string moduleName = row_module["ModuleName"].ToString();
                string name = row_module["ModuleNameFlag"].ToString();
                //string parentEntityName = "/";
                json += jsons.Replace("@path", path).
                             Replace("@component", component).
                             Replace("@moduleName", moduleName).
                             Replace("@name", name).
                             Replace("@parentEntityName", parentEntit) + ",";
            }
            json = header + json.TrimEnd(',') + floot;
            return json;
        }
    }
}
