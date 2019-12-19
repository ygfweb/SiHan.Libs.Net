using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SiHan.Libs.Net.Common;

namespace SiHan.Libs.Net
{
    /// <summary>
    /// HTTP响应
    /// </summary>
    public class HttpResponse
    {
        /// <summary>
        /// Cookie对象集合
        /// </summary>
        public CookieCollection CookieCollection { get; set; }

        /// <summary>
        /// 返回的Byte数组 只有ResultType.Byte时才返回数据，其它情况为空
        /// </summary>
        public byte[] ResultByte { get; set; }

        /// <summary>
        /// header对象
        /// </summary>
        public WebHeaderCollection Header { get; set; }
        /// <summary>
        /// 返回状态说明
        /// </summary>
        public string StatusDescription { get; set; }
        /// <summary>
        /// 返回状态码,默认为OK
        /// </summary>
        public HttpStatusCode StatusCode { get; set; }

        /// <summary>
        /// 最后访问的URl
        /// </summary>
        public string ResponseUri { get; set; }

        /// <summary>
        /// 返回数据的编码，该值是HttpRequest对象的传递值。如果为空，表示自动获取编码。常用编码有utf-8,gbk,gb2312
        /// </summary>
        public Encoding Encoding { get; set; } = Encoding.UTF8;
        /// <summary>
        /// 字符集
        /// </summary>
        public string CharacterSet { get; set; }

        /// <summary>
        /// 获取重定向的URl
        /// </summary>
        public string GetRedirectUrl()
        {
            try
            {
                if (Header != null && Header.Count > 0)
                {
                    if (Header.AllKeys.Any(k => k.ToLower().Contains("location")))
                    {
                        string baseurl = Header["location"].ToString().Trim();
                        string locationurl = baseurl.ToLower();
                        if (!string.IsNullOrWhiteSpace(locationurl))
                        {
                            bool b = locationurl.StartsWith("http://") || locationurl.StartsWith("https://");
                            if (!b)
                            {
                                baseurl = new Uri(new Uri(ResponseUri), baseurl).AbsoluteUri;
                            }
                        }
                        return baseurl;
                    }
                }
            }
            catch { }
            return string.Empty;
        }
        /// <summary>
        /// 响应是否成功
        /// </summary>
        public bool IsSuccess()
        {
            int code = Convert.ToInt32(this.StatusCode);
            if (code >= 200 && code < 300)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 是否是重定向
        /// </summary>
        public bool IsRedirect()
        {
            return this.StatusCode == HttpStatusCode.Redirect;
        }

        /// <summary>
        /// 将响应字节解码成字符串（如果未指定编码，将自动识别）
        /// </summary>
        public string GetHtml()
        {
            if (this.ResultByte == null || this.ResultByte.Length == 0)
            {
                return "";
            }
            else
            {
                Encoding encoding = this.Encoding;
                if (encoding != null)
                {
                    return encoding.GetString(this.ResultByte);
                }
                else
                {
                    return EncodingConverter.GetAutoEncodingString(this.ResultByte, this.CharacterSet);
                }
            }
        }
    }
}
