using Mammothcode.Data.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;

namespace Mammothcode.Data.Log
{
    public class DapperLogUtils
    {
        /// <summary>
        /// 参数前缀 
        /// </summary>
        protected const string ParamPrefix = "@";

        /// <summary>
        ///  记录Mintcode.Public.Data的日志文件
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="param">sql参数</param>
        /// <param name="stopWatch">StopWatch</param>
        /// <param name="ex">异常信息</param>
        public static void Log(string sql, object param, Stopwatch stopWatch, Exception ex)
        {
            string message = "\r\n--------------------------------------------------------------------------------------";

            var sqlStr = GetExecuteSql(sql, param);
            var execptionStr = GetExecption(ex);
            var timeStr = GetExecuteTime(stopWatch);

            message += string.Format("\r\n 访问页面：{0} \r\n 原生SQL：{1} \r\n 执行时间：{2} \r\n 访问者IP：{3} \r\n ",
                   string.IsNullOrWhiteSpace(GetRequestPath()) ? "空" : GetRequestPath(),
                   string.IsNullOrWhiteSpace(sqlStr) ? "空" : sqlStr,
                   string.IsNullOrWhiteSpace(timeStr) ? "空" : timeStr,
                   string.IsNullOrWhiteSpace(GetIp()) ? "空" : GetIp()
                );

            if (!string.IsNullOrWhiteSpace(execptionStr))
            {
                message += string.Format("异常信息：{0}", execptionStr);
            }
            //记录到日志文件 
            DapperLog4netCommon.Info(message);
        }

        #region 记录的私有方法

        /// <summary>
        /// 根据传入Sql和param生成原装的SQL语句
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        protected static string GetExecuteSql(string sql, object param)
        {
            string sqlQuery = string.Empty;

            try
            {
                IEnumerable multiExec = param as IEnumerable;
                if (multiExec != null && !(multiExec is string))
                {
                    int index = 0;
                    foreach (var obj in multiExec)
                    {
                        sqlQuery += index == 0 ? GetSingleExecuteSql(obj, sql) + "\r\n\r\n" : "          " + GetSingleExecuteSql(obj, sql) + "\r\n\r\n";
                        index++;
                    }
                }
                else
                {
                    sqlQuery = GetSingleExecuteSql(param, sql);
                }
            }
            catch (Exception)
            {
                
                //throw;
            }
            return sqlQuery;
        }

        /// <summary>
        ///   私有根据传入Sql和param生成原装的SQL语句：获取单条SQL
        /// </summary>
        /// <param name="param"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        private static string GetSingleExecuteSql(object param, string sql)
        {
            string sqlQuery = string.Empty;
            var fields = param == null ? null : param.GetType().GetProperties();
            int index = 0;
            if (fields != null)
            {
                foreach (PropertyInfo property in fields)
                {
                    var pro = property.GetValue(param, null);
                    string setSqlQuery1 = index == 0 ? " SET {0}{1} = {2};\r\n" : "           SET {0}{1} = {2};\r\n";
                    string setSqlQuery2 = index == 0 ? " SET {0}{1} = '{2}';\r\n" : "           SET {0}{1} = '{2}';\r\n";

                    if (pro.GetType().Equals(typeof(int)) || pro.GetType().Equals(typeof(Int32)) || pro.GetType().Equals(typeof(Int64)))
                    {
                        sqlQuery += string.Format(setSqlQuery1, ParamPrefix, property.Name, pro);
                    }
                    else
                    {
                        sqlQuery += string.Format(setSqlQuery2, ParamPrefix, property.Name, pro);
                    }
                    index++;
                }
                sqlQuery += "           " + sql;
            }
            else
            {
                sqlQuery += sql;
            }

            return sqlQuery;
        }

        /// <summary>
        /// 统计反射时间和执行时间的综合
        /// </summary>
        /// <param name="stopWatch"></param>
        /// <returns></returns>
        protected static string GetExecuteTime(Stopwatch stopWatch)
        {
            if (stopWatch.IsRunning)
            {
                stopWatch.Stop();
            }
            return (stopWatch != null ? stopWatch.Elapsed.TotalSeconds * 1000 : -100).ToString() + "毫秒";
        }

        /// <summary>
        /// 获取执行异常信息
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        protected static string GetExecption(Exception ex)
        {
            return ex == null ? string.Empty : ex.ToString();
        }

        #endregion

        #region 获取IP的方法

        /// <summary>
        /// 获取客户端IP
        /// </summary>
        /// <returns></returns>
        public static string GetIp()
        {
            try
            {
                return HttpContext.Current != null ? HttpContext.Current.Request.UserHostAddress : string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        ///   获取客户端的请求地址
        /// </summary>
        /// <returns></returns>
        public static string GetRequestPath()
        {
            try
            {
                return HttpContext.Current != null ? HttpContext.Current.Request.Url.AbsolutePath : string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        #endregion
    }
}
