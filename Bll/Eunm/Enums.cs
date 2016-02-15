using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mammothcode.BLL.Eunm
{
    public class Enums
    {
        /// <summary>
        /// 枚举类型
        /// 甘春雨
        /// 2015年12月2日15:01:43
        /// </summary>
        public enum classifyType
        {
            栏目组 = 1 << 0,
            单页 = 1 << 1,
            图片列表 = 1 << 2,
            文章列表 = 1 << 3,
            办公资料 = 1 << 4,
            多媒体 = 1 << 5
        }
        /// <summary>
        /// 排序类型
        /// 甘春雨
        /// 2015年12月2日15:01:36
        /// </summary>
        public enum sortType
        {
            降序 = 1 << 0,
            升序 = 1 << 1
        }

        #region 文章发布状态
        /// <summary>
        /// 功能描述：文章发布状态
        /// 创建人：金协民
        /// 创建时间：2015年12月2日22:16:02
        /// </summary>
        public enum ArticleState
        {
            已发布 = 1,
            未发布 = 0
        }
        #endregion

        #region 抓取任务状态
        /// <summary>
        /// 功能描述：抓取任务状态
        /// 创建人：金协民
        /// 创建时间：2015年12月8日15:11:34
        /// </summary>
        public enum GrabBaiduState
        {
            队列中 = 0,
            正在抓取 = 1,
            抓取完成 = 2,
        }
        #endregion

        #region 自媒体类型
        /// <summary>
        /// 功能描述：自媒体类型
        /// 创建人：张活生
        /// 创建时间：2015年12月16日15:37:05
        /// </summary>
        public enum selfmedialearn_type
        {
            屌丝日志 = 0,
            库存方案 = 1,
        }
        #endregion

        #region 自媒体等级
        /// <summary>
        /// 功能描述：自媒体等级
        /// 创建人：张活生
        /// 创建时间：2015年12月18日15:19:34
        /// </summary>
        public enum selfmedialearn_grade
        {
            初级=0,
            中级=1,
            高级=2,
        }

        #endregion

        #region 单页开启模式
        /// <summary>
        /// 功能描述：单页开启模式
        /// 创建人：甘春雨
        /// 创建时间：2015年12月9日16:19:16
        /// </summary>
        public enum PageMode
        {
            非公共模式 = 0,
            公共模式 = 1,
        }
        #endregion

        #region 微信用户类型

        /// <summary>
        /// 枚举：微信用户类型
        /// 创建人：林以恒
        /// 2016-1-14
        /// </summary>
        public enum WcUserEnum
        {
            修车厂用户 = 0,
            普通用户 = 1,
            未完善用户 = 2
        }

        #endregion

        #region 修车点补充状态

        /// <summary>
        /// 修车点补充状态
        /// </summary>
        public enum PointExamineState
        {
            补充开放 = 0,
            补充关闭 = 1
        }

        #endregion

        #region 城市开放状态

        public enum CityOpenState
        {
            关闭 = 0,
            开放 = 1
        }

        #endregion
    }
}
