using System;
using System.Data;
using Mammothcode.Public.Data;

namespace Mammothcode.Bll.Bll_Auto
{
    public class BaseTran
    {
        /// <summary>
        /// 事务壳
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public static bool TranCommon(Func<DbBase, IDbTransaction, bool> func)
        {
            bool isSuccess = true;
            //打开数据库
            using (DbBase dbContext = new DbBase(BllConfig.ConnectionStringsName))
            {
                using (IDbTransaction tran = dbContext.DbTransaction)
                {
                    try
                    {
                        isSuccess = func(dbContext, tran);
                    }
                    catch (Exception e)
                    {
                        isSuccess = false;
                        throw e;
                    }
                    finally
                    {
                        if (isSuccess)
                        {
                            tran.Commit();
                        }
                        else
                        {
                            tran.Rollback();
                        }
                    }
                }
                return isSuccess;
            }
        }
    }
}
