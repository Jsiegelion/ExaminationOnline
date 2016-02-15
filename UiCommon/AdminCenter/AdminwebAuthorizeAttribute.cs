using System.Web;

namespace Mammothcode.UICommon.Common.AdminCenter
{
    public class AdminwebAuthorizeAttribute
    {
        /// <summary>
        /// 页面权限
        /// </summary>
        private string ViewPower { get; set; }

        /// <summary>
        /// 设置页面权限
        /// </summary>
        /// <param name="viewPower"></param>
        public void SetViewPower(string viewPower = "")
        {
            this.ViewPower = viewPower;
        }

        /// <summary>
        /// 验证授权。
        /// </summary>
        /// <returns>如果用户已经过授权，则为 true；否则为 false。</returns>
        public bool VerifyPower()
        {
            bool result = false;
            int userId = 0;
            string userName = string.Empty;
            if (AdminwebUserManager.IsLogIn(ref userId, ref userName))
            {
                if (!string.IsNullOrEmpty(ViewPower))
                {
                    if (AdminwebUserManager.CompareRole(ViewPower))
                    {
                        result = true;
                    }
                    else
                    {
                        //没有权限，重定向到404页面
                        HttpContext.Current.Response.Redirect("/login.aspx");
                        //FormsAuthentication.RedirectToLoginPage();
                    }
                }
            }
            else
            {
                //没有登入，重定向到登入页面
                HttpContext.Current.Response.Redirect("/login.aspx");
            }
            return result;
        }
    }
}