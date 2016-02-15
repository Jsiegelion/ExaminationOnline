using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FineUI;
using Mammothcode.BLL;
using Mammothcode.Core.Data.DataAnaly;
using Mammothcode.Core.Data.DataAutomatic;
using Mammothcode.Model;
using Mammothcode.Public.Data;
using Mammothcode.UICommon.Common.AdminCenter;

namespace Mammothcode.Demo.Adminweb.admin.system_manage
{
    /// <summary>
    /// 权限修改
    /// </summary>
    public partial class power_edit : System.Web.UI.Page
    {
        #region 声明

        /// <summary>
        /// BLL 表：权限表
        /// </summary>
        private readonly T_POWERS_BLL _powersBll = new T_POWERS_BLL();

        //权限相关操作
        private static readonly AdminwebAuthorizeAttribute Power = new AdminwebAuthorizeAttribute();

        #endregion

        #region 主函数

        protected void Page_Load(object sender, EventArgs e)
        {

            //设置页面权限
            Power.SetViewPower("mod_power");
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
        /// 添加或修改数据绑定
        /// </summary>
        private void LoadData()
        {
            if (Request.QueryString["id"].IsNum())
            {
                string id = Request.QueryString["id"].ToString();
                var query = new DapperExQuery<T_POWERS>().AndWhere(n => n.ID, OperationMethod.Equal, id);
                T_POWERS powers = null;
                powers = _powersBll.GetEntity(query);
                if (powers == null) return;
                tbxP_Name.Text = powers.P_NAME.ToString();
                tbxP_CHINESE_NAME.Text = powers.P_CHINESE_NAME != null ? powers.P_CHINESE_NAME.ToString() : "";
                //tbx_groupname.Text = powers.GROUP_NAME != null ? powers.GROUP_NAME.ToString() : "";
                //tbxAP_GROUP_NAME.Text = (powers.AP_GROUP_NAME.ToString() != null
                //    ? powers.AP_GROUP_NAME.ToString()
                //    : "");
                //tbxAP_REMARK.Text = (powers.AP_REMARK.ToString() != null
                //    ? powers.AP_REMARK.ToString()
                //    : "");
                //tbxAP_TITLE.Text = (powers.AP_TITLE.ToString() != null ? powers.AP_TITLE.ToString() : "");
            }
        }
        #endregion

        //============操作方法===========
        #region 数据保存
        /// <summary>
        /// 数据保存按钮事件
        /// 创建人：林以恒
        /// 2015年7月6日21:28:17
        /// </summary>
        protected void btnSaveClose_Click(object sender, EventArgs e)
        {
            ////设置页面权限
            //Power.SetViewPower("mod_power");
            ////验证权限
            //if (Power.VerifyPower() == false)
            //{
            //    return;
            //}
            string str;
            if (Request.QueryString["id"].IsNum())
            {
                T_POWERS powers = null;
                string id = Request.QueryString["id"];
                //修改
                var query = new DapperExQuery<T_POWERS>().AndWhere(n => n.ID, OperationMethod.Equal, int.Parse(id));
                powers = _powersBll.GetEntity(query);
                powers = Save(powers);
                str = _powersBll.Update(powers) ? "修改成功！" : "修改失败！";
            }
            else
            {
                T_POWERS powers = new T_POWERS();
                //添加
                powers = Save(powers);

                powers.FATHER_CODE = Request.QueryString["fathercode"].ToString();
                str = _powersBll.Add(powers) ? "添加成功！" : "添加失败！";
            }
            // 2. 关闭本窗体，然后刷新父窗体
            PageContext.RegisterStartupScript(ActiveWindow.GetHideRefreshReference());
            Alert.ShowInTop(str);
        }

        /// <summary>
        /// 更新实体
        /// 创建人：林以恒
        /// 2015年7月6日21:30:29
        /// </summary>
        /// <returns></returns>
        private T_POWERS Save(T_POWERS powers)
        {
            powers.P_NAME = tbxP_Name.Text.Trim();
            powers.P_CHINESE_NAME = tbxP_CHINESE_NAME.Text.Trim();
            if (powers.ID == 0)
            {
                //获取随机码
                powers.P_CODE = StringRandomUtil.GuidTo16String();
                powers.CREATE_TIME = DateTime.Now;
                var creatAdminUser = AdminwebUserManager.GetCurrentAdminUser();
                if (creatAdminUser != null)
                {
                    powers.CREATE_USER = creatAdminUser.A_NAME;
                    powers.CREATE_USER_NAME = creatAdminUser.A_CHINESE_NAME;
                }
            }
            //组别
            T_POWERS entity = new T_POWERS();
            var F_CODE = Request.QueryString["fathercode"];
            if (F_CODE != null)
            {
                if (F_CODE != "0")
                {
                    var fatherquery = new DapperExQuery<T_POWERS>().AndWhere(n => n.P_CODE, OperationMethod.Equal,
                        F_CODE);
                    entity = _powersBll.GetEntity(fatherquery);
                    powers.GROUP_NAME = entity.P_CHINESE_NAME;
                }
                else
                {
                    powers.GROUP_NAME = powers.P_CHINESE_NAME;
                }
            }
            return powers;
        }

        #endregion

        #region 关闭

        /// <summary>
        /// 关闭
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