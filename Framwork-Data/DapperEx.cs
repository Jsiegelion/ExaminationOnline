using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dapper;
using System.Data;
using System.Text.RegularExpressions;
using System.Diagnostics;
namespace Mammothcode.Public.Data
{
    /// <summary>
    /// DapperEx的ORM操作类
    /// </summary>
    public static class DapperEx
    {
        
        #region 增 删 改

        /// <summary>
       /// 插入数据
       /// </summary>
       /// <typeparam name="T"></typeparam>
       /// <param name="dbs"></param>
       /// <param name="t"></param>
       /// <param name="transaction"></param>
       /// <param name="commandTimeout"></param>
       /// <returns></returns>
        public static bool Insert<T>(this DbBase dbs, T t, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
        {
            int flag = 0;

            Stopwatch watch = new Stopwatch();
            try
            {
                var db = dbs.DbConnecttion;
                    watch.Start();
                    var sql = SqlQuery<T>.Builder(dbs);
                    bool isAutoId = false;
                    string insertSql = sql.InsertSql;

                    ModelDes modelDes = Common.GetModelDes<T>();
                    if (modelDes != null && modelDes.Properties != null)
                    {
                        foreach (var item in modelDes.Properties)
                        {
                            if ((item.CusAttribute is IdAttribute) && ((item.CusAttribute as IdAttribute).CheckAutoId))
                            {
                                isAutoId = true;
                                break;
                            }
                        }
                    }

                    if (isAutoId)
                    {
                        insertSql += ";SELECT @@IDENTITY;";
                        var recordId = db.ExecuteScalar(insertSql, t, transaction, commandTimeout);
                        flag = Convert.ToInt32(recordId);
                    }
                    else
                    {
                        flag = db.Execute(insertSql, t, transaction, commandTimeout, null, watch);
                    }

                    if (isAutoId)
                    {
                        #region 如果主键是自增Id,则将生成的id赋值到实体类中

                        if (modelDes != null && modelDes.Properties != null)
                        {
                            foreach (var item in modelDes.Properties)
                            {
                                if ((item.CusAttribute is IdAttribute) && ((item.CusAttribute as IdAttribute).CheckAutoId))
                                {
                                    var filed = t.GetType().GetProperty(item.Field);
                                    filed.SetValue(t, Convert.ChangeType(flag, filed.PropertyType), null);
                                    break;
                                }
                            }
                        }

                        #endregion

                        return true;
                    }
                return flag > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (watch.IsRunning)
                {
                    watch.Stop();
                }
            }
        }


        /// <summary>
        ///  批量插入数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbs"></param>
        /// <param name="lt"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public static bool InsertBatch<T>(this DbBase dbs, IList<T> lt, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
        {
            Stopwatch watch = new Stopwatch();
            try
            {
                   var db = dbs.DbConnecttion;
                    watch.Start();
                    var sql = SqlQuery<T>.Builder(dbs);
                    var flag = db.Execute(sql.InsertSql, lt, transaction, commandTimeout, null, watch);
                    if (watch.IsRunning)
                    {
                        watch.Stop();
                    }
                    return flag == lt.Count;
                }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 按条件删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbs"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static bool Delete<T>(this DbBase dbs, SqlQuery sql = null, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
        {
            Stopwatch watch = new Stopwatch();
            try
            {
                     var db = dbs.DbConnecttion;
                    watch.Start();
                    if (sql == null)
                    {
                        sql = SqlQuery<T>.Builder(dbs);
                    }
                    var f = db.Execute(sql.DeleteSql, sql.Param, transaction, commandTimeout, null, watch);
                    if (watch.IsRunning)
                    {
                        watch.Stop();
                    }
                    return f > 0;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 按照实体进行删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbs"></param>
        /// <param name="t"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public static bool Delete<T>(this DbBase dbs, T t, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
        {
            Stopwatch watch = new Stopwatch();
            try
            {
                    var db = dbs.DbConnecttion;
                    watch.Start();
                    var sql = SqlQuery<T>.Builder(dbs);
                    var f = db.Execute(sql.DeleteSql, t, transaction, commandTimeout, null, watch);
                    if (watch.IsRunning)
                    {
                        watch.Stop();
                    }
                    return f > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 批量删除实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbs"></param>
        /// <param name="lt"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public static bool DeleteBatch<T>(this DbBase dbs, IList<T> lt, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
        {
            Stopwatch watch = new Stopwatch();
            try
            {
                    var db = dbs.DbConnecttion;
                    watch.Start();
                    var sql = SqlQuery<T>.Builder(dbs);
                    var flag = db.Execute(sql.DeleteSql, lt, transaction, commandTimeout, null, watch);
                    if (watch.IsRunning)
                    {
                        watch.Stop();
                    }
                    return flag == lt.Count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 修改数据（根据某几个字段进行修改）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbs"></param>
        /// <param name="t">如果sql为null，则根据t的主键进行修改</param>
        /// <param name="sql">按条件修改</param>
        /// <returns></returns>
        public static bool Update<T>(this DbBase dbs, T t,  IDbTransaction transaction = null, SqlQuery sql = null, int? commandTimeout = null) where T : class
        {   
            Stopwatch watch = new Stopwatch();
            try
            {
                int updateResult = 0;
                var db = dbs.DbConnecttion;
                    watch.Start();
                    if (sql == null)
                    {
                        sql = SqlQuery<T>.Builder(dbs);
                        //获取查询内容
                        if (t is ModelBase)
                        {
                            ModelBase store = t as ModelBase;
                            sql = sql.AppendParam<T>(t).SetExcProperties<T>(store.Fields);
                        }
                        else
                        {
                            sql = sql.AppendParam<T>(t);
                        }
                    }
                    updateResult = db.Execute(sql.UpdateSql, sql.Param, transaction, commandTimeout, null, watch);


                    if (watch.IsRunning)
                    {
                        watch.Stop();
                    }
                    return updateResult > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region 查询

        /// <summary>
        /// 获取默认一条数据，没有则为NULL
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbs"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static T SingleOrDefault<T>(this DbBase dbs, SqlQuery sql, IDbTransaction transaction = null) where T : class
        {
            try
            {
                Stopwatch watch = new Stopwatch();
                var db = dbs.DbConnecttion;
                    watch.Start();
                    if (sql == null)
                    {
                        sql = SqlQuery<T>.Builder(dbs);
                    }
                    sql = sql.Top(1);
                    var result = db.Query<T>(sql.QuerySql, sql.Param, transaction, false, null, null, watch);

                    //var result = db.Query<T>(sql.QuerySql, sql.Param, null, false, null, null, watch);
                    if (watch.IsRunning)
                    {
                        watch.Stop();
                    }
                    if (result != null)
                    {
                        return result.FirstOrDefault();
                    }
                    else
                    {
                        return null;
                    }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbs"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="dataCount"></param>
        /// <param name="sqlQuery"></param>
        /// <returns></returns>
        public static List<T> PageData<T>(this DbBase dbs, int pageIndex, int pageSize,out long dataCount,SqlQuery sqlQuery = null) where T : class
        {
            Stopwatch watch = new Stopwatch();
            try
            {
                    var db = dbs.DbConnecttion;
                    var result = new List<T>();
                    dataCount = 0;
                    watch.Start();
                    if (sqlQuery == null)
                    {
                        sqlQuery = SqlQuery<T>.Builder(dbs);
                    }
                    sqlQuery = sqlQuery.Page(pageIndex, pageSize);
                    var para = sqlQuery.Param;
                    var cr = db.Query(sqlQuery.CountSql, para, null, false, null, null, watch).SingleOrDefault();
                    dataCount = (long)cr.DataCount;
                    if (watch.IsRunning)
                    {
                        watch.Stop();
                    }
                    else
                    {
                        watch.Start();
                    }
                    //pageCount = cr.DataCount % pageSize == 0 ? (int)cr.DataCount / pageSize : (int)cr.DataCount / pageSize + 1;//计算页数
                    result = db.Query<T>(sqlQuery.PageSql, para, null, false, null, null, watch).ToList();
                    if (watch.IsRunning)
                    {
                        watch.Stop();
                    }
                    return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbs"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="dataCount"></param>
        /// <param name="sqlQuery"></param>
        /// <returns></returns>
        public static List<T> PageDataByRowNumber<T>(this DbBase dbs, int startRow, int endRow, out long dataCount, SqlQuery sqlQuery = null) where T : class
        {
            Stopwatch watch = new Stopwatch();
            try
            {
                    var db = dbs.DbConnecttion;
                    var result = new List<T>();
                    dataCount = 0;
                    watch.Start();
                    if (sqlQuery == null)
                    {
                        sqlQuery = SqlQuery<T>.Builder(dbs);
                    }
                    sqlQuery = sqlQuery.PageRowNumber(startRow, endRow);
                    var para = sqlQuery.Param;
                    var cr = db.Query(sqlQuery.CountSql, para, null, false, null, null, watch).SingleOrDefault();
                    dataCount = (long)cr.DataCount;
                    if (watch.IsRunning)
                    {
                        watch.Stop();
                    }
                    else
                    {
                        watch.Start();
                    }
                    //pageCount = cr.DataCount % pageSize == 0 ? (int)cr.DataCount / pageSize : (int)cr.DataCount / pageSize + 1;//计算页数
                    result = db.Query<T>(sqlQuery.PageSql, para, null, false, null, null, watch).ToList();
                    if (watch.IsRunning)
                    {
                        watch.Stop();
                    }
                    return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbs"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static List<T> QueryData<T>(this DbBase dbs, SqlQuery sql = null) where T : class
        {
            Stopwatch watch = new Stopwatch();
            try
            {
                   var db = dbs.DbConnecttion;
                    watch.Start();
                    if (sql == null)
                    {
                        sql = SqlQuery<T>.Builder(dbs);
                    }
                    var result = db.Query<T>(sql.QuerySql, sql.Param, null, false, null, null, watch);
                    if (watch.IsRunning)
                    {
                        watch.Stop();
                    }
                    return result.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
     

        /// <summary>
        /// 数据数量
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbs"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static long Count<T>(this DbBase dbs, SqlQuery sql = null) where T : class
        {
            Stopwatch watch = new Stopwatch();
            try
            {
                var db = dbs.DbConnecttion;
                    watch.Start();
                    if (sql == null)
                    {
                        sql = SqlQuery<T>.Builder(dbs);
                    }
                    var cr = db.Query(sql.CountSql, sql.Param, null, false, null, null, watch).SingleOrDefault();
                    if (watch.IsRunning)
                    {
                        watch.Stop();
                    }
                    return (long)cr.DataCount;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region 存储过程

        /// <summary>
        ///  执行存储过程
        /// 具体二次分装需要参考：http://blog.csdn.net/wang463584281/article/details/21244933
        /// </summary>
        /// <param name="dbs"></param>
        /// <param name="sotreName"></param>
        /// <param name="inParems"></param>
        /// <param name="outResult"></param>
        /// <returns></returns>
        public static int StoreProcedure(this DbBase dbs,string sotreName, DynamicParameters inParems, string outResult)
        {
            try
            {
                var db = dbs.DbConnecttion;
                db.Execute(sotreName, inParems, null, null, CommandType.StoredProcedure);
                return inParems.Get<int>(outResult);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        /// <summary>
        /// 直接执行SQL语句返回TRUE，FALSE
        /// </summary>
        /// <param name="dbs"></param>
        /// <param name="sql"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public static bool ExecuteSql(this DbBase dbs, string sql,IDbTransaction transaction = null)
        {
            try
            {
                int result = 0;
                var db = dbs.DbConnecttion;
                result = db.Execute(sql, null, transaction, null, null);
                return result > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///  执行查询SQL语句返回单个实体类
        /// </summary>
        /// <param name="dbs"></param>
        /// <param name="sql"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public static T SingleOrDefault<T>(this DbBase dbs, string sql, object param = null, IDbTransaction transaction = null)
        {
            try
            {
                var db = dbs.DbConnecttion;
                var result = db.Query<T>(sql, param, transaction, false, null, null);
                return result.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<T> QueryData<T>(this DbBase dbs, string sql, object param = null, IDbTransaction transaction = null)
        {
            try
            {
                var db = dbs.DbConnecttion;
                    var result = db.Query<T>(sql, param, transaction, false, null, null);
                    return result.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
