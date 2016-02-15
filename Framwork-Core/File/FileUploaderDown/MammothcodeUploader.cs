using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;

namespace Mammothcode.Core.File.FileUploaderDown
{

    /// <summary>
    /// 文件上传类
    /// 创建人：孙佳杰  创建时间 :2015.5.19
    /// </summary>
    public class MammothcodeUploader
    {
        private string ImageExts = ".BMP|.GIF|.JPE|.JPEG|.JPG|.PNG";
        string state = "SUCCESS";

        string URL = null;
        string currentType = null;
        //string state = "SUCCESS";
        //string URL = null;
        //string currentType = null;
        string uploadpath = null;
        string filename = null;
        string originalName = null;
        //string filename = null;
        //string originalName = null;
        List<string> stateList = new List<string>();
        List<string> URLList = new List<string>();
        List<string> currentTypeList = new List<string>();
        List<string> filenameList = new List<string>();
        List<string> originalNameList = new List<string>();
        List<int> filesizeList = new List<int>();
        List<string> filetypeList = new List<string>();
        HttpPostedFileBase uploadFile = null;

        #region 上传文件的主处理方法
        /// <summary>
        /// MVC:上传文件的主处理方法
        /// </summary>
        /// <param name="cxt"></param>
        /// <param name="pathbase"></param>
        /// <param name="filetype"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public Hashtable upFile(HttpContextBase cxt, string pathbase, string[] filetype, int size)
        {
            pathbase = pathbase + DateTime.Now.ToString("yyyy-MM-dd") + "/";
            uploadpath = cxt.Server.MapPath(pathbase);//获取文件上传路径
            //.Request.Files[0];
            int fileCount = cxt.Request.Files.Count;//文件上传个数
            //目录创建
            CreateFolder(uploadpath);
            for (int i = 0; i < fileCount; i++)
            {
                try
                {
                    stateList.Add("SUCCESS");
                    //获取文件请求集合（HTTP)
                    uploadFile = cxt.Request.Files[i];

                    //获取文件原始名称
                    originalNameList.Add(uploadFile.FileName);

                    //格式验证
                    if (checkType(filetype))
                    {
                        stateList[i] = "不允许的文件类型";
                        //state = "不允许的文件类型";
                    }
                    //大小验证
                    if (checkSize(size))
                    {
                        stateList[i] = "文件大小超出网站限制";
                        //state = "文件大小超出网站限制";
                    }
                    //保存图片
                    //if (state == "SUCCESS")
                    if (stateList[i] == "SUCCESS")
                    {
                        string realname = reName();

                        SaveFile(uploadFile, uploadpath + realname);//保存文件过滤
                        URLList.Add(pathbase + realname);
                        #region 获取每次遍历的数据(nameList,sizeList,typeList)
                        filenameList.Add(Path.GetFileName(URLList[i]));
                        filesizeList.Add(uploadFile.ContentLength);
                        filetypeList.Add(Path.GetExtension(originalNameList[i]));
                        #endregion
                    }

                }
                catch (Exception e)
                {
                    stateList[i] = "未知错误";
                    URLList[i] = "";
                }
            }
            return getUploadInfo();
        }
        #endregion

        #region 上传文件的主处理方法
        /// <summary>
        /// WEBFORM:上传文件的主处理方法
        /// </summary>
        /// <param name="cxt"></param>
        /// <param name="pathbase"></param>
        /// <param name="filetype"></param>
        /// <param name="size">M(兆为单位）</param>
        /// <returns></returns>
        public Hashtable web_upFile(HttpContext cxt, string pathbase, string[] filetype, int size)
        {
            pathbase = pathbase + DateTime.Now.ToString("yyyy-MM-dd") + "/";
            uploadpath = cxt.Server.MapPath(pathbase);//获取文件上传路径
            //.Request.Files[0];
            int fileCount = cxt.Request.Files.Count;//文件上传个数
            //目录创建
            CreateFolder(uploadpath);
            for (int i = 0; i < fileCount; i++)
            {
                try
                {
                    stateList.Add("SUCCESS");
                    //获取文件请求集合（HTTP)
                    uploadFile = new HttpPostedFileWrapper(cxt.Request.Files[i]);

                    //获取文件原始名称
                    originalNameList.Add(uploadFile.FileName);

                    //格式验证
                    if (checkType(filetype))
                    {
                        stateList[i] = "不允许的文件类型";
                        //state = "不允许的文件类型";
                    }
                    //大小验证
                    if (checkSize(size))
                    {
                        stateList[i] = "文件大小超出网站限制";
                        //state = "文件大小超出网站限制";
                    }
                    //保存图片
                    //if (state == "SUCCESS")
                    if (stateList[i] == "SUCCESS")
                    {
                        string realname = reName();
                        //真正保存到文件路径
                        //过滤格式 图片格式需要压缩

                        uploadFile.SaveAs(uploadpath + realname);
                        URLList.Add(pathbase + realname);
                    }
                    #region 获取每次遍历的数据(nameList,sizeList,typeList)
                    filenameList.Add(Path.GetFileName(URLList[i]));

                    filesizeList.Add(uploadFile.ContentLength);

                    filetypeList.Add(Path.GetExtension(originalNameList[i]));
                    #endregion
                }
                catch (Exception e)
                {
                    stateList[i] = "未知错误";
                    URLList[i] = "";
                }
            }
            return getUploadInfo();
        }
        #endregion

        #region 私有方法

        /// <summary>
        /// 获取上传信息
        /// 创建人：孙佳杰  创建时间:2015.5.19
        /// </summary>
        /// <returns></returns>
        private Hashtable getUploadInfo()
        {
            Hashtable infoList = new Hashtable();

            infoList.Add("state", state);
            infoList.Add("url", URL);
            infoList.Add("originalName", originalName);
            infoList.Add("name", Path.GetFileName(URL));
            infoList.Add("size", uploadFile != null ? uploadFile.ContentLength : 0);
            infoList.Add("type", Path.GetExtension(originalName));
            infoList.Add("stateList", stateList);
            infoList.Add("URLList", URLList);
            infoList.Add("originalNameList", originalNameList);
            infoList.Add("nameList", filenameList);
            infoList.Add("sizeList", filesizeList);
            infoList.Add("typeList", filetypeList);

            return infoList;
        }

        /// <summary>
        /// 按照日期自动创建存储文件夹
        /// 创建人：孙佳杰  创建时间：2015.5.19
        /// </summary>
        public static void CreateFolder(string uploadpath)
        {
            //if (!Directory.Exists(uploadpath))
            //{
            //    Directory.CreateDirectory(uploadpath);
            //}
            if (!Directory.Exists(uploadpath))
            {
                Directory.CreateDirectory(uploadpath);
            }
        }


        /// <summary>
        /// 文件类型检测
        /// 创建人：孙佳杰  创建时间 :2015.5.19
        /// </summary>
        /// <param name="filetype"></param>
        /// <returns></returns>
        private bool checkType(string[] filetype)
        {
            //currentType = getFileExt();
            string currentType = getFileExt();
            return Array.IndexOf(filetype, currentType) == -1;
        }


        /// <summary>
        /// 获取文件扩展名
        /// 创建人：孙佳杰 创建时间:2015.5.19
        /// </summary>
        /// <returns></returns>
        private string getFileExt()
        {
            string[] temp = uploadFile.FileName.Split('.');
            return "." + temp[temp.Length - 1].ToLower();
        }

        /// <summary>
        /// 文件大小检测
        /// 创建人：孙佳杰  创建时间:2015.5.19
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        private bool checkSize(int size)
        {
            return uploadFile.ContentLength >= (size * 1024 * 1024);
        }


        /// <summary>
        /// 重命名文件
        /// 创建人：孙佳杰  创建时间;2015.5.19
        /// </summary>
        /// <returns></returns>
        private string reName()
        {
            return Guid.NewGuid() + getFileExt();
        }

        #endregion

        #region 保存文件过滤格式

        /// <summary>
        /// 功能描述：保存文件过滤格式
        /// 创建人：甘春雨
        /// 创建时间：2015年11月27日14:20:09
        /// </summary>
        public void SaveFile(HttpPostedFileBase file, string path)
        {
            FileInfo info = new FileInfo(path);
            try
            {
                if (Regex.IsMatch(info.Extension.ToUpper(), ImageExts)) //是图片
                {
                    ImageUtil.StreamThumbnail(file.InputStream, path, 70);
                }
                else
                {
                    file.SaveAs(path);
                }
            }
            catch (Exception)
            {
            }
        }
        #endregion

        public void upFile()
        {
            throw new NotImplementedException();
        }


    }
}
