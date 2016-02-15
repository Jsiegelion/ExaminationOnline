using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

namespace Mammothcode.Core.Data.DataConvert
{
    /// <summary>
    /// 枚举类型工具类
    /// </summary>
    public class EnumUtil
    {
        /// <summary>
        /// 将枚举类转化为list集合
        /// </summary>
        /// <param name="enumType">Type</param>
        /// <returns>list集合</returns>
        public static IList EnumToList(Type enumType)
        {
            ArrayList list = new ArrayList();

            foreach (int i in Enum.GetValues(enumType))
            {
                ListItem listitem = new ListItem(Enum.GetName(enumType, i), i.ToString());
                list.Add(listitem);
            }
            return list;
        }
    }
}
