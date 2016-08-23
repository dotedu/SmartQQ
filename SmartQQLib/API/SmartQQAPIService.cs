using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using SmartQQLib.API.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            string url = ApiUrl.Get_QrCode;
            var bytes = http.GETbyte(url, "");
            return Image.FromStream(new MemoryStream(bytes));

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

            return result;
        }



        public User GetCurrentQQ()
        {

            User user = new User();
            user.qqNum = Convert.ToInt32((http.GetCookie("superuin").ToString()).Substring(2));
            return user;
        }
        /// <summary>
        /// 获得ptwebqq
        /// </summary>
        /// <param name="AuthRedirect"></param>
        /// <returns></returns>
        public void GetPtwebqq(string redirect_uri)
        {
            string rep = http.GET(redirect_uri, ApiUrl.Verify_QrCode[1]);

            Cookies.ptwebqq = http.GetCookie("ptwebqq").ToString();
            Debug.Write(Cookies.ptwebqq);

            Cookies.skey = http.GetCookie("skey").ToString();
            Debug.Write(Cookies.skey);
            //return http.GetCookie("ptwebqq").ToString();
        }

        /// <summary>
        /// 获得VFwebqq
        /// </summary>
        /// <param name="GetVFwebqq"></param>
        /// <returns></returns>
        public string GetVfwebqq(string ptwebqq)
        {
            string GetVfwebqq_URL = ApiUrl.Get_VFwebqq[0] + ptwebqq + ApiUrl.Get_VFwebqq[1];
            string result = http.GET(GetVfwebqq_URL, ApiUrl.Get_VFwebqq[2]);
            JObject JoGetDate = (JObject)JsonConvert.DeserializeObject(result);
            string strVFwebqq = JoGetDate["result"]["vfwebqq"].ToString();
            Debug.Write(strVFwebqq);
            return strVFwebqq;
        }

        public void getUinAndPsessionid(string ptwebqq)
        {

            //return strVFwebqq;
            string Poststr = "r=%7B%22ptwebqq%22%3A%22" + ptwebqq + "%22%2C%22clientid%22%3A%2053999199%2C%20%22psessionid%22%3A%20%22%22%2C%22status%22%3A%20%22online%22%7D";
            //MessageBox.Show(ptwebqq);
            Debug.Write(Poststr);

            var client = new RestClient(ApiUrl.Get_Uin_And_Psessionid[0]);
            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            request.AddHeader("postman-token", "6a4c961b-3572-4e8f-f8f6-6f8896b0a069");
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("referer", ApiUrl.Get_Uin_And_Psessionid[1]);
            request.AddParameter("application/x-www-form-urlencoded", Poststr, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            var content = response.Content;
            MessageBox.Show(content);

            Debug.Write(content);
            JObject JoGetDate = (JObject)JsonConvert.DeserializeObject(content);
            Cookies.psessionid = JoGetDate["result"]["psessionid"].ToString();
            Cookies.uin =Convert.ToInt32(JoGetDate["result"]["uin"]);
        }


        public Image GetUserLogo(int qqnum)
        {
            string url = "http://q1.qlogo.cn/g?b=qq&nk="+ qqnum+ "&s=5";
            var bytes = http.GETbyte(url, "");
            return Image.FromStream(new MemoryStream(bytes));
        }

    }
}
