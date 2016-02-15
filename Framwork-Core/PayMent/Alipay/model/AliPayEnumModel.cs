using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mammothcode.Core.PayMent.Alipay.model
{

        /// <summary>
        /// 支付之前验证的状态码
        /// 创建人:孙佳杰
        /// </summary>
        public enum AliPayBeforeVerifyStatus
        {
            /// <summary>
            /// 验证成功可以进行支付
            /// </summary>
            PAY_VERIFY_SUCCESS,
            /// <summary>
            /// 订单不存在
            /// </summary>
            PAY_VERIFY_NOT_EXIST,
            /// <summary>
            /// 该订单非等待付款状态，无法付款
            /// </summary>
            PAY_VERIFY_NOT_NEED_PAY,
           
        }


        /// <summary>
        /// 支付之后结果的状态码
        /// 创建人:孙佳杰
        /// </summary>
        public enum AliPayAfterResultStatus
        { 
            /// <summary>
            /// 错误信息：验证失败
            /// </summary>
            验证失败,
            /// <summary>
            /// 错误信息：无返回参数
            /// </summary>
            无返回参数 ,
            /// <summary>
            /// 错误信息： 支付宝返回数据错误
            /// </summary>
            支付宝返回数据错误,
            /// <summary>
            /// 信息：返回成功
            /// </summary>
            返回成功
            
        }

}
