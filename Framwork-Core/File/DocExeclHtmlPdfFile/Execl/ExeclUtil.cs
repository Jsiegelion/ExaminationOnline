using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace Mammothcode.Core.File.DocExeclHtmlPdfFile.Execl
{
    /// <summary>
    /// Execl的常用操作类
    /// 创建人：孙佳杰 创建时间:2015.3.15
    /// </summary>
    public class ExeclUtil
    {
        public class ExeclModel
        {
            public string name { get; set; }

            public DataTable dt { get; set; }

        }

        /// <summary>
        /// 功能：由Excel(2007)导入DataTable
        /// </summary>
        /// <param name="excelFilePath">Excel文件路径，为物理路径。</param>
        /// <param name="sheetName">Excel工作表名称(默认为NULL)</param>
        /// <param name="headerRowIndex">Excel表头行索引(默认为0)</param>
        /// <param name="error">出错信息</param>
        /// <returns>DataTable</returns>
        public static DataTable ExcelToDataTable(string excelFilePath, string sheetName, int? headerRowIndex, ref string error)
        {

            if (System.IO.File.Exists(excelFilePath) == true)
            {
                using (FileStream stream = System.IO.File.OpenRead(excelFilePath))
                {
                    DataTable dataTable = new DataTable();
                    try
                    {
                        XSSFWorkbook workbook = new XSSFWorkbook(stream);
                        XSSFSheet sheet;
                        XSSFRow headerRow;
                        if (string.IsNullOrEmpty(sheetName) == true)
                        {
                            sheet = (XSSFSheet)workbook.GetSheetAt(0);
                        }
                        else
                        {
                            sheet = (XSSFSheet)workbook.GetSheet(sheetName);
                        }
                        if (headerRowIndex == null)
                        {
                            headerRow = (XSSFRow)sheet.GetRow(0);
                        }
                        else
                        {
                            headerRow = (XSSFRow)sheet.GetRow((int)headerRowIndex);
                        }
                        int cellCount = headerRow.LastCellNum;
                        //添加列名              
                        for (int i = headerRow.FirstCellNum; i < cellCount; i++)
                        {
                            DataColumn column = new DataColumn(headerRow.GetCell(i).StringCellValue);
                            dataTable.Columns.Add(column);
                        }
                        //添加数据
                        for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
                        {
                            XSSFRow row = (XSSFRow)sheet.GetRow(i);
                            DataRow dataRow = dataTable.NewRow();
                            for (int j = row.FirstCellNum; j < cellCount; j++)
                            {
                                if (row.GetCell(j) == null)
                                {
                                    dataRow[j] = null;
                                }
                                else
                                {
                                    dataRow[j] = row.GetCell(j).ToString();
                                }
                            }
                            dataTable.Rows.Add(dataRow);
                        }
                        workbook = null;
                        sheet = null;
                        return dataTable;
                    }
                    catch (Exception ex)
                    {
                        error = "由Excel导入DataTable(ex)" + ex.ToString();
                        throw;
                    }
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 功能：将多个表的数据导出到EXECL
        /// 创建人：孙佳杰  创建时间：2015.4.20
        /// </summary>
        /// <param name="modelList"></param>
        /// <param name="execlFilePath"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool DataTableToExeclMoreSheet(List<ExeclModel> modelList,string execlFilePath,ref string error)
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    foreach (ExeclModel model in modelList)
                    {
                        HSSFSheet sheet = (HSSFSheet)workbook.CreateSheet(model.name);
                        HSSFRow headerRow = (HSSFRow)sheet.CreateRow(0);
                        // handling header.
                        foreach (DataColumn column in model.dt.Columns)
                        {
                            headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
                        }
                        // handling value.
                        int rowIndex = 1;
                        foreach (DataRow row in model.dt.Rows)
                        {
                            HSSFRow dataRow = (HSSFRow)sheet.CreateRow(rowIndex);
                            foreach (DataColumn column in model.dt.Columns)
                            {
                                dataRow.CreateCell(column.Ordinal).SetCellValue(row[column].ToString());
                            }
                            rowIndex++;
                        }
                    }
                    workbook.Write(ms);
                    ms.Position = 0;
                    return FileUtil.WriteFile(execlFilePath, ms, ref error);
                }
            }
            catch(Exception ex)
            {
                error = "由DataSet导出Excel(ex)" + ex.ToString();
                throw;
            }
        }

        /// <summary>
        /// 功能：将EXECL中的数据导出到
        /// </summary>
        /// <param name="execlFilePath"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static List<ExeclModel> ExeclMoSheetToDataTable(string execlFilePath,ref string error)
        {
            List<ExeclModel> returnList = new List<ExeclModel>();
            if (System.IO.File.Exists(execlFilePath) == true)
            {
                using (FileStream stream = System.IO.File.OpenRead(execlFilePath))
                {
                    DataTable dataTable = new DataTable();
                    try
                    {

                        HSSFWorkbook workbook = new HSSFWorkbook(stream);
                        HSSFSheet sheet;
                        HSSFRow headerRow;
                        int sheetsNum = workbook.NumberOfSheets;
                        for (int i = 0; i < sheetsNum; i++)
                        {
                            sheet = (HSSFSheet)workbook.GetSheetAt(i);
                            headerRow = (HSSFRow)sheet.GetRow(0);
                            int cellCount = headerRow.Cells.Count;
                            int trueCellCount = 0;  //实际的列数
                            //添加列名              
                            for (int j = headerRow.FirstCellNum; j < cellCount; j++)
                            {
                                if (headerRow.GetCell(j) != null)
                                {
                                    string columnName = headerRow.GetCell(j).StringCellValue;
                                    if (!string.IsNullOrEmpty(columnName))
                                    {
                                        DataColumn column = new DataColumn(columnName);
                                        dataTable.Columns.Add(column);
                                        trueCellCount++;
                                    }
                                }
                            }
                            int spaceCount = 0;
                            //添加数据
                            for (int k = (sheet.FirstRowNum + 1); k <= sheet.LastRowNum; k++)
                            {
                                HSSFRow row = (HSSFRow)sheet.GetRow(k);
                                DataRow dataRow = dataTable.NewRow();
                                if (row != null)
                                {
                                    for (int j = row.FirstCellNum; j < trueCellCount; j++)
                                    {
                                        if (row.GetCell(j) == null)
                                        {
                                            dataRow[j] = null;
                                        }
                                        else
                                        {
                                            dataRow[j] = row.GetCell(j).ToString();
                                        }

                    
                                    }
                               
                                dataTable.Rows.Add(dataRow);
                                     }
                            }
                        
                            returnList.Add(new ExeclModel()
                            {
                                dt = dataTable,
                                name = sheet.SheetName
                            });
                            sheet = null;
                            dataTable = new DataTable();
                        }
                        return returnList;
                    }
                    catch (Exception ex)
                    {
                        error = "由Excel导入DataTable(ex)" + ex.ToString();
                        throw;
                    }
                }
            }
            else
            {
                return null;
            }
        }

        ///// <summary>
        ///// 功能：由DataSet导出Excel 
        ///// </summary>
        ///// <param name="sourceDs">导入的DataSet</param>
        ///// <param name="fileName">文件名称（如果是Web版就 直接文件名称；否则完整路径）</param>
        ///// <param name="sheetName">工作表名称</param>
        ///// <param name="needHttpContext">是否需要HttpContext</param>
        ///// <param name="error">错误信息</param>
        ///// <returns></returns>
        //public static bool DataTableToExcel(DataSet sourceDs, string fileName, string sheetName, bool needHttpContext, ref string error)
        //{
        //    HSSFWorkbook workbook = new HSSFWorkbook();
        //    try
        //    {
        //        using (MemoryStream ms = new MemoryStream())
        //        {
        //            string[] sheetNames = sheetName.Split(',');
        //            for (int i = 0; i < sheetNames.Length; i++)
        //            {
        //                HSSFSheet sheet = (HSSFSheet)workbook.CreateSheet(sheetNames[i]);
        //                HSSFRow headerRow = (HSSFRow)sheet.CreateRow(0);
        //                // handling header.
        //                foreach (DataColumn column in sourceDs.Tables[i].Columns)
        //                {
        //                    headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
        //                }
        //                // handling value.
        //                int rowIndex = 1;
        //                foreach (DataRow row in sourceDs.Tables[i].Rows)
        //                {
        //                    HSSFRow dataRow = (HSSFRow)sheet.CreateRow(rowIndex);
        //                    foreach (DataColumn column in sourceDs.Tables[i].Columns)
        //                    {
        //                        dataRow.CreateCell(column.Ordinal).SetCellValue(row[column].ToString());
        //                    }
        //                    rowIndex++;
        //                }
        //            }
        //            workbook.Write(ms);
        //            ms.Position = 0;
        //            if (needHttpContext == true)
        //            {
        //                HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + fileName);
        //                HttpContext.Current.Response.BinaryWrite(ms.ToArray());
        //                //少HttpContext.Current.Response.End();
        //                return true;
        //            }
        //            else
        //            {
        //                return FileOperate.WriteFile(fileName, ms, ref error);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        error = "由DataSet导出Excel(ex)" + ex.ToString();
        //        throw;
        //    }
        //}

        /// <summary>
        ///  功能：将表数据导出到网页流中
        ///  创建人：孙佳杰  创建时间：2015年9月2日17:02:52
        /// </summary>
        /// <param name="modelList"></param>
        /// <param name="fileName"></param>
        /// <param name="error"></param>
        public static void DataTableToExeclMoreSheetHttpContext(ExeclModel ExeclModel, string fileName, ref string error)
        {
            #region HttpContext数据包

            var filename = string.Format("{0}.xls", fileName);
            System.Web.HttpContext.Current.Response.ClearContent();
            System.Web.HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=" + filename + "");
            System.Web.HttpContext.Current.Response.ContentType = "application/excel";
            System.Web.HttpContext.Current.Response.ContentEncoding = Encoding.UTF8;

            #endregion

            StringBuilder stringBuilder = new StringBuilder();
            //头部html
            stringBuilder.Append("<meta http-equiv=\"content-type\" content=\"application/excel; charset=UTF-8\"/>");
            stringBuilder.Append("<table cellspacing=\"0\" rules=\"all\" border=\"1\" style=\"border-collapse:collapse;\">");

            HSSFWorkbook workbook = new HSSFWorkbook();

            foreach (DataColumn column in ExeclModel.dt.Columns)
            {
                stringBuilder.AppendFormat("<td>{0}</td>", column.ColumnName);
            }

            foreach (DataRow row in ExeclModel.dt.Rows)
            {
                stringBuilder.Append("<tr>");
                foreach (DataColumn column in ExeclModel.dt.Columns)
                {
                    stringBuilder.AppendFormat("<td>{0}</td>", row[column]);
                }
                stringBuilder.Append("</tr>");
            }
            stringBuilder.Append("</table>");
            System.Web.HttpContext.Current.Response.Write(stringBuilder);
            System.Web.HttpContext.Current.Response.End();

        }

    }
}
