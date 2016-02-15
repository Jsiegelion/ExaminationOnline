using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mammothcode.Core.PayMent.Alipay.model
{
    /// <summary>
    /// 支付宝支付后返回的对象
    /// </summary>
    public class AliPayReturnModel
    {
        private System.Web.HttpRequest _request;

        public AliPayReturnModel(System.Web.HttpRequest Request)
        {
            this._request = Request;
        }

        /// <summary>
        /// 商户订单号
        /// </summary>
        public string Out_trade_no
        {
            get
            {
                return _request.QueryString["out_trade_no"];
            }

        }

        /// <summary>
        ///  支付宝交易号
        /// </summary>
        public string Trade_no
        {
            get
            {
                return _request.QueryString["trade_no"]; 
            }
         
        }

        /// <summary>
        /// 支付完成后的内容
        /// </summary>
        public string Body
        {
            get
            {
                return _request.QueryString["body"]; 
            }
        }

        /// <summary>
        /// 交易状态
        /// </summary>
        public string Trade_status
        {
            get 
            {
                return _request.QueryString["trade_status"]; 
            }
          
        }

        /// <summary>
        /// 买家支付宝账号
        /// </summary>
        public string Buyer_email
        {
            get
            {
                return _request.QueryString["buyer_email"]; 
            }
        
        }

    }
}
