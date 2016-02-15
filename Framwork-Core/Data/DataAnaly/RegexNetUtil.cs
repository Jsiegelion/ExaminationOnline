using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Mammothcode.Core.Data.DataAnaly
{
    /// <summary>
    /// 在NET基础上的正则解析类
    /// 作者：孙佳杰
    /// 修改时间：2015.1.13
    /// 功能：GetHtmlImageUrlList（正则取得HTML中所有图片的 URL）
    /// </summary>
    public class RegexNetUtil
    {

        /// <summary>
        /// 正则取得HTML中所有图片的 URL
        /// </summary>
        /// <param name="sHtmlText">HTML代码</param>
        /// <returns>图片的URL列表</returns>
        public static List<string> GetHtmlImageUrlList(string sHtmlText)
        {
            List<string> imgList = new List<string>();
            // 定义正则表达式用来匹配 img 标签
            Regex regImg = new Regex(@"<img\b[^<>]*?\bsrc[\s\t\r\n]*=[\s\t\r\n]*[""']?[\s\t\r\n]*(?<imgUrl>[^\s\t\r\n""'<>]*)[^<>]*?/?[\s\t\r\n]*>", RegexOptions.IgnoreCase);

            // 搜索匹配的字符串
            MatchCollection matches = regImg.Matches(sHtmlText);
            int i = 0;
            string[] sUrlList = new string[matches.Count];

            // 取得匹配项列表
            foreach (Match match in matches)
            {
               imgList.Add(match.Groups["imgUrl"].Value);
            }
            return imgList;
        }

        /// <summary>
        /// 正则取得HTML中所有锚标签
        /// 创建人：孙佳杰  创建时间:2015.3.11
        /// </summary>
        /// <param name="sHtmlText"></param>
        /// <returns></returns>
        public static List<string> GetHtmlAnchorlList(string sHtmlText)
        {
            List<string> achorList = new List<string>();
            //定义正则表达式用来匹配锚点
            Regex regAchor = new Regex(@"<a\sname=""(.+?)</a>",RegexOptions.Multiline);
            // 搜索匹配的字符串
            MatchCollection matches = regAchor.Matches(sHtmlText);
             int i = 0;
            string[] sUrlList = new string[matches.Count];

            // 取得匹配项列表
            foreach (Match match in matches)
            {
                //去除HTML中的标签，只获得纯文本
                achorList.Add(GetTextNoHtml(match.Groups[1].Value));
            }
            return achorList;

        }

        /// <summary>
        ///  正则表达式获取HTML所有的文本，不需要HTML标签（也可以生成文章摘要）
        ///  创建人：孙佳杰 创建时间：2015.3.11
        /// </summary>
        /// <param name="sHtmlText"></param>
        /// <returns></returns>
        public static string GetTextNoHtml(string sHtmlText, int length = 0)
        {

            //删除脚本
            sHtmlText = Regex.Replace(sHtmlText, @"<script[^>]+?>[\s\S]*?</script>", "", RegexOptions.IgnoreCase);
            //删除HTML
            sHtmlText = Regex.Replace(sHtmlText, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            sHtmlText = Regex.Replace(sHtmlText, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            sHtmlText = Regex.Replace(sHtmlText, @"-->", "", RegexOptions.IgnoreCase);
            sHtmlText = Regex.Replace(sHtmlText, @"<!--.*", "", RegexOptions.IgnoreCase);
            sHtmlText = Regex.Replace(sHtmlText, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            sHtmlText = Regex.Replace(sHtmlText, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            sHtmlText = Regex.Replace(sHtmlText, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            sHtmlText = Regex.Replace(sHtmlText, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            sHtmlText = Regex.Replace(sHtmlText, @"&(nbsp|#160);", " ", RegexOptions.IgnoreCase);
            sHtmlText = Regex.Replace(sHtmlText, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            sHtmlText = Regex.Replace(sHtmlText, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            sHtmlText = Regex.Replace(sHtmlText, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            sHtmlText = Regex.Replace(sHtmlText, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
            sHtmlText = Regex.Replace(sHtmlText, @"&#(\d+);", "", RegexOptions.IgnoreCase);
            sHtmlText = sHtmlText.Replace("\"", "");
            sHtmlText = Regex.Replace(sHtmlText, @"//\(function\(\)[\s\S]+?}\)\(\);", "", RegexOptions.IgnoreCase);
            sHtmlText = sHtmlText.Replace("<", "");
            sHtmlText = sHtmlText.Replace(">", "");
            sHtmlText = sHtmlText.Replace("\r\n", "");

            if (length > 0 && sHtmlText.Length > length)
                return sHtmlText.Substring(0, length);

            return sHtmlText;
        }

        /// <summary>
        /// 正则表达式获取分页HTML中自己需要内容匹配的A标签
        /// 创建人：孙佳杰  创建时间:2015.4.9
        /// </summary>
        /// <param name="html">要处理的HTML</param>
        /// <param name="text">摘要</param>
        /// <param name="iscontain"></param>
        /// <returns></returns>
        public static string GetHtmlPageSelectA(string html,string text,bool iscontain)
        {
            string url = string.Empty;
            Regex reg = new Regex(@"<a href=""(?<url>.*?)""(.*?)>(?<text>.*?)</a>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            MatchCollection mctitleList = reg.Matches(html);
            if (mctitleList.Count > 0)
            {
                foreach (Match m in mctitleList)
                {
                    string str = m.Groups["text"].Value.ToString().RemoveHtml();
                    if (iscontain)
                    {
                        if (str.Trim().Contains(text))
                        {
                            url = m.Groups["url"].Value.ToString().Trim();
                            break;
                        }
                    }
                    else
                    {
                        if (str.Trim().Equals(text))
                        {
                            url = m.Groups["url"].Value.ToString().Trim();
                            break;
                        }
                    }
                }
            }
            return url;
        }

    }
}
