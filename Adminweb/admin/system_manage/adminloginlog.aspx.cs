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
    /// 登录日志
    /// </summary>
    public partial class adminloginlog : System.Web.UI.Page
    {
        #region 声明

        /// <summary>
        /// BLL 视图：后台用户-后台用户登录
        /// 创建人：林以恒
        /// 2015年7月6日17:37:21
        /// </summary>
        private readonly V_ADMIN_LOGIN_BLL _adminLoginBll = new V_ADMIN_LOGIN_BLL();
        //权限相关操作
        private static readonly AdminwebAuthorizeAttribute Power = new AdminwebAuthorizeAttribute();
        #endregion

        #region 主函数

        /// <summary>
        /// 主函数
        /// 创建人：林以恒 2015年7月2日13:57:30
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //            //设置页面权限
            //            Power.SetViewPower("mod_admin_user");
            //            //验证权限
            //            if (Power.VerifyPower() == false)
            //            {
            //                return;
            //            }
            //首次加载
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
        /// 2015年7月6日17:38:03
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
                desced = true,
                orderName = "CREATE_TIME",
            };
            Grid1.PageSize = parm.size = 20; //每页记录数(重要)
            //错误信息
            string massage = string.Empty;
            var str = _adminLoginBll.BindData(parm, out massage);
            // 在查询添加之后，获取总记录数
            Grid1.RecordCount = int.Parse(parm.allcount.ToString());
            Grid1.DataSource = str;
            Grid1.DataBind();
        }

        #endregion

        #region 获取当前是第几页
        /// <summary>
        /// 获取当前是第几页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Grid1_PageIndexChange(object sender, GridPageEventArgs e)
        {
            Grid1.PageIndex = e.NewPageIndex;
            BindGrid();
        }

        #endregion
    }
}