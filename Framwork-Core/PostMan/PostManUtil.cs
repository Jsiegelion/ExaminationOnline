using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Mammothcode.Core.PostMan
{
    public class PostManUtil
    {
        #region 同步GET和POST请求

        /// <summary>
        /// 同步POST请求(WEB页面,JSON,XML)
        /// </summary>
        /// <param name="postRequest"></param>
        /// <param name="cookie"></param>
        /// <returns></returns>
        public static string SynchronousPost(PostManRequest postRequest, ref CookieContainer cookie)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(postRequest.Url);
            //请求参数
            string postDataStr = postRequest.ParamToString();
            string returnContent = string.Empty;
            if (cookie != null)
            {
                if (cookie.Count == 0)
                {
                    request.CookieContainer = new CookieContainer();
                    cookie = request.CookieContainer;
                }
                else
                {
                    request.CookieContainer = cookie;
                }
            }
            //请求RequestHeaders内容
            request.Method = "POST";
            request.AllowAutoRedirect = postRequest.AllowAutoRedirect;
            request.ContentType = postRequest.ContentType;
            request.ContentLength = postDataStr.Length;
            request.Referer = postRequest.Referer;
            //request.KeepAlive = true;
            request.Timeout = postRequest.Timeout;  //20秒的超时时间
            StreamWriter myStreamWriter =null;
            Stream myRequestStream = null;
            try
            {
                myRequestStream = request.GetRequestStream();
                myStreamWriter = new StreamWriter(myRequestStream, postRequest.Encoding);
                myStreamWriter.Write(postDataStr);
                myRequestStream.Close();
                myStreamWriter.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("请求Request错误" + ex.ToString());
            }
            finally
            {
                if(myRequestStream !=null)
                {
                   myRequestStream.Close();
                }
                if(myStreamWriter != null)
                {
                   myStreamWriter.Close();
                }
            }
            HttpWebResponse response = null;
            StreamReader myStreamReader = null;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
                myStreamReader = new StreamReader(response.GetResponseStream(), postRequest.Encoding);
                returnContent = myStreamReader.ReadToEnd();
                if (cookie != null)
                {
                    cookie.Add(response.Cookies);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("请求Response错误" + ex.ToString());
            }
            finally
            {
                response.Close();
                myStreamReader.Close();
            }
            return returnContent;
        }

        /// <summary>
        /// 同步GET请求（WEB页面，JSON,XML)
        /// </summary>
        /// <param name="postRequest"></param>
        /// <param name="cookie"></param>
        /// <returns></returns>
        public static string SynchronousGet(PostManRequest postRequest, ref CookieContainer cookie)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(postRequest.Url);
            //请求参数
            string postDataStr = postRequest.ParamToString();
            string returnContent = string.Empty;
            if (cookie != null)
            {
                if (cookie.Count == 0)
                {
                    request.CookieContainer = new CookieContainer();
                    cookie = request.CookieContainer;
                }
                else
                {
                    request.CookieContainer = cookie;
                }
            }
            //请求RequestHeaders内容
            request.Method = "GET";
            request.AllowAutoRedirect = postRequest.AllowAutoRedirect;
            request.ContentType = postRequest.ContentType;
            request.ContentLength = postDataStr.Length;
            request.Referer = postRequest.Referer;
            //request.KeepAlive = true;
            request.Timeout = postRequest.Timeout;  //20秒的超时时间
            HttpWebResponse response = null;
            StreamReader myStreamReader = null;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
                myStreamReader = new StreamReader(response.GetResponseStream(), postRequest.Encoding);
                returnContent = myStreamReader.ReadToEnd();
                if (cookie != null)
                {
                    cookie.Add(response.Cookies);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("请求Response错误" + ex.ToString());
            }
            finally
            {
                if (myStreamReader != null)
                {
                    myStreamReader.Close();
                }
                if (response != null)
                {
                    response.Close();
                }
            }
            return returnContent;
        }

        #endregion

        #region 废弃-下载图片

        /// <summary>
        /// 下载网上文件到本地
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="url"></param>
        /// <param name="localPath"></param>
        //public static string  DownloadOneFileByURLWithWebClient(string fileName, string url, string localPath)
        //{
        //    string savePath = string.Empty;
        //    try
        //    {
        //        using (System.Net.WebClient wc = new System.Net.WebClient())
        //        {
        //            if (System.IO.File.Exists(localPath + fileName))
        //            {
        //                System.IO.File.Delete(localPath + fileName);
        //            }
        //            if (Directory.Exists(localPath) == false)
        //            {
        //                Directory.CreateDirectory(localPath);
        //            }
        //            wc.DownloadFile(url , localPath + fileName);
        //            //创建之后进行判断文件是否存在
        //            if (System.IO.File.Exists(localPath + fileName))
        //            {
        //                savePath = localPath + fileName;
        //            }               
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("WebClient下载文件错误" + ex.ToString());
        //    }
        //    return savePath;
        // }

        #endregion

    }
}
