using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mammothcode.Core.PayMent.Alipay
{
    public class AlipayConfig
    {
        //TODO 孙佳杰-支付宝支付接口配置
        /// <summary>
        /// 合作身份者ID，以2088开头由16位纯数字组成的字符串
        /// </summary>
        public static string PARTNER
        {
            get
            {
                return "2088912773363148";
            }
        }

        /// <summary>
        ///  交易安全检验码，由数字和字母组成的32位字符串
        /// </summary>
        protected static string KEY
        {
            get
            {
                return "snmtv8e2festcttceyy8zzqyz4lygeuy";
            }
        }

        /// <summary>
        /// 字符编码格式 目前支持 gbk 或 utf-8
        /// </summary>
        public static string INPUT_CHARSET
        {
            get
            {
                return "utf-8";
            }
        
        }

        /// <summary>
        /// 签名方式，选择项：RSA、DSA、MD5
        /// </summary>
        protected static string SIGN_TYPE
        {
            get
            {
               return  "MD5";
            }
        }

        /// <summary>
        /// 异步通知url
        /// </summary>
        public static string NOTIFY_URL
        {
            get
            {
                return "http://www.mammothcode.com/alipay.demo/notify_url.aspx";
            }
        }

        /// <summary>
        /// 同步返回url
        /// </summary>
        public static string RETURN_URL
        {
            get
            {
                return "http://www.mammothcode.com/alipay.demo/return_url.aspx";
            }
        }

        /// <summary>
        /// 卖家支付宝账号
        /// </summary>
        public static string SELLER_EMAIL
        {
            get 
            {
                return "sunjiajie@mammothcode.com";
            }
        }
    }
}
