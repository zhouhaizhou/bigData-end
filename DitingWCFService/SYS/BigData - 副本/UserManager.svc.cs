using Readearth.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;

namespace WcfSmcGridService.SYS.BigData
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“UserManager”。
    // 注意: 为了启动 WCF 测试客户端以测试此服务，请在解决方案资源管理器中选择 UserManager.svc 或 UserManager.svc.cs，然后开始调试。
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class UserManager : IUserManager
    {
        UserManagerImpl umImpl = new UserManagerImpl();


        /// <summary>
        /// 获取用户个人信息
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public string GetPersonInfo(string account)
        {
            try
            {
                return JsonHelper.ToJSON(umImpl.GetPersonInfo(account));
            }
            catch (Exception ex)
            {

                return ex.Message;
            }

        }

        /// <summary>
        /// 获取调查问卷的数据
        /// </summary>
        /// <returns></returns>
        public string GetQuestionData() {
            try
            {
                return JsonHelper.ToJSON(umImpl.GetQuestionData());
            }
            catch (Exception ex)
            {

                return ex.Message;
            }
        }

        /// <summary>
        /// 调查问卷 新增数据
        /// </summary>
        /// <param name="funParams"></param>
        /// <returns></returns>
        public string InsertQuestionData(string funParams)
        {
            try
            {
                return JsonHelper.ToJSON(umImpl.InsertQuestionData(funParams));
            }
            catch (Exception ex)
            {

                return ex.Message;
            }
        }
        /// <summary>
        /// 调查问卷 编辑数据
        /// </summary>
        /// <param name="funParams"></param>
        /// <returns></returns>
        public string UpdateQuestionData(string funParams)
        {
            try
            {
                return JsonHelper.ToJSON(umImpl.UpdateQuestionData(funParams));
            }
            catch (Exception ex)
            {

                return ex.Message;
            }
        }
        /// <summary>
        /// 调查问卷   批量删除
        /// </summary>
        /// <param name="taskNumbers"></param>
        /// <returns></returns>
        public string DeleteQuestionData(string taskNumbers)
        {
            try
            {
                return JsonHelper.ToJSON(umImpl.DeleteQuestionData(taskNumbers));
            }
            catch (Exception ex)
            {

                return ex.Message;
            }

        }

        public void DoWork()
        {
            throw new NotImplementedException();
        }
    }
}
