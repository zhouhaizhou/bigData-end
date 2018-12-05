using Re.Common;
using Readearth.Data;
using Readearth.Utility;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using WcfSmcGridService.BLL;
using WcfSmcGridService.Model;

namespace WcfSmcGridService.SYS.BigData
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“Register”。
    // 注意: 为了启动 WCF 测试客户端以测试此服务，请在解决方案资源管理器中选择 Register.svc 或 Register.svc.cs，然后开始调试。
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    
    public class Register : IRegister
    {
        RegisterImpl regImpl = new RegisterImpl();
        /// <summary>
        /// 获取注册页面的条款
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public string GetRegisterItem(string roleId)
        {
            try
            {
                return JsonHelper.ToJSON(regImpl.GetRegisterItem(roleId));
            }
            catch (Exception ex)
            {
                string recordInfo = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " DataService_GetProvince " + ex.Message;
                LogManager.WriteLogRecord(DateTime.Now.ToString("yyyy年MM月") + "污染天气大数据平台", recordInfo);
                return "ERROR"+ex.Message;
            }
        }
        /// <summary>
        /// 获取注册页面的基本信息
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public string GetInfo(string roleId)
        {
            try
            {
                return JsonHelper.ToJSON(regImpl.GetInfo(roleId));
            }
            catch (Exception ex)
            {
                string recordInfo = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " DataService_GetProvince " + ex.Message;
                LogManager.WriteLogRecord(DateTime.Now.ToString("yyyy年MM月") + "污染天气大数据平台", recordInfo);
                return "ERROR" + ex.Message;
            }
        }
        public string ClickRegister(Stream streams)
        {
            try
            {
                IncomingWebRequestContext request = WebOperationContext.Current.IncomingRequest;
                WebHeaderCollection headers = request.Headers;
                string contentType = headers["Content-Type"];
                //string boundary = contentType.Split('=')[1];
                regImpl.ClickRegister(streams);
                return "";
                //string p1 = Request.Form["crePhone"];
                //return JsonHelper.ToJSON(regImpl.ClickRegister(streams));
            }
            catch (Exception ex)
            {
                string recordInfo = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " DataService_GetProvince " + ex.Message;
                LogManager.WriteLogRecord(DateTime.Now.ToString("yyyy年MM月") + "污染天气大数据平台", recordInfo);
                return "ERROR" + ex.Message;
            }
        }
        public string CheckAccount(string account) {
            try
            {
                return regImpl.CheckAccount(account);
            }
            catch (Exception ex)
            {
                string recordInfo = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " DataService_GetProvince " + ex.Message;
                LogManager.WriteLogRecord(DateTime.Now.ToString("yyyy年MM月") + "污染天气大数据平台", recordInfo);
                HttpContext.Current.Response.Write(ex.Message);
                throw ex;
            }
        }
        public string GetcheckItem(string roleId)
        {
            try
            {
                return JsonHelper.ToJSON(regImpl.GetcheckItem(roleId));
            }
            catch (Exception ex)
            {
                string recordInfo = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " DataService_GetProvince " + ex.Message;
                LogManager.WriteLogRecord(DateTime.Now.ToString("yyyy年MM月") + "污染天气大数据平台", recordInfo);
                HttpContext.Current.Response.Write(ex.Message);
                throw ex;
            }
        }
        public string GetDefault(string rootName) {
            try
            {
                return JsonHelper.ToJSON(regImpl.GetDefault(rootName));
            }
            catch (Exception ex)
            {
                string recordInfo = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " DataService_GetProvince " + ex.Message;
                LogManager.WriteLogRecord(DateTime.Now.ToString("yyyy年MM月") + "污染天气大数据平台", recordInfo);
                HttpContext.Current.Response.Write(ex.Message);
                throw ex;
            }
        }
    }
}
