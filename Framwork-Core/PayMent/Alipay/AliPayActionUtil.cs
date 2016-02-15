using Com.Alipay;
using Mammothcode.Core.Data.DataConvert;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mammothcode.Core.PayMent.Alipay.model;
using System.Collections.Specialized;
namespace Mammothcode.Core.PayMent.Alipay
{
    /// <summary>
    /// 支付宝支付通用方法公共类
    /// </summary>
    public class AliPayActionUtil
    {

        #region 支付宝即时支付
        /// <summary>
        /// 支付宝即时支付
        /// 创建人：甘春雨 时间：2015年10月23日12:50:39
        /// </summary>
        /// <param name="verifyMethod">支付前进行验证的函数</param>
        /// <param name="alipayModel">支付宝支付实体模型</param>
        /// <param name="Html">返回的HTML</param>
        /// <returns></returns>
        public static bool DirectPay(Func<string, AliPayBeforeVerifyStatus> verifyMethod, AliPayModel alipayModel, ref string Html)
        {
            #region 支付前对订单进行验证

            //对订单进行验证
            bool IsCanContinune = false;
            var verifyResult = verifyMethod(alipayModel.Out_trade_no);
            switch (verifyResult)
            {
                case AliPayBeforeVerifyStatus.PAY_VERIFY_SUCCESS: IsCanContinune = true; break;
                case AliPayBeforeVerifyStatus.PAY_VERIFY_NOT_EXIST: Html = "订单不存在！"; break;
                case AliPayBeforeVerifyStatus.PAY_VERIFY_NOT_NEED_PAY: Html = "该订单非等待付款状态，无法付款！"; break;

            }
            #endregion

            if (IsCanContinune)
            {

                #region 请求参数
                if (alipayModel.Defaultbank != "")
                {
                    //默认支付方式
                    alipayModel.Payment_type= "bankPay";
                }
                int minutes = 0;
                if (alipayModel.Exceed_minutes > 120)
                {
                    //订单超时后返回HTML
                    Html = alipayModel.Exceed_returnHtml;
                    return false;
                }

                #endregion

                Html = DirectPayCreateHtml(alipayModel);//将请求参数打包进行支付请求

                return true;
            }
            else
            {
                return false;
            }
        } 
        #endregion

        #region 支付宝-手机网站支付
        /// <summary>
        /// 支付宝-手机网站支付
        /// 创建 甘春雨
        /// 2015年10月23日12:50:58
        /// </summary>
        /// <param name="verifyMethod">支付前进行验证的函数</param>
        /// <param name="alipayModel">支付宝支付实体模型</param>
        /// <param name="Html">返回的HTML</param>
        /// <returns></returns>
        public static bool WapDirectPay(Func<string, AliPayBeforeVerifyStatus> verifyMethod, AliPayModel alipayModel, ref string Html)
        {
            #region 支付前对订单进行验证

            //对订单进行验证
            bool IsCanContinune = false;
            var verifyResult = verifyMethod(alipayModel.Out_trade_no);
            switch (verifyResult)
            {
                case AliPayBeforeVerifyStatus.PAY_VERIFY_SUCCESS: IsCanContinune = true; break;
                case AliPayBeforeVerifyStatus.PAY_VERIFY_NOT_EXIST: Html = "订单不存在！"; break;
                case AliPayBeforeVerifyStatus.PAY_VERIFY_NOT_NEED_PAY: Html = "该订单非等待付款状态，无法付款！"; break;

            }
            #endregion

            if (IsCanContinune)
            {

                #region 请求参数包装过滤

                if (alipayModel.Defaultbank != "")
                {
                    //默认支付方式
                    alipayModel.Payment_type = "bankPay";
                }
                int minutes = 0;
                if (alipayModel.Exceed_minutes > 120)
                {
                    //订单超时后返回HTML
                    Html = alipayModel.Exceed_returnHtml;
                    return false;
                }

                #endregion

                Html = WapDirectPayCreateHtml(alipayModel);//将请求参数打包进行支付请求

                return true;
            }
            else
            {
                return false;
            }
        }  
        #endregion

        #region 处理支付同步返回的信息(主要是支付成功后返回页面）
        /// <summary>
        /// 处理支付同步返回的信息(主要是支付成功后返回页面）
        /// 创建人：孙佳杰 时间：2015.1.16
        /// </summary>
        /// <param name="method">订单处理逻辑</param>
        /// <param name="Request">当前请求对象</param>
        /// <returns></returns>
        public static string ReturnAction(Func<AliPayReturnModel, AliPayAfterResultStatus> method, System.Web.HttpRequest Request)
        {
            SortedDictionary<string, string> sPara = GetRequestGet(Request);
            string returnResult = string.Empty;//返回结果
            if (sPara.Count > 0)//判断是否有带返回参数
            {
                Notify aliNotify = new Notify();
                bool verifyResult = aliNotify.Verify(sPara, Request.QueryString["notify_id"], Request.QueryString["sign"]);

                if (verifyResult)//验证成功
                {
                    AliPayReturnModel aliPayReturn = new AliPayReturnModel(Request);
                    if (aliPayReturn.Trade_status == "TRADE_FINISHED" || aliPayReturn.Trade_status == "TRADE_SUCCESS")
                    {
                        //对返回的结果进行处理
                        returnResult = method(aliPayReturn).ToString();
                    }
                    else
                    {
                        return AliPayAfterResultStatus.支付宝返回数据错误.ToString();
                    }

                }
                else//验证失败
                {
                    returnResult = AliPayAfterResultStatus.验证失败.ToString();
                }
            }
            else
            {
                returnResult = AliPayAfterResultStatus.无返回参数.ToString();
            }
            return returnResult;
        } 
        #endregion

        #region 处理支付异步返回的信息（主要是使用该函数进行日志记录logResult）
        /// <summary>
        /// 处理支付异步返回的信息（主要是使用该函数进行日志记录logResult）
        /// 创建人：孙佳杰 时间：2015.1.16
        /// </summary>
        /// <param name="method">订单处理逻辑</param>
        /// <param name="Request">当前请求对象</param>
        /// <returns>获得处理情况</returns>
        public static string  NotifyAction(Func<AliPayReturnModel, string> method, System.Web.HttpRequest Request)
        {
            string msg = "";
            SortedDictionary<string, string> sPara = GetRequestPost(Request);
            if (sPara.Count > 0)//判断是否有带返回参数
            {
                Notify aliNotify = new Notify();
                bool verifyResult = aliNotify.Verify(sPara, Request.Form["notify_id"], Request.Form["sign"]);
                if (verifyResult)//验证成功
                {
                    AliPayReturnModel aliPayReturn = new AliPayReturnModel(Request);
                    if (aliPayReturn.Trade_status == "TRADE_FINISHED" || aliPayReturn.Trade_status == "TRADE_SUCCESS")
                    {
                        //对返回的结果进行处理
                        msg = method(aliPayReturn);
                    }
                    else
                    {
                        //Tools.Tool.Log.WritePurWeb("无效支付宝状态");
                        msg = "fail";
                    }
                }
                else//验证失败
                {
                    msg = "trade_status=" + Request.Form["trade_status"];
                }
            }
            else
            {
                // Tools.Tool.Log.WritePurWeb("无通知参数");
                msg = "无通知参数";
            }
            return msg;
        } 
        #endregion


        #region 私有方法

        #region 即时支付 支付方法
        /// <summary>
        /// 即时支付 支付方法
        /// 创建 甘春雨
        /// 2015年8月27日11:27:18 
        /// </summary>
        /// <param name="model">支付对象</param>
        /// <returns></returns>
        public static string DirectPayCreateHtml(AliPayModel model)
        {
            //把请求参数打包成数组
            SortedDictionary<string, string> sParaTemp = new SortedDictionary<string, string>();
            sParaTemp.Add("partner", model.Partner);
            sParaTemp.Add("_input_charset", model.Input_charset.ToLower());
            sParaTemp.Add("service", "create_direct_pay_by_user");
            sParaTemp.Add("payment_type", model.Payment_type);
            sParaTemp.Add("notify_url", model.Notify_url);
            sParaTemp.Add("return_url", model.Return_url);
            sParaTemp.Add("seller_email", model.Seller_email);
            sParaTemp.Add("out_trade_no", model.Out_trade_no);
            sParaTemp.Add("subject", model.Subject);
            sParaTemp.Add("total_fee", model.Total_fee.ToString("0.00"));
            sParaTemp.Add("body", model.Body);
            sParaTemp.Add("show_url", model.Show_url);
            sParaTemp.Add("anti_phishing_key", model.Anti_phishing_key);
            sParaTemp.Add("exter_invoke_ip", model.Exter_invoke_ip);
            //建立请求
            string sHtmlText = Submit.BuildRequest(sParaTemp, "get", "确认");
            return sHtmlText;
        } 

        #endregion

        #region 手机网站支付
        /// <summary>
        /// 手机网站支付
        /// 创建 甘春雨
        /// 2015年8月27日13:56:19
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static string WapDirectPayCreateHtml(AliPayModel model)
        {
            //把请求参数打包成数组
            SortedDictionary<string, string> sParaTemp = new SortedDictionary<string, string>();
            sParaTemp.Add("partner", model.Partner);
            sParaTemp.Add("seller_id", model.Seller_email);
            sParaTemp.Add("_input_charset", model.Input_charset.ToLower());
            sParaTemp.Add("service", "alipay.wap.create.direct.pay.by.user");
            sParaTemp.Add("payment_type", model.Payment_type);
            sParaTemp.Add("notify_url", model.Notify_url);
            sParaTemp.Add("return_url", model.Return_url);
            sParaTemp.Add("out_trade_no", model.Out_trade_no);
            sParaTemp.Add("subject", model.Subject);
            sParaTemp.Add("total_fee", model.Total_fee.ToString("0.00"));
            sParaTemp.Add("show_url", model.Show_url);
            sParaTemp.Add("body", model.Body);
            sParaTemp.Add("it_b_pay", model.It_b_pay);
            sParaTemp.Add("extern_token", model.Extern_token);

            //建立请求
            string sHtmlText = Submit.BuildRequest(sParaTemp, "get", "确认");
            return sHtmlText;
        } 
        #endregion


        /// <summary>
        /// 获取支付宝POST过来通知消息，并以“参数名=参数值”的形式组成数组
        /// 创建人：孙佳杰 时间：2015.1.16
        /// </summary>
        /// <returns>request回来的信息组成的数组</returns>
        private static SortedDictionary<string, string> GetRequestPost(System.Web.HttpRequest request)
        {
            int i = 0;
            SortedDictionary<string, string> sArray = new SortedDictionary<string, string>();
            NameValueCollection coll;
            //Load Form variables into NameValueCollection variable.
            coll = request.Form;

            // Get names of all forms into a string array.
            String[] requestItem = coll.AllKeys;

            for (i = 0; i < requestItem.Length; i++)
            {
                sArray.Add(requestItem[i], request.Form[requestItem[i]]);
            }

            return sArray;
        }

        /// <summary>
        /// 获取支付宝GET过来通知消息，并以“参数名=参数值”的形式组成数组
        /// 创建人：孙佳杰 时间：2015.1.16
        /// </summary>
        /// <returns>request回来的信息组成的数组</returns>
        private static SortedDictionary<string, string> GetRequestGet(System.Web.HttpRequest request)
        {
            int i = 0;
            SortedDictionary<string, string> sArray = new SortedDictionary<string, string>();
            NameValueCollection coll;
            //Load Form variables into NameValueCollection variable.
            coll = request.QueryString;

            // Get names of all forms into a string array.
            String[] requestItem = coll.AllKeys;

            for (i = 0; i < requestItem.Length; i++)
            {
                sArray.Add(requestItem[i], request.QueryString[requestItem[i]]);
            }

            return sArray;
        }
       
        #endregion
    }
}
