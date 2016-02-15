using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FineUI;
using Mammothcode.Bll.Search;
using Mammothcode.BLL;
using Mammothcode.Model;
using Mammothcode.Public.Data;
using Mammothcode.UICommon.Common.AdminCenter;

namespace Mammothcode.Demo.Adminweb.admin.system_manage
{
    /// <summary>
    /// 增加权限 mod_admin_user
    /// 2015年8月1日21:24:28
    /// 创建人：林以恒
    /// </summary>
    public partial class admin_user : System.Web.UI.Page
    {

        #region 声明

        /// <summary>
        /// BLL
        /// 创建人：林以恒
        /// 2015年7月28日19:53:32
        /// </summary>
        private static readonly T_ADMIN_BLL t_admin_bll = new T_ADMIN_BLL();

        //权限相关操作
        private static AdminwebAuthorizeAttribute power = new AdminwebAuthorizeAttribute();

        #endregion

        #region 主函数
        /// <summary>
        /// 主函数
        /// 创建人：林以恒
        /// 2015年7月27日17:24:15
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
            //首次加载
            if (IsPostBack) return;
            LoadData();
            btnNew.OnClientClick = Add_Admin_User.GetShowReference("/admin/system_manage/admin_user_edit.aspx", "添加");
        }
        #endregion

        //=========数据绑定

        #region 数据绑定

        /// <summary>
        /// 功能：数据绑定
        /// 创建：金协民 2015年10月28日
        /// </summary>
        private void LoadData()
        {
            BindGrid();
        }
        
        #endregion

        #region 列表数据绑定
        /// <summary>
        /// 功能：列表数据绑定
        /// 创建：金协民 
        /// 时间：2015年10月28日
        /// </summary>
        private void BindGrid()
        {
            //获取数据
            BaseSearchParam parm = new BaseSearchParam
            {
                //当前页数
                index = Grid1.PageIndex + 1
            };
            Grid1.PageSize = parm.size;
            string massage = string.Empty;
            var modelList = T_ADMIN_BLL.BindData(parm, out massage);
            // 在查询添加之后，获取总记录数
            Grid1.RecordCount = int.Parse(parm.allcount.ToString());
            Grid1.DataSource = modelList;
            Grid1.DataBind();
        }

        #endregion

        #region 分页事件
        /// <summary>
        /// 获取当前是第几页
        /// 创建人：林以恒
        /// 2015年7月4日19:41:27   
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Grid1_PageIndexChange(object sender, GridPageEventArgs e)
        {
            Grid1.PageIndex = e.NewPageIndex;
            BindGrid();
        }

        #endregion

        //============操作方法===========

        #region 删除
        /// <summary>
        /// 删除后台用户
        /// 创建人： 林以恒  
        /// 2015年7月29日11:46:14
        /// </summary>
        protected void Grid1_RowCommand(object sender, GridCommandEventArgs e)
        {
            //设置页面权限
            power.SetViewPower("mod_admin");
            //验证权限
            if (power.VerifyPower() == false)
            {
                return;
            }
            object[] keys = Grid1.DataKeys[e.RowIndex];
            int id = int.Parse(keys[0].ToString());
            if (e.CommandName != "Delete") return;
            var query = new DapperExQuery<T_ADMIN>().AndWhere(n => n.ID, OperationMethod.Equal, id);
            Alert.ShowInTop(t_admin_bll.Delete(query) ? "删除成功！" : "删除失败！");
            BindGrid();
        }

        #endregion
    }
}