using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FineUI;
using Mammothcode.BLL;
using Mammothcode.Core.Data.DataAnaly;
using Mammothcode.Core.Data.DataAutomatic;
using Mammothcode.Model;
using Mammothcode.Public.Data;
using Mammothcode.Core.Data.DataSecurity;
using Mammothcode.UICommon.Common.AdminCenter;

namespace Mammothcode.Demo.Adminweb.admin.system_manage
{
    /// <summary>
    /// 权限绑定 mod_admin_user
    /// 创建人：林以恒
    /// 2015年8月1日21:33:16
    /// </summary>
    public partial class admin_user_edit : System.Web.UI.Page
    {
        #region 声明

        /// <summary>
        /// BLL 表：后台用户表
        /// 创建人：林以恒
        /// 2015年7月12日12:24:21
        /// </summary>
        private readonly T_ADMIN_BLL _adminUserBll = new T_ADMIN_BLL();

        //权限相关操作
        private static AdminwebAuthorizeAttribute power = new AdminwebAuthorizeAttribute();

        #endregion

        #region 主函数

        /// <summary>
        /// 主函数
        /// 创建人：l林以恒 2015年7月2日16:56:57
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            ////设置页面权限
            //power.SetViewPower("mod_admin");
            ////验证权限
            //if (power.VerifyPower() == false)
            //{
            //    return;
            //}
            //首次加载执行
            if (!IsPostBack)
            {
                LoadData();
            }
        }
        #endregion

        //=========数据绑定

        #region 数据绑定
        /// <summary>
        /// 数据绑定
        /// 创建人：林以恒
        /// 2015年7月6日14:09:00
        /// </summary>
        private void LoadData()
        {
            if (!Request.QueryString["id"].IsNum()) return;
            string id = Request.QueryString["id"];
            var query = new DapperExQuery<T_ADMIN>().AndWhere(n => n.ID, OperationMethod.Equal, Int32.Parse(id));
            var adminUser = _adminUserBll.GetEntity(query);
            tbxA_NAME.Text = adminUser.A_NAME;
            tbxA_CHINESE_NAME.Text = adminUser.A_TRUE_NAME;
            tbxPassword.Text = adminUser.PASSWORD;
            tbxCfm_Password.Text = adminUser.PASSWORD;
            tbxPhone.Text = adminUser.A_PHONE;
            if (adminUser.A_GENDER == 1)
            {
                rbtnFirst.Checked = true;
            }
            else
            {
                rbtnSecond.Checked = true;
            }
            //tbxAD_REMARK.Text = T_ADMIN_ROLES.AD_REMARK.ToString();
        }
        #endregion

        //============操作方法===========
        #region 数据保存按钮事件
        /// <summary>
        /// 保存
        /// 创建人：林以恒
        /// 2015年7月6日14:24:11
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSaveClose_Click(object sender, EventArgs e)
        {
            //设置页面权限
            power.SetViewPower("mod_admin");
            //验证权限
            if (power.VerifyPower() == false)
            {
                return;
            }
            string str;
            if (Request.QueryString["id"].IsNum())
            {
                string id = Request.QueryString["id"];
                //修改
                var query = new DapperExQuery<T_ADMIN>().AndWhere(n => n.ID, OperationMethod.Equal, int.Parse(id));
                var adminUser = _adminUserBll.GetEntity(query);
                if (tbxPassword.Text != adminUser.PASSWORD && tbxPassword.Text == tbxCfm_Password.Text)
                {
                    adminUser.PASSWORD = EncryptUtil.Md5Encode(tbxPassword.Text.Trim(), 16);
                }
                else if (tbxPassword.Text != adminUser.PASSWORD && tbxPassword.Text != tbxCfm_Password.Text)
                {
                    Alert.Show("确认密码与填写密码不匹配");
                }
                adminUser = Save(adminUser);

                str = _adminUserBll.Update(adminUser) ? "修改成功！" : "修改失败！";
            }
            else
            {
                T_ADMIN adminUser = new T_ADMIN();
                //添加
                adminUser = Save(adminUser);
                if (tbxPassword.Text != tbxCfm_Password.Text)
                {
                    Alert.Show("确认密码与填写密码不匹配");
                }
                else
                {
                    adminUser.PASSWORD = EncryptUtil.Md5Encode(tbxPassword.Text.Trim(), 16);
                }
                str = _adminUserBll.Add(adminUser) ? "添加成功！" : "添加失败！";
            }
            // 2. 关闭本窗体，然后刷新父窗体
            PageContext.RegisterStartupScript(ActiveWindow.GetHideRefreshReference());
            Alert.ShowInTop(str);
        }


        /// <summary>
        /// 更新用户实体
        /// 创建人：林以恒
        /// 2015年7月25日10:22:06
        /// 修改：密码MD5
        /// </summary>
        /// <param name="adminUser">用户实体</param>
        /// <returns></returns>
        private T_ADMIN Save(T_ADMIN adminUser)
        {
            adminUser.A_NAME = tbxA_NAME.Text.Trim();
            adminUser.A_TRUE_NAME = tbxA_CHINESE_NAME.Text.Trim();
            adminUser.A_PHONE = tbxPhone.Text.Trim();
            adminUser.A_GENDER = rbtnFirst.Checked ? 1 : 0;
            if (adminUser.ID != 0) return adminUser;
            //添加随机验证码
            adminUser.A_CODE = StringRandomUtil.GuidTo16String();
            //获取登入用户信息
            var adminUserModel = AdminwebUserManager.GetCurrentAdminUser();
            if (adminUserModel != null)
            {
                adminUser.CREATE_USER = adminUserModel.A_NAME;
                adminUser.CREATE_USER_NAME = adminUserModel.A_CHINESE_NAME;
            }
            adminUser.CREATE_TIME = DateTime.Now;
            return adminUser;
        }
        #endregion

        #region 关闭
        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnClose_Click(object sender, EventArgs e)
        {
            // 2. 关闭本窗体，然后返回父窗体
            PageContext.RegisterStartupScript(ActiveWindow.GetHideRefreshReference());
        }
        #endregion
    }
}