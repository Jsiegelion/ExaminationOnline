using Mammothcode.Core.Data.DataSecurity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mammothcode.Core.Data.DataAutomatic
{
    /// <summary>
    /// 随机字符串产生公共类
    /// </summary>
    public class StringRandomUtil
    {
        #region 验证码数据产生模块

        //进行随机产生的原始数据
        private static string numChar = "0,1,2,3,4,5,6,7,8,9";   //纯数字
        private static string letChar = "a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,i,s,t,u,v,w,x,y,z";  //只有字母
        private static string numandletChar = "0,1,2,3,4,5,6,7,8,9,a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,i,s,t,u,v,w,x,y,z";  //数字加字母

        /// <summary>
        /// 随机产生验证码的数据
        /// 创建人：孙佳杰  时间：2015.1.19
        /// </summary>
        /// <param name="codeNum">长度</param>
        /// <param name="mode">模式选择(0,1,2)</param>
        /// <returns>验证码的数据</returns>
        public static string RandomCode(int codeNum, StringRadomType mode)
        {
            string[] VcArray = null;
            //模式选择
            switch (mode)
            {
                case StringRadomType.SingleNumber: VcArray = numChar.Split(','); break;
                case StringRadomType.SingleLetter: VcArray = letChar.Split(','); break;
                case StringRadomType.NumberAndLetter: VcArray = numandletChar.Split(','); break;
                default: return "模式选择错误";
            }
            //定义随机器
            Random rand = new Random();
            int temp = -1;
            string returnStr = "";//由于字符很短所以不适用stringbuilder
            for (int i = 0; i < codeNum; i++)
            {
                if (temp != -1)
                {
                    int ranSeed = i * temp * unchecked((int)DateTime.Now.Ticks);  //进行随机的种子公式
                    rand = new Random(ranSeed);
                }
                int t = rand.Next(VcArray.Length);   //产生随机字符
                temp = t;
                //定义公式是否将字符大写
                if (t % 2 != 0 && t > 9)
                {
                    returnStr += VcArray[t];
                }
                else
                {
                    returnStr += VcArray[t].ToUpper();
                }
            }
            return returnStr;
        }

        #endregion

        #region 生成GUID

        /// <summary>  
        /// 根据GUID获取16位的唯一字符串  
        /// </summary>  
        /// <param name=\"guid\"></param>  
        /// <returns></returns>  
        public static string GuidTo16String()
        {
            long i = 1;
            foreach (byte b in Guid.NewGuid().ToByteArray())
                i *= ((int)b + 1);
            return string.Format("{0:x}", i - DateTime.Now.Ticks);
        }

        /// <summary>  
        /// 根据GUID获取19位的唯一数字序列  
        /// </summary>  
        /// <returns></returns>  
        public static long GuidToLongID()
        {
            byte[] buffer = Guid.NewGuid().ToByteArray();
            return BitConverter.ToInt64(buffer, 0);
        }  

        #endregion

        /// <summary>
        /// 生成LoginToken
        /// </summary>
        /// <returns></returns>
        public static string CreateLoginToken()
        {
            string startToken = System.Guid.NewGuid().ToString();
            return EncryptUtil.Base64Encode(startToken);
        }

        /// <summary>
        /// 随机字符串产生类型
        /// </summary>
        public enum StringRadomType
        { 
            /// <summary>
            /// 单一的数字
            /// </summary>
           SingleNumber,
            /// <summary>
            /// 单一的字符
            /// </summary>
           SingleLetter,
            /// <summary>
            /// 既有数字又有字符
            /// </summary>
            NumberAndLetter
        }
    }
}
