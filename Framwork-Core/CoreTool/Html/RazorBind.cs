using Mammothcode.Core.Data.DataAnaly;

namespace Mammothcode.Core.CoreTool.Html
{
    public static class RazorBind
    {
        /// <summary>
        /// use razor bind html
        /// author: Baby
        /// 2016-2-23
        /// </summary>
        /// <param name="str">source string</param>
        /// <param name="prop">properties</param>
        /// <param name="defaultStr">
        /// when source is null or empty use default string to bind html
        /// </param>
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
        /// make string as prop='str'
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
