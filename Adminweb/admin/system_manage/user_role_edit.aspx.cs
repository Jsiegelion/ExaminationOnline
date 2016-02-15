using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using FineUI;
using Mammothcode.BLL;
using Mammothcode.Core.Data.DataAnaly;
using Mammothcode.Middle.Core.Tool;
using Mammothcode.Middle.Core.Tool.FineUI;
using Mammothcode.Model;
using Mammothcode.Public.Data;
using Mammothcode.UICommon.Common.AdminCenter;

namespace Mammothcode.Demo.Adminweb.admin.system_manage
{
    /// <summary>
    /// 用户关联角色修改
    /// </summary>
    public partial class user_role_edit : Page
    {
        #region  全局变量
        protected static T_ADMIN_ROLES_BLL T_ADMIN_ROLES_BLL = new T_ADMIN_ROLES_BLL();
        protected static T_ADMIN_BLL T_ADMIN_BLL = new T_ADMIN_BLL();
        protected static T_ROLES_BLL T_ROLES_BLL = new T_ROLES_BLL();
        protected string requestStr = "id";//获取的URL参数类型   

         //权限相关操作
        private static readonly AdminwebAuthorizeAttribute Power = new AdminwebAuthorizeAttribute();
        #endregion

        #region  主函数
        /// <summary>
        /// 毛枫
        /// 创建 2015-4-17
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
            //首次加载执行
            if (!IsPostBack)
            {

                LoadData();
            }
        }
        #endregion

        #region  初始化数据绑定

        /// <summary>
        /// 初始化数据绑定
        /// 创建 2015-4-17
        /// </summary>
        private void LoadData()
        {
            btnClose.OnClientClick = ActiveWindow.GetHideReference();
            int rolesid = 0;
            if (Request.QueryString[requestStr].IsNum())
            {
                rolesid = Int32.Parse(Request.QueryString[requestStr]);
            }
            T_ROLES T_ROLES = new T_ROLES();
            var query = new DapperExQuery<T_ROLES>().AndWhere(n => n.ID, OperationMethod.Equal, rolesid);
            T_ROLES = T_ROLES_BLL.GetEntity(query);
            if (T_ROLES == null)
            {
                // 参数错误，首先弹出Alert对话框然后关闭弹出窗口
                Alert.Show("参数错误！", String.Empty, ActiveWindow.GetHideReference());
                return;
            }
            // 每页记录数
            Grid1.PageSize = 10;
            //ddlGridPageSize.SelectedValue = ConfigHelper.PageSize.ToString();
            BindGrid();
        }

        /// <summary>
        /// 用户数据绑定
        /// 创建 2015-4-17
        /// </summary>
        private void BindGrid()
        {
            List<T_ADMIN> T_ADMIN_LIST = new List<T_ADMIN>();
            //当前页数
            int PageIndex = Grid1.PageIndex + 1;
            //一页的数据条数
            int PageSize = Grid1.PageSize;
            //数据总条数
            long allcount;
            //分页拿到总数据
            var queryall = new DapperExQuery<T_ADMIN>();
            T_ADMIN_LIST = T_ADMIN_BLL.GetListByPage(queryall, "", PageIndex, PageSize, out allcount);
            // 在名称中搜索
            string searchText = ttbSearchMessage.Text.Trim();
            if (!String.IsNullOrEmpty(searchText))
            {
                var query = new DapperExQuery<T_ADMIN>().AndWhere(n => n.A_NAME, OperationMethod.Equal, searchText)
                 .AndWhere(n => n.A_TRUE_NAME, OperationMethod.Equal, searchText);
                T_ADMIN_LIST = T_ADMIN_LIST.Where(n => n.A_TRUE_NAME.Contains(searchText) || n.A_NAME.Contains(searchText)).ToList();
            }
            // 排除已经属于本角色的用户
            int rolesid = 0;
            if (Request.QueryString[requestStr].IsNum())
            {
                rolesid = Int32.Parse(Request.QueryString[requestStr]);

                T_ROLES T_ROLES = new T_ROLES();
                var queryrole = new DapperExQuery<T_ROLES>().AndWhere(n => n.ID, OperationMethod.Equal, rolesid);
                T_ROLES = T_ROLES_BLL.GetEntity(queryrole);
                List<T_ADMIN_ROLES> T_ADMIN_ROLES_LIST = new List<T_ADMIN_ROLES>();
                var query1 = new DapperExQuery<T_ADMIN_ROLES>().AndWhere(n => n.R_CODE, OperationMethod.Equal, T_ROLES.R_CODE);
                T_ADMIN_ROLES_LIST = T_ADMIN_ROLES_BLL.GetAllList(query1);
                for (var i = 0; i < T_ADMIN_ROLES_LIST.Count; i++)
                {
                    T_ADMIN_LIST = T_ADMIN_LIST.Where(n => n.A_CODE != T_ADMIN_ROLES_LIST[i].A_CODE).ToList();
                }
                // 在查询添加之后，获取总记录数
                Grid1.RecordCount = Int32.Parse(allcount.ToString());
                Grid1.DataSource = T_ADMIN_LIST;
                Grid1.DataBind();
            }
        }

        #endregion

        #region 事件
        /// <summary>
        /// 添加角色对应的用户
        /// 创建 2015-4-17
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSaveClose_Click(object sender, EventArgs e)
        {
            SyncSelectedRowIndexArrayToHiddenField();
            string str = "";

            // 排除已经属于本角色的用户
            int roleID = 0;
            if (Request.QueryString[requestStr].IsNum())
            {
                roleID = Int32.Parse(Request.QueryString[requestStr]);
                T_ROLES T_ROLES = new T_ROLES();
                var queryrole = new DapperExQuery<T_ROLES>().AndWhere(n => n.ID, OperationMethod.Equal, roleID);
                T_ROLES = T_ROLES_BLL.GetEntity(queryrole);
                var r_code = T_ROLES.R_CODE;
                // 从每个选中的行中获取ID（在Grid1中定义的DataKeyNames）
                List<int> userids = FineUITable.GetSelectedDataKeyIDs(Grid1);
                foreach (int userID in userids)
                {
                    var queryAdmin = new DapperExQuery<T_ADMIN>().AndWhere(n => n.ID, OperationMethod.Equal, userID);
                    T_ADMIN T_ADMIN = new Model.T_ADMIN();
                    T_ADMIN = T_ADMIN_BLL.GetEntity(queryAdmin);
                    T_ADMIN_ROLES T_ADMIN_ROLES = new T_ADMIN_ROLES();
                    T_ADMIN_ROLES.R_CODE = r_code;
                    T_ADMIN_ROLES.A_CODE = T_ADMIN.A_CODE;
                    T_ADMIN_ROLES.CREATE_TIME = DateTime.Parse("2015-10-9");
                    T_ADMIN_ROLES.CREATE_USER = "";
                    T_ADMIN_ROLES.CREATE_USER_NAME = "";

                    if (T_ADMIN_ROLES_BLL.Add(T_ADMIN_ROLES))
                    {
                        str = "添加成功！";
                    }
                    else
                    {
                        str = "添加失败！";
                    }
                }
                //DB.SaveChanges();

                PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
                Alert.Show("str");
            }
        }

        private void SyncSelectedRowIndexArrayToHiddenField()
        {
            //// 重新绑定表格数据之前，将当前表格页选中行的数据同步到隐藏字段中
            //SyncSelectedRowIndexArrayToHiddenField(hfSelectedIDS, Grid1);
        }

        protected void ttbSearchMessage_Trigger2Click(object sender, EventArgs e)
        {
            SyncSelectedRowIndexArrayToHiddenField();

            ttbSearchMessage.ShowTrigger1 = true;
            BindGrid();
        }

        /// <summary>
        /// 查询
        /// 创建 2015-4-17
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ttbSearchMessage_Trigger1Click(object sender, EventArgs e)
        {
            SyncSelectedRowIndexArrayToHiddenField();

            ttbSearchMessage.Text = String.Empty;
            ttbSearchMessage.ShowTrigger1 = false;
            BindGrid();
        }

        /// <summary>
        /// 用户列表排序
        /// 创建 2015-4-17
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Grid1_Sort(object sender, GridSortEventArgs e)
        {
            SyncSelectedRowIndexArrayToHiddenField();

            Grid1.SortDirection = e.SortDirection;
            Grid1.SortField = e.SortField;
            BindGrid();
        }
        /// <summary>
        /// 获取当前是第几页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Grid1_PageIndexChange(object sender, GridPageEventArgs e)
        {
            SyncSelectedRowIndexArrayToHiddenField();
            Grid1.PageIndex = e.NewPageIndex;
            BindGrid();
        }
        #endregion
    }
}