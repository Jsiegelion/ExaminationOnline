using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace Mammothcode.Core.Data.DataConvert
{
    /// <summary>
    /// 数据格式转化基础类(Json,xml,DataTable...)
    /// 作者：孙佳杰
    /// 修改时间：2015.1.13
    /// 功能：toJson（将实体转化为Json数据）
    ///       toJsonObject（将Json数据转化为实体（如果转化失败返回NULL)）
    /// </summary>
    public static class DataExchangeUtil
    {

        /// <summary>
        /// dateTime转成string
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string ToDateString(this DateTime dt)
        {
           return dt.ToString("yyyy-MM-dd HH:mm:ss");
        }

        #region Json转化库

        /// <summary>
        /// 将实体转化为Json数据
        /// </summary>
        /// <param name="obj">实体</param>
        /// <returns>JSON数据</returns>
        public static string toJson(this object obj)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        }

        /// <summary>
        /// 将Json数据转化为实体（如果转化失败返回NULL)
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="jsonStr">JSON数据</param>
        /// <returns>实体</returns>
        public static T toJsonObject<T>(this string jsonStr)
        {
            try
            {
                return (T)Newtonsoft.Json.JsonConvert.DeserializeObject(jsonStr, typeof(T));
            }
            catch
            {
                return default(T);
            }
        }

        /// <summary>
        /// 将DataTable转化为Json数据
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="dtName"></param>
        /// <returns></returns>
        public static string DataTableToJSON(this DataTable dt, string dtName)
        {
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);

            using (JsonWriter jw = new JsonTextWriter(sw))
            {
                JsonSerializer ser = new JsonSerializer();
                jw.WriteStartObject();
                jw.WritePropertyName(dtName);
                jw.WriteStartArray();
                foreach (DataRow dr in dt.Rows)
                {
                    jw.WriteStartObject();

                    foreach (DataColumn dc in dt.Columns)
                    {
                        jw.WritePropertyName(dc.ColumnName);
                        ser.Serialize(jw, dr[dc].ToString());
                    }

                    jw.WriteEndObject();
                }
                jw.WriteEndArray();
                jw.WriteEndObject();

                sw.Close();
                jw.Close();

            }

            return sb.ToString();
        }

        #endregion

        #region  Javascript 中的Json数据转化

        /// <summary>
        /// Javascript中将实体转化为json字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string javaScriptToJson(this object obj)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Serialize(obj);
        }

        /// <summary>
        /// Javascript中将json字符串解析为实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonStr"></param>
        /// <returns></returns>
        public static T javaScriptToJsonObject<T>(this string jsonStr)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Deserialize<T>(jsonStr);
        }


        #endregion

        #region 集合和DataTable的转化

        /// <summary>
        /// 功能：将List集合转化为DataTable
        /// 创建人:孙佳杰 创建时间：2015.3.15
        /// </summary>
        /// <param name="list">List集合</param>
        /// <param name="error">error错误信息</param>
        /// <returns>转化好的DataTable</returns>
        public static DataTable ToDataTable(IList list, ref string error)
        {
            try
            {
                DataTable result = new DataTable();
                if (list.Count > 0)
                {
                    //设置列属性
                    PropertyInfo[] propertys = list[0].GetType().GetProperties();
                    foreach (PropertyInfo pi in propertys)
                    {
                        result.Columns.Add(pi.Name, pi.PropertyType);
                    }
                    //填充数据
                    for (int i = 0; i < list.Count; i++)
                    {
                        ArrayList tempList = new ArrayList();
                        foreach (PropertyInfo pi in propertys)
                        {
                            object obj = pi.GetValue(list[i], null);
                            tempList.Add(obj);
                        }
                        object[] array = tempList.ToArray();
                        result.LoadDataRow(array, true);
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                error = "将List集合转化为DataTable" + ex.ToString();
                throw;
            }
        }

        /// <summary>
        /// 功能：将DataTable转化为List集合
        /// 创建人：孙佳杰  创建时间:2015.3.15
        /// </summary>
        /// <typeparam name="T">List集合中T类型</typeparam>
        /// <param name="dt">需要转化的DataTable</param>
        /// <param name="error">error错误信息</param>
        /// <returns>转化好的List集合</returns>
        public static List<T> ToList<T>(DataTable dt, ref string error) where T : class,new()
        {
            try
            {
                List<T> oblist = new List<T>();
                //创建一个属性列表器
                List<PropertyInfo> prList = new List<PropertyInfo>();
                Type t = typeof(T);
                //获得所有List中属性和DataTable属性匹配的列表
                Array.ForEach<PropertyInfo>(t.GetProperties(), p =>
                {
                    if (dt.Columns.IndexOf(p.Name) != -1) prList.Add(p);
                });
                foreach (DataRow row in dt.Rows)
                {
                    T ob = new T();
                    //找到对应的数据并且赋值
                    prList.ForEach(p =>
                    {
                        if (row[p.Name] != DBNull.Value) p.SetValue(ob, row[p.Name], null);
                    });
                    oblist.Add(ob);
                }
                return oblist;
            }
            catch (Exception ex)
            {
                error = "将DataTable转化为List集合" + ex.ToString();
                throw;
            }
        }

        #endregion
    }
}