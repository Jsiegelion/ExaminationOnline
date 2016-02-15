using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Mammothcode.Core.Data.DataEncoding
{
    /// <summary>
    /// HTML文本的编码工具
    /// 作者：孙佳杰
    /// 修改时间：2015.1.14
    /// 功能：HtmlEntitiesEncode（HTMLEntities编码）
    ///       HtmlEnititesDecode（HTMLEntities解码)
    /// </summary>
    public class HtmlUtil
    {
        /// <summary>
        /// HTMLEntities编码
        /// </summary>
        /// <param name="text">需要转换的html文本</param>s
        /// <returns>HTMLEntities编码后的文本</returns>
        public static  string HtmlEntitiesEncode(string text)
        {
            // 获取文本字符数组
            char[] chars = HttpUtility.HtmlEncode(text).ToCharArray();
            HttpUtility.HtmlDecode("").ToCharArray();

            // 初始化输出结果
            StringBuilder result = new StringBuilder(text.Length + (int)(text.Length * 0.1));

            foreach (char c in chars)
            {
                // 将指定的 Unicode 字符的值转换为等效的 32 位有符号整数
                int value = Convert.ToInt32(c);

                // 内码为127以下的字符为标准ASCII编码，不需要转换，否则做 &#{数字}; 方式转换
                if (value > 127)
                {
                    result.AppendFormat("&#{0};", value);
                }
                else
                {
                    result.Append(c);
                }
            }

            return result.ToString();
        }

        /// <summary>
        /// HTMLEntities解码
        /// </summary>
        /// <param name="text">需要解密码文本</param>
        /// <returns></returns>
        public static string HtmlEnititesDecode(string text)
        {
            return HttpUtility.HtmlDecode(text);
        }
    }
}
