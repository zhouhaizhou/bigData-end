using Readearth.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;
using System.Web;
using WcfSmcGridService.BLL;

namespace WcfSmcGridService.SYS.BigData
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“SystemAdmin”。
    // 注意: 为了启动 WCF 测试客户端以测试此服务，请在解决方案资源管理器中选择 SystemAdmin.svc 或 SystemAdmin.svc.cs，然后开始调试。
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class SystemAdmin : ISystemAdmin
    {
        SystemAdminImpl sysAdmImp = new SystemAdminImpl();
        public string ModifyPass(string oldPass, string newPass)
        {
            HttpContext Context = HttpContext.Current;
            try
            {
                string account = Context.Request.Headers["Authorization"];
                sysAdmImp.ModifyPass(account,oldPass, newPass);
                return "ok";
            }
            catch (Exception e) {
                Context.Response.Write("error,修改密码"+e.Message);
                throw e;
            }
        }
        public string GetRoleData() {
            try
            {
                return JsonHelper.ToJSON(sysAdmImp.GetRoleData());
            }
            catch (Exception e)
            {
                HttpContext.Current.Response.Write("error:" + e.Message);
                throw e;
            }
        }
        /// <summary>
        /// 获取该角色的所有权限
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public string GetRoleAuth(string roleId)
        {
            try
            {
                return JsonHelper.ToJSON(sysAdmImp.GetAllRoleAuth(roleId));
            }
            catch (Exception e)
            {
                HttpContext.Current.Response.Write("error:" + e.Message);
                throw e;
            }
        }

        public string GetRoleExistAuth(string roleId)
        {
            try
            {
                //return JsonHelper.ToJSON(sysAdmImp.GetRoleExistAuth(roleId));
                return JsonHelper.ToJSON(sysAdmImp.GetRoleExistAuthII(roleId));
            }
            catch (Exception e)
            {
                HttpContext.Current.Response.Write("error:" + e.Message);
                throw e;
            }
        }
        public void SaveAuth(string auth,string roleId)
        {
            try
            {
                //return JsonHelper.ToJSON(sysAdmImp.GetRoleExistAuth(roleId));
                sysAdmImp.SaveAuth(auth,roleId);
            }
            catch (Exception e)
            {
                HttpContext.Current.Response.Write("error:" + e.Message);
                throw e;
            }
        }
        public string GetDynamicInfo() {
            try
            {
                return JsonHelper.ToJSON(sysAdmImp.GetDynamicInfo());
            }
            catch (Exception e)
            {
                HttpContext.Current.Response.Write("error:" + e.Message);
                throw e;
            }
        }
        public void DelDynamicInfo(string ids) {
            try
            {
                sysAdmImp.DelDynamicInfo(ids);
            }
            catch (Exception e)
            {
                HttpContext.Current.Response.Write("error:" + e.Message);
                throw e;
            }
        }
        public string GetUser() {
            try
            {
                return JsonHelper.ToJSON(sysAdmImp.GetUser());
            }
            catch (Exception e)
            {
                HttpContext.Current.Response.Write("error:" + e.Message);
                throw e;
            }
        }
        public void UserManagerSave(string values,string id)
        {
            try
            {
                sysAdmImp.UserManagerSave(values,id);
            }
            catch (Exception e)
            {
                HttpContext.Current.Response.Write("error:" + e.Message);
                throw e;
            }
        }

        public string GetRelateResult()
        {
            try
            {
                return JsonHelper.ToJSON(sysAdmImp.GetRelateResult());
            }
            catch (Exception e)
            {
                HttpContext.Current.Response.Write("error:" + e.Message);
                throw e;
            }
        }

        public void DelRelateResult(string ids)
        {
            try
            {
                sysAdmImp.DelRelateResult(ids);
            }
            catch (Exception e)
            {
                HttpContext.Current.Response.Write("error:" + e.Message);
                throw e;
            }
        }
        public void SaveRelateResult(string type, string name, string url,string status, string id)
        {
            try
            {
                string account = HttpContext.Current.Request.Headers["Authorization"];
                sysAdmImp.SaveRelateResult(type,name,url, status,id, account);
            }
            catch (Exception e)
            {
                HttpContext.Current.Response.Write("error:" + e.Message);
                throw e;
            }
        }
        public string MyDownload(string startTime, string endTime)
        {
            try
            {
                string account = HttpContext.Current.Request.Headers["Authorization"];
                return JsonHelper.ToJSON(sysAdmImp.MyDownload(account, startTime,endTime));
            }
            catch (Exception e)
            {
                HttpContext.Current.Response.Write("error:" + e.Message);
                throw e;
            }
        }
        public void DelMyDownload(string ids)
        {
            try
            {
                sysAdmImp.DelMyDownload(ids);
            }
            catch (Exception e)
            {
                HttpContext.Current.Response.Write("error:" + e.Message);
                throw e;
            }
        }
        public string GetMyCollect()
        {
            try
            {
                string account = HttpContext.Current.Request.Headers["Authorization"];
                return JsonHelper.ToJSON(sysAdmImp.GetMyCollect(account));
            }
            catch (Exception e)
            {
                HttpContext.Current.Response.Write("error:" + e.Message);
                throw e;
            }
        }
        public void DelMyCollect(string ids)
        {
            try
            {
                sysAdmImp.DelMyCollect(ids);
            }
            catch (Exception e)
            {
                HttpContext.Current.Response.Write("error:" + e.Message);
                throw e;
            }
        }
        public void SaveMyDownload(string id)
        {
            try
            {
                sysAdmImp.SaveMyDownload(id);
            }
            catch (Exception e)
            {
                HttpContext.Current.Response.Write("error:" + e.Message);
                throw e;
            }
        }
    }
}
