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
    /// 权限
    /// </summary>
    public partial class power : System.Web.UI.Page
    {
        #region 声明
        /// <summary>
        /// BLL 表：权限表
        /// 创建人：林以恒
        /// 2015年7月6日18:29:06
        /// </summary>
        private static string fathercode = "0";//父亲CODE

        private static string CrumbString;//面包屑

        private readonly T_POWERS_BLL _powersbll = new T_POWERS_BLL();

        //权限相关操作
        private static readonly AdminwebAuthorizeAttribute Powers = new AdminwebAuthorizeAttribute();

        #endregion

        #region 主函数
        /// <summary>
        /// 创建人：林以恒 
        /// 2015年7月2日15:06:48
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //设置页面权限
            Powers.SetViewPower("mod_power");
            //验证权限
            if (Powers.VerifyPower() == false)
            {
                return;
            }
            //首次加载
            if (IsPostBack) return;
            LoadData();
            btnNew.OnClientClick = Add_Demo.GetShowReference("/admin/system_manage/power_edit.aspx?fathercode=" + fathercode, "添加");
        }
        #endregion

        //=========数据绑定

        #region 数据绑定
        /// <summary>
        /// 数据绑定
        /// 创建 林以恒  
        ///  2015-4-13      
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
                orderName = "ID",
            };
            Grid1.PageSize = parm.size = 20; //每页记录数(重要)
            //错误信息
            string massage = string.Empty;
            var str = _powersbll.BindData(parm, fathercode, out massage);

            // 在查询添加之后，获取总记录数
            Grid1.RecordCount = Int32.Parse(parm.allcount.ToString());
            Grid1.DataSource = str;
            Grid1.DataBind();
        }
        #endregion

        #region 获取当前是第几页
        /// <summary>
        /// 获取当前是第几页
        /// 创建人：林以恒 2015年7月2日15:21:05
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Grid1_PageIndexChange(object sender, GridPageEventArgs e)
        {
            Grid1.PageIndex = e.NewPageIndex;
            BindGrid();
        }
        #endregion

        //============ GRID事件==========
        #region 删除
        /// <summary>
        /// 列表数据绑定
        /// 创建 林以恒   2015-4-13  
        /// 修改：林以恒  2015-4-18
        /// </summary>
        protected void Grid1_RowCommand(object sender, GridCommandEventArgs e)
        {
            object[] keys = Grid1.DataKeys[e.RowIndex];
            int id = Int32.Parse(keys[0].ToString());
            if (e.CommandName != "Delete") return;
            var query = new DapperExQuery<T_POWERS>().AndWhere(n => n.ID, OperationMethod.Equal, id);
            Alert.ShowInTop(_powersbll.Delete(query) ? "删除成功！" : "删除失败！");
            BindGrid();
        }

        #endregion

        //============操作方法===========
        #region 二级菜单
        /// <summary>
        /// 行双击事件
        ///创建   毛枫   2015-4-20
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Grid1_RowClick(object sender, GridRowClickEventArgs e)
        {
            object[] keys = Grid1.DataKeys[e.RowIndex];
            fathercode = keys[1].ToString();
            CrumbString += fathercode + ",";
            Replace();
        }

        /// <summary>
        /// 返回按钮事件
        ///创建 毛枫   2015-4-20
        /// </summary>
        protected void back_Click(object sender, EventArgs e)
        {
            if (fathercode != "0")
            {
                var query = new DapperExQuery<T_POWERS>().AndWhere(n => n.P_CODE, OperationMethod.Equal, fathercode);
                var q = _powersbll.GetEntity(query);
                if (q != null)
                {
                    fathercode = q.FATHER_CODE;
                    Replace();
                }
            }
        }

        /// <summary>
        ///页面更新
        ///创建 毛枫   2015-4-20
        /// </summary>
        private void Replace()
        {
            btnNew.OnClientClick = Add_Demo.GetShowReference("/admin/system_manage/power_edit.aspx?fathercode=" + fathercode, "添加");
            BindGrid();
        }
        #endregion
    }
}