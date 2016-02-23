using Mammothcode.Core.Data.DataAnaly;

namespace Mammothcode.Core.CoreTool.Html
{
    public static class RazorBind
    {
        /// <summary>
        /// 页面绑定：根据传入的值判断前台input是否应该显示
        /// 创建人：林以恒
        /// 2015-12-23
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string InputValue(string str)
        {
            string value = string.Empty;
            if (!string.IsNullOrWhiteSpace(str))
            {
                value = "value=" + str;
            }
            return value;
        }

        /// <summary>
        /// 页面绑定：根据传入的值判断前台tag是否显示data
        /// 创建人：林以恒
        /// 2016-1-8
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string HtmlData(string str)
        {
            string data = string.Empty;
            if (!string.IsNullOrWhiteSpace(str))
            {
                data = "data=" + str;
            }
            return data;
        }

        /// <summary>
        /// 页面绑定：判断参数返回值或默认值
        /// 创建人：林以恒
        /// 2016-1-13
        /// </summary>
        /// <param name="str"></param>
        /// <param name="defaultString"></param>
        /// <returns></returns>
        public static string HtmlTag(string str, string defaultString)
        {
            return !string.IsNullOrWhiteSpace(str) ? str : defaultString;
        }

        /// <summary>
        /// 页面绑定：判断参数返回值或默认值
        /// 创建人：林以恒
        /// 2016-1-13
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string HtmlTag(string str)
        {
            return !string.IsNullOrWhiteSpace(str) ? str : "";
        }

        /// <summary>
        /// Razor绑定图片地址
        /// 创建人：林以恒
        /// 2016-2-19
        /// </summary>
        /// <param name="str"></param>
        /// <param name="defaultUrl"></param>
        /// <returns></returns>
        public static string HtmlSrc(string str, string defaultUrl)
        {
            return string.IsNullOrWhiteSpace(str) ?
                   (string.IsNullOrWhiteSpace(defaultUrl) ? "" : AddSrcTag(defaultUrl)) : AddSrcTag(str);
        }

        public static string Properties(string str, string prop, string defaultStr)
        {
            string result = string.Empty;
            if (!StringUtil.ArrayIsNullOrEmpty(new[] { str, prop }))
            {
                result = AddStr(str, prop);
            }
            else if (!StringUtil.ArrayIsNullOrEmpty(new[] { defaultStr, prop }))
            {
                result = AddStr(defaultStr, prop);
            }
            return result;
        }

        #region private

        #region stringAdd

        /// <summary>
        /// 将字符串两边加上str=""
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static string AddSrcTag(string str)
        {
            return "src=" + str;
        }

        /// <summary>
        /// 组合字符串为prop='str'
        /// </summary>
        /// <param name="str"></param>
        /// <param name="prop"></param>
        /// <returns></returns>
        private static string AddStr(string str, string prop)
        {
            return prop + "='" + str + "'";
        }

        #endregion

        #endregion
    }


}
