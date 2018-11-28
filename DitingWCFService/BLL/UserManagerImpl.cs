using Readearth.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using WcfSmcGridService.BLL;
using WcfSmcGridService.Model;
using Newtonsoft.Json;

namespace WcfSmcGridService.SYS.BigData
{
    public class UserManagerImpl
    {
        private Database ds_DB = new Database("DBCONFIG116");

        /// <summary>
        /// 获取个人信息
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public List<RegisterItem> GetPersonInfo(string account)
        {

            string sqlUserInfo = "  select u.UserName,u.RoleID,u.starLevel,u.auditState,u.[date],i. * from T_UserInfo i left join T_User u on u.Account=i.account  where u.account='" + account + "'";
            DataTable DT_UserInfo = ds_DB.GetDataTable(sqlUserInfo);

            DataRow drRoleId = DT_UserInfo.Rows[0];
            string roleId = Convert.ToString(drRoleId["RoleID"]);
            string starLevel = Convert.ToString(drRoleId["starLevel"]);
            string auditState = Convert.ToString(drRoleId["auditState"]);
            string registerDate = Convert.ToString(drRoleId["date"]);
            string strSQL = "    SELECT i.*,a.[roleId],a.[orderIndex],a.[require] FROM D_Registerauth a  LEFT JOIN dbo.D_RegisterItem i ON i.infoKey = a.infoKey WHERE a.roleId = '{0}' and i.infoKey not in ('pass','checkpass')  ORDER BY i.infoType asc, a.orderIndex ASC";
            strSQL = string.Format(strSQL, roleId);
            DataTable DT_Item = ds_DB.GetDataTable(strSQL);

            foreach (DataRow dr in DT_Item.Rows)
            {
                //根据dbo.D_RegisterItem表中的infoKey取T_UserInfo表中对应值放到dbo.D_RegisterItem表中的value
                string key = dr["infoKey"].ToString();
                //获取此infoKey在DT_UserInfo中对应的value
                string userInfoValue = DT_UserInfo.Rows[0][key].ToString();
                //判断infoKey在[D_RegisterItemOptions]表中是否存在，如果存在，dr["value"]就取此表中的MC值，否则dr["value"]就取userInfoValue
                string sqlOption = " select * from [D_RegisterItemOptions] where PATINDEX('%" + key + "%',[TYPE])>0";
                DataTable DT_Option = ds_DB.GetDataTable(sqlOption);
                if (DT_Option.Rows.Count > 0)
                {
                    string sqlMC = " select MC from [D_RegisterItemOptions] where [type]='" + key + "' and Code='" + userInfoValue + "'";
                    string MC = ds_DB.GetFirstValue(sqlMC);
                    dr["value"] = MC;
                }
                else
                {
                    dr["value"] = DT_UserInfo.Rows[0][key].ToString();
                }
                
            }
            //3、添加数据行

            //获取用户类型
            string strSQL4 = "  select [Description]  FROM [EMBDShare].[dbo].[T_Role] where ID='{0}'";
            strSQL4 = string.Format(strSQL4, roleId);
            DataTable DT_userType = ds_DB.GetDataTable(strSQL4);
            string userType = DT_userType.Rows[0]["Description"].ToString();
            DataRow dr0 = DT_Item.NewRow();
            dr0["label"] = "用户类型"; //通过索引赋值
            dr0["infoKey"] = "userType";
            dr0["value"] = userType;
            dr0["infoType"] = "0";
            dr0["isEdit"] = 0;
            DT_Item.Rows.InsertAt(dr0, 0);//这里的h 就是要插入的第几行

            if (roleId != "0") {
                //用户状态
                DataRow dr1 = DT_Item.NewRow();
                dr1["label"] = "审核状态"; //通过索引赋值
                dr1["infoKey"] = "auditState";
                if (auditState == "0" || auditState=="")//数据库中的值为空时
                {
                    auditState = "待审核";
                }
                else if (auditState == "1")
                {
                    auditState = "审核通过";
                }
                dr1["value"] = auditState;
                dr1["infoType"] = "0";
                dr1["isEdit"] = 0;
                DT_Item.Rows.InsertAt(dr1, 1);//这里的h 就是要插入的第几行
            }

            //用户星级
            DataRow dr2 = DT_Item.NewRow();
            dr2["label"] = "用户星级"; //通过索引赋值
            switch (starLevel)
            {
                case "":
                    starLevel = "无星级";
                    break;
                case "1":
                    starLevel = "一星级";
                    break;
                case "2":
                    starLevel = "二星级";
                    break;
                case "3":
                    starLevel = "三星级";
                    break;
                case "4":
                    starLevel = "四星级";
                    break;
                case "5":
                    starLevel = "五星级";
                    break;
                default:
                    break;
            };
            dr2["infoKey"] = "starLevel";
            dr2["value"] = starLevel;
            dr2["infoType"] = "0";
            dr2["isEdit"] = 0;
            DT_Item.Rows.InsertAt(dr2, 2);

            if (roleId != "0") { //管理员的个人信息中不包含此项
                //增加用户注册时间
                DataRow dr3 = DT_Item.NewRow();
                dr3["label"] = "注册时间"; //通过索引赋值
                dr3["infoKey"] = "registerDate";
                dr3["value"] = registerDate;
                dr3["infoType"] = "0";
                dr3["isEdit"] = 0;
                DT_Item.Rows.InsertAt(dr3, 3);
            }

            RegisterImpl registerImp = new RegisterImpl();
            List<RegisterItem> regItemList = new List<RegisterItem>();
            registerImp.ProItems(regItemList, DT_Item);
            return regItemList;
        }


        public List<QuestionItem> GetQuestionData()
        {
            //查询出要遍历的数据
            //string sql = "  SELECT [taskNumber],[question],[value] ,[startTime],[endTime],[createTime],[condition],[enable] FROM [EMBDShare].[dbo].[D_Questionnaire_Module] where startTime<CONVERT(varchar(100), GETDATE(), 20) and endTime>CONVERT(varchar(100), GETDATE(), 20) and enable='1' order by [taskNumber]  desc";
            string sql = "  SELECT [taskNumber],[question],[value] ,[startTime],[endTime],[createTime],[condition],[enable] FROM [EMBDShare].[dbo].[D_Questionnaire_Module]  order by [taskNumber]  desc";
            DataTable dt1 = ds_DB.GetDataTable(sql);
            DataView dv = dt1.DefaultView;
            DataTable taskNumberDT = dv.ToTable(true, "taskNumber");//true表示是选择不同，c#获取DataTable某一列不重复的值，或者获取某一列的所有值   得到只有某列不重复的表

            //实例化一个要返回的集合
           List<QuestionItem> questionList =new List<QuestionItem>();


            //遍历数据整合成新表并返回新表
            if (dt1!=null && dt1.Rows.Count > 0)
            {
                foreach (DataRow dr in taskNumberDT.Rows) {
                    QuestionItem ques = new QuestionItem();
                    string taskNumber = dr["taskNumber"].ToString();
                    DataRow[] rowArr = dt1.Select("taskNumber='" + taskNumber + "'");//对DataTable根据条件查询返回新的DataTable表，在这里通过循环相当于分别返回了三张表
                    string question = "";
                    int num = 1;
                    foreach (DataRow row in rowArr) {
                        //question += "(" + num + ")" + row["question"].ToString() + ";" + Environment.NewLine;
                        question +=  row["question"].ToString()  + Environment.NewLine;
                        num++;
                    }
                    question = question.TrimEnd(Environment.NewLine.ToCharArray());
                    string value = rowArr[0]["value"].ToString();
                    string startTime = rowArr[0]["startTime"].ToString();
                    string endTime = rowArr[0]["endTime"].ToString();
                    string createTime = rowArr[0]["createTime"].ToString();
                    string condition = rowArr[0]["condition"].ToString();
                    int enable = Convert.ToInt32(rowArr[0]["enable"]);//前台需要int类型
                    ques.taskNumber = taskNumber;
                    ques.question = question;
                    ques.value = value;
                    ques.startTime = startTime;
                    ques.endTime = endTime;
                    ques.createTime = createTime;
                    ques.condition = condition;
                    ques.enable = enable;
                    questionList.Add(ques);
                }
            }

            return questionList;
        }
        /// <summary>
        /// 调查问卷  新增
        /// </summary>
        /// <param name="funParams"></param>
        /// <returns></returns>
        public int InsertQuestionData(string funParams)
        {
            //处理前台传过来的json字符串
            JsonSerializerSettings jSetting = new JsonSerializerSettings();//实例化一个DeserializeObject<>定义中需要的JsonSerializerSettings类型的对象，public static T DeserializeObject<T>(string value, JsonSerializerSettings settings);
            jSetting.NullValueHandling = NullValueHandling.Ignore;
            QuestionItem ques = JsonConvert.DeserializeObject<QuestionItem>(funParams, jSetting);
            //查询出表中taskNumber的最大值
            string sql = @"select MAX(taskNumber) as 'maxv' from [EMBDShare].[dbo].[D_Questionnaire_Module]";
            DataTable taskNumMaxDT = ds_DB.GetDataTable(sql);
            DataRow dr=taskNumMaxDT.Rows[0];
            string taskNumMax = Convert.ToString(dr["maxv"]);
            string taskNumAddOne = Convert.ToString(Convert.ToInt32(taskNumMax) + 1);
            //将问卷内容字符串转化为数组，遍历数组进行逐条插入
            string quesStr = ques.question;
            string[] quesArr = quesStr.Split(Environment.NewLine.ToCharArray());
            int result = 0;
            for (int i = 0; i < quesArr.Length;i++ ) {
                string question = quesArr[i];
                //编写插入语句
                string insertSQL = @"insert into [EMBDShare].[dbo].[D_Questionnaire_Module]([taskNumber],[question],[startTime],[endTime],[createTime],[condition],[enable]) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}')";
                insertSQL = string.Format(insertSQL, taskNumAddOne, question, ques.startTime, ques.endTime, ques.createTime, ques.condition, ques.enable);
                result = ds_DB.Execute(insertSQL);
            }
            
            return result;

        }

        /// <summary>
        /// 调查问卷  编辑
        /// </summary>
        /// <param name="funParams"></param>
        /// <returns></returns>
        public int UpdateQuestionData(string funParams)
        {
            //处理前台传过来的json字符串
            JsonSerializerSettings jSetting = new JsonSerializerSettings();//实例化一个DeserializeObject<>定义中需要的JsonSerializerSettings类型的对象，public static T DeserializeObject<T>(string value, JsonSerializerSettings settings);
            jSetting.NullValueHandling = NullValueHandling.Ignore;
            QuestionItem ques = JsonConvert.DeserializeObject<QuestionItem>(funParams, jSetting);
            
            //将问卷内容字符串转化为数组，遍历数组进行逐条更新
            string quesStr = ques.question;
            string[] quesArr = quesStr.Split(Environment.NewLine.ToCharArray());
            int result = 0;
            for (int i = 0; i < quesArr.Length; i++)
            {
                string question = quesArr[i];
                //编写插入语句
                string updateSQL = @"update [EMBDShare].[dbo].[D_Questionnaire_Module] set question = '{1}',startTime = '{2}',endTime = '{3}',createTime = '{4}',condition = '{5}',enable = '{6}' where taskNumber = '{0}'";
                updateSQL = string.Format(updateSQL, ques.taskNumber, question, ques.startTime, ques.endTime, ques.createTime, ques.condition, ques.enable);
                result = ds_DB.Execute(updateSQL);
            }

            return result;

        }

        /// <summary>
        /// 调查问卷  批量删除
        /// </summary>
        /// <param name="funParams"></param>
        /// <returns></returns>
        public int DeleteQuestionData(string taskNumbers)
        {
            //将删除的编号字符串转化为数组
            string[] taskArr = taskNumbers.Split(new Char[] { ',' });
            int result = 0;
            string taskStr="";
            for (int i = 0; i < taskArr.Length; i++)
            {
                taskStr += "'" + Convert.ToString(taskArr[i]) + "'" + ",";
            }
            taskStr = taskStr.Substring(0, taskStr.Length - 1);
            //编写插入语句
            string deleteSQL = @" DELETE FROM [EMBDShare].[dbo].[D_Questionnaire_Module] WHERE taskNumber in({0})";
            deleteSQL = string.Format(deleteSQL, taskStr);
            result = ds_DB.Execute(deleteSQL);
            return result;

        }
   
    }
}
