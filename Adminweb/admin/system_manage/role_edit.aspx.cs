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
using Mammothcode.UICommon.Common.AdminCenter;

namespace Mammothcode.Demo.Adminweb.admin.system_manage
{
    /// <summary>
    /// 角色修改
    /// </summary>
    public partial class role_edit : System.Web.UI.Page
    {
        #region 声明

        /// <summary>
        /// BLL
        /// 创建人：林以恒
        /// 2015年8月1日22:32:47
        /// </summary>
 
        private readonly T_ROLES_BLL _rolesBll = new T_ROLES_BLL();

        //权限相关操作
        private static readonly AdminwebAuthorizeAttribute Power = new AdminwebAuthorizeAttribute();
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
            //设置页面权限
            Power.SetViewPower("mod_role");
            //验证权限
            if (Power.VerifyPower() == false)
            {
                return;
            }
            //首次加载执行
            if (!IsPostBack)
            {
                LoadData();
            }
        }
        #endregion

        //=========数据绑定

        #region 修改数据绑定
        /// <summary>
        /// 数据绑定
        /// 创建人：林以恒
        /// 2015年7月12日15:21:16
        /// </summary>
        private void LoadData()
        {
            if (Request.QueryString["id"].IsNum())
            {
                T_ROLES roles = new T_ROLES();
                string id = Request.QueryString["id"].ToString();
                var query = new DapperExQuery<T_ROLES>().AndWhere(n => n.ID, OperationMethod.Equal, Int32.Parse(id));
                roles = _rolesBll.GetEntity(query);
                tbxR_Name.Text = roles.R_NAME.ToString();
                //tbxAD_REMARK.Text = T_ADMIN_ROLES.AD_REMARK.ToString();
            }
        }
        #endregion

        //============操作方法===========
        #region 数据保存按钮事件

        protected void btnSaveClose_Click(object sender, EventArgs e)
        {
            ////设置页面权限
            //Power.SetViewPower("mod_role");
            ////验证权限
            //if (Power.VerifyPower() == false)
            //{
            //    return;
            //}
            string str;
            if (Request.QueryString["id"].IsNum())
            {
                T_ROLES roles = new T_ROLES();
                string id = Request.QueryString["id"].ToString();
                //修改
                var query = new DapperExQuery<T_ROLES>().AndWhere(n => n.ID, OperationMethod.Equal, Int32.Parse(id));
                roles = _rolesBll.GetEntity(query);
                roles = Save(roles);
                str = _rolesBll.Update(roles) ? "修改成功！" : "修改失败！";
            }
            else
            {
                T_ROLES roles = new T_ROLES();
                //添加
                roles = Save(roles);
                str = _rolesBll.Add(roles) ? "添加成功！" : "添加失败！";
            }
            // 2. 关闭本窗体，然后刷新父窗体
            PageContext.RegisterStartupScript(ActiveWindow.GetHideRefreshReference());
            Alert.ShowInTop(str);
        }

        /// <summary>
        /// 更新实体
        /// 创建人：林以恒
        /// 2015年7月6日21:49:09
        /// </summary>
        /// <param name="roles"></param>
        /// <returns></returns>
        private T_ROLES Save(T_ROLES roles)
        {
            roles.R_NAME = tbxR_Name.Text.Trim();
            if (roles.ID == 0)
            {
                roles.CREATE_TIME = DateTime.Now;
                roles.R_CODE = StringRandomUtil.GuidTo16String();
                var creatAdminUser = AdminwebUserManager.GetCurrentAdminUser();
                if (creatAdminUser != null)
                {
                    roles.CREATE_USER = creatAdminUser.A_NAME;
                    roles.CREATE_USER_NAME = creatAdminUser.A_CHINESE_NAME;
                }
            }
            return roles;
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
            PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
        }
        #endregion
    }
}