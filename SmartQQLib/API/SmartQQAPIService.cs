
using SmartQQLib.API.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        /// 获得登录二维码
        /// </summary>
        /// <param name="GetQRCodeImage"></param>
        /// <returns></returns>
        public Image GetQRCodeImage()
        {
            return http.GetImage(ApiUrl.Get_QrCode, "");
           

        }
        /// <summary>
        /// 获得二维码验证结果
        /// </summary>
        /// <param name="GetQRCodeImage"></param>
        /// <returns></returns>
        public string GetAuthStatus()
        {

            string result = http.GET(ApiUrl.Verify_QrCode[0], ApiUrl.Verify_QrCode[1]);
            Debug.Write(result);
            
            http.ListCookie();

            return result;
        }

        /// <summary>
        /// 获得ptwebqq
        /// </summary>
        /// <param name="AuthRedirect"></param>
        /// <returns></returns>
        public string GetPtwebqq(string redirect_uri)
        {
            try
            {
                http.GET(redirect_uri, ApiUrl.Verify_QrCode[1]);

                string result = http.GetCookie("ptwebqq").ToString();
                Debug.Write(result);
                
                http.ListCookie();

                return result;
            }
            catch (Exception e)
            {

                Debug.WriteLine(e);
            }
            return null;

        }

        /// <summary>
        /// 获得VFwebqq
        /// </summary>
        /// <param name="GetVFwebqq"></param>
        /// <returns></returns>
        public string GetVfwebqq(string ptwebqq)
        {
            try
            {
                string GetVfwebqq_URL = ApiUrl.Get_VFwebqq[0] + ptwebqq + ApiUrl.Get_VFwebqq[1];
                string result = http.GET(GetVfwebqq_URL, ApiUrl.Get_VFwebqq[2]);

                
                http.ListCookie();
                return result;

            }
            catch (Exception e)
            {

                Debug.WriteLine(e);
            }
            return null;

        }

        public string getUinAndPsessionid(string ptwebqq)
        {
            List<string> result = new List<string>();

            string loginstr = "{\"ptwebqq\":\""+ ptwebqq + "\",\"clientid\": 53999199, \"psessionid\": \"\",\"status\": \"online\"}";
            Debug.Write(loginstr);

            IDictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("r", loginstr);

            try
            {
                var content = http.POST(ApiUrl.Get_Uin_And_Psessionid[0], ApiUrl.Get_Uin_And_Psessionid[1], "", parameters);
                http.ListCookie();
                Debug.Write("显示返回内容");
                Debug.Write(content);

                return content;
            }
            catch (Exception e)
            {

                Debug.WriteLine(e);
            }
            return null;

        }

        public void CookieProxy(string p_skey, string p_uin)
        {
            http.AddCookie("p_skey", p_skey, "/", "w.qq.com");
            http.AddCookie("p_uin", p_uin, "/", "w.qq.com");
            
            http.ListCookie();
        }

        public List<string> GetCookies(string[] namelist)
        {
            List<string> result = new List<string>();
            try
            {
                foreach (var name in namelist)
                {
                    result.Add(http.GetCookie(name));
                }
                return result;

            }
            catch (Exception e)
            {

                Debug.WriteLine(e);
            }
            return null;

        }


        public Image GetUserLogo(int qqnum)
        {
            string url = "http://q1.qlogo.cn/g?b=qq&nk="+ qqnum+ "&s=5";
            
            return http.GetImage(url, "");
        }

    }
}
