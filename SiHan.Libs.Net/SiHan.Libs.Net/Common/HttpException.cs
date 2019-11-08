using System;
using System.Collections.Generic;
using System.Text;

namespace SiHan.Libs.Net.Common
{
    /// <summary>
    /// HTTP异常
    /// </summary>
    public class HttpException : Exception
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public int StatusCode { get; }
        /// <summary>
        /// 状态描叙
        /// </summary>
        public string StatusDescription { get; }
        /// <summary>
        /// 响应文本
        /// </summary>
        public string ResponseText { get; set; }

        /// <summary>
        /// HTTP异常
        /// </summary>
        /// <param name="statusCode">状态码</param>
        /// <param name="statusDescription">状态描述</param>
        /// <param name="responseText">响应文本</param>
        public HttpException(int statusCode, string statusDescription, string responseText) : base(statusDescription)
        {
            this.StatusCode = statusCode;
            this.StatusDescription = statusDescription;
            this.ResponseText = responseText;
        }
    }
}


