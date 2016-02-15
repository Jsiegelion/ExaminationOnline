using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Runtime.Serialization;
using System.IO;
using System.Text;
using System.Net;
using System.Web.Security;
using LitJson;
using WxPayAPI;
using Mammothcode.Core.Data.DataConvert;
using Mammothcode.Core.Data.DataSecurity;
using Mammothcode.Core.SystemTool;

namespace Mammothcode.Core.PayMent.WeChatpay
{

    /// <summary>
    ///  支付接口类
    ///  创建  毛枫   2015-8-1
    /// </summary>
    public static  class WeChatpay
    {
       /// <summary>
       /// 进行微信授权验证，获取Openid
       /// </summary>
       /// <param name="UrlCode">URL参数</param>
       /// <param name="UrlHost">URL服务地址</param>
       /// <param name="UrlPath">URL路径</param>
       /// <returns></returns>
        public static  string GetOpenidAndAccessToken(string UrlCode,string Url)
        {
            string str="";
            JsApiPay jsApiPay = new JsApiPay();
            try
            {
                //调用【网页授权获取用户信息】接口获取用户的openid和access_token
                jsApiPay.GetOpenidAndAccessToken(UrlCode,Url);
             
                if (string.IsNullOrEmpty(UrlCode))
                {  
                    //如果UrlCode为空，则获取重定向URL 
                    str = jsApiPay.url1;
                }
                else
                { 
                    //如果UrlCode不为空，则解析Code，获取Openid
                    //用户统一下单接口
                    str = jsApiPay.openid;     
                    str +=","+ jsApiPay.access_token;
                }
          
            }
            catch (Exception ex)
            {

            }
            return str;
        }

        
        public static void weixin(string Url)
        {
            JsApiPay jsApiPay = new JsApiPay();
            try
            {
                jsApiPay.weixin(Url);
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 调用JSAPI
        /// </summary>
        /// <param name="openid">用户对于公众号唯一标识</param>
        /// <param name="total_fee"></param>
        /// <returns></returns>
        public static JSApi_model JsPay(string openid,string total_fee,string oName)
        {
            JSApi_model model = new JSApi_model();
            string wxJsApiParam = "";
            double money = double.Parse(total_fee);
            int truemoney = (int)(money * 100);
            //检测是否给当前页面传递了相关参数
            if (string.IsNullOrEmpty(openid) || string.IsNullOrEmpty(total_fee))
            {
                return null;
            }
            //若传递了相关参数，则调统一下单接口，获得后续相关接口的入口参数
            JsApiPay jsApiPay = new JsApiPay();
            //JSAPI支付预处理
            try
            {
                //进行统一支付认证
                WxPayData unifiedOrderResult = jsApiPay.GetUnifiedOrderResult(oName,openid, truemoney);
                //获取H5调起JS API参数       
                wxJsApiParam = jsApiPay.GetJsApiParameters(); 
                model = wxJsApiParam.toJsonObject<JSApi_model>();
            }
            catch (Exception ex)
            {
                //Response.Write("<span style='color:#FF0000;font-size:20px'>" + ex + "下单失败，请返回重试" + "</span>");
                //submit.Visible = false;
            }
            return model;
        }


        /***
      * 订单查询完整业务流程逻辑
      * @param transaction_id 微信订单号（优先使用）
      * @param out_trade_no 商户订单号
      * @return 订单查询结果（xml格式）
      */
        public static string Run(string transaction_id, string out_trade_no)
        {
            Log.Info("OrderQuery", "OrderQuery is processing...");

            WxPayData data = new WxPayData();
            if (!string.IsNullOrEmpty(transaction_id))//如果微信订单号存在，则以微信订单号为准
            {
                data.SetValue("transaction_id", transaction_id);
            }
            else//微信订单号不存在，才根据商户订单号去查单
            {
                data.SetValue("out_trade_no", out_trade_no);
            }

            WxPayData result = WxPayApi.OrderQuery(data);//提交订单查询请求给API，接收返回数据

            Log.Info("OrderQuery", "OrderQuery process complete, result : " + result.ToXml());
            return result.ToPrintStr();
        }

    }
}
