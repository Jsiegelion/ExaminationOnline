using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FineUI;
using Mammothcode.BLL;
using Mammothcode.Middle.Core.Tool;
using Mammothcode.Middle.Core.Tool.FineUI;
using Mammothcode.Model;
using Mammothcode.Public.Data;
using Mammothcode.UICommon.Common.AdminCenter;

namespace Mammothcode.Demo.Adminweb.admin.system_manage
{
    /// <summary>
    /// 用户关联角色
    /// </summary>
    public partial class admin_user_manage : System.Web.UI.Page
    {
        #region  全局变量
        protected static T_ADMIN_BLL T_ADMIN_BLL = new T_ADMIN_BLL();
        protected static T_ADMIN_ROLES_BLL T_ADMIN_ROLES_BLL = new T_ADMIN_ROLES_BLL();
        protected static T_ROLES_BLL T_ROLES_BLL = new T_ROLES_BLL();

        //权限相关操作
        private static readonly AdminwebAuthorizeAttribute Power = new AdminwebAuthorizeAttribute();
        #endregion

        #region 主函数
        /// <summary>
        /// 主函数
        /// 创建  毛枫  2015-4-17
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        protected void Page_Load(object sender, EventArgs e)
        {
            //设置页面权限
            Power.SetViewPower("mod_user_role");
            //验证权限
            if (Power.VerifyPower() == false)
            {
                return;
            }
            if (!IsPostBack)
            {
                LoadData();
            }
        }
        #endregion

        #region 初始化数据绑定
        /// <summary>
        /// 数据绑定
        /// 创建  毛枫  2015-4-17
        /// </summary>
        private void LoadData()
        {

            BindGrid1();
            // 默认选中第一个角色
            Grid1.SelectedRowIndex = 0;
            // 每页记录数
            Grid2.PageSize = 10;//待修改
            //ddlGridPageSize.SelectedValue = "2";
            BindGrid2();
        }

        /// <summary>
        /// 左侧角色导航栏数据绑定
        /// 创建  毛枫  2015-4-17
        /// </summary>
        private void BindGrid1()
        {
            // 排列
            //q = Sort<Role>(q, Grid1);
            var q = T_ROLES_BLL.GetAllList();
            Grid1.DataSource = q;
            Grid1.DataBind();
        }

        /// <summary>
        /// 右侧角色对应用户的数据绑定
        /// 创建  毛枫  2015-4-17
        /// </summary>
        private void BindGrid2()
        {
            string R_CODE = FineUITable.GetSelectedDataKeyCode(Grid1);
            if (R_CODE =="")
            {
                Grid2.RecordCount = 0;
                Grid2.DataSource = null;
                Grid2.DataBind();
            }
            else
            {
                // 过滤选中角色下的所有用户
                long allcount = 0;
                var query = new DapperExQuery<T_ADMIN_ROLES>().AndWhere(n => n.R_CODE, OperationMethod.Equal, R_CODE);
                int PageIndex = Grid2.PageIndex + 1;
                int PageSize = Grid2.PageSize;
                List<T_ADMIN_ROLES> qs = T_ADMIN_ROLES_BLL.GetListByPage(query, "", PageIndex, PageSize, out allcount);

                List<T_ADMIN> q = new List<T_ADMIN>();
                foreach (T_ADMIN_ROLES t in qs)
                {
                    var query1 = new DapperExQuery<T_ADMIN>().AndWhere(n => n.A_CODE, OperationMethod.Equal, t.A_CODE);
                    T_ADMIN T_ADMIN = new T_ADMIN();
                    T_ADMIN = T_ADMIN_BLL.GetEntity(query1);
                    q.Add(T_ADMIN);
                }

                //获取总记录数
                Grid2.RecordCount = Int32.Parse(allcount.ToString());
                Grid2.DataSource = q;
                Grid2.DataBind();
            }

        }

        #endregion

        #region 左侧角色导航相关事件
        /// <summary>
        /// 左侧导航排序
        /// 创建  毛枫  2015-4-17
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Grid1_Sort(object sender, GridSortEventArgs e)
        {
            Grid1.SortDirection = e.SortDirection;
            Grid1.SortField = e.SortField;
            BindGrid1();
            // 默认选中第一个角色
            Grid1.SelectedRowIndex = 0;
            BindGrid2();
        }

        /// <summary>
        /// 更新右侧用户列表
        /// 创建  毛枫  2015-4-17
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Grid1_RowClick(object sender, GridRowClickEventArgs e)
        {
            BindGrid2();
        }
        #endregion

        protected void ttbSearchUser_Trigger2Click(object sender, EventArgs e)
        {
            //ttbSearchUser.ShowTrigger1 = true;
            BindGrid2();
        }

        /// <summary>
        /// 按用户名查询
        /// 创建  毛枫  2015-4-17
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ttbSearchUser_Trigger1Click(object sender, EventArgs e)
        {
            //ttbSearchUser.Text = String.Empty;
            //ttbSearchUser.ShowTrigger1 = false;
            BindGrid2();
        }

        /// <summary>
        /// 权限检查  （待开发）
        /// 创建  毛枫  2015-4-17
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Grid2_PreDataBound(object sender, EventArgs e)
        {
            //// 数据绑定之前，进行权限检查
            //CheckPowerWithLinkButtonField("CoreRoleUserDelete", Grid2, "deleteField");
        }

        /// <summary>
        /// 用户列表排序
        /// 创建  毛枫  2015-4-17
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
        /// 分页
        /// 创建  毛枫  2015-4-17
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Grid2_PageIndexChange(object sender, GridPageEventArgs e)
        {
            Grid2.PageIndex = e.NewPageIndex;
            BindGrid2();
        }

        /// <summary>
        /// 删除选中的用户
        /// 创建  毛枫  2015-4-17
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDeleteSelected_Click(object sender, EventArgs e)
        {
            //// 在操作之前进行权限检查
            //if (!CheckPower("CoreRoleUserDelete"))
            //{
            //    CheckPowerFailWithAlert();
            //    return;
            //}
            // 从每个选中的行中获取ID（在Grid1中定义的DataKeyNames）
            string R_CODE = FineUITable.GetSelectedDataKeyCode(Grid1);
            //T_ROLES T_ROLES = new Model.T_ROLES();
            //var queryrole = new DapperExQuery<T_ROLES>().AndWhere(n => n.ID, OperationMethod.Equal, roleID);
            //T_ROLES = T_ROLES_BLL.GetEntity(queryrole);
            //string r_code = T_ROLES.R_CODE;
           List<string> A_CODES = FineUITable.GetSelectedDataKeyCODES(Grid2);
            foreach (string  A_CODE in A_CODES)
            {
                //T_ADMIN T_ADMIN = new T_ADMIN();
                //var queryuser= new DapperExQuery<T_ADMIN>().AndWhere(n => n.ID, OperationMethod.Equal, userID);
                //T_ADMIN = T_ADMIN_BLL.GetEntity(queryuser);
                //string a_code = T_ROLES.R_CODE;
                
                var query = new DapperExQuery<T_ADMIN_ROLES>().AndWhere(n => n.R_CODE, OperationMethod.Equal, R_CODE)
              .AndWhere(n => n.A_CODE, OperationMethod.Equal,A_CODE);
                var user_role = T_ADMIN_ROLES_BLL.GetEntity(query);
                if (user_role != null)
                {
                    T_ADMIN_ROLES_BLL.Delete(query);
                }
            }

            //DB.SaveChanges();

            // 清空当前选中的项
            Grid2.SelectedRowIndexArray = null;

            // 重新绑定表格
            BindGrid2();
        }

        /// <summary>
        /// 单行删除 
        /// 创建  毛枫  2015-4-17
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Grid2_RowCommand(object sender, GridCommandEventArgs e)
        {
            object[] values = Grid2.DataKeys[e.RowIndex];
            string  A_CODE =values[1].ToString();
            //T_ADMIN T_ADMIN = new T_ADMIN();
            //var queryuser = new DapperExQuery<T_ADMIN>().AndWhere(n => n.ID, OperationMethod.Equal, userID);
            //T_ADMIN = T_ADMIN_BLL.GetEntity(queryuser);
            //string a_code = T_ADMIN.A_CODE;
            //判断点击的是删除
            if (e.CommandName == "Delete")
            {
                //// 在操作之前进行权限检查
                //if (!CheckPower("CoreRoleUserDelete"))
                //{
                //    CheckPowerFailWithAlert();
                //    return;
                //}
                string R_CODE = FineUITable.GetSelectedDataKeyCode(Grid1);
                var query = new DapperExQuery<T_ADMIN_ROLES>().AndWhere(n => n.R_CODE, OperationMethod.Equal, R_CODE)
                 .AndWhere(n => n.A_CODE, OperationMethod.Equal, A_CODE);
                var user_role = T_ADMIN_ROLES_BLL.GetEntity(query);
                if (user_role != null)
                {
                    T_ADMIN_ROLES_BLL.Delete(query);
                }
                //绑定刷新
                BindGrid2();
            }
        }

        /// <summary>
        /// 关闭弹出框
        /// 创建  毛枫  2015-4-17
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Window1_Close(object sender, EventArgs e)
        {
            BindGrid2();
        }

        /// <summary>
        /// 新增角色对应用户
        /// 创建  毛枫  2015-4-17
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnNew_Click(object sender, EventArgs e)
        {
            int roleID = FineUITable.GetSelectedDataKeyID(Grid1);
            string addUrl = String.Format("/admin/system_manage/user_role_edit.aspx?id={0}", roleID);
            PageContext.RegisterStartupScript(Window1.GetShowReference(addUrl, "添加用户到当前色角"));
        }
    }
}