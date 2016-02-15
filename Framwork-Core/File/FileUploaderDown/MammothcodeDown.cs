using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
namespace Mammothcode.Core.File.FileUploaderDown
{
    /// <summary>
    /// 文件下载类
    /// 创建人：孙佳杰  创建时间:2015.7.31
    /// </summary>
    public class MammothcodeDown
    {
        /// <summary>
        /// 下载文件
        /// 创建人：孙佳杰  创建时间：2015年7月31日16:24:26
        /// </summary>
        /// <param name="file"></param>
        public static bool DownFile(string file)
        {
            bool isSuccess = true;
            try
            {
                //获取文件的物理路径
                string filePath = System.Web.HttpContext.Current.Server.MapPath(file);
                System.Web.HttpContext.Current.Response.Write(filePath);
                //获取文件名
                string fileName = System.IO.Path.GetFileName(file);
                //以字符流的形式下载文件
                FileStream fs = new FileStream(filePath, FileMode.Open);
                byte[] bytes = new byte[(int)fs.Length];
                fs.Read(bytes, 0, bytes.Length);
                fs.Close();
                System.Web.HttpContext.Current.Response.ContentType = "application/octet-stream";
                //通知浏览器下载文件而不是打开
                System.Web.HttpContext.Current.Response.AddHeader("Content-Disposition",
                    "attachment;  filename=" + HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8));
                System.Web.HttpContext.Current.Response.BinaryWrite(bytes);
                System.Web.HttpContext.Current.Response.Flush();
                System.Web.HttpContext.Current.Response.End();
            }
            catch (Exception ex)
            {
                isSuccess = false;
            }
            return isSuccess;
        }
    }
}
