using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartQQLib.API
{
    public class AuthResult
    {
        /// <summary>
        /// 65: 已经失效
        /// 66: 等待扫描
        /// </summary>
        public int code;
        /// <summary>
        /// 二维码状态
        /// </summary>
        public string authStatus;
        /// <summary>
        /// 0：失效
        /// 1：等待扫描
        /// 2：等待认证
        /// 3：成功
        /// </summary>
        public int StatusCode;
        /// <summary>
        /// 登录重定向Url
        /// </summary>
        public string redirect_uri;
    }
}
