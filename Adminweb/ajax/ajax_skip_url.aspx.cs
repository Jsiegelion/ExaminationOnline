using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Mammothcode.Demo.Adminweb.ajax
{
    public partial class ajax_skip_url : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //跳转URL
            //创建：金协民 
            //时间：2015年10月31日
            string url = Request.QueryString["url"].ToString();
            Response.Redirect(url);
        }
    }
}