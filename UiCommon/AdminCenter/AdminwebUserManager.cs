using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FineUI;
using Mammothcode.Core.Data.DataSecurity;
using Mammothcode.Core.SystemTool;
using Mammothcode.Public.Data;
using Mammothcode.BLL;
using Mammothcode.BLL.Tool;
using Mammothcode.Core.Data.DataConvert;
using Mammothcode.Core.Data.DataWeb;
using Mammothcode.Model;
using Mammothcode.Model.Model_Custom;

namespace Mammothcode.UICommon.Common.AdminCenter
{
    public class AdminwebUserManager
    {
        private static T_ADMIN_BLL T_ADMIN_BLL = new T_ADMIN_BLL();
        private static T_POWERS_BLL T_POWERS_BLL = new T_POWERS_BLL();
        private static T_ROLES_POWERS_BLL T_ROLES_POWERS_BLL = new T_ROLES_POWERS_BLL();
        protected const int ExpiresDayCookiesSession = 7;

        #region 管理员登录，登出，判断是否登录，获取用户模型

        /// <summary>
        /// 管理员登录
        /// 创建  毛枫  2015-4-21
        /// 修改  毛枫  2015-7-31
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static bool AdminLoginIn(string username, string password)
        {
            bool result = false;
            try
            {
                T_ADMIN admin = new T_ADMIN();
                admin = IsLoginFromSql(username, password);
                if (admin != null)
                {
                    #region Cookie和Session的设置
                    //System.Web.HttpContext.Current.Session["A_ID"] = admin.ID;
                    //System.Web.HttpContext.Current.Session["A_Code"] = admin.A_CODE;
                    //System.Web.HttpContext.Current.Session["A_TrueName"] = admin.A_TRUE_NAME;
                    //System.Web.HttpContext.Current.Session["A_AdminName"] = admin.A_NAME;
                    //System.Web.HttpContext.Current.Session.Timeout = 10800;
                    WebHelperUtil.SessionAdd("A_ID", admin.ID.ToString(), 60);
                    WebHelperUtil.SessionAdd("A_Code", admin.A_CODE, 60);
                    WebHelperUtil.SessionAdd("A_TrueName", admin.A_TRUE_NAME, 60);
                    WebHelperUtil.SessionAdd("A_AdminName", admin.A_NAME, 60);
                    ////生成用户模型
                    //System.Web.HttpContext.Current.Session["A_AdminUser"] = new AdminUserModel()
                    //{
                    //    A_ID = admin.ID,
                    //    A_CODE = admin.A_CODE,
                    //    A_NAME = admin.A_NAME,
                    //    A_CHINESE_NAME = admin.A_TRUE_NAME,
                    //};
                    string userModle = new AdminUserModel()
                    {
                        A_ID = admin.ID,
                        A_CODE = admin.A_CODE,
                        A_NAME = admin.A_NAME,
                        A_CHINESE_NAME = admin.A_TRUE_NAME,
                    }.toJson();
                    //加密处理
                    userModle = EncryptUtil.Base64Encode(userModle);
                    WebHelperUtil.SetCookie("A_AdminUser", userModle, ExpiresDayCookiesSession);

                    //生成验证字符串cookie
                    string authStr = admin.ID + "^" + username.ToLower() + "^" + DateTime.Now.AddHours(2);
                    authStr = EncryptUtil.DESEncryptString(authStr);

                    //添加Cookie
                    WebHelperUtil.SetCookie("AdminToken", EncryptUtil.MD5(admin.ID.ToString(), 16), ExpiresDayCookiesSession);
                    WebHelperUtil.SetCookie("AdminAuth", authStr, ExpiresDayCookiesSession);
                    WebHelperUtil.SetCookie("AdminLastLogTime", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), ExpiresDayCookiesSession);
                    WebHelperUtil.SetCookie("AdminUserLogin", "login", ExpiresDayCookiesSession);
                    #endregion

                    LoginCommon.InsertAdminLoginLog(admin);
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }

        /// <summary>
        /// 功能描述：验证是否已登录
        /// 创建：  毛枫
        /// 2015年10月26日10:21:33
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="userName">用户名称</param>
        /// <returns>是否成功</returns>
        public static bool IsLogIn(ref int userId, ref string userName)
        {
            string tokenCookie = WebHelperUtil.GetCookie("AdminToken");
            string authCookie = WebHelperUtil.GetCookie("AdminAuth");
            string ckLastLogTime = WebHelperUtil.GetCookie("AdminLastLogTime");
            //验证是否存在登录状态cookie
            if (string.IsNullOrEmpty(tokenCookie))
            {
                return false;
            }
            ////验证当前站点Session
            ////当前站点已登录，存在session
            AdminUserModel loginInfo = GetCurrentAdminUser();
            if (!string.IsNullOrEmpty(loginInfo.A_NAME))
            {
                userId = Convert.ToInt32(loginInfo.A_ID);
                userName = loginInfo.A_NAME;
            }
            else
            {
                return false;
            }
            //if (HttpContext.Current.Session["A_Id"] != null)
            //{
            //    userId = Convert.ToInt32(HttpContext.Current.Session["A_Id"]);
            //    userName = HttpContext.Current.Session["A_AdminName"].ToString();
            //}
            //else
            //{
            //    if (string.IsNullOrEmpty(authCookie))
            //    {
            //        return false;
            //    }
            //    if (!CheckAuthInfo(authCookie, ref userId, ref userName))
            //    {
            //        return false;
            //    }

            //}
            return true;
        }

        /// <summary>
        /// 根据验证字符串获取登录信息
        /// 创建  毛枫  2015-4-21
        /// </summary>
        /// <param name="authStr"></param>
        /// <param name="userId"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        private static bool CheckAuthInfo(string authStr, ref int userId, ref string userName)
        {
            //解密验证字符串
            string decrypt = EncryptUtil.DESDecryptString(authStr);
            if (string.IsNullOrEmpty(decrypt))
                return false;
            string[] infoTmp = decrypt.Split('^');
            if (infoTmp.Length < 3)
                return false;

            try
            {
                userId = Convert.ToInt32(infoTmp[0]);
                userName = infoTmp[1];
                DateTime expireTime = Convert.ToDateTime(infoTmp[2]);
                //判断该验证字符串是否过期
                if (expireTime.CompareTo(DateTime.Now) < 0)
                {
                    return false;
                }
                else
                {
                    if (System.Web.HttpContext.Current.Session["A_Id"] != null && Convert.ToInt32(System.Web.HttpContext.Current.Session["A_Id"]) == userId
                        && System.Web.HttpContext.Current.Session["A_AdminName"] != null && System.Web.HttpContext.Current.Session["A_AdminName"].ToString() == userName)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 获取用户模型
        /// 创建  毛枫  2015-4-21
        /// </summary>
        /// <returns></returns>
        public static AdminUserModel GetCurrentAdminUser()
        {
            AdminUserModel returnModel = new AdminUserModel();
            //var model = System.Web.HttpContext.Current.Session["A_AdminUser"];
            var model = HttpContext.Current.Request.Cookies["A_AdminUser"];
            if (model != null)
            {
                //解密处理
                string result = EncryptUtil.Base64Decode(model.Value);
                returnModel = result.toJsonObject<AdminUserModel>();
            }
            return returnModel;
        }

        /// <summary>
        /// 退出登录
        /// 创建  毛枫  2015-4-21
        /// </summary>
        public static void LogOut()
        {
            WebHelperUtil.ClearCookie("AdminToken");
            WebHelperUtil.ClearCookie("AdminAuth");
            WebHelperUtil.ClearCookie("AdminLastLogTime");
            WebHelperUtil.ClearCookie("AdminUserLogin");
            WebHelperUtil.ClearCookie("A_AdminUser");
            ClearUserPower();
            //回到登陆页面
            PageContext.Redirect("/login.aspx");

        }

        /// <summary>
        /// 校验登入用户是否拥有页面权限
        /// 创建  毛枫  2015-4-21
        /// </summary>
        /// <param name="viewPower"></param>
        /// <returns></returns>
        public static bool CompareRole(string viewPower)
        {
            //HttpContext context = HttpContext.Current;

            List<T_POWERS> adminPowersList = new List<T_POWERS>();
            bool result = false;
            int userId = 0;
            string userName = string.Empty;
            if (HttpContext.Current.Session["A_Power"] == null)
            {
                //如果A_Power的Session是为空的话就首先判断当前用户是否登录
                if (IsLogIn(ref userId, ref userName))
                {
                    AdminUserModel user = GetCurrentAdminUser();
                    string A_Code = user.A_CODE;
                    //获取当前用户角色CODE
                    List<string> userRoleList = GetUserRole(A_Code);
                    foreach (string q in userRoleList)
                    {
                        //获取角色对应权限
                        adminPowersList.AddRange(GetUserPower(q));
                    }
                    //放入Session中
                }
                else
                {
                    return false;
                }
            }
            else
            {
                //存在Session
                string powerList = WebHelperUtil.SessionGet("A_Power");
                adminPowersList = powerList.toJsonObject<List<T_POWERS>>();
            }

            if (adminPowersList != null)
            {
                //将权限Model放入SESSION中
                string powerList = adminPowersList.toJson();
                WebHelperUtil.SessionAdd("A_Power", powerList, 60);
            }
            //判断登入用户有无页面权限
            if (adminPowersList.Any(qq => qq.P_NAME == viewPower))
            {
                result = true;
            }
            return result;
        }

        /// <summary>
        /// 根据登入的用户ID，获取用户角色ID列表
        /// 创建  金协民  2015-7-3
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        private static List<string> GetUserRole(string adminCode)
        {
            List<string> RoleList = new List<string>();
            var query = new DapperExQuery<T_ADMIN_ROLES>().AndWhere(n => n.A_CODE, OperationMethod.Equal, adminCode);
            List<T_ADMIN_ROLES> ROLE_USERS_LIST = new List<T_ADMIN_ROLES>();
            //获取用户角色CODE列表
            ROLE_USERS_LIST = new T_ADMIN_ROLES_BLL().GetAllList(query);
            if (ROLE_USERS_LIST != null)
            {
                RoleList.AddRange(ROLE_USERS_LIST.Select(q => q.R_CODE));
            }
            else
            {
                RoleList = null;
            }
            return RoleList;
        }

        /// <summary>
        /// 根据用户角色code来获取权限列表
        /// 创建  毛枫  2015-4-21
        /// </summary>
        /// <param name="RoleId"></param>
        /// <returns></returns>
        public static List<T_POWERS> GetUserPower(string r_code)
        {
            List<T_POWERS> T_POWERS_LIST = new List<T_POWERS>();
            List<T_ROLES_POWERS> T_ROLES_POWER_LIST = new List<Mammothcode.Model.T_ROLES_POWERS>();
            var query = new DapperExQuery<T_ROLES_POWERS>().AndWhere(n => n.R_CODE, OperationMethod.Equal, r_code);
            //获取权限列表
            T_ROLES_POWER_LIST = T_ROLES_POWERS_BLL.GetAllList(query);
            if (T_ROLES_POWER_LIST != null)
            {
                foreach (T_ROLES_POWERS q in T_ROLES_POWER_LIST)
                {
                    string powerCode = q.P_CODE;
                    var querypower = new DapperExQuery<T_POWERS>().AndWhere(n => n.P_CODE, OperationMethod.Equal, powerCode);
                    T_POWERS T_POWERS = new T_POWERS();
                    //获取权限model
                    T_POWERS = T_POWERS_BLL.GetEntity(querypower);
                    T_POWERS_LIST.Add(T_POWERS);
                }

            }
            else
            {
                T_POWERS_LIST = null;
            }
            return T_POWERS_LIST;
        }

        /// <summary>
        /// 清空保存到服务器Session中的权限
        /// 创建人:孙佳杰  创建时间:2015.5.8
        /// </summary>
        public static void ClearUserPower()
        {
            HttpContext.Current.Session.Remove("A_Power");
        }
        #endregion

        #region 从数据库中判断用户是否登录

        /// <summary>
        /// 私有方法：从数据库中判断用户是否登录
        /// 创建  毛枫  2015-4-21
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private static T_ADMIN IsLoginFromSql(string username, string password)
        {
            T_ADMIN_BLL T_ADMIN_BLL = new T_ADMIN_BLL();
            var query = new DapperExQuery<T_ADMIN>().AndWhere(n => n.A_NAME, OperationMethod.Equal, username)
                .AndWhere(n => n.PASSWORD, OperationMethod.Equal, EncryptUtil.Md5Encode(password.Trim(), 16));
            var admin = T_ADMIN_BLL.GetEntity(query);
            return admin;
        }

        #endregion

    }
}