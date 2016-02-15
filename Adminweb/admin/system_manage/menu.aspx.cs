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
    /// 菜单
    /// </summary>
    public partial class menu : System.Web.UI.Page
    {
        #region 声明

        private static int? fatherId = 0;

        private static string CrumbString;

        private readonly T_ADMIN_MENUS_BLL _adminMenusBll = new T_ADMIN_MENUS_BLL();

        ///权限相关操作
        private static readonly AdminwebAuthorizeAttribute Power = new AdminwebAuthorizeAttribute();
        #endregion

        #region 主函数
        protected void Page_Load(object sender, EventArgs e)
        {

            //设置页面权限
            Power.SetViewPower("mod_menu");
            //验证权限
            if (Power.VerifyPower() == false)
            {
                return;
            }
            if (!IsPostBack)
            {
                LoadData();
                btnNew.OnClientClick = Add_Menu.GetShowReference("/admin/system_manage/menu_edit.aspx?fatherId=" + fatherId, "添加");
            }
            CrumbString = "0,";
        }
        #endregion

        //=========数据绑定

        #region 数据绑定
        /// <summary>
        /// 数据绑定
        /// 创建 林以恒 2015年4月14日
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
                index = Grid1.PageIndex + 1,//当前页数
                desced = false,
                orderName = "AM_SORTINDEX",
            };
            Grid1.PageSize = parm.size = 15; //每页记录数(重要)
            //错误信息
            string massage = string.Empty;
            var str = _adminMenusBll.BindData(parm, fatherId, out massage);

            // 在查询添加之后，获取总记录数
            Grid1.RecordCount = Int32.Parse(parm.allcount.ToString());
            Grid1.DataSource = str;
            Grid1.DataBind();
        }
        #endregion

        //============ GRID事件==========
        #region Grid事件

        /// <summary>
        /// 删除
        /// 创建 林以恒 
        /// 2015年4月14日
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Grid1_RowCommand(object sender, GridCommandEventArgs e)
        {
            object[] keys = Grid1.DataKeys[e.RowIndex];
            int id = int.Parse(keys[0].ToString());
            if (e.CommandName == "Delete")
            {
                var query = new DapperExQuery<T_ADMIN_MENUS>().AndWhere(n => n.ID, OperationMethod.Equal, id);
                Alert.ShowInTop(_adminMenusBll.Delete(query) ? "删除成功！" : "删除失败！");
                Replace();
            }
            else
            {
                Alert.ShowInTop("点击成功！");
            }
        }

        /// <summary>
        /// 获取当前是第几页
        ///创建   毛枫   2015-4-20
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Grid1_PageIndexChange(object sender, GridPageEventArgs e)
        {
            Grid1.PageIndex = e.NewPageIndex;
            Replace();
        }

        /// <summary>
        /// 行双击事件
        ///创建   毛枫   2015-4-20
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Grid1_RowClick(object sender, GridRowClickEventArgs e)
        {
            ////设置页面权限
            //Power.SetViewPower("mod_menu");
            ////验证权限
            //if (Power.VerifyPower() == false)
            //{
            //    return;
            //}
            object[] keys = Grid1.DataKeys[e.RowIndex];
            //如果URL为空，则不是叶子节点
            if (!string.IsNullOrEmpty(keys[1].ToString())) return;
            fatherId = int.Parse(keys[0].ToString());
            CrumbString += fatherId + ",";
            Replace();
        }

        #endregion

        //============操作方法===========
        #region 页面按钮

        /// <summary>
        ///页面更新
        ///创建 毛枫   2015-4-20
        /// </summary>
        private void Replace()
        {
            btnNew.OnClientClick = Add_Menu.GetShowReference("/admin/system_manage/menu_edit.aspx?fatherId=" + fatherId, "添加");
            BindGrid();
        }

        /// <summary>
        /// 返回按钮事件
        ///创建 毛枫   2015-4-20
        /// </summary>
        protected void back_Click(object sender, EventArgs e)
        {
            if (fatherId != 0)
            {
                var query = new DapperExQuery<T_ADMIN_MENUS>().AndWhere(n => n.ID, OperationMethod.Equal, fatherId);
                var q = _adminMenusBll.GetEntity(query);
                if (q != null)
                {
                    fatherId = q.PARENT_ID;
                    Replace();
                }
            }
        }
        #endregion
    }
}