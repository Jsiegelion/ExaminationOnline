using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mammothcode.Core.PostMan
{
    /// <summary>
    /// POST HTTP请求参数配置类
    /// </summary>
    public class PostManRequest
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="url"></param>
        public PostManRequest(string url)
        {
            Url = url;
            ContentType = "application/x-www-form-urlencoded";
            Timeout = 20 * 1000;
            Encoding = Encoding.UTF8;
            Parameters = new List<PostManParameter>();
            AllowAutoRedirect = false;
        }
        public PostManRequest(string url, string referer, bool allowAutoRedirect)
        {
            Url = url;
            ContentType = "application/x-www-form-urlencoded";
            Timeout = 20 * 1000;
            Encoding = Encoding.UTF8;
            Referer = referer;
            Parameters = new List<PostManParameter>();
            AllowAutoRedirect = allowAutoRedirect;
        }

        public PostManRequest(string url, System.Text.Encoding encoding, int timeout)
        {
            Url = url;
            ContentType = "application/x-www-form-urlencoded";
            Timeout = timeout;
            Encoding = encoding;
            Parameters = new List<PostManParameter>();
            AllowAutoRedirect = false;
        }
        /// <summary>
        /// 请求地址
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 请求参数
        /// </summary>
        public List<PostManParameter> Parameters { get; set; }

        /// <summary>
        /// Content-type HTTP的头
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// Referer HTTP的头
        /// </summary>
        public string Referer { get; set; }

        /// <summary>
        /// 超时时间
        /// </summary>
        public int Timeout { get; set; }

        /// <summary>
        /// 请求返回编码
        /// </summary>
        public Encoding Encoding { get; set; }

        /// <summary>
        /// 是否重定向
        /// </summary>
        public bool AllowAutoRedirect { get; set; }

        /// <summary>
        /// 新增参数
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void AddParam(string name, string value)
        {
            Parameters.Add(new PostManParameter() { name = name, value = value });
        }

        /// <summary>
        /// 将参数集合转化为请求参数字符串
        /// </summary>
        /// <returns></returns>
        public string ParamToString()
        {
            string postDataStr = string.Empty;
            int i = 0;
            foreach (var p in Parameters)
            {
                if (i != 0)
                {
                    postDataStr += "&";
                }
                postDataStr += p.name + "=" + p.value;
                i++;
            }
            return postDataStr;
        }
    }

    public class PostManParameter
    {
        public string name { get; set; }

        public string value { get; set; }
    }

}
