using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Management;
using System.Net.NetworkInformation;
using System.Net;
using System.Runtime.InteropServices;

namespace Mammothcode.Core.SystemTool
{
    /// <summary>
    /// 获取系统信息通用类
    /// </summary>
    public class SystemUtil
    {

        #region 获取MAC地址

        /// <summary>
        /// 获取网卡ID代码 
        /// </summary>
        /// <returns></returns>
        public static string GetNetworkAdpaterID()
        {
            try
            {
                string mac = "";
                ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                    if ((bool)mo["IPEnabled"] == true)
                    {
                        mac += mo["MacAddress"].ToString() + " ";
                        break;
                    }
                moc = null;
                mc = null;
                return mac.Trim();
            }
            catch (Exception e)
            {
                return "uMnIk";
            }
        }

        #endregion
    }
}
