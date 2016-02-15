using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mammothcode.Core.Data.DataAnaly
{
    /// <summary>
    /// 数据判断公共类
    /// </summary>
    public static class DataJudgeUtil
    {

        /// <summary>
        /// 功能描述：判断字符串是否为整数
        /// 创建人：孙佳杰 
        /// 时间：2015.1.27
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNum(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return false;
            int i = 0;
            return int.TryParse(s: str, result: out i);
        }
        /// <summary>
        /// 功能描述：扩展方法-将字符串转化为枚举类型 todoin添加到文档
        /// 创建人：甘春雨
        /// 时间2015年10月26日21:59:41
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="input">当前枚举对象</param>
        /// <param name="EnumValue">枚举int值</param>
        /// <returns>成功-指定到枚举类型，失败是对应类型的默认值</returns>
        public static T GetEnumType<T>(this Enum input, string EnumValue) where T : struct
        {
            try
            {
                T type = (T)Enum.Parse(typeof(T), EnumValue);
                return type;
            }
            catch (Exception)
            {
                return  default(T);;
            }
        }
    }
}
