using Newtonsoft.Json;
using Readearth.Data;
using Readearth.Utility;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Web;
using WcfSmcGridService.Model;
namespace WcfSmcGridService.SYS.BigData
{
    /// <summary>
    /// Handler 的摘要说明
    /// </summary>
    
    public class Handler : IHttpHandler
    {
        public Database m_Database;
        public string[] itemsArr = {"account","pass","UserName" };
        public void ProcessRequest(HttpContext context) {
            m_Database = new Database("DBCONFIG116");
            context.Response.Buffer = true;//互不影响
            context.Response.ExpiresAbsolute = DateTime.Now.AddSeconds(0);
            context.Response.Expires = 0;
            context.Response.AddHeader("Pragma", "No-Cache");

            string action = context.Request["action"];
            //string action =HttpContext.Current.Request.QueryString["action"];

            switch (action) {
                case "register": Register(context); break;//获取文件列表
                case "dynamicInfo": SaveDynamicInfo(context); break;
            }
        }

        private void Register(HttpContext Context)
        {
            try {
                NameValueCollection nvc = Context.Request.Form;
                string path =Path.Combine(Context.Server.MapPath(Context.Request["value"]),DateTime.Now.ToString("yyyy-MM-dd"), nvc["account"]);
                string fileNameStr = Context.Request["filename"];
                string roleId = Context.Request["roleId"];
                List<RegisterItemFileName> listRegFileName = new List<RegisterItemFileName>();
                listRegFileName = JsonConvert.DeserializeObject<List<RegisterItemFileName>>(fileNameStr);
                Dictionary<string, string> items = GetRegisterItemType(roleId);
                string insertField = "", insertValues = "";
                string insertToUser = "INSERT INTO [T_User] (Account,Password,userName,roleId,date) VALUES ";
                string userValues = "";
                foreach (string key in items.Keys)
                {
                    if (key == "checkPass") continue;
                    if (key == itemsArr[0] || key == itemsArr[1] || key == itemsArr[2]) {
                        userValues += "'" + nvc[key] + "',";
                        if (key == "account") {
                            insertField += key + ",";
                            insertValues += "'" + nvc[key] + "',";
                        }
                        continue;
                    }
                    insertField += key + ",";
                    string type = items[key];
                    if (type == "file")
                    {
                        string fileName = GetItemFileName(key, listRegFileName);
                        if (fileName == "")
                        {
                            insertValues += "'null',";
                        }
                        else {
                            insertValues += "'" + Path.Combine(path, fileName) + "',";
                        }
                    }
                    else
                    {
                        string value = nvc[key];
                        insertValues += "'"+value+"',";
                    }
                }
                userValues = userValues + roleId+",'"+ DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'";
                insertField = insertField.TrimEnd(',');
                insertValues = insertValues.TrimEnd(',');
                string insert = "INSERT INTO [T_UserInfo] ("+insertField+") VALUES ("+insertValues+");"+insertToUser+"("+userValues+")";
                m_Database.Execute(insert);
                SaveRegisterFile(Context, path);
            }
            catch (Exception e) {
                Context.Response.Write(e.Message);
                throw e;
            }
        }

        private static void SaveRegisterFile(HttpContext Context, string path)
        {
            HttpFileCollection files = Context.Request.Files;
            long allSize = 0;
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            for (int i = 0; i < files.Count; i++)
            {
                allSize += files[i].ContentLength;
            }
            if (allSize > 20 * 1024 * 1024)
            {
                Context.Response.Write("error");
                return;
            }
            for (int i = 0; i < files.Count; i++)
            {
                files[i].SaveAs(Path.Combine(path,Path.GetFileName(files[i].FileName)));
            }
           // throw new NotImplementedException();
        }

        public string GetItemFileName(string key,List<RegisterItemFileName> listRegFileName) {
            string fileName = "";
            foreach (RegisterItemFileName fn in listRegFileName)
            {
                if (fn.key == key)
                {
                    fileName = fn.fileName;
                    break;
                }
            }
            return fileName;
        }
        public Dictionary<string, string> GetRegisterItemType(string roleId) {
            //根据roleID获取表单类型
            string sqlItem = "SELECT i.infoKey,i.type FROM [EMBDShare].[dbo].[D_RegisterItem] i LEFT JOIN dbo.D_RegisterAuth a ON a.infoKey=i.infoKey WHERE a.roleId="+roleId+"";
            DataTable DTItem = m_Database.GetDataTable(sqlItem);
            Dictionary<string, string> dic = new Dictionary<string, string>();
            foreach (DataRow row in DTItem.Rows) {
                string key = row["infoKey"].ToString();
                string type = row["type"].ToString();
                dic.Add(key, type);
            }
            return dic;
        }
        private void SaveDynamicInfo(HttpContext Context) {
            try
            {
                string content = Context.Server.UrlDecode(Context.Request.Form.ToString());
                SaveDynamicInfo saveDyn = new SaveDynamicInfo();
                saveDyn = JsonHelper.Deserialize<SaveDynamicInfo>(content);
                string data = saveDyn.data;
                string time = saveDyn.date;
                string title = saveDyn.title;
                data = data.Replace("#", "<").Replace("*", ">");
                string insert = "INSERT INTO [T_DynamicInfo] (Item,Date,content) VALUES ('" + title + "','" + time + "','" + data + "')";
                m_Database.Execute(insert);
            }
            catch (Exception e) {
                Context.Response.Write("error"+e.Message);
                throw e;
            }
        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}