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
    /// 角色
    /// </summary>
    public partial class role : System.Web.UI.Page
    {
        #region 声明

        /// <summary>
        /// BLL 表：角色表
        /// 创建人：林以恒
        /// 2015年8月1日22:05:23
        /// </summary>
        private readonly T_ROLES_BLL _rolesBll = new T_ROLES_BLL();

        //权限相关操作
        private static readonly AdminwebAuthorizeAttribute Power = new AdminwebAuthorizeAttribute();

        #endregion

        #region 主函数
        /// <summary>
        ///  创建人：林以恒 2015年7月2日16:48:24
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
            if (IsPostBack) return;
            LoadData();
            btnNew.OnClientClick = Add_Role.GetShowReference("/admin/system_manage/role_edit.aspx", "添加");
        }
        #endregion

        //============操作方法===========

        #region 绑定
        /// <summary>
        /// 数据绑定        
        /// </summary>
        private void LoadData()
        {
            Bind();
        }
        /// <summary>
        /// 功能：列表数据绑定
        /// 创建：金协民 
        /// 时间：2015年10月28日
        /// </summary>
        private void Bind()
        {
            //获取数据
            BaseSearchParam parm = new BaseSearchParam
            {
                index = Grid1.PageIndex + 1,//当前页数
            };
            Grid1.PageSize = parm.size = 20; //每页记录数(重要)
            //错误信息
            string massage = string.Empty;
            var str = _rolesBll.BindData(parm, out massage);

            // 在查询添加之后，获取总记录数
            Grid1.RecordCount = int.Parse(parm.allcount.ToString());
            Grid1.DataSource = str;
            Grid1.DataBind();
        }

        #endregion

        //============ GRID事件==========
        #region 删除

        /// <summary>
        /// 删除
        /// 修改：林以恒 2015-4-19
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Grid1_RowCommand(object sender, GridCommandEventArgs e)
        {
            ////设置页面权限
            //Power.SetViewPower("mod_role");
            ////验证权限
            //if (Power.VerifyPower() == false)
            //{
            //    return;
            //}
            object[] keys = Grid1.DataKeys[e.RowIndex];
            int id = Int32.Parse(keys[0].ToString());
            if (e.CommandName != "Delete") return;
            var query = new DapperExQuery<T_ROLES>().AndWhere(n => n.ID, OperationMethod.Equal, id);
            Alert.ShowInTop(_rolesBll.Delete(query) ? "删除成功！" : "删除失败！");
            //页面刷新
            Bind();
        }

        /// <summary>
        /// 获取当前是第几页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Grid1_PageIndexChange(object sender, GridPageEventArgs e)
        {
            Grid1.PageIndex = e.NewPageIndex;
            Bind();
        }
        #endregion
    }
}