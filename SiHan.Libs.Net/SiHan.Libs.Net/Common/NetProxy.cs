using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace SiHan.Libs.Net.Common
{
    /// <summary>
    /// 网络代理
    /// </summary>
    public class NetProxy
    {
        /// <summary>
        /// IP地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        public WebProxy GetWebProxy()
        {
            WebProxy proxy = new WebProxy(Address);
            proxy.Credentials = new NetworkCredential(UserName, Password);
            return proxy;
        }
    }
}
