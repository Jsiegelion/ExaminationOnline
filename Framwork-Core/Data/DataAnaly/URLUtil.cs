using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Mammothcode.Core.Data.DataAnaly
{
    /// <summary>
    /// URL基础类
    /// 作者：孙佳杰
    /// 修改时间：2015.1.13
    /// 功能：GetURLDomain（ 根据字符串获得URL域名）
    ///       GetMainDomain（根据输入字符串获得主域名）
    ///       GetCompeleteUrl（无论是绝对url还是相对url都转化为绝对url）
    /// </summary>
    public class URLUtil
    {
        #region 获取域名

        /// <summary>
        ///  根据字符串获得URL域名
        /// </summary>
        /// <param name="inPut"></param>
        /// <returns></returns>
        public static string GetURLDomain(string inPut)
        {
            string str = string.Empty;
            try
            {
                if (IsUrl(inPut) == true)
                {
                    Uri uri = new Uri(inPut);
                    str = uri.Host.ToLower();
                    return "http://"+str;
                }
                else
                {
                    return string.Empty;
                }
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 根据输入字符串获得主域名(如果有主域名，二级域名）
        /// </summary>
        /// <param name="inPut"></param>
        /// <returns></returns>
        public static string GetMainDomain(string inPut)
        {
            string host = string.Empty;
            string[] BeReplacedStrs = new string[] { ".com.cn", ".edu.cn", ".net.cn", ".org.cn", ".co.jp", ".gov.cn", ".co.uk", ".ac.cn", ".edu", ".tv", ".info", ".com", ".ac", ".ag", ".am", ".at", ".be", ".biz", ".bz", ".cc", ".cn", ".com", ".de", ".es", ".eu", ".fm", ".gs", ".hk", ".in", ".info", ".io", ".it", ".jp", ".la", ".md", ".ms", ".name", ".net", ".nl", ".nu", ".org", ".pl", ".ru", ".sc", ".se", ".sg", ".sh", ".tc", ".tk", ".tv", ".tw", ".us", ".co", ".uk", ".vc", ".vg", ".ws", ".il", ".li", ".nz" };
            Uri uri;
            try
            {
                if (IsUrl(inPut) == true)
                {
                    uri = new Uri(inPut);
                    host = uri.Host.ToLower();  //转化为小写
                }
                else
                {
                    return string.Empty;
                }
                foreach (string oneBeReplacedStr in BeReplacedStrs)
                {
                    string BeReplacedStr = oneBeReplacedStr.Trim();
                    if (host.IndexOf(BeReplacedStr) != -1)
                    {
                        host = host.Replace(BeReplacedStr, string.Empty);
                        break;
                    }
                }
                int dotIndex = host.LastIndexOf(".");
                host = "www." + uri.Host.Substring(dotIndex + 1);
                return host;
            }
            catch
            {
                return string.Empty;
            }
        }

        #endregion

        #region 获取URL

        /// <summary>
        /// 无论是绝对url还是相对url都转化为绝对url
        /// </summary>
        /// <param name="inPutUrl"></param>
        /// <param name="inDomain"></param>
        /// <returns></returns>
        public static string GetCompeleteUrl(string inPutUrl, string inDomain)
        {
            try
            {
                if (inPutUrl.ToLower().Contains(inDomain) == true && inPutUrl.Length >= inDomain.Length)
                {
                    return inPutUrl;
                }
                else if (inPutUrl.IndexOf("javascript", StringComparison.OrdinalIgnoreCase) == -1)
                {
                    if (inPutUrl.IndexOf('/') == 0)
                    {
                        return string.Format("http://{1}{0}", inPutUrl, inDomain);
                    }
                    else if (inPutUrl.IndexOf('.') == 0)
                    {
                        return string.Format("http://{1}{0}", inPutUrl.Remove(0, 2), inDomain);
                    }
                    else
                    {
                        return string.Format("http://{1}/{0}", inPutUrl, inDomain);
                    }
                }
                else
                {
                    return string.Empty;
                }
            }
            catch
            {
                return string.Empty;
            }
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 判断输入的文本是否为URL
        /// </summary>
        /// <param name="inPut"></param>
        /// <returns></returns>
        private static bool IsUrl(string input)
        {
            string reg = @"(((([?](\w)+){1}[=]*))*((\w)+){1}([\&](\w)+[\=](\w)+)*)*";
            Regex r = new Regex(reg);
            //给网址去所有空格
            string urlStr = input.Trim();
            Match m = r.Match(urlStr);

            if (!m.Success)
                return false;
            else
                return true;
        }

        #endregion
    }
}
