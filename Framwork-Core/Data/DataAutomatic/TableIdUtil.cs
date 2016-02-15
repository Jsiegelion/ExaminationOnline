using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mammothcode.Core.Data.DataAutomatic
{
    /// <summary>
    /// 数据表的ID
    /// </summary>
    public class TableIdUtil
    {

        /// <summary>
        /// 自动生成表的GUID主键ID
        /// 创建人：孙佳杰  创建时间:2015.8.3
        /// </summary>
        public static string GetGuidId()
        {
            string code = DateTime.Now.ToString("yyyyMMdd") + Guid.NewGuid().ToString().Substring(0, 8);
            return code;
        }
    }
}
