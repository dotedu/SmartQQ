using System.Net;


namespace SmartQQLib.Http
{
    /// <summary>
    /// http请求 cookie 存储类
    /// </summary>
    public class HttpCookieType
    {
        /// <summary>
        /// cookie集合
        /// </summary>
        public CookieCollection CookieCollection { get; set; }
        /// <summary>
        /// Cookie字符串
        /// </summary>
        public string CookieString { get; set; }
    }
}
