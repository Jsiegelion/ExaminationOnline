using Mammothcode.Core.Data.DataAnaly;

namespace Mammothcode.Core.CoreTool.Html
{
    public static class RazorBind
    {
        /// <summary>
        /// 前台Razor绑定
        /// 创建人：林以恒
        /// 2016-2-23
        /// </summary>
        /// <param name="str"></param>
        /// <param name="prop"></param>
        /// <param name="defaultStr"></param>
        /// <returns></returns>
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
