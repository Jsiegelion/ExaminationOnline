using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Word;
using O2S.Components.PDFRender4NET;
using ApplicationClass = Microsoft.Office.Interop.Word.ApplicationClass;
using PowerPoint = Microsoft.Office.Interop.PowerPoint;
using Microsoft.Office.Core;
using System.Drawing.Drawing2D;

namespace Mammothcode.Core.File.DocExeclHtmlPdfFile
{
    /// <summary>
    /// OFFICE转化PDF格式文件,PDF转化为图片
    /// 创建人：金协民   创建时间：2015年12月18日10:19:37
    /// 修改人：孙佳杰  修改时间：2015年12月30日10:49:27
    /// 修改内容：修复了PDF文件转化为图片文件时的背景缺失问题
    /// </summary>
    public class OfficeToPdf
    {
        #region WORD文档转换PDF格式文件
        /// <summary>
        /// WORD文档转换PDF格式文件
        /// 创建人：张活生
        /// 创建时间：2015年12月10日10:20:37
        /// </summary>
        /// <param name="sourcePath">源文件路径</param>
        /// <param name="targetPath">目标文件路径</param>
        /// <returns>true：转换成功；false：转换失败</returns>
        public static bool DocConvertToPdf(string sourcePath, string targetPath)
        {
            bool result = false;
            WdExportFormat exportFormat = WdExportFormat.wdExportFormatPDF;
            object paramMissing = Type.Missing;
            ApplicationClass wordApplication = new ApplicationClass();
            Document wordDocument = null;
            try
            {
                object paramSourceDocPath = sourcePath;
                string paramExportFilePath = targetPath;

                WdExportFormat paramExportFormat = exportFormat;
                bool paramOpenAfterExport = false;
                WdExportOptimizeFor paramExportOptimizeFor = WdExportOptimizeFor.wdExportOptimizeForPrint;
                WdExportRange paramExportRange = WdExportRange.wdExportAllDocument;
                int paramStartPage = 0;
                int paramEndPage = 0;
                WdExportItem paramExportItem = WdExportItem.wdExportDocumentContent;
                bool paramIncludeDocProps = true;
                bool paramKeepIRM = true;
                WdExportCreateBookmarks paramCreateBookmarks = Microsoft.Office.Interop.Word.WdExportCreateBookmarks.wdExportCreateWordBookmarks;
                bool paramDocStructureTags = true;
                bool paramBitmapMissingFonts = true;
                bool paramUseISO19005_1 = false;

                wordDocument = wordApplication.Documents.Open(
                ref paramSourceDocPath, ref paramMissing, ref paramMissing,
                ref paramMissing, ref paramMissing, ref paramMissing,
                ref paramMissing, ref paramMissing, ref paramMissing,
                ref paramMissing, ref paramMissing, ref paramMissing,
                ref paramMissing, ref paramMissing, ref paramMissing,
                ref paramMissing);

                if (wordDocument != null)
                    wordDocument.ExportAsFixedFormat(paramExportFilePath,
                    paramExportFormat, paramOpenAfterExport,
                    paramExportOptimizeFor, paramExportRange, paramStartPage,
                    paramEndPage, paramExportItem, paramIncludeDocProps,
                    paramKeepIRM, paramCreateBookmarks, paramDocStructureTags,
                    paramBitmapMissingFonts, paramUseISO19005_1,
                    ref paramMissing);
                result = true;
            }
            catch
            {
                result = false;
            }
            finally
            {
                if (wordDocument != null)
                {
                    wordDocument.Close(ref paramMissing, ref paramMissing, ref paramMissing);
                    wordDocument = null;
                }
                if (wordApplication != null)
                {
                    wordApplication.Quit(ref paramMissing, ref paramMissing, ref paramMissing);
                    wordApplication = null;
                }
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            return result;
        }
        #endregion

        #region 把Excel文件转换成PDF格式文件
        private void button2_Click(object sender, EventArgs e)
        {
            if (XLSConvertToPDF("E:/ZHS/city.xls", "E:/ZHS/testX.pdf"))
            {
                //MessageBox.Show("XLS转换成功！");
            }
        }

        /// <summary>
        /// 把Excel文件转换成PDF格式文件
        /// </summary>
        /// <param name="sourcePath">源文件路径</param>
        /// <param name="targetPath">目标文件路径</param>
        /// <returns>true：转换成功；false：转换失败</returns>
        public static bool XLSConvertToPDF(string sourcePath, string targetPath)
        {
            bool result = false;
            XlFixedFormatType targetType = XlFixedFormatType.xlTypePDF;
            object missing = Type.Missing;
            Microsoft.Office.Interop.Excel.ApplicationClass application = null;
            Workbook workBook = null;
            try
            {
                application = new Microsoft.Office.Interop.Excel.ApplicationClass();
                object target = targetPath;
                object type = targetType;
                workBook = application.Workbooks.Open(sourcePath, missing, missing, missing, missing, missing,
                        missing, missing, missing, missing, missing, missing, missing, missing, missing);

                workBook.ExportAsFixedFormat(targetType, target, XlFixedFormatQuality.xlQualityStandard, true, false, missing, missing, missing, missing);
                result = true;
            }
            catch
            {
                result = false;
            }
            finally
            {
                if (workBook != null)
                {
                    workBook.Close(true, missing, missing);
                    workBook = null;
                }
                if (application != null)
                {
                    application.Quit();
                    application = null;
                }
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            return result;
        }

        #endregion

        #region 把PowerPoing(PPT)文件转换成PDF格式文件

        /// <summary>
        /// 把PowerPoing文件转换成PDF格式文件
        /// </summary>
        /// <param name="sourcePath">源文件路径</param>
        /// <param name="targetPath">目标文件路径</param>
        /// <returns>true=转换成功</returns>
        public static bool PPTConvertToPDF(string sourcePath, string targetPath)
        {
            bool result;
            PowerPoint.PpSaveAsFileType targetFileType = PowerPoint.PpSaveAsFileType.ppSaveAsPDF;
            object missing = Type.Missing;
            PowerPoint.ApplicationClass application = null;
            PowerPoint.Presentation persentation = null;
            try
            {
                application = new PowerPoint.ApplicationClass();
                persentation = application.Presentations.Open(sourcePath, MsoTriState.msoTrue, MsoTriState.msoFalse, MsoTriState.msoFalse);
                persentation.SaveAs(targetPath, targetFileType, Microsoft.Office.Core.MsoTriState.msoTrue);

                result = true;
            }
            catch (Exception ex)
            {
                result = false;
            }
            finally
            {
                if (persentation != null)
                {
                    persentation.Close();
                    persentation = null;
                }
                if (application != null)
                {
                    application.Quit();
                    application = null;
                }
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            return result;
        }

        #endregion

        #region 把PDF文件转换成图片文件

        public enum Definition
        {
            One = 1, Two = 2, Three = 3, Four = 4, Five = 5, Six = 6, Seven = 7, Eight = 8, Nine = 9, Ten = 10
        }

        public static int BytesLastIndexOf(Byte[] buffer, int length, string Search)
        {
            if (buffer == null)
                return -1;
            if (buffer.Length <= 0)
                return -1;
            byte[] SearchBytes = Encoding.Default.GetBytes(Search.ToUpper());
            for (int i = length - SearchBytes.Length; i >= 0; i--)
            {
                bool bFound = true;
                for (int j = 0; j < SearchBytes.Length; j++)
                {
                    if (ByteUpper(buffer[i + j]) != SearchBytes[j])
                    {
                        bFound = false;
                        break;
                    }
                } if (bFound)
                    return i;
            }
            return -1;
        }

        private static byte ByteUpper(byte byteValue)
        {
            char charValue = Convert.ToChar(byteValue);
            if (charValue < 'a' || charValue > 'z')
                return byteValue;
            else
                return Convert.ToByte(byteValue - 32);
        }

        /// <summary>
        /// 获取PDF的文件页数
        /// 创建人：孙佳杰  创建时间：2015年12月30日11:07:44
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static int GetPDFPageCount(string path) //获取pdf文件的页数
        {
            byte[] buffer = System.IO.File.ReadAllBytes(path);
            int length = buffer.Length;
            if (buffer == null)
                return -1;
            if (buffer.Length <= 0)
                return -1;
            try
            {
                int i = 0;
                int nPos = BytesLastIndexOf(buffer, length, "/Type/Pages");
                if (nPos == -1)
                    return -1;
                string pageCount = null;
                for (i = nPos; i < length - 10; i++)
                {
                    if (buffer[i] == '/' && buffer[i + 1] == 'C' && buffer[i + 2] == 'o' && buffer[i + 3] == 'u' && buffer[i + 4] == 'n' && buffer[i + 5] == 't')
                    {
                        int j = i + 3;
                        while (buffer[j] != '/' && buffer[j] != '>')
                            j++;
                        pageCount = Encoding.Default.GetString(buffer, i, j - i);
                        break;
                    }
                }
                if (pageCount == null)
                    return -1;
                int n = pageCount.IndexOf("Count");
                if (n > 0)
                {
                    pageCount = pageCount.Substring(n + 5).Trim();
                    for (i = pageCount.Length - 1; i >= 0; i--)
                    {
                        if (pageCount[i] >= '0' && pageCount[i] <= '9')
                        {
                            return int.Parse(pageCount.Substring(0, i + 1));
                        }
                    }
                }
                return -1;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        /// <summary>
        /// 将PDF文档转换为图片的方法
        /// </summary>
        /// <param name="pdfInputPath">PDF文件路径</param>
        /// <param name="imageOutputPath">图片输出完整路径(包括文件名)</param>
        /// <param name="startPageNum">从PDF文档的第几页开始转换</param>
        /// <param name="endPageNum">从PDF文档的第几页开始停止转换</param>
        /// <param name="imageFormat">设置所需图片格式</param>
        /// <param name="definition">设置图片的清晰度，数字越大越清晰</param>
        /// <returns></returns>
        public static void ConvertPdf2Image(string pdfInputPath, string imageOutputPath, string imageName,
             int startPageNum, int endPageNum, ImageFormat imageFormat, Definition definition,ref string logContent)
        {
            try
            {
                using (PDFFile pdfFile = PDFFile.Open(pdfInputPath))
                {
                    logContent += "成功打开" + pdfInputPath+"文件";
                    if (startPageNum <= 0)
                    {
                        startPageNum = 1;
                    }

                    if (endPageNum > pdfFile.PageCount)
                    {
                        endPageNum = pdfFile.PageCount;
                    }

                    if (startPageNum > endPageNum)
                    {
                        int tempPageNum = startPageNum;
                        startPageNum = endPageNum;
                        endPageNum = startPageNum;
                    }

                    var bitMap = new Bitmap[endPageNum];
                    //设置保存路径，如果没有文件夹的话就重新创建一个
                    imageOutputPath += "\\" + imageName + "\\";
                    if (!Directory.Exists(imageOutputPath))
                    {
                        Directory.CreateDirectory(imageOutputPath);
                        logContent += "成功保存" + imageOutputPath+"文件夹";
                    }
                    for (int i = startPageNum; i <= endPageNum; i++)
                    {
                        Bitmap pageImage = pdfFile.GetPageImage(i - 1, 56 * (int)definition);
                        Bitmap newPageImage = new Bitmap(pageImage.Width / 4, pageImage.Height / 4);
                        Graphics g = Graphics.FromImage(newPageImage);
                        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        //重新画图的时候Y轴减去130，高度也减去130  这样水印就看不到了
                        g.DrawImage(pageImage, new System.Drawing.Rectangle(0, 0, pageImage.Width / 4, pageImage.Height / 4),
                            new System.Drawing.Rectangle(0, 130, pageImage.Width, pageImage.Height - 130), GraphicsUnit.Pixel);
                        bitMap[i - 1] = newPageImage;

                        string saveFilePath = imageOutputPath + i.ToString() + "." + imageFormat.ToString();
                        bitMap[i - 1].Save(saveFilePath, imageFormat);
                        bitMap[i - 1].Dispose();
                        g.Dispose();
                        pageImage.Dispose();
                        newPageImage.Dispose();
                        //图片保存的大小
                        ImageUtil.SetImgSize(saveFilePath, 1180, 1572);
                    }
                    logContent += "成功保存完所有图片";
                    //把所有的图片保存到文件夹中
                   // SaveImgToFolder(imageOutputPath, imageName, imageFormat, bitMap);
                }
            }
            catch (Exception ex)
            {
                logContent += ex.ToString();
                throw ex;
            }
            finally
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        /// <summary>
        /// 将图片保存到对应的文件夹中
        /// 创建人：孙佳杰  创建时间：2015年12月30日11:04:39
        /// </summary>
        /// <param name="imageOutputPath">图片输出的路径</param>
        /// <param name="imageName">图片文件夹名称</param>
        /// <param name="imageFormat">图片格式</param>
        /// <param name="maps">需要保存的图片集合</param>
        /// <returns></returns>
        private static bool SaveImgToFolder(string imageOutputPath, string imageName, ImageFormat imageFormat, params Bitmap[] maps)
        {
            //设置保存路径，如果没有文件夹的话就重新创建一个
            imageOutputPath += "\\" + imageName + "\\";
            if (!Directory.Exists(imageOutputPath))
            {
                Directory.CreateDirectory(imageOutputPath);
            }

            try
            {
                for (int i = 1; i <= maps.Length; i++)
                {
                    string saveFilePath = imageOutputPath + i.ToString() + "." + imageFormat.ToString();
                    maps[i - 1].Save(saveFilePath, imageFormat);
                    //图片保存的大小
                    //ImageUtil.SetImgSize(saveFilePath, 1180, 1572);
                    maps[i - 1].Dispose();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

        /// <summary>
        /// 合并图片
        /// 创建人：孙佳杰 创建时间：2015年12月30日10:50:55
        /// 备注：暂时不用到
        /// </summary>
        /// <param name="maps"></param>
        /// <returns></returns>
        private static Bitmap MergerImg(params Bitmap[] maps)
        {
            int i = maps.Length;

            if (i == 0)
                throw new Exception("图片数不能够为0");
            else if (i == 1)
                return maps[0];

            //创建要显示的图片对象,根据参数的个数设置宽度
            Bitmap backgroudImg = new Bitmap(maps[0].Width, i * maps[0].Height);
            Graphics g = Graphics.FromImage(backgroudImg);
            //清除画布,背景设置为白色
            g.Clear(System.Drawing.Color.White);
            for (int j = 0; j < i; j++)
            {
                g.DrawImage(maps[j], 0, j * maps[j].Height, maps[j].Width, maps[j].Height);
            }
            g.Dispose();
            return backgroudImg;
        }

        #endregion

    }
}
