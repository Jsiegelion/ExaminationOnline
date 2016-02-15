using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mammothcode.Core.PayMent.WeChatpay
{
    public class JSApi_model
    {
        /// <summary>
        /// 公众号名称，由商户传入     
        /// </summary>
        public string appId;

        /// <summary>
        /// 随机串
        /// </summary>
        public string nonceStr;

        /// <summary>
        /// 统一下单接口返回的prepay_id参数值，提交格式如：prepay_id=***
        /// </summary>
        public string package;

        /// <summary>
        /// 微信签名 
        /// </summary>
        public string paySign;

        /// <summary>
        ///  //微信签名方式
        /// </summary>
        public string signType;

        /// <summary>
        /// 时间戳，自1970年以来的秒数  
        /// </summary>
        public string timeStamp;
    }
}
