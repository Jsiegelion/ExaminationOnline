using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mammothcode.UICommon.Common.AdminCenter;

namespace Mammothcode.Demo.Adminweb
{
    public partial class login : System.Web.UI.Page
    { 
        #region 声明

        /// <summary>
        /// BLL
        /// 创建人：林以恒
        /// 2015年7月6日20:55:17
        /// </summary>
        private const string LoginInRedirectUrl = "/main.aspx";

        #endregion

        #region 主函数

        /// <summary>
        /// 主函数
        /// 创建人：林以恒
        /// 2015年7月27日16:50:55
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            string act = Request["act"];
            if (act != "userlogin") return;
            string username = Request["username"];
            string password = Request["password"];
            bool? isSaveAccount = null;
            if (Request["isSaveAccount"] != null)
            {
                isSaveAccount = bool.Parse(Request["isSaveAccount"]);
            }
                    
            if (username != null && password != null && username != "" && password != "")
            {
                UserLogin(username, password, isSaveAccount);
            }
        }

        #endregion

        #region 用户登入

        /// <summary>
        /// 系统管理模块（用户登入）
        /// </summary>
        /// <returns></returns>
        private void UserLogin(string username,string password,bool ? isSaveAccount)
        {
            if (isSaveAccount != null && isSaveAccount.Value)
            {
                if (AdminwebUserManager.AdminLoginIn(username, password))
                {
                    Response.Write(LoginInRedirectUrl);
                    Response.End();
                }
                else
                {
                    Response.Write("/");
                    Response.End();
                }
            }
            else
            {
                if (AdminwebUserManager.AdminLoginIn(username, password))
                {
                    Response.Write(LoginInRedirectUrl);
                    Response.End();
                }
                else
                {
                    Response.Write("/");
                    Response.End();
                }
            } 
        }

        #endregion 
    }
}