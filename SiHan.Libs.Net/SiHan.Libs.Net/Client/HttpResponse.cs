using SiHan.Libs.Net.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;

namespace SiHan.Libs.Net.Client
{
    /// <summary>
    /// HTTP响应
    /// </summary>
    public sealed class HttpResponse : IDisposable
    {
        private HttpWebResponse HttpWebResponse { get; set; }
        /// <summary>
        /// 内容字节
        /// </summary>
        public byte[] ContentBytes { get; }

        public HttpResponse(HttpWebResponse httpWebResponse)
        {
            HttpWebResponse = httpWebResponse ?? throw new ArgumentNullException(nameof(httpWebResponse));
            var content = new MemoryStream();
            using (Stream responseStream = this.HttpWebResponse.GetResponseStream())
            {
                responseStream.CopyTo(content);
            }
            this.ContentBytes = content.ToArray();
        }

        /// <summary>
        /// 获取响应文本
        /// </summary>
        /// <param name="isAutoEncoding">是否自动编码，true：无视编码获取文本。false：使用UTF8获取文本</param>
        /// <returns></returns>
        public string GetString(bool isAutoEncoding = false)
        {
            return EncodingConverter.GetHtmlString(this.ContentBytes, this.CharacterSet, isAutoEncoding);
        }

        /// <summary>
        /// 获取响应HTML编码
        /// </summary>
        public Encoding GetEncoding()
        {
            return EncodingConverter.GetEncoding(this.ContentBytes, this.CharacterSet);
        }

        /// <summary>
        /// 字符集
        /// </summary>
        public string CharacterSet
        {
            get
            {
                return this.HttpWebResponse.CharacterSet;
            }
        }

        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess
        {
            get
            {
                return this.StatusCode >= 200 && this.StatusCode < 300;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            if (this.HttpWebResponse != null)
            {
                this.HttpWebResponse.Dispose();
            }            
        }

        /// <summary>
        /// 状态码
        /// </summary>
        public int StatusCode
        {
            get
            {
                return (int)HttpWebResponse.StatusCode;
            }
        }

        /// <summary>
        /// 状态描叙
        /// </summary>
        public string StatusDescription
        {
            get
            {
                return this.HttpWebResponse.StatusDescription;
            }
        }

        /// <summary>
        /// 当前页面Cookie，注意这不是该网站的cookie
        /// </summary>
        public CookieCollection Cookies
        {
            get
            {
                return this.HttpWebResponse.Cookies;
            }
        }

        /// <summary>
        /// HTTP头信息
        /// </summary>
        public WebHeaderCollection Headers
        {
            get
            {
                return this.HttpWebResponse.Headers;
            }
        }

        /// <summary>
        /// 获取响应结果
        /// </summary>
        /// <returns></returns>
        public ResponseResult GetResult()
        {
            return new ResponseResult(this.StatusCode, this.StatusDescription, this.ContentBytes, this.CharacterSet);
        }
    }
}



