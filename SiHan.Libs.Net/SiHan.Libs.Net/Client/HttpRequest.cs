using SiHan.Libs.Net.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SiHan.Libs.Net.Client
{
    /// <summary>
    /// HTTP请求
    /// </summary>
    public sealed class HttpRequest
    {
        private HttpWebRequest HttpWebRequest { get; set; }

        private string _Encoding = "UTF-8";
        /// <summary>
        /// 编码
        /// </summary>
        public string Encoding
        {
            get
            {
                return _Encoding;
            }
            set
            {
                _Encoding = value;
                this.SetContentTypeAndEncoding();
            }
        }

        private string _ContentType = MimeTypes.Text;
        /// <summary>
        /// Content-type HTTP 标头的值
        /// </summary>
        public string ContentType
        {
            get
            {
                return _ContentType;
            }
            set
            {
                _ContentType = value;
                this.SetContentTypeAndEncoding();
            }
        }

        private void SetContentTypeAndEncoding()
        {
            this.HttpWebRequest.ContentType = $"{_ContentType};charset={_Encoding}";
        }


        public HttpRequest(Uri url, string method = "GET")
        {
            if (url == null)
            {
                throw new ArgumentNullException(nameof(url));
            }
            if (string.IsNullOrWhiteSpace(method))
            {
                throw new ArgumentNullException(nameof(method));
            }
            HttpWebRequest = WebRequest.CreateHttp(url);
            HttpWebRequest.UserAgent = UserAgents.PC;
            HttpWebRequest.CookieContainer = new CookieContainer();
            // 禁用缓存
            HttpRequestCachePolicy noCachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);
            HttpWebRequest.CachePolicy = noCachePolicy;
            HttpWebRequest.AllowAutoRedirect = true;
            HttpWebRequest.KeepAlive = true;
            HttpWebRequest.ProtocolVersion = System.Net.HttpVersion.Version11;
            this.SetContentTypeAndEncoding();
            HttpWebRequest.Method = method;
            HttpWebRequest.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
            //禁用代理，默认是开启自动代理，会导致网络访问缓慢。
            // https://stackoverflow.com/questions/2519655/httpwebrequest-is-extremely-slow
            HttpWebRequest.Proxy = null;
            HttpWebRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3";
            HttpWebRequest.Headers.Add("accept-language", "zh-CN,zh;q=0.9,en;q=0.8");
            HttpWebRequest.Headers.Add("accept-encoding", "gzip, deflate, br");
        }

        /// <summary>
        /// HTTP访问方法名称，默认是GET,设置值不能为空，并自动转换为大写
        /// </summary>
        public string Method
        {
            get
            {
                return this.HttpWebRequest.Method;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException(nameof(value));
                }
                this.HttpWebRequest.Method = value.Trim().ToUpper();
            }
        }

        /// <summary>
        /// 来源地址
        /// </summary>
        public string Referer
        {
            get
            {
                return this.HttpWebRequest.Referer;
            }
            set
            {
                this.HttpWebRequest.Referer = value;
            }
        }

        /// <summary>
        /// 客户端标识
        /// </summary>
        public string UserAgent
        {
            get
            {
                return this.HttpWebRequest.UserAgent;
            }
            set
            {
                this.HttpWebRequest.UserAgent = value;
            }
        }

        private NetProxy _NetProxy = null;
        /// <summary>
        /// 代理设置
        /// </summary>
        public NetProxy Proxy
        {
            get
            {
                return _NetProxy;
            }
            set
            {
                _NetProxy = value;
                if (value == null)
                {
                    this.HttpWebRequest.Proxy = null;
                }
                else
                {
                    this.HttpWebRequest.Proxy = value.GetWebProxy();
                }
            }
        }

        /// <summary>
        /// Cookie容器
        /// </summary>
        public CookieContainer CookieContainer
        {
            get
            {
                return this.HttpWebRequest.CookieContainer;
            }
            set
            {
                this.HttpWebRequest.CookieContainer = value;
            }
        }

        internal HttpWebRequest GetHttpWebRequest()
        {
            return this.HttpWebRequest;
        }

        /// <summary>
        /// 是否允许自动跳转（支持支付跳转）（某些情况下需要将其设置为false才能获取Cookie）
        /// </summary>
        public bool AllowAutoRedirect
        {
            get
            {
                return this.HttpWebRequest.AllowAutoRedirect;
            }
            set
            {
                this.HttpWebRequest.AllowAutoRedirect = value;
            }
        }
        /// <summary>
        /// Content-length HTTP 标头
        /// </summary>
        public long ContentLength
        {
            get
            {
                return this.HttpWebRequest.ContentLength;
            }
            set
            {
                this.HttpWebRequest.ContentLength = value;
            }
        }

        /// <summary>
        /// 设置提交数据
        /// </summary>
        public void SetData(string data, Encoding encoding)
        {
            if (string.IsNullOrWhiteSpace(data))
            {
                throw new ArgumentNullException(nameof(data));
            }
            if (encoding == null)
            {
                throw new ArgumentNullException(nameof(encoding));
            }
            byte[] bytes = encoding.GetBytes(data);
            using (var stream = this.HttpWebRequest.GetRequestStream())
            {
                stream.Write(bytes, 0, data.Length);
            }
            this.HttpWebRequest.ContentLength = bytes.Length;
        }

        /// <summary>
        /// 设置HTTP请求头
        /// </summary>
        public void SetHeader(string name, string value)
        {
            if (string.IsNullOrWhiteSpace(nameof(name)))
            {
                throw new ArgumentNullException(nameof(name));
            }
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(nameof(value));
            }
            if (this.HttpWebRequest.Headers[name] == null)
            {
                this.HttpWebRequest.Headers.Add(name, value);
            }
            else
            {
                this.HttpWebRequest.Headers.Set(name, value);
            }
        }

        /// <summary>
        /// 发送请求
        /// </summary>
        public async Task<HttpResponse> SendAsync()
        {
            HttpWebResponse response = await this.HttpWebRequest.GetResponseAsync() as HttpWebResponse;
            response.Cookies = this.HttpWebRequest.CookieContainer.GetCookies(this.HttpWebRequest.RequestUri);
            return new HttpResponse(response);
        }
    }
}