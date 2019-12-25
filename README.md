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
HttpResponse result = await HttpHelper.GetAsync("http://www.google.com");
```

### 方式2（自定义方式）：

```c#
HttpRequest request = new HttpRequest("http://www.google.com");
HttpResponse response = await request.SendAsync();
```

### 方式3（多个请求共享cookie）：

```c#
HttpClient client = new HttpClient();
HttpResponse response = await client.GetAsync("http://localhost:3353/admin");
if (response.IsRedirect())
{
    string loginUrl = response.GetRedirectUrl();
    Dictionary<string,string> loginForm = new Dictionary<string, string>();
    loginForm.Add("UserName","aaa");
    loginForm.Add("Password","bbb");
    loginForm.Add("Code","ccc");
    HttpResponse loginActiveResponse = await client.PostFormAsync(loginUrl,loginForm);
    if (loginActiveResponse.StatusCode == HttpStatusCode.Found)
    {
        HttpResponse adminResponse = await client.GetAsync("http://localhost:3353/admin");
        if (adminResponse.StatusCode == HttpStatusCode.OK)
        {
            textBox1.Text = adminResponse.GetHtml();
        }
    }
}
```

## 工具类

HttpRequest：HTTP请求对象。

HttpResponse：HTTP响应对象，无需使用using释放资源。

Encodings：常用编码枚举。

HttpClient：HTTP客户端，共享多个请求之间的cookie。

MimeTypes：请求或响应类型的常量集合。

UserAgents：提供常见的浏览器UserAgents。

HttpHelper：HTTP帮助类，提供便捷的静态方法。

ProxyFactory：代理工程类，用于生成代理设置。

## 编码

ASP.NET Core中不包含GB2312等编码，如果需要对此编码的HTML文档解码，必须需要安装扩展包：

```
PM> Install-Package System.Text.Encoding.CodePages
```

并在程序的入口添加以下代码：

```c#
Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
```



