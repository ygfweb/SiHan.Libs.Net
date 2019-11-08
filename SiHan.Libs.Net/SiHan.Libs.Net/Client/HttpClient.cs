using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Cache;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SiHan.Libs.Net.Client
{
    /// <summary>
    /// 网络访问客户端
    /// </summary>
    public class HttpClient
    {
        private readonly SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);

        /// <summary>
        /// cookie容器
        /// </summary>
        protected CookieContainer CookieContainer { get; set; } = new CookieContainer();

        /// <summary>
        /// 发送HTTP请求
        /// </summary>
        public async Task<HttpResponse> SendAsync(HttpRequest request)
        {
            await this.semaphore.WaitAsync();
            try
            {
                request.CookieContainer = this.CookieContainer;
                HttpWebRequest httpWebRequest = request.GetHttpWebRequest();
                HttpWebResponse response = (HttpWebResponse)await httpWebRequest.GetResponseAsync();
                response.Cookies = httpWebRequest.CookieContainer.GetCookies(httpWebRequest.RequestUri);
                return new HttpResponse(response);
            }
            finally
            {
                this.semaphore.Release();
            }
        }
    }
}
