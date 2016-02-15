using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: Mammothcode.Public.Core.Config.XmlConfigurator(Watch = true)]
namespace Mammothcode.Public.Core
{
    public class Log4netUtil
    {
        private static ILog log = LogManager.GetLogger(typeof(Log4netUtil));

        /// <summary>
        /// 保存日志信息
        /// </summary>
        /// <param name="message"></param>
        public static void Info(string message)
        {
            message = "\r\n---------------------------------" + message;
            message += "\r\n--------------------------------------------------------------------------------------";
            log.Info(message);
        }

        /// <summary>
        /// 保存日志信息
        /// </summary>
        /// <param name="message"></param>
        public static void Info(string message,string info)
        {
            message = "\r\n---------------------------------\r\n" + message;
            message += "\r\n---------------------------------";
            message += info + "\r\n--------------------------------------------------------------------------------------";
            log.Info(message);
        }

        /// <summary>
        /// 保存异常信息
        /// </summary>
        /// <param name="message"></param>
        public static void Error(string logInfo, Exception e)
        {
            logInfo = "\r\n---------------------------------\r\n" + logInfo;
            logInfo += "\r\n--------------------------------------------------------------------------------------";
            log.Error(logInfo, e);
        }

        /// <summary>
        /// 保存异常信息
        /// </summary>
        /// <param name="message"></param>
        public static void Error(string logInfo, string errorMessage)
        {
            logInfo = "\r\n---------------------------------\r\n" + logInfo;
            logInfo += "\r\n---------------------------------";
            logInfo += errorMessage + "\r\n--------------------------------------------------------------------------------------";
            log.Error(logInfo);
        }
    }
}
