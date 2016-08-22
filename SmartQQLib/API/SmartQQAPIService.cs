using SmartQQLib.API.Http;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartQQLib.API
{
    public class SmartQQAPIService
    {
        static HttpClient http;
        public SmartQQAPIService(HttpClient httpClient)
        {
            http = httpClient;

        }

        /// <summary>
        /// 获得登录二维码URL
        /// </summary>
        /// <param name="QRLoginSessionID"></param>
        /// <returns></returns>
        public Image GetQRCodeImage()
        {
            string url = ApiUrl.Get_QrCode;
            var bytes = http.GET(url, "");
            return Image.FromStream(new MemoryStream(bytes));
        }

        public string GetAuthStatus()
        {

            byte[] bytes = http.GET(ApiUrl.Verify_QrCode[0], ApiUrl.Verify_QrCode[1]);
            string result = Encoding.UTF8.GetString(bytes);
           
            return result;
        }

        public User GetCurrentQQ()
        {

            User user = new User();
            user.qqNum = Convert.ToInt32((http.GetCookie("pac_uid").ToString()).Substring(2));
            return user;
        }

        public AuthRedirectResult AuthRedirect(string redirect_uri)
        {
            byte[] bytes = http.GET(redirect_uri, ApiUrl.Verify_QrCode[1]);
            string rep = Encoding.UTF8.GetString(bytes);

            AuthRedirectResult result = new AuthRedirectResult();
            result.ptwebqq = http.GetCookie("ptwebqq").ToString();
            return result;
        }
        public Image GetUserLogo(int qqnum)
        {
            string url = "http://q1.qlogo.cn/g?b=qq&nk="+ qqnum+ "&s=5";
            var bytes = http.GET(url, "");
            return Image.FromStream(new MemoryStream(bytes));
        }

    }
}
