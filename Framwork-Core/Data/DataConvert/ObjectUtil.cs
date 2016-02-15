using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Mammothcode.Core.Data.DataConvert
{
    /// <summary>
    /// 实体转化基础类
    /// 作者：孙佳杰
    /// 修改时间：2015.1.14
    /// 功能：ToLong（将实体转化为long型）
    ///       ToDouble（将实体转化为Doble型）
    /// </summary>
    public static class ObjectUtil
    {

        /// <summary>
        /// 将实体转化为long型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inputValue"></param>
        /// <returns></returns>
        public static long ToLong(this object value)
        {
            return Convert.ToInt64(value);
        }

        /// <summary>
        /// 将实体转化为Int型
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int ToInt(this object value)
        {
            return Convert.ToInt32(value);
        }

        /// <summary>
        /// 将实体转化为Doble型
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double ToDouble(this object value)
        {
            return Convert.ToDouble(value);
        }

  
    }
}
