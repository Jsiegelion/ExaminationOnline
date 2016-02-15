using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Security;

namespace Mammothcode.Core.Data.DataSecurity
{
    /// <summary>
    /// 数据加密基础类
    /// 作者：孙佳杰
    /// 修改时间：2015.1.14
    /// 功能：MD5(MD5加密)
    ///      Base64Encode,Base64Decode(Base64加解密）
    ///      CreateLoginToken（生成LoginToken）
    /// </summary>
    public class EncryptUtil
    {

        #region MD5加密

        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="sourse"></param>
        /// <param name="code">编码</param>
        /// <returns></returns>
        public static string MD5(string sourse,int code=16)
        {
            //if (code == 16) //16位MD5加密（取32位加密的9~25字符）
            //{
            //    return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sourse, "MD5").ToLower().Substring(8, 16);
            //}
            //if (code == 32) //32位加密
            //{
            //    return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sourse, "MD5").ToLower();
            //}
            //return "00000000000000000000000000000000";
            return FormsAuthentication.HashPasswordForStoringInConfigFile(sourse, "MD5");
        }

        /// <summary>
        /// MD5加密（以后开发用这个)
        /// 创建人:孙佳杰  创建时间:2015.3.18
        /// </summary>
        /// <param name="str">加密字符串</param>
        /// <param name="code">16位或者32位</param>
        /// <returns></returns>
        public static string Md5Encode(string str, int code)
        {
            if (code == 16) //16位MD5加密（取32位加密的9~25字符）
            {
                return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5").ToLower().Substring(8, 16);
            }
            if (code == 32) //32位加密
            {
                return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5").ToLower();
            }
            return "00000000000000000000000000000000";
        }
        #endregion

        #region Base64加解密

        /// <summary>
        /// Base64加密
        /// </summary>
        /// <param name="soure">需要加密的文本</param>
        /// <returns>加密后的字符串</returns>
        public static string Base64Encode(string soure)
        {
            byte[] enbuff = Encoding.UTF8.GetBytes(soure);
            return Convert.ToBase64String(enbuff);
        }

        /// <summary>
        /// Base64解密
        /// </summary>
        /// <param name="soure">需要解密的文本</param>
        /// <returns>解密后的字符串</returns>
        public static string Base64Decode(string soure)
        {
            byte[] deuff = Convert.FromBase64String(soure);
            return Encoding.UTF8.GetString(deuff);
        }

        #endregion

        #region URL加解密

        /// <summary>
        /// URL中出现字符串的加密
        /// 创建人：孙佳杰  创建时间：2015.2.3
        /// </summary>
        /// <param name="input">需要加密的文本</param>
        /// <returns>解密后的文本</returns>
        public static string UrlEncode(string input)
        {
            return HttpServerUtility.UrlTokenEncode(Encoding.Default.GetBytes(input));
        }

        /// <summary>
        /// URL中出现字符串的解密
        /// 创建人：孙佳杰  创建时间：2015.2.3
        /// </summary>
        /// <param name="input">需要解密的文本</param>
        /// <returns>解密后的文本</returns>
        public static string UrlDecode(string input)
        {
            return Encoding.Default.GetString(HttpServerUtility.UrlTokenDecode(input));
        }

        #endregion

        #region DES加减密
        //密钥
        private const string sKey = "qJzGEh6hESZDVJeCnFPGuxzaiB7NLQM4";
        //向量，必须是12个字符
        private const string sIV = "jsafojxliqd=";

        //构造一个对称算法
        private static  SymmetricAlgorithm mCSP = new TripleDESCryptoServiceProvider();

        #region 加密解密函数

        /// <summary>
        /// 字符串的加密
        /// </summary>
        /// <param name="Value">要加密的字符串</param>
        /// <returns>加密后的字符串</returns>
        public static string DESEncryptString(string Value)
        {
            try
            {
                ICryptoTransform ct;
                MemoryStream ms;
                CryptoStream cs;
                byte[] byt;
                mCSP.Key = Convert.FromBase64String(sKey);
                mCSP.IV = Convert.FromBase64String(sIV);
                //指定加密的运算模式
                mCSP.Mode = System.Security.Cryptography.CipherMode.CBC;
                //获取或设置加密算法的填充模式
                mCSP.Padding = System.Security.Cryptography.PaddingMode.Zeros;
                ct = mCSP.CreateEncryptor(mCSP.Key, mCSP.IV);//创建加密对象
                byt = Encoding.UTF8.GetBytes(Value);
                ms = new MemoryStream();
                cs = new CryptoStream(ms, ct, CryptoStreamMode.Write);
                cs.Write(byt, 0, byt.Length);
                cs.FlushFinalBlock();
                cs.Close();

                return Convert.ToBase64String(ms.ToArray());
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 解密字符串
        /// </summary>
        /// <param name="Value">加密后的字符串</param>
        /// <returns>解密后的字符串</returns>
        public static string DESDecryptString(string Value)
        {
            try
            {
                ICryptoTransform ct;//加密转换运算
                MemoryStream ms;//内存流
                CryptoStream cs;//数据流连接到数据加密转换的流
                byte[] byt;
                //将3DES的密钥转换成byte
                mCSP.Key = Convert.FromBase64String(sKey);
                //将3DES的向量转换成byte
                mCSP.IV = Convert.FromBase64String(sIV);
                mCSP.Mode = System.Security.Cryptography.CipherMode.CBC;
                mCSP.Padding = System.Security.Cryptography.PaddingMode.Zeros;
                ct = mCSP.CreateDecryptor(mCSP.Key, mCSP.IV);//创建对称解密对象
                byt = Convert.FromBase64String(Value);
                ms = new MemoryStream();
                cs = new CryptoStream(ms, ct, CryptoStreamMode.Write);
                cs.Write(byt, 0, byt.Length);
                cs.FlushFinalBlock();
                cs.Close();

                return Encoding.UTF8.GetString(ms.ToArray());
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
        #endregion
        #endregion

    }
}



