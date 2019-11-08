# SiHan.Libs.Net
## 介绍

使用HttpWebRequest实现的轻量级HTTP客户端库。基于netstandard2.0，可以在winform、ASP.NET Core中使用。

## 安装

```
PM> Install-Package SiHan.Libs.Net
```

## 使用

### 方式1（常用于调用API）：

```c#
ResponseResult result = await HttpHelper.GetAsync("http://www.google.com");
```

### 方式2（自定义方式）：

```c#
HttpRequest request = new HttpRequest(new Uri("http://www.google.com"));
using (HttpResponse response = await request.SendAsync())
{
    string result = response.GetString();
}
```

### 方式3（多个请求共享cookie）：

```c#
HttpClient client = new HttpClient();
HttpRequest request = new HttpRequest(new Uri("http://www.google.com"));
using (HttpResponse response = await client.SendAsync(request))
{
    string result = response.GetString();
}
```

## 工具类

CookieHelper：将cookie保存到文件。

EncodingConverter：实现无视编码获取HTML内容。

Encodings：常用编码枚举。

HttpException：HTTP请求异常类。

HttpRequestType：HTTP请求类型枚举。

MimeTypes：请求或响应类型的常量集合。

NetProxy：封装HTTP请求代理。

UserAgents：提供常见的浏览器UserAgents。

## 编码

ASP.NET Core中不包含GB2312等编码，如果需要对此编码的HTML文档解码，必须需要安装扩展包：

```
PM> Install-Package System.Text.Encoding.CodePages
```

并在程序的入口添加以下代码：

```c#
Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
```



