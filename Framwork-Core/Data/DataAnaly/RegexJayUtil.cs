using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mammothcode.Core.Data.DataAnaly
{
    /// <summary>
    ///  自创的正则提取算法
    ///  作者：孙佳杰
    ///  修改时间：2015.1.13
    ///  功能：RegexGetListByContent（使用正则截取内容获得相同规则下匹配的内容集合），
    ///        RegexGetStringByContent（使用正则截取内容获得该规则下匹配的内容字符串）
    /// </summary>
    public class RegexJayUtil
    {
        /// <summary>
        ///  功能：使用正则截取内容获得相同规则下匹配的内容集合
        /// </summary>
        /// <param name="input">要处理的内容</param>
        /// <param name="beginReg">开始标记</param>
        /// <param name="endReg">结束标记</param>
        /// <param name="boolNeedHeadFooter">是否需要头尾</param>
        /// <returns>匹配后的内容集合</returns>
        public static List<string> RegexGetListByContent(string input, string beginReg, string endReg, bool boolNeedHeadFooter)
        {
            List<string> result = new List<string>();     //返回抓取到的数据
            int lenBeginReg = beginReg.Length;   //开始标记
            int lenEndReg = endReg.Length;      //结束标记
            int lenInput = input.Length;   //要处理文本的长度
            StringBuilder boxStr = new StringBuilder();   //每次存放的String箱子
            bool isFindbeginReg = false;   //开始，中间，结束的是否找到标记

            for (int i = 0; i < lenInput; i++)
            {
                try
                {
                    //步骤①:检测是否找到开始标记
                    if (isFindbeginReg == false)
                    {
                        //步骤②:如果找到开始标记就说明找到并且把开始标记字符串进行存储
                        if (IsRegExist(input, beginReg, i, lenInput, lenBeginReg) == true)
                        {
                            if (boolNeedHeadFooter == true)
                            {
                                boxStr.Append(beginReg);  //放入箱子中
                            }
                            i = i + lenBeginReg;   //长度自增
                            isFindbeginReg = true;   //标记找到开始标记
                        }
                    }
                    //步骤③:如果检测到找到开始标记就要去检测是否找到结束标记
                    if (isFindbeginReg == true)
                    {
                        //步骤④:如果找到了结束标记就将结束标记字符串进行存储，并且将整个找到字符串进行放入集合中
                        if (IsRegExist(input, endReg, i, lenInput, lenEndReg) == true)  //找结束标记
                        {
                            if (boolNeedHeadFooter == true)
                            {
                                boxStr.Append(endReg);
                            }
                            i = i - 1;  //重新开始标记
                            result.Add(boxStr.ToString());
                            boxStr = new StringBuilder();
                            isFindbeginReg = false;
                        }
                    }
                    //步骤⑤:如果找到开始标记但是没有找到结束标记就将中间的字符进行存储
                    if (isFindbeginReg == true && i < lenInput)
                    {
                        boxStr.Append(input[i]);
                    }
                }
                catch
                {
                    continue;
                }
            }
            return result;
        }

        /// <summary>
        ///  功能：使用正则截取内容获得该规则下匹配的内容字符串
        /// </summary>
        /// <param name="input">要处理的内容</param>
        /// <param name="beginReg">开始标记</param>
        /// <param name="endReg">结束标记</param>
        /// <param name="boolNeedHeadFooter">是否需要头尾</param>
        /// <returns>匹配后的内容字符串</returns>
        public static string RegexGetStringByContent(string input, string beginReg, string endReg, bool boolNeedHeadFooter)
        {
            string returnStr = string.Empty;   //要查找的内容
            int lenBeginReg = beginReg.Length;   //开始标记
            int lenEndReg = endReg.Length;      //结束标记
            int lenInput = input.Length;   //要处理文本的长度
            StringBuilder boxStr = new StringBuilder();   //每次存放的String箱子
            bool isFindbeginReg = false;   //开始，中间，结束的是否找到标记

            for (int i = 0; i < lenInput; i++)
            {
                try
                {
                    if (isFindbeginReg == false)
                    {
                        if (IsRegExist(input, beginReg, i, lenInput, lenBeginReg) == true)  //找开始标记
                        {
                            if (boolNeedHeadFooter == true)
                            {
                                boxStr.Append(beginReg);  //放入箱子中
                            }
                            i = i + lenBeginReg;   //长度自增
                            isFindbeginReg = true;   //标记找到开始标记
                        }
                    }
                    else
                    {
                        if (IsRegExist(input, endReg, i, lenInput, lenEndReg) == true)  //找结束标记
                        {
                            if (boolNeedHeadFooter == true)
                            {
                                boxStr.Append(endReg);
                            }
                            i = i - 1;  //重新开始标记
                            returnStr = boxStr.ToString();
                            boxStr = new StringBuilder();
                            isFindbeginReg = false;
                            break;
                        }
                    }
                    if (isFindbeginReg == true && i < lenInput)
                    {
                        boxStr.Append(input[i]);
                    }
                }
                catch
                {
                    continue;
                }
            }
            return returnStr;
        }


        #region 私有方法

        /// <summary>
        ///  判断标记是否存在
        /// </summary>
        /// <param name="contentStr">检测内容</param>
        /// <param name="regStr">reg标记</param>
        /// <param name="nowIndex">当前位置</param>
        /// <param name="contentLength">检测内容长度</param>
        /// <param name="regLength">reg标记长度</param>
        /// <returns>标记是否存在</returns>
        private static bool IsRegExist(string contentStr, string regStr, int nowIndex, int contentLength, int regLength)
        {
            if (nowIndex + regLength <= contentLength)
            {
                string tempStr = contentStr.Substring(nowIndex, regLength);   //找到需要匹配字符串中的beginReg部分
                if (tempStr == regStr)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        #endregion

    }
}
