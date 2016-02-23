using System;
using System.Collections.Generic;
using System.Data;
using Mammothcode.Public.Data;

namespace Mammothcode.Bll
{
    public class BLLBase<T>
        where T : class,new()
    {
        /// <summary>
        /// connectionStrings的连接字符串名称
        /// </summary>
        public static string connectionName = BllConfig.ConnectionStringsName;

        /// <summary>
        ///   数据库基础类
        /// </summary>
        //protected   DbBase  DbContext = new DbBase(connectionName);

        #region 增加、修改、删除

        /// <summary>
        /// 新增单个实体
        /// </summary>
        /// <param name="entity"></param>
        public virtual bool Add(T entity, IDbTransaction transaction = null)
        {
            bool flag = false;
            using (DbBase DbContext = new DbBase(connectionName))
            {
                flag=DbContext.Insert(entity, transaction);
            }
            return flag;
        }

        /// <summary>
        /// 批量新增实体
        /// </summary>
        /// <param name="entityList"></param>
        public virtual void BatchAdd(List<T> entityList)
        {
            using (DbBase DbContext = new DbBase(connectionName))
            {
                //自动新增事务处理
                using (var tran = DbContext.GetDbTransaction())
                {
                    DbContext.InsertBatch(entityList, tran);
                    tran.Commit();
                }
                //DbContext.CloseConnection();
            }
        }

        /// <summary>
        ///  更新实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual bool Update(T entity, IDbTransaction transaction = null)
        {
            using (DbBase DbContext = new DbBase(connectionName))
            {
                bool returnBool = DbContext.Update(entity, transaction);
                DbContext.CloseConnection();
                return returnBool;
            }
        }

        public virtual void BatchUpdate(List<T> entityList)
        {
            using (DbBase DbContext = new DbBase(connectionName))
            {
                bool success = true;

                //自动新增事务处理
                using (var tran = DbContext.GetDbTransaction())
                {
                    try
                    {
                        foreach (var t in entityList)
                        {
                            DbContext.Update(t, tran);
                        }
                    }
                    catch (Exception ex)
                    {
                        success = false;
                        throw ex;
                    }
                    finally
                    {
                        if (success)
                        {
                            tran.Commit();
                        }
                        else
                        {
                            tran.Rollback();
                        }
                    }
                }

                DbContext.CloseConnection();
            }
        }

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="expr"></param>
        /// <param name="operation"></param>
        /// <param name="value"></param>
        public virtual bool Delete(DapperExQuery<T> query, IDbTransaction transaction = null)
        {
            bool flag = false;
            using (DbBase DbContext = new DbBase(connectionName))
            {
               flag= DbContext.Delete<T>(query.GetSqlQuery(DbContext), transaction);
                DbContext.CloseConnection();
            }
            return flag;
        }

        /// <summary>
        /// 批量删除实体
        /// </summary>
        /// <param name="entityList"></param>
        public virtual void BatchDelete(List<T> entityList)
        {
            using (DbBase DbContext = new DbBase(connectionName))
            {
                //自动新增事务处理
                using (var tran = DbContext.GetDbTransaction())
                {
                    DbContext.DeleteBatch<T>(entityList, tran);
                    tran.Commit();
                }
                DbContext.CloseConnection();
            }
        }

        #endregion

    #region  查询

        public virtual bool IsExist(DapperExQuery<T> query)
        {
            bool returnBool = false;
            using (DbBase DbContext = new DbBase(connectionName))
            {

                if (DbContext.SingleOrDefault<T>(query.GetSqlQuery(DbContext)) != null)
                {
                    returnBool = true;
                }
            }
            return returnBool;
        }

        /// <summary>
        /// 获得单个实体
        /// </summary>
        /// <param name="expr">查询对象</param>
        /// <param name="operation"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual T GetEntity(DapperExQuery<T> query)
        {
            using (DbBase DbContext = new DbBase(connectionName))
            {
                return DbContext.SingleOrDefault<T>(query.GetSqlQuery(DbContext));
            }
        }

        /// <summary>
        /// 获得所有数据集合
        /// </summary>
        /// <param name="queryList"></param>
        /// <returns></returns>
        public virtual List<T> GetAllList(string orderString = "")
        {
            using (DbBase DbContext = new DbBase(connectionName))
            {
                if (!string.IsNullOrEmpty(orderString))
                {
                    DapperExQuery<T> query = new DapperExQuery<T>();
                    query.SetOrder(orderString);
                    return DbContext.QueryData<T>(query.GetSqlQuery(DbContext));
                }
                else
                {
                    return DbContext.QueryData<T>();
                }
            }

        }

        /// <summary>
        ///  获取指定条件的所有数据集合
        /// </summary>
        /// <param name="queryList"></param>
        /// <returns></returns>
        public virtual List<T> GetAllList(DapperExQuery<T> query, string orderString = "")
        {
            using (DbBase DbContext = new DbBase(connectionName))
            {
                if (!string.IsNullOrEmpty(orderString))
                {
                    //设置Order条件
                    query.SetOrder(orderString);
                }
                return DbContext.QueryData<T>(query.GetSqlQuery(DbContext));
            }
        }

        /// <summary>
        /// 分页_获取指定条件的数据集合（第几页，每页个数）
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="pageIndex">开始位置（默认从1开始）</param>
        /// <param name="pageSize">页面显示数量</param>
        /// <param name="pageCount">总页数</param>
        /// <param name="dataCount">总查询数</param>
        /// <returns></returns>
        public virtual List<T> GetListByPage(DapperExQuery<T> query, string orderString, int pageIndex, int pageSize, out long dataCount)
        {
            using (DbBase DbContext = new DbBase(connectionName))
            {
                //设置Order条件
                query.SetOrder(orderString);
                return DbContext.PageData<T>(pageIndex, pageSize, out dataCount, query.GetSqlQuery(DbContext));
            }
        }

        /// <summary>
        /// 分页_获取指定条件的数据集合（开始位置，结束位置）
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="pageIndex">开始位置（默认从1开始）</param>
        /// <param name="pageSize">当前页数</param>
        /// <param name="pageCount">总页数</param>
        /// <param name="dataCount">总查询数</param>
        /// <returns></returns>
        public virtual List<T> GetListByByRowNumber(DapperExQuery<T> query, string orderString, int startRowNumber, int endRowNumber, out long dataCount)
        {
            using (DbBase DbContext = new DbBase(connectionName))
            {
                //设置Order条件
                query.SetOrder(orderString);
                return DbContext.PageDataByRowNumber<T>(startRowNumber, endRowNumber, out dataCount, query.GetSqlQuery(DbContext));
            }
        }


        /// <summary>
        /// 统计指定条件的数据量
        /// </summary>
        /// <param name="queryList"></param>
        /// <returns></returns>
        public virtual long GetCount(DapperExQuery<T> query)
        {
            using (DbBase DbContext = new DbBase(connectionName))
            {
                return DbContext.Count<T>(query.GetSqlQuery(DbContext));
            }
        }

        /// <summary>
        /// 获得子查询条件
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public virtual InWhereQuery GetInWhereSql(string selectField, DapperExQuery<T> query)
        {
            using (DbBase DbContext = new DbBase(connectionName))
            {
                SqlQuery sql = query.GetSqlQuery(DbContext);
                return query.TranInWhereQuery(selectField, sql);
            }
        }


        #endregion

        #region 自定义sql
        /// <summary>
        /// 功能描述：自定义Sql查询
        /// 创建人：甘春雨
        /// 创建时间：2016年1月7日20:18:09
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static List<T> SqlQueryList(string sql)
        {
            List<T> data = default(List<T>);
            try
            {
                using (DbBase DbContent = new DbBase(connectionName))
                {
                    data = DapperEx.QueryData<T>(DbContent, sql);
                }
            }
            catch (Exception)
            {
            }
            return data;
        }
        #endregion
    }
}
