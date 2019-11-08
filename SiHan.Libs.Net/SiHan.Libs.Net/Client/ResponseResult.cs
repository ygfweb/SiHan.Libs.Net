using SiHan.Libs.Net.Common;
using SiHan.Libs.Utils.Text;
using System;
using System.Collections.Generic;
using System.Text;

namespace SiHan.Libs.Net.Client
{
    /// <summary>
    /// 响应结果
    /// </summary>
    public class ResponseResult
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public int StatusCode { get; }

        /// <summary>
        /// 状态描述
        /// </summary>
        public string StatusDescription { get; }

        /// <summary>
        /// 响应内容
        /// </summary>
        public byte[] Bytes { get; } = Array.Empty<byte>();

        /// <summary>
        /// 字符集
        /// </summary>
        public string CharacterSet { get; }

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
        /// 响应结果
        /// </summary>
        /// <param name="statusCode">状态码</param>
        /// <param name="statusDescription">状态描述</param>
        /// <param name="contentBytes">响应内容</param>
        /// <param name="characterSet">响应字符集</param>
        public ResponseResult(int statusCode, string statusDescription, byte[] contentBytes, string characterSet)
        {
            StatusCode = statusCode;
            StatusDescription = statusDescription;
            Bytes = contentBytes;
            CharacterSet = characterSet;
        }

        /// <summary>
        /// 获取响应文本
        /// </summary>
        public string GetString(bool isAutoEncoding = false)
        {
            return EncodingConverter.GetHtmlString(this.Bytes, this.CharacterSet, isAutoEncoding);
        }

        /// <summary>
        /// 使用JSON将响应文本反序列化为对象
        /// </summary>
        public T ToObject<T>() where T : class, new()
        {
            string text = Encoding.UTF8.GetString(this.Bytes);
            return SerializeHelper<T>.FromJson(text);
        }
    }
}
