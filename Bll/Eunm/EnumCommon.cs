using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mammothcode.BLL.Eunm
{
    /// <summary>
    /// 功能描述：枚举公共类
    /// 创建人：甘春雨
    /// 创建时间：2015年10月29日15:56:22
    /// </summary>
    public class EnumsCommon
    {
        #region 手机消息发送状态类型
        /// <summary>
        /// 功能描述：手机消息发送状态类型
        /// 创建人：甘春雨
        /// 创建时间：2015年10月29日15:56:22
        /// </summary>
        public enum MobileverifySendEnum
        {
            /// <summary>
            /// 发送
            /// </summary>
            sending = 0,
            /// <summary>
            /// 认证成功
            /// </summary>
            success = 1,
            /// <summary>
            /// 认证失败
            /// </summary>
            fail = 2
        }
        #endregion

        #region 手机验证码发送状态
        /// <summary>
        /// 功能描述：手机验证码发送状态
        /// 创建人：甘春雨
        /// 创建时间：2015年10月29日15:56:22
        /// </summary>
        [Flags]
        public enum PhoneVerifyCodeEnum
        {
            验证码错误 = 0,
            时间超时 = -2,
            验证成功 = 1
        }
        #endregion

        #region 性别
        /// <summary>
        /// 功能描述：性别
        /// 创建人：甘春雨
        /// 创建时间：2015年10月29日15:56:22
        /// </summary>
        public enum GenderEnum
        {
            /// <summary>
            /// 男
            /// </summary>
            男 = 1,
            /// <summary>
            /// 女
            /// </summary>
            女 = 0
        }
        #endregion

        #region 短信认证类别
        /// <summary>
        /// 功能描述：短信认证类别
        /// 创建人：甘春雨
        /// 创建时间：2015年10月29日15:56:22
        /// </summary>
        public enum UME_TypeEnum
        {
            /// <summary>
            /// 注册
            /// </summary>
            regist = 0,
            /// <summary>
            /// 找回密码
            /// </summary>
            findPwd = 1
        }
        #endregion

        #region 共调用的功能枚举类型
        /// <summary>
        /// 功能描述：共调用的功能枚举类型
        /// 创建人：甘春雨
        /// 创建时间：2015年10月29日15:56:22
        /// </summary>
        public enum Common
        {
        }
        #endregion

        #region 是否
        /// <summary>
        /// 功能描述：是否
        /// 创建人：甘春雨
        /// 创建时间：2015年11月2日14:35:13
        /// </summary>
        public enum TrueFalse
        {
            否 = 0,
            是 = 1
        }
        #endregion

        #region 公共审核枚举
        /// <summary>
        /// 功能描述：公共审核枚举
        /// 创建人：甘春雨
        /// 创建时间：2015年11月4日15:26:25
        /// </summary>
        public enum Examine
        {
            无审核 = 0,
            待审核 = 1 << 0,
            审核中 = 1 << 1,
            审核通过 = 1 << 2,
            审核失败 = 1 << 3,
        }
        #endregion
    }
}
