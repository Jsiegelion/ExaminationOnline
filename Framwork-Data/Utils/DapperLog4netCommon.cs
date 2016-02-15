using Mammothcode.Public.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[assembly: Mammothcode.Public.Core.Config.XmlConfigurator(Watch = true)]
namespace Mammothcode.Data.Utils
{
    public class DapperLog4netCommon
    {
        private static ILog log = LogManager.GetLogger(typeof(DapperLog4netCommon));

        /// <summary>
        /// 保存日志信息
        /// </summary>
        /// <param name="message"></param>
        public static void Info(string message)
        {
            message += "\r\n--------------------------------------------------------------------------------------";
            log.Info(message);
        }

        /// <summary>
        /// 保存异常信息
        /// </summary>
        /// <param name="message"></param>
        public static void Error(string logInfo, Exception e)
        {
            logInfo += "\r\n--------------------------------------------------------------------------------------";
            log.Error(logInfo, e);
        }
    }
}
