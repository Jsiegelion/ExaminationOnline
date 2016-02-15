using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Mammothcode.Demo.Adminweb.ashx
{
    /// <summary>
    /// upload 的摘要说明
    /// </summary>
    public class upload : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var Response = context.Response;
            var Request = context.Request;
            var Session = context.Session;
            string result = "";

            string s_rpath = context.Server.MapPath("UploadAdmin/Attachment");
            string Datedir = DateTime.Now.ToString("yy-MM-dd");
            string updir = s_rpath + "\\" + Datedir;
            Response.CacheControl = "no-cache";
            if (Request.Files.Count > 0)
            {
                try
                {

                    for (var j = 0; j < Request.Files.Count; j++)
                    {
                        var uploadFile = Request.Files[j];
                        if (uploadFile.ContentLength <= 0) continue;
                        if (!Directory.Exists(updir))
                        {
                            Directory.CreateDirectory(updir);
                        }
                        var extname = Path.GetExtension(uploadFile.FileName);
                        var fullname = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() +
                                       DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() +
                                       DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
                        var filename = uploadFile.FileName;
                        uploadFile.SaveAs(string.Format("{0}\\{1}", updir, filename));
                        result = string.Format("{0}\\{1}", "\\UploadAdmin\\Attachment", filename);
                    }
                }
                catch (Exception ex)
                {
                    Response.Write("Message" + ex.ToString());
                }
            }
            Response.Write(result);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}