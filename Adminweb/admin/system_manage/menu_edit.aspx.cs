using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FineUI;
using Mammothcode.BLL;
using Mammothcode.Core.Data.DataAnaly;
using Mammothcode.Model;
using Mammothcode.Public.Data;
using Mammothcode.UICommon.Common.AdminCenter;

namespace Mammothcode.Demo.Adminweb.admin.system_manage
{
    /// <summary>
    /// 菜单修改
    /// </summary>
    public partial class menu_edit : System.Web.UI.Page
    {
        #region 声明
        /// <summary>
        /// BLL mod_menu
        /// 创建人：林以恒
        /// 2015年7月6日18:21:29
        /// </summary>

        private readonly T_ADMIN_MENUS_BLL _adminMenusBll = new T_ADMIN_MENUS_BLL();
        T_ADMIN_MENUS _adminMenus = new T_ADMIN_MENUS();
        private readonly T_POWERS_BLL _powersBll = new T_POWERS_BLL();

        //权限相关操作
        private static readonly AdminwebAuthorizeAttribute Power = new AdminwebAuthorizeAttribute();
        #endregion

        #region 主函数

        /// <summary>
        /// 数据绑定
        /// 创建 林以恒 2015年4月14日
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {

            //设置页面权限
            Power.SetViewPower("mod_menu");
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

        #region 数据绑定

        /// <summary>
        /// 修改数据绑定
        /// 创建 林以恒 
        /// 2015年4月14日
        /// </summary>
        private void LoadData()
        {
            if (!Request.QueryString["id"].IsNum()) return;
            string id = Request.QueryString["id"].ToString();
            var query = new DapperExQuery<T_ADMIN_MENUS>().AndWhere(n => n.ID, OperationMethod.Equal, id);
            _adminMenus = _adminMenusBll.GetEntity(query);
            tbxAM_NAME.Text = (_adminMenus.AM_NAME != null ? _adminMenus.AM_NAME.ToString() : "");
            tbxAM_NAVIGATE_URL.Text = (_adminMenus.AM_NAVIGATE_URL != null ? _adminMenus.AM_NAVIGATE_URL.ToString() : "");
            tbxAM_REMARK.Text = (_adminMenus.AM_REMARK != null ? _adminMenus.AM_REMARK.ToString() : "");
            tbxAM_SORTINDEX.Text = _adminMenus.AM_SORTINDEX.ToString();
            if (_adminMenus.P_CODE != null && _adminMenus.P_CODE != "")
            {
                tbxVIEWPOWER_ID.Text = _powersBll.GetEntity(new DapperExQuery<T_POWERS>().AndWhere(n => n.P_CODE, OperationMethod.Equal, _adminMenus.P_CODE)).P_NAME;
            }
        }

        #endregion

        //============操作方法===========
        #region 数据保存

        /// <summary>
        /// 数据保存按钮事件
        /// 创建 林以恒 
        /// 2015年4月14日
        /// </summary>
        protected void btnSaveClose_Click(object sender, EventArgs e)
        {
            ////设置页面权限
            //Power.SetViewPower("mod_menu");
            ////验证权限
            //if (Power.VerifyPower() == false)
            //{
            //    return;
            //}
            string str;
            if (Request.QueryString["id"].IsNum())
            {
                string id = Request.QueryString["id"].ToString();
                //修改
                T_ADMIN_MENUS T_ADMIN_MENUS = new T_ADMIN_MENUS();
                var q = new DapperExQuery<T_ADMIN_MENUS>().AndWhere(n => n.ID, OperationMethod.Equal, id);
                T_ADMIN_MENUS = _adminMenusBll.GetEntity(q);
                T_ADMIN_MENUS = GetnewModel(T_ADMIN_MENUS);
                str = _adminMenusBll.Update(T_ADMIN_MENUS) ? "修改成功！" : "修改失败！";
            }
            else
            {
                //添加
                T_ADMIN_MENUS adminMenus = new T_ADMIN_MENUS();
                adminMenus = GetnewModel(adminMenus);
                adminMenus.PARENT_ID = Int32.Parse(Request.QueryString["fatherId"].ToString());
                str = _adminMenusBll.Add(adminMenus) ? "添加成功！" : "添加失败！";
            }
            // 2. 关闭本窗体，然后刷新父窗体
            PageContext.RegisterStartupScript(ActiveWindow.GetHideRefreshReference());
            Alert.ShowInTop(str);
        }

        /// <summary>
        /// 更新实体
        /// 创建人：林以恒
        /// 2015年7月6日18:16:18
        /// </summary>
        /// <param name="adminMenus"></param>
        /// <returns></returns>
        private T_ADMIN_MENUS GetnewModel(T_ADMIN_MENUS adminMenus)
        {
            adminMenus.AM_NAME = tbxAM_NAME.Text.Trim();
            adminMenus.AM_NAVIGATE_URL = tbxAM_NAVIGATE_URL.Text.Trim();
            adminMenus.AM_REMARK = tbxAM_REMARK.Text.Trim();
            adminMenus.AM_SORTINDEX = int.Parse(tbxAM_SORTINDEX.Text.Trim());
            var queryPower = new DapperExQuery<T_POWERS>().AndWhere(n => n.P_NAME, OperationMethod.Equal, tbxVIEWPOWER_ID.Text.ToString());
            var powerid = _powersBll.GetEntity(queryPower);
            adminMenus.P_CODE= powerid != null ? powerid.P_CODE : "";
            adminMenus.AM_ISTREELEAF = tbxAM_NAVIGATE_URL.Text.Trim() == "" ? 0 : 1;
            if (adminMenus.ID == 0)
            {
                //获取登入用户信息
                var AdminUserModel = AdminwebUserManager.GetCurrentAdminUser();
                if (AdminUserModel != null)
                {
                    adminMenus.CREATE_USER = AdminUserModel.A_NAME;
                    adminMenus.CREATE_USER_NAME = AdminUserModel.A_CHINESE_NAME;
                }
                adminMenus.CREATE_TIME = DateTime.Now;
            }
            return adminMenus;
        }
        #endregion

        #region 关闭

        /// <summary>
        /// 关闭窗体
        /// 创建人：林以恒
        /// 2015年7月6日18:17:36
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnClose_Click(object sender, EventArgs e)
        {
            // 2. 关闭本窗体，然后刷新父窗体
            PageContext.RegisterStartupScript(ActiveWindow.GetHideRefreshReference());
        }
        #endregion
    }
}