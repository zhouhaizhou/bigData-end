using Readearth.Data;
using Readearth.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using WcfSmcGridService.Model;

namespace WcfSmcGridService.BLL
{
    public class RegisterImpl
    {
        private Database DB = new Database("DBCONFIG116");
        public List<RegisterItem> GetRegisterItem(string roleId)
        {
            List<RegisterItem> regItemList = new List<RegisterItem>();
            //string sql = "SELECT [label],[infoKey],[value],[type],[roleId],[orderIndex],[require] FROM D_RegisterItem where roleId like '%" + roleId + "%'";
            string sql = "SELECT i.[label],i.[infoKey],i.[value],i.[type],a.[roleId],a.[orderIndex],a.[require],i.sizerestrict,i.description,i.accept,i.regular,i.infoType,i.isEdit FROM D_Registerauth a " +
                "LEFT JOIN dbo.D_RegisterItem i ON i.infoKey = a.infoKey WHERE a.roleId = "+roleId+" ORDER BY a.orderIndex ASC";
            DataTable dt = DB.GetDataTable(sql);
            if (dt != null && dt.Rows.Count > 0)
			{
                ProItems(regItemList, dt);
            }
            return regItemList;
 		}
		/// <summary>
        /// 遍历表中的每一行，形成键值对（字段名：值）  
        /// </summary>
        /// <param name="regItemList"></param>
        /// <param name="dt"></param>
        public void ProItems(List<RegisterItem> regItemList, DataTable dt)
            {
                foreach (DataRow row in dt.Rows)
                {
                    RegisterItem regItem = new RegisterItem();
                    List<RegisterItemOptions> regItemOptList = new List<RegisterItemOptions>();
                    string label = row["label"].ToString();
                    string infoKey = row["infoKey"].ToString();
                    string value = row["value"].ToString();
                    string type = row["type"].ToString();
                    string orderIndex = row["orderIndex"].ToString();
                    string require = row["require"].ToString();
                    string sizeRestrict = row["sizerestrict"].ToString();
                    string accept = row["accept"].ToString();
                    string des = row["description"].ToString();
                    string regular = row["regular"].ToString();
					 string infoType = row["infoType"].ToString();
               		 string isEdit = row["isEdit"].ToString();
                    if (type == "select")
                    {
                        GetRegisterOptions(regItemOptList, infoKey);
                    }
                    regItem.label = label;
                    regItem.infoKey = infoKey;
                    regItem.value = value;
                    regItem.type = type;
                    regItem.ordex = orderIndex;
                    regItem.require = require;
                    regItem.options = regItemOptList;
                    regItem.sizeRestrict = sizeRestrict;
                    regItem.description = des;
                    regItem.accept = accept;
                    regItem.regular = regular;
					regItem.infoType = infoType;
                	regItem.isEdit = isEdit;
                    regItemList.Add(regItem);
            }
        }

        private List<RegisterItemOptions> GetRegisterOptions(List<RegisterItemOptions> regItemOptList, string infoKey)
        {
            string sqlOptions = "SELECT dm,code,mc FROM [D_RegisterItemOptions] WHERE TYPE = '" + infoKey + "'";
            DataTable DT_Opt = DB.GetDataTable(sqlOptions);
            foreach (DataRow dr in DT_Opt.Rows)
            {
                RegisterItemOptions regItemOpt = new RegisterItemOptions();
                string dm = dr["dm"].ToString();
                string code = dr["code"].ToString();
                string mc = dr["mc"].ToString();
                regItemOpt.DM = dm;
                regItemOpt.code = code;
                regItemOpt.MC = mc;
                regItemOptList.Add(regItemOpt);
            }
            return regItemOptList;
        }
        public DataTable GetInfo(string roleId) {
            string sql = "SELECT r.Id, r.RoleName,t.type,t.provision,t.Description FROM dbo.D_RegisterType t LEFT JOIN dbo.T_Role r ON t.roleName=r.RoleName WHERE r.id='"+roleId+"'";
            DataTable dt = DB.GetDataTable(sql);
            return dt;
        }
        public void ClickRegister(Stream streams)
        {
            byte[] bytes = null;
            string returnByte = "";
            int length = Convert.ToInt32(100000);
            using (var binaryReader = new BinaryReader(streams))
            {
                bytes = binaryReader.ReadBytes(length);
            }
            if (bytes != null)
            {
                returnByte = System.Text.Encoding.Default.GetString(bytes); ;
            }
            StreamReader sr = new StreamReader(streams, Encoding.GetEncoding("UTF-8"));
            string s = sr.ReadToEnd();
            //string filePath = System.Web.Hosting.HostingEnvironment.MapPath("F:\\桌面\\延伸期修改 09拓展.docx");
            sr.Dispose();
            Dictionary<string, object> dic = ResStr(s); ;
        }
        public Dictionary<string, object> ResStr(string str)
        {
            string boundary = str.Split(Environment.NewLine.ToCharArray())[0];
            Dictionary<string, object> dic = new Dictionary<string, object>();
            string[] items = Regex.Split(str, boundary, RegexOptions.None);
            List<string> lists = items.ToList();
            lists.RemoveAt(0);
            lists.RemoveAt(lists.Count - 1);
            foreach (string list in lists)
            {
                string[] arrItem = Regex.Split(list, @"\r\n", RegexOptions.None);
                string key = Regex.Split(arrItem[1], "=", RegexOptions.None)[1];
                string value = arrItem[3];
                if (arrItem[1].Contains("filename"))
                { //上传的是文件
                    string filename = arrItem[1].Split('=')[1];
                    Dictionary<string, string> d = new Dictionary<string, string>();
                    d.Add("filename", filename);
                    dic.Add(key, d);
                }
                else
                {
                    dic.Add(key, value);
                }
            }
            return dic;
        }
        public string CheckAccount(string account) {
            string sql = "SELECT* FROM dbo.T_User WHERE Account = '"+account+"'";
            DataTable dt = DB.GetDataTable(sql);
            if (dt.Rows.Count > 0)
            {
                throw new Exception("error");
            }
            else {
                return "ok";
            }
        }
        public DataTable GetcheckItem(string roleId) {
            string sql = "SELECT i.infoKey,i.regular FROM dbo.D_RegisterItem i LEFT JOIN dbo.D_RegisterAuth a ON i.infoKey=a.infoKey WHERE a.roleId='"+roleId+"' ORDER BY a.orderIndex ASC";
            DataTable dt = DB.GetDataTable(sql);
            return dt;
        }

        public DataTable GetDefault(string rootName) {
            string account = HttpContext.Current.Request.Headers["Authorization"];
            string sqlUser = "SELECT RoleID FROM dbo.T_User WHERE Account='"+account+"'";
            DataTable DT_User = DB.GetDataTable(sqlUser);
            string roleId = DT_User.Rows[0]["roleId"].ToString();
            string sql = "SELECT top 1 rootName,m.ModuleName FROM D_DefaultRoute d LEFT JOIN dbo.T_RoleAuthority r ON r.ModuleID=d.moduleId "+
                "LEFT JOIN dbo.T_Module m ON m.ModuleID=r.ModuleID WHERE rootName='"+rootName+"' AND r.RoleID='"+roleId+"' ORDER BY grade ASC";
            DataTable dt = DB.GetDataTable(sql);
            return dt;
        }
    }
}