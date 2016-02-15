using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mammothcode.Core.Data.DataWeb;
using Mammothcode.Core.SystemTool;

namespace Mammothcode.Demo.Adminweb.ajax
{
    /// <summary>
    /// RedirectHandler 的摘要说明
    /// </summary>
    public class RedirectHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string url = WebHelperUtil.GetRequestString("url");//获取值一律使用代码库中的方法
            context.Response.Redirect(url);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}