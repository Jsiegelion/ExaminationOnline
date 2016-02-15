using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;

namespace Mammothcode.Core.File
{
    /// <summary>
    ///  文件操作类
    /// </summary>
    public class FileUtil
    {

        #region 获得文件名称和文件扩展名

        /// <summary>
        /// 功能：获取文件的名字
        /// </summary>
        /// <param name="fileName">文件名称</param>
        /// <returns>返回的文件名称</returns>
        public static string GetFileName(string fileName)
        {
            int i = fileName.LastIndexOf("\\") + 1;
            string Name = fileName.Substring(i);
            return Name;
        }
        /// <summary>
        /// 功能：获取文件扩展名
        /// </summary>
        /// <param name="fileName">文件名称</param>
        /// <returns>返回的文件扩展名</returns>
        public static string GetExtension(string fileName)
        {
            int i = fileName.LastIndexOf(".") + 1;
            string Name = fileName.Substring(i);
            return Name;
        }

        #endregion

        #region 获得项目路径

        /// <summary>
        /// 功能：获得项目路径
        /// </summary>
        /// <returns>项目路径（主要用到C/S）</returns>
        public static string GetAppStartPath()
        {
            Assembly Asm = Assembly.GetExecutingAssembly();
            //获取项目文件的路径
            string appStartupPath = Asm.Location.Substring(0, (Asm.Location.LastIndexOf("\\") + 1));
            return appStartupPath;
        }

        #endregion

        #region 获得服务器映射后的路径

        /// <summary>
        ///  功能：获得服务器上的映射后的路径
        /// </summary>
        /// <param name="filePath">要映射的地址</param>
        /// <returns></returns>
        public static string GetServerMapPath(string filePath)
        {
            return System.Web.HttpContext.Current.Server.MapPath(filePath);
        }

        #endregion

        #region 创建和删除文件夹

        /// <summary>
        /// 功能：创建文件夹
        /// </summary>
        /// <param name="saveFolder"></param>
        public static void CreateFolder(string saveFolder, ref string error)
        {
            try
            {
                if (!System.IO.Directory.Exists(saveFolder))
                {
                    System.IO.Directory.CreateDirectory(saveFolder);
                }
            }
            catch (Exception ex)
            {
                error += "创建文件夹（ex）：" + ex.Message + "\r\n";
                error += "path:" + saveFolder + "\r\n";
            }
        }

        /// <summary> 
        /// 功能：用递归方法删除文件夹目录及文件 
        /// </summary> 
        /// <param name="dir">带文件夹名的路径</param>  
        /// <param name="error">出错信息</param>
        public static void DeleteFolder(string dir, ref string error)
        {
            try
            {
                if (System.IO.Directory.Exists(dir)) //如果存在这个文件夹删除之  
                {
                    foreach (string d in System.IO.Directory.GetFileSystemEntries(dir))
                    {
                        if (System.IO.File.Exists(d))
                        {
                            System.IO.FileInfo fi = new System.IO.FileInfo(d);
                            if (fi.Attributes.ToString().IndexOf("ReadOnly") != -1)
                                fi.Attributes = System.IO.FileAttributes.Normal;

                            System.IO.File.Delete(d); //直接删除其中的文件 
                        }
                        else
                        {
                            DeleteFolder(dir, ref error); //递归删除子文件夹 
                        }
                    }
                    System.IO.Directory.Delete(dir, true); //删除已空文件夹                  
                }
            }
            catch (Exception ex)
            {
                error += "递归方法删除文件夹目录及文件（ex）：" + ex.Message + "\r\n";
                error += "path:" + dir + "\r\n";
            }
        }
        #endregion

        #region 移动文件

        /// <summary>
        /// 移动文件
        /// 创建人：孙佳杰 创建时间:2015.5.12
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="targetDic"></param>
        public static void MoveFile(string filePath,string targetDic)
        {
            FileInfo file = new FileInfo(filePath);
            if (file.Exists)
            {
                file.MoveTo(targetDic);
            }
        }

        #endregion

        #region 读取文本文件

        /// <summary>
        /// 功能：读取文件的所有内容
        /// </summary>
        /// <param name="FilePath">文件路径</param>
        /// <param name="FileEncoding">文件的编码</param>
        /// <param name="error">出错信息</param>
        /// <returns>读取到的文件内容</returns>
        public static string GetFileTotalContent(string FilePath, string FileEncoding, ref string error)
        {
            System.IO.FileStream fs = null;
            System.IO.StreamReader sr = null;
            string content = string.Empty;
            try
            {
                fs = new System.IO.FileStream(FilePath, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.ReadWrite);
                if (string.IsNullOrEmpty(FileEncoding))
                {
                    FileEncoding = "utf-8";
                }
                sr = new System.IO.StreamReader(fs, System.Text.Encoding.GetEncoding(FileEncoding));
                content = sr.ReadToEnd();
                fs.Close();
                sr.Close();
            }
            catch (Exception ex)
            {
                error += "读取文件（ex）：" + ex.Message + "\r\n";
                error += "path:" + FilePath + "\r\n";
                fs.Close();
                sr.Close();
            }
            return content;
        }

        /// <summary>
        /// 功能：读取txt文本内容(转化为List集合)
        /// </summary>
        /// <param name="path">txt本地路径</param>
        /// <returns>list数据集合</returns>
        public List<string> ReadFileTxtToList(string path)
        {
            //申明要返回list集合
            List<string> list = null;
            try
            {
                //根据path中文件读取字符
                using (StreamReader sr = new StreamReader(path))
                {

                    //依次按行进行读取
                    String line = sr.ReadLine();
                    list = new List<string>();
                    while (line != null)
                    {
                        list.Add(line);
                        line = sr.ReadLine();
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("文件无法读取:" + e.ToString());
            }
            return list;
        }

  
        #endregion

        #region 写入文本文件

        /// <summary>
        /// 写文件(子符）
        /// </summary>
        /// <param name="Path">写入文件地址</param>
        /// <param name="Encoding">编码格式</param>
        /// <param name="Strings">写入内容</param>
        /// <param name="error">错误信息</param>
        public static bool WriteFile(string Path, string Encoding, string Strings, ref string error)
        {
            bool flag = false;
            System.IO.StreamWriter f2 = null;
            try
            {
                //如果无文件夹先创建文件夹
                string dirPath = Path.Remove(Path.LastIndexOf("."));
                string errorCreateFolder = string.Empty;
                CreateFolder(dirPath, ref errorCreateFolder);

                if (!System.IO.File.Exists(Path))
                {
                    System.IO.FileStream f = System.IO.File.Create(Path);
                    f.Close();
                }
                //f2 = new System.IO.StreamWriter(Path, false, System.Text.Encoding.Default);
                f2 = new System.IO.StreamWriter(Path, false, System.Text.Encoding.GetEncoding("utf-8"));
                f2.WriteLine(Strings);
                f2.Close();
                f2.Dispose();

                flag = true;
            }
            catch (Exception ex)
            {
                error += " 写文件（ex）：" + ex.Message + "\r\n";
                error += "path:" + Path + "\r\n";
                f2.Close();
                f2.Dispose();
            }
            return flag;
        }

        /// <summary>
        ///  写文件(流）
        /// </summary>
        /// <param name="Path">写入文件地址</param>
        /// <param name="stream">流</param>
        /// <param name="error">错误信息</param>
        /// <returns></returns>
        public static bool WriteFile(string Path, Stream stream, ref string error)
        {
            try
            {
                //如果无文件夹先创建文件夹
                //string dirPath = Path.Remove(Path.LastIndexOf("\\"));
                //string errorCreateFolder = string.Empty;
                //CreateFolder(dirPath, ref errorCreateFolder);

                // 把 Stream 转换成 byte[] 
                byte[] bytes = new byte[stream.Length];
                stream.Read(bytes, 0, bytes.Length);
                // 设置当前流的位置为流的开始 
                stream.Seek(0, SeekOrigin.Begin);

                // 把 byte[] 写入文件 
                FileStream fs = new FileStream(Path, FileMode.Create);
                BinaryWriter bw = new BinaryWriter(fs);
                bw.Write(bytes);
                bw.Close();
                fs.Close();
                return true;
            }
            catch (Exception ex)
            {
                error = "写文件(ex)" + ex.ToString();
                error += "path:" + Path + "\r\n";
                throw;
            }
        }

        #endregion

        #region 删除文件

        /// <summary>
        /// 功能：删除文件
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="error">出错信息</param>
        public static void DeleteFile(string path, ref string error)
        {
            try
            {
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
            }
            catch (Exception ex)
            {
                error += "删除文件（ex）：" + ex.Message + "\r\n";
                error += "path:" + path + "\r\n";
            }
        }

        #endregion

        #region 读取配置文件

        /// <summary>
        /// 读取配置文件
        /// 创建人：孙佳杰  创建时间:2015.8.15
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetConfig(string name)
        {
            return System.Configuration.ConfigurationManager.AppSettings[name];
        }
        
        #endregion
    }
}
