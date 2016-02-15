using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Mammothcode.Core.Data.DataAnaly
{
    public static class StringUtil
    {
        #region 获取可空文本的默认值

        /// <summary>
        /// 功能描述：获取可空文本的默认值
        /// 创建人：甘春雨
        /// 创建时间：2015年11月23日14:36:45
        /// </summary>
        /// <param name="value">可用文本</param>
        /// <param name="defaultStr">默认值</param>
        /// <returns></returns>
        public static string HandlerEmptyStr(this string value, string defaultStr)
        {
            string result = "";
            if (String.IsNullOrEmpty(value))
            {
                result = defaultStr;
            }
            else
            {
                result = value;
            }
            return result;
        }

        #endregion
        /// <summary>
        /// 移除字符串从第一个开始的字符的length的字符串
        /// 创建人：孙佳杰  时间：2015.1.17
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <param name="length">长度</param>
        /// <returns></returns>
        public static string RemoveFristChar(this string input,int length=1)
        {
            return input.Length > 0 + length ? input.Remove(0, length) : String.Empty;
        }

        /// <summary>
        ///  移除字符串从最后一个开始的字符的前length的字符串
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <param name="length">长度</param>
        /// <returns></returns>
        public static string RemoveLastChar(this string input,int length=1)
        {
            return input.Length > 0 + length ? input.Remove(input.Length - length, length) : String.Empty;
        }

        /// <summary>
        /// 移除字符串中所有的HTML标签
        /// 创建人：孙佳杰  创建时间:2015.4.9
        /// </summary>
        /// <param name="input">要处理的HTML</param>
        /// <param name="length">要取字符串的长度，默认为0</param>
        /// <returns></returns>
        public static string RemoveHtml(this string input,int length = 0)
        {
            string strText = Regex.Replace(input, "<[^>]+>", "");
            strText = Regex.Replace(strText, "&[^;]+;", "");

            if (length > 0 && strText.Length > length)
                return strText.Substring(0, length);

            return strText;
        }
    }
}
