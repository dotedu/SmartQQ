using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SmartQQLib.Http
{
    /// <summary>
    /// 请求参数类
    /// </summary>
    public class HttpRequestParameter
    {
        #region 构造函数 - 设置 默认值

        public HttpRequestParameter()
        {
            Method = HttpMethodEnum.Get;
            HttpVersion = System.Net.HttpVersion.Version11;

            Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            ContentType = "application/x-www-form-urlencoded";
            UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/52.0.2743.116 Safari/537.36";
            KeepAlive = true;
            Timeout = 10000;

            Encoding = Encoding.UTF8;
            ResponseEnum = HttpResponseEnum.String;
        }

        #endregion


        #region 请求行 - 参数
        /// <summary>
        /// 请求 方式
        /// </summary>
        public HttpMethodEnum Method { get; set; }
        /// <summary>
        /// 请求 资源 url地址
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// http协议版本
        /// </summary>
        public Version HttpVersion { get; set; }
        #endregion

        #region 请求头 - 参数
        /// <summary>
        /// 支持内容类型
        /// </summary>
        public string Accept { get; set; }
        /// <summary>
        /// 请求体 内容 类型
        /// </summary>
        public string ContentType { get; set; }
        /// <summary>
        /// 浏览器标识
        /// </summary>
        public string UserAgent { get; set; }
        /// <summary>
        /// 来源页
        /// </summary>
        public string Referer { get; set; }
        /// <summary>
        /// 最初请求发起地址
        /// </summary>
        public string Origin { get; set; }
        /// <summary>
        /// 保持TCP连接
        /// </summary>
        public bool KeepAlive { get; set; }
        /// <summary>
        /// 超时限制 - 毫秒为单位（即：1000等于1分钟）
        /// </summary>
        public int Timeout { get; set; }
        /// <summary>
        /// Cookie对象
        /// </summary>
        public HttpCookieType Cookie { get; set; }
        /// <summary>
        /// 启用代理 - true：是，false：否
        /// </summary>
        public bool IsProxy { get; set; }
        /// <summary>
        /// 代理ip地址，当IsProxy为true时 有效
        /// </summary>
        public string ProxyIp { get; set; }
        #endregion

        #region 请求体 - 参数
        /// <summary>
        /// 请求体 - 字典
        /// </summary>
        public IDictionary<string, object> Parameters { get; set; }
        /// <summary>
        /// 请求体 - 字符串
        /// </summary>
        public string PostData { get; set; }
        #endregion

        #region 其他 - 相关
        /// <summary>
        /// 内容 读取 编码
        /// </summary>
        public Encoding Encoding { get; set; }
        /// <summary>
        /// 响应内容 操作方式
        /// </summary>
        public HttpResponseEnum ResponseEnum { get; set; }
        /// <summary>
        /// 流操作的 委托 - 如果需要
        /// </summary>
        public Action<Stream> StreamAction { get; set; }
        #endregion
    }
}

