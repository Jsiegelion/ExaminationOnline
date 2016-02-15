using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mammothcode.Model.Model_Custom
{
    /// <summary>
    /// 功能描述：后台管理员模型
    /// </summary>
    public class AdminUserModel
    {
        /// <summary>
        /// 管理员ID
        /// </summary>
        public int A_ID { get; set; }
        /// <summary>
        /// 管理员CODE
        /// </summary>
        public string A_CODE { get; set; }
        /// <summary>
        /// 管理员姓名
        /// </summary>
        public string A_NAME { get; set; }
        /// <summary>
        /// 管理员真实姓名
        /// </summary>
        public string A_CHINESE_NAME { get; set; }
        /// <summary>
        /// 管理员手机号
        /// </summary>
        public string A_PHONE { get; set; }
    }
}
