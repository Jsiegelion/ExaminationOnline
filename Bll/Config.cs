using System.Collections.Generic;

namespace Mammothcode.BLL
{
    public class Config
    {
        /// <summary>
        /// 连接字符串名称(SqlServer,strMySql)
        /// </summary>
        public const string connectionStringsName = "strMySql";

        /// <summary>
        /// 星星与评分
        /// </summary>
        public readonly Dictionary<int, int> StarIntegral = new Dictionary<int, int>
        {
            {1,-1},{2,-1},{3, 0},{4,1},{5,2}
        };

        /// <summary>
        /// 用户对同一个修车点评价间隔的时间
        /// </summary>
        public const int DifferTime = 15;

        /// <summary>
        /// 评论提醒数
        /// </summary>
        public const int RemindWeTimes = 10;
    }
}
