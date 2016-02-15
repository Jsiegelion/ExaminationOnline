using System;
using System.Data;
using Mammothcode.Public.Data;

namespace Mammothcode.BLL.Bll_Auto
{
    public class BaseTran
    {
        public static bool tran_common(Func<DbBase, IDbTransaction, bool> func)
        {
            bool isSuccess = true;
            //打开数据库
            using (DbBase DbContext = new DbBase(Config.connectionStringsName))
            {
                using (IDbTransaction tran = DbContext.DbTransaction)
                {
                    try
                    {
                        isSuccess = func(DbContext, tran);
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
