using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mammothcode.Core.Thread
{
    /// <summary>
    ///  线程公共类
    ///  创建人：孙佳杰  创建时间：2015.3.22
    /// </summary>
    public class ThreadUtil
    {
        /// <summary>
        ///  并行执行自定义方法
        ///  创建人:孙佳杰  创建时间:2015.3.22
        /// </summary>
        /// <param name="objList">要执行的集合</param>
        /// <param name="maxDegree">最大并行量</param>
        /// <param name="methodMain">执行主函数</param>
        /// <returns>执行结果</returns>
        public bool ExecuteParaller(IList<object> objList, int maxDegree, Action<object> methodMain)
        {
            bool success = false;
            try
            {
                Parallel.ForEach(objList,
                                 new ParallelOptions { MaxDegreeOfParallelism = maxDegree },
                                 methodMain);
                success = true;
            }
            catch (AggregateException ae)
            {
                foreach (var ex in ae.InnerExceptions)
                {
                    //记录日志
                }
            }
            return success;
        }
    }
}
