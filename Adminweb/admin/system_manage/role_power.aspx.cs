using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FineUI;
using Mammothcode.BLL;
using Mammothcode.Model;
using Mammothcode.Public.Data;
using Mammothcode.UICommon.Common.AdminCenter;
using AspNet = System.Web.UI.WebControls;

namespace Mammothcode.Demo.Adminweb.admin.system_manage
{
    /// <summary>
    /// 角色关联权限
    /// </summary>
    public partial class role_power : System.Web.UI.Page
    {
        #region  全局变量
        /// <summary>
        /// 定义字典   暂存原有数据
        /// 修改人：金协民 2015年7月29日
        /// </summary>
        private Dictionary<string, bool> _currentRolePowers = new Dictionary<string, bool>();
        protected T_ROLES_POWERS_BLL T_ROLES_POWERS_BLL = new T_ROLES_POWERS_BLL();
        protected T_POWERS_BLL T_POWERS_BLL = new T_POWERS_BLL();
        protected T_ROLES_BLL T_ROLES_BLL = new T_ROLES_BLL();
        //权限相关操作
        protected static AdminwebAuthorizeAttribute power = new AdminwebAuthorizeAttribute();
        #endregion

        #region  主函数
        /// <summary>
        /// 主函数
        /// 修改人：金协民 2015年7月29日
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //设置页面权限
            power.SetViewPower("mod_role_power");
            //验证权限
            if (power.VerifyPower() == false)
            {
                return;
            }
            if (!IsPostBack)
            {
                LoadData();
            }
        }
        #endregion

        //=========数据绑定

        #region 初始化数据绑定
        /// <summary>
        /// 数据绑定
        /// 修改人：金协民 2015年7月29日
        /// </summary>
        private void LoadData()
        {
            BindGrid();
            // 默认选中第一个角色
            Grid1.SelectedRowIndex = 0;
            BindGrid2();
        }
        #endregion

        #region 左侧导航 数据绑定
        /// <summary>
        /// 左侧导航 数据绑定
        /// 修改人：金协民 2015年7月29日
        /// </summary>
        private void BindGrid()
        {
            // 排列
            var str = T_ROLES_BLL.GetAllList();
            Grid1.DataSource = str;
            Grid1.DataBind();
        }
        #endregion

        #region 右侧分组权限绑定，并显示对应角色有哪些权限
        /// <summary>
        ///右侧分组权限绑定，并显示对应角色有哪些权限
        /// 修改人：金协民 2015年7月29日
        /// </summary>
        private void BindGrid2()
        {
            var roleId = GetSelectedDataKeyID(Grid1);
            if (string.IsNullOrEmpty(roleId))
            {
                Grid2.DataSource = null;
                Grid2.DataBind();
            }
            else
            {
                // 当前选中角色拥有的权限列表
                _currentRolePowers.Clear();
                List<T_ROLES_POWERS> T_ADMIN_ROLE_POWER_LIST = new List<T_ROLES_POWERS>();
                var query = new DapperExQuery<T_ROLES_POWERS>().AndWhere(n => n.R_CODE, OperationMethod.Equal, roleId);
                //拿到角色对应的权限，标记为选中
                T_ADMIN_ROLE_POWER_LIST = T_ROLES_POWERS_BLL.GetAllList(query);
                if (T_ADMIN_ROLE_POWER_LIST != null)
                {
                    foreach (var power in T_ADMIN_ROLE_POWER_LIST)
                    {
                        var powersId = power.P_CODE;
                        var query1 = new DapperExQuery<T_POWERS>().AndWhere(n => n.P_CODE, OperationMethod.Equal, powersId);
                        T_POWERS T_POWERS = new Model.T_POWERS();
                        T_POWERS = T_POWERS_BLL.GetEntity(query1);
                        string powerName = T_POWERS.P_NAME;
                        if (!_currentRolePowers.ContainsKey(powerName))
                        {
                            _currentRolePowers.Add(powerName, true);
                        }
                    }
                }
                //分组拿到所有权限
                var q = T_POWERS_BLL.GetAllList().GroupBy(n => n.GROUP_NAME);
                //自定数据结构，包括组名，组内权限集
                var powers = q.Select(g => new
                {
                    GroupName = g.Key,
                    Powers = g
                });
                Grid2.DataSource = powers;
                Grid2.DataBind();
            }

        }

        #endregion

        //============ GRID事件==========
        #region  左侧角色导航相关事件
        /// <summary>
        /// 刷新右侧权限列表
        /// 修改人：金协民 2015年7月29日
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Grid1_RowClick(object sender, GridRowClickEventArgs e)
        {
            BindGrid2();
        }

        /// <summary>
        /// 列表排序
        /// 修改人：金协民 2015年7月29日
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Grid1_Sort(object sender, GridSortEventArgs e)
        {
            Grid1.SortDirection = e.SortDirection;
            Grid1.SortField = e.SortField;
            BindGrid();
            // 默认选中第一个角色
            Grid1.SelectedRowIndex = 0;

            BindGrid2();
        }

        /// <summary>
        /// 获取选中角色的ID
        /// 修改人：金协民 2015年7月29日
        /// </summary>
        /// <param name="grid"></param>
        /// <returns></returns>
        public string GetSelectedDataKeyID(Grid grid)
        {
            var id = string.Empty;
            int rowIndex = grid.SelectedRowIndex;
            if (rowIndex >= 0)
            {
                id = grid.DataKeys[rowIndex][0].ToString();
            }
            return id;
        }
        #endregion

        #region 列表相关事件

        /// <summary>
        /// 分组绑定右侧权限
        /// 修改人：金协民 2015年7月29日
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Grid2_RowDataBound(object sender, GridRowEventArgs e)
        {
            AspNet.CheckBoxList ddlPowers = (AspNet.CheckBoxList)Grid2.Rows[e.RowIndex].FindControl("ddlPowers");
            var GroupName = e.DataItem.GetType().GetProperty("GroupName").GetValue(e.DataItem, null);
            List<T_POWERS> T_POWERS_LIST = new List<T_POWERS>();
            var query = new DapperExQuery<T_POWERS>().AndWhere(n => n.GROUP_NAME, OperationMethod.Equal, GroupName);
            T_POWERS_LIST = T_POWERS_BLL.GetAllList(query);
            foreach (var po in T_POWERS_LIST)
            {
                AspNet.ListItem item = new AspNet.ListItem
                {
                    Value = po.P_CODE.ToString(),
                    Text = string.Format("{0}({1})", po.P_CHINESE_NAME, po.P_NAME)
                };
                item.Attributes["data-qtip"] = po.P_NAME;
                if (_currentRolePowers.ContainsKey(po.P_NAME))
                {
                    item.Selected = true;
                }
                else
                {
                    item.Selected = false;
                }
                ddlPowers.Items.Add(item);
            }
        }

        /// <summary>
        /// 权限列表排序
        /// 修改人：金协民 2015年7月29日
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Grid2_Sort(object sender, GridSortEventArgs e)
        {
            Grid2.SortDirection = e.SortDirection;
            Grid2.SortField = e.SortField;
            BindGrid2();
        }

        /// <summary>
        /// 编辑角色对应的权限 （待优化）
        /// 修改人：金协民 2015年7月29日
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGroupUpdate_Click(object sender, EventArgs e)
        {
            var currentUser = AdminwebUserManager.GetCurrentAdminUser();
            var roleId = GetSelectedDataKeyID(Grid1);
            if (string.IsNullOrEmpty(roleId))
            {
                return;
            }
            // 当前角色新的权限列表
            List<string> newPowerIDs = new List<string>();
            for (int i = 0; i < Grid2.Rows.Count; i++)
            {
                AspNet.CheckBoxList ddlPowers = (AspNet.CheckBoxList)Grid2.Rows[i].FindControl("ddlPowers");
                foreach (AspNet.ListItem item in ddlPowers.Items)
                {
                    if (item.Selected)
                    {
                        newPowerIDs.Add((item.Value));
                    }
                }
            }
            //删除原有权限
            var query = new DapperExQuery<T_ROLES_POWERS>().AndWhere(n => n.R_CODE, OperationMethod.Equal, roleId);
            if (T_ROLES_POWERS_BLL.GetEntity(query) != null)
            {
                T_ROLES_POWERS_BLL.Delete(query);
            }
            int newPowerLen = newPowerIDs.Count;
            //新增权限
            for (var i = 0; i < newPowerLen; i++)
            {
                T_ROLES_POWERS T_ROLES_POWERS = new T_ROLES_POWERS
                {
                    P_CODE = newPowerIDs[i],
                    R_CODE = roleId,
                    CREATE_TIME = DateTime.Now,
                    CREATE_USER = currentUser.A_NAME,
                    CREATE_USER_NAME = currentUser.A_CHINESE_NAME
                };
                T_ROLES_POWERS_BLL.Add(T_ROLES_POWERS);
            }
            //Alert.Show(str);
        }
        #endregion
    }
}