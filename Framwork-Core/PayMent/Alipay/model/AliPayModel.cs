using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Com.Alipay;

namespace Mammothcode.Core.PayMent.Alipay
{

    /// <summary>
    ///支付宝快捷支付模型
    /// </summary>
    public class AliPayModel
    {
        public AliPayModel()
        {
            //默认是1
            Payment_type = "1";
            Notify_url = Config.Notify_Url;
            Return_url = Config.Return_Url;
            Seller_email = Config.Seller_Email;
            Input_charset = Config.Input_charset;
            Key = Config.Key;
            Partner = Config.Partner;
            Anti_phishing_key = Submit.Query_timestamp();
            Exter_invoke_ip = "";
        }
        /// <summary>
        /// 字符编码格式 目前支持 gbk 或 utf-8
        /// </summary>
        public string Input_charset { get; set; }
        /// <summary>
        /// 交易安全检验码，由数字和字母组成的32位字符串
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 合作身份者ID，以2088开头由16位纯数字组成的字符串
        /// </summary>
        public string Partner { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Seller_email { get; set; }
        /// <summary>
        /// 页面跳转同步通知页面路径
        /// </summary>
        public string Return_url { get; set; }
        /// <summary>
        //必填，不能修改
        //服务器异步通知页面路径
        /// </summary>
        public string Notify_url { get; set; }
        ///// <summary>
        ///// 支付类型(必填，不能修改）
        ///// </summary>
        public string Payment_type{get;set;}

        /// <summary>
        /// 支付类型（必填，不能修改）
        /// </summary>
        public string Out_trade_no { get; set; }

        /// <summary>
        /// 订单名称（必填）
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// 付款金额（必填）
        /// </summary>
        public double Total_fee { get; set; }

        /// <summary>
        /// 订单描述
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// 支付方法
        /// </summary>
        public string Paymethod { get; set; }

        /// <summary>
        /// 支付银行
        /// </summary>
        public string Defaultbank { get; set; }

        /// <summary>
        /// 商品展示地址(需以http://开头的完整路径，例如：http://www.xxx.com/myorder.html)
        /// </summary>
        public string Show_url { get; set; }

        /// <summary>
        /// 订单支付截至时间
        /// </summary>
        public DateTime Exceed_time { get; set; }

        /// <summary>
        /// 订单停留时间
        /// </summary>
        public int Exceed_minutes { get; set; }

        /// <summary>
        /// 订单超时后返回HTML
        /// </summary>
        public string Exceed_returnHtml { get; set; }
        /// <summary>
        /// 即时支付-防钓鱼时间戳
        /// 若要使用请调用类文件submit中的query_timestamp函数
        /// </summary>
        public string Anti_phishing_key { get; set; }
        /// <summary>
        /// 即时支付-客户端的IP地址
        /// 非局域网的外网IP地址，如：221.0.0.1
        /// </summary>
        public string Exter_invoke_ip { get; set; }
        /// <summary>
        /// 选填-超时时间
        /// </summary>
        public string It_b_pay { get; set; }
        /// <summary>
        /// 选填-钱包token
        /// </summary>
        public string Extern_token { get; set; }
    }

    /// <summary>
    /// 支付宝即时支付模型
    /// 创建 甘春雨
    /// 2015年8月27日11:15:25
    /// </summary>
    public class AliPayDirectModel
    {
        /// <summary>
        /// 即时支付模型初始化函数
        /// </summary>
        /// <param name="out_trade_no">商户订单号</param>
        /// <param name="subject">订单名称</param>
        /// <param name="body">订单描述</param>
        /// <param name="show_url"></param>
        /// <param name="anti_phishing_key"></param>
        /// <param name="exter_invoke_ip"></param>
        public AliPayDirectModel(string out_trade_no, string total_fee, string subject, string body = "", string show_url = "", string anti_phishing_key = "", string exter_invoke_ip = "")
        {
            try
            {
                double fee = double.Parse(out_trade_no);
                total_fee = fee.ToString("F");
            }
            catch (Exception e)
            {
                this.total_fee = "0";
            }
            finally
            {
                this.out_trade_no = out_trade_no;
                this.total_fee = total_fee;
                this.subject = subject;
                this.body = body;
                this.show_url = show_url;
                this.anti_phishing_key = anti_phishing_key == "" ? Submit.Query_timestamp() : anti_phishing_key;
                this.exter_invoke_ip = exter_invoke_ip;
            }
        }
        //支付类型
        public string payment_type = "1";
        //必填，不能修改
        //服务器异步通知页面路径
        public string notify_url = AlipayConfig.NOTIFY_URL;
        //需http://www.mammothcode.com/alipay.demo/notify_url.aspx格式的完整路径，不能加?id=123这类自定义参数

        //页面跳转同步通知页面路径
        public string return_url = AlipayConfig.RETURN_URL;
        //需http://www.mammothcode.com/alipay.demo/return_url.aspx格式的完整路径，不能加?id=123这类自定义参数，不能写成http://localhost/

        //卖家支付宝帐户
        public string seller_email = AlipayConfig.SELLER_EMAIL;
        //必填

        //商户订单号
        public string out_trade_no = "";
        //商户网站订单系统中唯一订单号，必填

        //订单名称
        public string subject = "";
        //必填

        //付款金额
        public string total_fee = "";//0.01
        //必填

        //订单描述
        public string body = "";//订单描述
        //商品展示地址
        public string show_url = "";
        //需以http://开头的完整路径，例如：http://www.mammothcode.com/alipay.demo/product.html
        //防钓鱼时间戳
        public string anti_phishing_key = "";
        //若要使用请调用类文件submit中的query_timestamp函数

        //客户端的IP地址
        public string exter_invoke_ip = "";
        //非局域网的外网IP地址，如：221.0.0.1
    }

    /// <summary>
    /// 手机网站支付模型
    /// 创建 甘春雨
    /// 2015年8月27日11:51:04
    /// </summary>
    public class AliPayWapDirect
    {
        /// <summary>
        /// 手机网站支付初始化构造函数
        /// </summary>
        /// <param name="out_trade_no"></param>
        /// <param name="subject"></param>
        /// <param name="total_fee"></param>
        /// <param name="body"></param>
        /// <param name="show_url"></param>
        /// <param name="it_b_pay"></param>
        /// <param name="extern_token"></param>
        public AliPayWapDirect(string out_trade_no, string total_fee, string subject, string show_url = "", string body = "", string it_b_pay = "", string extern_token = "")
        {
            try
            {
                double fee = double.Parse(out_trade_no);
                total_fee = fee.ToString("F");
            }
            catch (Exception e)
            {
                this.total_fee = "0";
            }
            finally
            {
                this.out_trade_no = out_trade_no;
                this.total_fee = total_fee;
                this.subject = subject;
                this.body = body;
                this.show_url = show_url;
                this.it_b_pay = it_b_pay;
                this.extern_token = extern_token;
            }
        }
        //支付类型
        public string payment_type = "1";
        //必填，不能修改
        //服务器异步通知页面路径
        public string notify_url = AlipayConfig.NOTIFY_URL;
        //需http://格式的完整路径http://www.mammothcode.com/alipay.demo/notify_url.aspx，不能加?id=123这类自定义参数

        //页面跳转同步通知页面路径
        public string return_url = AlipayConfig.RETURN_URL;
        //需http://格式的完整路径http://www.mammothcode.com/alipay.demo/return_url.aspx，不能加?id=123这类自定义参数，不能写成http://localhost/

        //商户订单号
        public string out_trade_no = "";
        //商户网站订单系统中唯一订单号，必填

        //订单名称
        public string subject = "";
        //必填

        //付款金额
        public string total_fee = "";
        //必填

        //商品展示地址
        public string show_url = "";
        //必填，需以http://开头的完整路径，例如：http://www.mammothcode.com/alipay.demo/product.html

        //订单描述
        public string body = "";
        //选填

        //超时时间
        public string it_b_pay = "";
        //选填

        //钱包token
        public string extern_token = "";
        //选填
    }
}
