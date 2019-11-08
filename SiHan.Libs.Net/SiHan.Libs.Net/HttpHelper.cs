using SiHan.Libs.Net.Client;
using SiHan.Libs.Net.Common;
using SiHan.Libs.Utils.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SiHan.Libs.Net
{
    /// <summary>
    /// HTTP帮助类
    /// </summary>
    public static class HttpHelper
    {
        /// <summary>
        /// 发送GET请求
        /// </summary>
        public static async Task<ResponseResult> GetAsync(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentNullException(nameof(url));
            }
            HttpRequest request = new HttpRequest(new Uri(url));
            using (HttpResponse response = await request.SendAsync())
            {
                return response.GetResult();
            }
        }


        /// <summary>
        /// POST(Json数据)
        /// </summary>
        public static async Task<ResponseResult> PostJsonAsync<T>(string url, T requestData) where T:class,new()
        {
            string json = SerializeHelper<T>.ToJson(requestData);
            HttpRequest request = new HttpRequest(new Uri(url),"POST");
            request.SetData(json, Encoding.UTF8);
            request.ContentType = MimeTypes.JSON;
            HttpClient client = new HttpClient();
            using (HttpResponse response = await client.SendAsync(request))
            {
                return response.GetResult();
            }
        }


        /// <summary>
        /// POST（表单）
        /// </summary>
        public static async Task<ResponseResult> PostFormAsync(string url, Dictionary<string, string> postParameters)
        {
            // https://stickler.de/en/information/code-snippets/httpwebrequest-with-post-data
            // https://www.c-sharpcorner.com/forums/how-to-covert-httpwebrequest-implementation-to-httpclient
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentNullException(nameof(url));
            }
            if (postParameters == null)
            {
                throw new ArgumentNullException(nameof(postParameters));
            }
            // 构建表单数据
            StringBuilder sb = new StringBuilder();
            foreach (var item in postParameters)
            {
                sb.Append($"{item.Key}={item.Value}&");
            }
            string postData = sb.ToString().Trim('&');
            // 提交数据
            HttpRequest request = new HttpRequest(new Uri(url), "POST");
            request.SetData(postData, Encoding.UTF8);
            request.ContentType = MimeTypes.Form;
            HttpClient client = new HttpClient();
            using (HttpResponse response = await client.SendAsync(request))
            {
                return response.GetResult();
            }
        }
    }
}

