using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FineUI;
using Mammothcode.BLL;
using Mammothcode.BLL.Bll_Custom;
using Mammothcode.BLL.Tool;
using Mammothcode.Core.Data.DataSecurity;
using Mammothcode.Middle.Core.Tool.FineUI;
using Mammothcode.Model;
using Mammothcode.Model.Model_Custom;
using Mammothcode.Public.Data;
using Mammothcode.UICommon.Common.AdminCenter;
using Newtonsoft.Json.Linq;

namespace Mammothcode.Demo.Adminweb
{
    public partial class main : System.Web.UI.Page
    {
        /// <summary>
        /// 创建人：金协民 日期：2015年7月3日
        /// </summary>
        protected T_ADMIN_MENUS_BLL T_ADMIN_MENUS_BLL = new T_ADMIN_MENUS_BLL();
        protected static T_POWERS_BLL T_POWERS_BLL = new T_POWERS_BLL();

        //权限相关操作
        private static readonly AdminwebAuthorizeAttribute power = new AdminwebAuthorizeAttribute();

        protected void Page_Load(object sender, EventArgs e)
        {
            //设置页面权限
            power.SetViewPower("mod_main");
            //验证权限
            if (power.VerifyPower() == false)
            {
                return;
            }
            //验证权限      
            bindLogUser();
            JObject ids = GetClientIDS(regionPanel, regionTop, mainTabStrip, txtUser, txtChineseName, txtCurrentTime,
                btnRefresh);
            List<T_ADMIN_MENUS> T_ADMIN_MENUS_LIST = new List<T_ADMIN_MENUS>();
            T_ADMIN_MENUS_LIST = Get_PowerMenu();
            //T_ADMIN_MENUS_LIST = T_ADMIN_MENUS_BLL.GetAllList();
            Tree treeMenu = FineUICreateTree.InitTreeMenu(T_ADMIN_MENUS_LIST, mainRegion.IFrameName);
            regionLeft.Items.Add(treeMenu);
            ids.Add("treeMenu", treeMenu.ClientID);
            ids.Add("menuType", "menu");
            string idsScriptStr = String.Format("window.DATA={0};", ids.ToString(Newtonsoft.Json.Formatting.None));
            PageContext.RegisterStartupScript(idsScriptStr);
        }

        /// <summary>
        /// 创建 毛枫  2015-4-13
        /// </summary>
        /// <param name="ctrls"></param>
        /// <returns></returns>
        private JObject GetClientIDS(params ControlBase[] ctrls)
        {
            JObject jo = new JObject();
            foreach (ControlBase ctrl in ctrls)
            {
                jo.Add(ctrl.ID, ctrl.ClientID);
            }
            return jo;
        }

        #region 退出
        /// <summary>
        /// 创建人：金协民 2015年5月16日
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExit_Click(object sender, EventArgs e)
        {
            AdminwebUserManager.LogOut();
        }
        #endregion

        #region 绑定当前登录信息

        /// <summary>
        /// 绑定当前登录信息
        /// 创建人： 林以恒
        /// 2015年7月16日15:49:35
        /// </summary>
        private void bindLogUser()
        {
            var logUser = AdminwebUserManager.GetCurrentAdminUser();
            txtUser.Text = "欢迎您：" + logUser.A_CHINESE_NAME;
            txtChineseName.Text = logUser.A_CHINESE_NAME;
        }

        #endregion

        #region  根据用户权限，获取菜单列表
        /// <summary>
        ///  根据用户权限，获取菜单列表 
        /// </summary>
        /// <returns></returns>
        public List<T_ADMIN_MENUS> Get_PowerMenu()
        {
            List<T_ADMIN_MENUS> T_ADMIN_MENUS_LIST = new List<T_ADMIN_MENUS>();
            List<T_POWERS> T_POWERS_LIST = new List<T_POWERS>();
            T_POWERS_LIST = T_POWERS_BLL.GetAllList();
            int count = T_POWERS_LIST.Count;
            int j = 0;
            for (int i = 0; i < count; i++)
            {
                if (AdminwebUserManager.CompareRole(T_POWERS_LIST[i].P_NAME))
                {
                    string p_code = T_POWERS_LIST[i].P_CODE;
                    var query = new DapperExQuery<T_ADMIN_MENUS>().AndWhere(n => n.P_CODE, OperationMethod.Equal, p_code);
                    T_ADMIN_MENUS T_ADMIN_MENUS = new T_ADMIN_MENUS();
                    T_ADMIN_MENUS = T_ADMIN_MENUS_BLL.GetEntity(query);
                    if (T_ADMIN_MENUS != null)
                    {
                        T_ADMIN_MENUS_LIST.Add(T_ADMIN_MENUS);
                    }
                }
            }
            //排序
            T_ADMIN_MENUS q = new T_ADMIN_MENUS();
            for (int i = 0; i < T_ADMIN_MENUS_LIST.Count - 1; i++)
            {

                for (j = 0; j < T_ADMIN_MENUS_LIST.Count - 1 - i; j++)
                {
                    if (T_ADMIN_MENUS_LIST[j].AM_SORTINDEX > T_ADMIN_MENUS_LIST[j + 1].AM_SORTINDEX)
                    {
                        q = T_ADMIN_MENUS_LIST[j];
                        T_ADMIN_MENUS_LIST[j] = T_ADMIN_MENUS_LIST[j + 1];
                        T_ADMIN_MENUS_LIST[j + 1] = q;
                    }
                }
            }
            //T_ADMIN_MENUS_LIST = T_ADMIN_MENUS_LIST.OrderBy(n => n.AM_SORTINDEX) as List<T_ADMIN_MENUS>;
            return T_ADMIN_MENUS_LIST;
        }
        #endregion

        #region 修改用户密码

        /// <summary>
        /// 修改用户密码
        /// 创建：金协民
        /// 时间：2016年1月15日15:37:38
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnUpdate_OnClick(object sender, EventArgs e)
        {
            updateWindow.Hidden = false;
        }

        /// <summary>
        /// 保存修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_OnClick(object sender, EventArgs e)
        {
            string message = "";
            T_ADMIN_BLL T_ADMIN_BLL = new T_ADMIN_BLL();
            try
            {
                //①获取当前登录用户
                AdminUserModel adminInfo = AdminwebUserManager.GetCurrentAdminUser();
                //②判断当前登录用户原密码
                var query = new DapperExQuery<T_ADMIN>().AndWhere(n => n.A_CODE, OperationMethod.Equal, adminInfo.A_CODE)
                    .AndWhere(n => n.PASSWORD, OperationMethod.Equal, EncryptUtil.Md5Encode(tbxOriPassword.Text.Trim(), 16));
                var entity = T_ADMIN_BLL.GetEntity(query);
                if (entity != null)
                {
                    //③判断确认密码是否等于密码
                    if (tbxPassword.Text == tbxCfm_Password.Text)
                    {
                        //④保存新密码
                        entity.PASSWORD = EncryptUtil.Md5Encode(tbxPassword.Text.Trim(), 16);

                        if (new T_ADMIN_BLL().Update(entity))
                        {
                            message = "修改成功";
                            updateWindow.Hidden = true;
                        }
                        else
                        {
                            message = "修改失败";
                        }
                    }
                    else
                    {
                        message = "确认密码错误";
                    }
                }
                else
                {
                    message = "用户密码错误,请输入原密码";
                }
            }
            catch (Exception ex)
            {
                message = ex.ToString();
            }
            Alert.Show(message);
        }

        #endregion

    }
}