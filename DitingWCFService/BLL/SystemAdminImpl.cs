using Newtonsoft.Json;
using Readearth.Data;
using Readearth.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using WcfSmcGridService.Model;

namespace WcfSmcGridService.BLL
{
    public class SystemAdminImpl
    {
        private Database DB = new Database("DBCONFIG116");
        public void ModifyPass(string account, string oldPass, string newPass)
        {
            string sql = "SELECT * FROM dbo.T_User WHERE Account='" + account + "' AND Password = '" + oldPass + "'";
            DataTable dt = DB.GetDataTable(sql);
            if (dt != null && dt.Rows.Count > 0)
            {  //用户输入的旧密码正确，修改密码
                string update = "UPDATE T_User SET Password='" + newPass + "' WHERE Account='" + account + "'";
                DB.Execute(update);
            }
        }
        public DataTable GetRoleData()
        {
            string sql = "SELECT* FROM dbo.T_Role Order BY orderIndex ASC";
            DataTable dt = DB.GetDataTable(sql);
            return dt;
        }
        /// <summary>
        /// 递归调用获取每个模块下面的权限
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="parentRoleAuthList"></param>
        public void GetParAuth(DataTable dt, List<ParentRoleAuth> parentRoleAuthList,string sql,string type)
        {
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    ParentRoleAuth parAuth = new ParentRoleAuth();
                    string moduleId = row["moduleId"].ToString();
                    string moduleNameFlag = row["moduleNameFlag"].ToString();
                    string moduleName = row["moduleName"].ToString();
                    string parentModule = row["parentModuleId"].ToString();
                    parAuth.moduleId = moduleId;
                    parAuth.moduleNameFlag = moduleNameFlag;
                    parAuth.moduleName = moduleName;
                    parAuth.parentModuleId = parentModule;
                    parAuth.children = new List<ParentRoleAuth>();
                    // string childrenSql = "SELECT * FROM dbo.T_Module WHERE ParentModuleID='" + moduleId + "' AND Enable='1' ORDER BY OrderIndex ASC ";
                    string childrenSql = string.Format(sql,moduleId);
                    DataTable childrenDT = DB.GetDataTable(childrenSql);
                    if (childrenDT != null && childrenDT.Rows.Count > 0)  //有子节点
                    {
                        GetParAuth(childrenDT, parAuth.children, sql,type);
                    }
                    parentRoleAuthList.Add(parAuth);
                    if (type == "exist")
                    {
                        string roleId = row["roleId"].ToString();
                       // FilterAuth(moduleId, parentModule, roleId, parentRoleAuthList);
                    }
                }
            }
        }
        public void FilterAuth(string moduleId,string parentModuleId,string roleId, List<ParentRoleAuth> RoleAuthList) {
            string moduleSql = " SELECT * FROM T_Module WHERE ParentModuleID='"+parentModuleId+"' AND Enable=1";
            string authSql = "SELECT * ,(SELECT a.ModuleID FROM T_Module a WHERE a.ModuleID=t.ModuleID  GROUP BY a.ModuleID) AS mm FROM T_Module t "+
            "LEFT JOIN T_RoleAuthority r ON r.ModuleID = t.ModuleID WHERE t.ParentModuleID = '"+parentModuleId+"' AND t.Enable = '1' AND r.RoleID = '"+roleId+"'  ORDER BY t.ModuleID asc";
            DataTable authDT = DB.GetDataTable(authSql);
            DataTable moduleDT = DB.GetDataTable(moduleSql);
            if ((authDT != null) && authDT.Rows.Count < moduleDT.Rows.Count) {  //这个父节点下面的子节点没有全部选中，需要把父节点去掉
                for (int i = 0; i < RoleAuthList.Count; i++) {
                    if (RoleAuthList[i].parentModuleId == parentModuleId) {
                        RoleAuthList.Remove(RoleAuthList[i]);
                    }
                }
            }
        }
        public void FilterAuthII(string moduleId, string parentModuleId, string roleId, DataTable dt)
        {
            string moduleSql = " SELECT * FROM T_Module WHERE ParentModuleID='" + parentModuleId + "' AND Enable=1";
            string authSql = "SELECT * ,(SELECT a.ModuleID FROM T_Module a WHERE a.ModuleID=t.ModuleID  GROUP BY a.ModuleID) AS mm FROM T_Module t " +
            "LEFT JOIN T_RoleAuthority r ON r.ModuleID = t.ModuleID WHERE t.ParentModuleID = '" + parentModuleId + "' AND t.Enable = '1' AND r.RoleID = '" + roleId + "'  ORDER BY t.ModuleID asc";
            DataTable authDT = DB.GetDataTable(authSql);
            DataTable moduleDT = DB.GetDataTable(moduleSql);
            if ((authDT != null) && authDT.Rows.Count < moduleDT.Rows.Count)
            {  //这个父节点下面的子节点没有全部选中，需要把父节点去掉
                var subStract = from r in moduleDT.AsEnumerable()
                                    where
                                    !(from rr in authDT.AsEnumerable() select rr.Field<int>("moduleId")).Contains(
                                    r.Field<int>("moduleId"))
                                    select r;
                DataTable dd = subStract.CopyToDataTable();
                if (dd != null && dd.Rows.Count > 0) {
                    string parentId = dd.Rows[0]["ParentModuleID"].ToString();
                    string sql = "select * from t_module where moduleId='"+parentId+"'";
                    DataTable temp = DB.GetDataTable(sql);
                    string condition = "",val="";
                    foreach (DataRow row in dd.Rows) {
                        val += "'"+ row["moduleId"].ToString() + "',";
                    }
                    val = val.TrimEnd(',');
                    if (temp == null || temp.Rows.Count == 0)
                    {
                        condition = "moduleId in("+val+")";
                    }
                    else {
                        condition = "moduleId in ("+ val + ",'"+ temp.Rows[0]["moduleId"].ToString() + "')";
                    }
                    DataRow[] foundRow = dt.Select(condition);
                    foreach (DataRow row in foundRow)
                    {
                        dt.Rows.Remove(row);
                    }
                }
                
            }
        }
        /// <summary>
        /// 获取所有的权限
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public List<ParentRoleAuth> GetAllRoleAuth(string roleId)
        {
            string parentModuleId = "0";
            List<ParentRoleAuth> allRole = new List<ParentRoleAuth>();
            string querySql = "SELECT * FROM dbo.T_Module WHERE ParentModuleID='{0}' AND Enable='1' ORDER BY OrderIndex ASC ";
            string sql = string.Format(querySql, parentModuleId);
            DataTable dt = DB.GetDataTable(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                GetParAuth(dt, allRole, querySql,"all");
            }
            return allRole;
        }
        public List<ParentRoleAuth> GetRoleExistAuth(string roleId) {
            string querySql = "SELECT * ,(SELECT a.ModuleID FROM T_Module a WHERE a.ModuleID=t.ModuleID  GROUP BY a.ModuleID) AS mm "+
                "FROM T_Module t LEFT JOIN dbo.T_RoleAuthority r ON r.ModuleID = t.ModuleID WHERE t.ParentModuleID='{0}' AND t.Enable='1' AND r.RoleID = '"+roleId+"'  ORDER BY t.ModuleID asc";
            string sql = string.Format(querySql,roleId,"0");
            DataTable dt = DB.GetDataTable(sql);
            List<ParentRoleAuth> existRole = new List<ParentRoleAuth>();
            if (dt != null && dt.Rows.Count > 0) {
                GetParAuth(dt, existRole, querySql,"exist");
            }
            return existRole;
        }
        public DataTable GetRoleExistAuthII(string roleId) {
            string querySql = "SELECT * ,(SELECT a.moduleId FROM T_Module a WHERE a.ModuleID=t.ModuleID  GROUP BY a.ModuleID) AS mm " +
                "FROM T_Module t LEFT JOIN dbo.T_RoleAuthority r ON r.ModuleID = t.ModuleID WHERE t.Enable='1' AND r.RoleID = '" + roleId + "'  ORDER BY t.ModuleID asc";
            //string sql = string.Format(querySql, roleId, "0");
            querySql = " SELECT * FROM T_Module WHERE Enable=1 order by moduleId desc";
            DataTable dt = DB.GetDataTable(querySql);
            int count = dt.Rows.Count;
            for (int i = 0; i < count; i++)
            {
                string moduleId = dt.Rows[i]["moduleId"].ToString();
                string parendModuleId = dt.Rows[i]["parentModuleId"].ToString();
                FilterAuthII(moduleId, parendModuleId, roleId, dt);
                if (count > dt.Rows.Count)
                {
                    count = count - (count - dt.Rows.Count);
                    i--;
                }
            }
            return dt;
        }
        public void SaveAuth(string auth,string roleId) {
            List<SaveAuth> saveAuthList= JsonConvert.DeserializeObject<List<SaveAuth>>(auth);
            string values = "";
            List<string> finalSaveAuthList = GetFinalSaveAuth(saveAuthList);
            foreach (string finalSaveAuth in finalSaveAuthList) {
                string moduleId = finalSaveAuth;
                values += "("+roleId+","+moduleId+"),";
            }
            values = values.TrimEnd(',');
            string del = " DELETE T_RoleAuthority WHERE RoleID="+roleId+"";
            string insert = "INSERT INTO T_RoleAuthority(RoleID,ModuleID) VALUES"+values;
            DB.Execute(del);
            DB.Execute(insert);
        }
        //获取最终的保存权限，如果有一个子节点，则他的所有父节点也应该保存
        public List<string> GetFinalSaveAuth(List<SaveAuth> saveAuthList) {
            List<string> finalSaveAuthList = new List<string>();
            foreach (SaveAuth saveAuth in saveAuthList)
            {
                string moduleId = saveAuth.moduleId;
                IteratorGetParentModule(moduleId, finalSaveAuthList);
            }
            return finalSaveAuthList;
        }
        //迭代获取所有父节点
        public void IteratorGetParentModule(string moduleId, List<string> finalSaveAuthList) {
            string sql = "select * from t_module where moduleId='" + moduleId + "' and enable='1'";
            DataTable dt = DB.GetDataTable(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                string parentModuleId = dt.Rows[0]["parentModuleId"].ToString();
                string mId = dt.Rows[0]["moduleId"].ToString();
                if (!finalSaveAuthList.Contains(mId)) {
                    finalSaveAuthList.Add(mId);
                }
                IteratorGetParentModule(parentModuleId, finalSaveAuthList);
            }
        }

        public DataTable GetDynamicInfo() {
            string sql = "SELECT * FROM dbo.T_DynamicInfo ORDER BY Date DESC";
            DataTable dt = DB.GetDataTable(sql);
            return dt;
        }
        public void DelDynamicInfo(string ids) {
            ids = ids.Replace("[", "").Replace("]","");
            string[] idArr = ids.Split(',');
            string id = "";
            foreach (string str in idArr) {
                id += "'" + str + "',";
            }
            id = id.TrimEnd(',');
            string delSql = "  DELETE T_DynamicInfo WHERE id in ("+id+")";
            DB.Execute(delSql);
        }

        public DataTable GetUser() {
            string sql = "SELECT u.id, u.account,u.userName,u.enable as userStatus,u.date as createTime,u.updateDate as updateTime,u.starLevel as userLevel,i.mobile,r.description as userType FROM dbo.T_User u " +
                "LEFT JOIN dbo.T_UserInfo i ON i.account = u.Account  LEFT JOIN dbo.T_Role r ON u.RoleID=r.Id order by updateDate desc";
            DataTable dt = DB.GetDataTable(sql);
            dt.Columns.Add("userStar",typeof(string));
            foreach (DataRow dr in dt.Rows) {
                string level = dr["userLevel"].ToString();
                if (level == "") {
                    dr["userLevel"] = 0;
                }
                string txt = GetUserStar(level);
                dr["userStar"] = txt;
            }
            return dt;
        }
        public string GetUserStar(string val) {
            string txt = "";
            val = val == "" ? "0" : val;
            switch (val) {
                case "0":txt = "无";break;
                case "1": txt = "一星"; break;
                case "2": txt = "二星"; break;
                case "3": txt = "三星"; break;
                case "4": txt = "四星"; break;
                case "5": txt = "五星"; break;
            }
            return txt;
        }

        public void UserManagerSave(string values,string id) {
            List<UserManagerSave> userSaveList = new List<UserManagerSave>();
            userSaveList = JsonHelper.JSONStringToList<UserManagerSave>(values);
            string sql = "select account from t_user where id='"+id+"'";
            DataTable dt = DB.GetDataTable(sql); //根据id找到修改之前的账号
            string oldAccount = dt.Rows[0]["account"].ToString();
            string account = userSaveList[0].value;
            string userName = userSaveList[1].value;
            string mobile = userSaveList[2].value;
            string userLevel = userSaveList[3].value;
            string userType = userSaveList[4].value;
            string userStatus = userSaveList[5].value;
            DateTime dNow = DateTime.Now;
            string update = "update t_user set Account='"+account+"',UserName='"+userName+"',starLevel='"+userLevel+"',enable='"+userStatus+"',updateDate='"+dNow.ToString("yyyy-MM-dd HH:mm:ss")+"'" +
               " WHERE Account='"+account+ "';update t_userInfo set Account='" + account + "',mobile='" + mobile+ "' where  Account='" + account + "'";
            DB.Execute(update);
        }
    }

    class UserManagerSave {
        public string value;
    }
}