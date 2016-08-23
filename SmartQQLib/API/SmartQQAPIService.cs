using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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

            return result;
        }

        /// <summary>
        /// 获得ptwebqq
        /// </summary>
        /// <param name="AuthRedirect"></param>
        /// <returns></returns>
        public string GetPtwebqq(string redirect_uri)
        {
            http.GET(redirect_uri, ApiUrl.Verify_QrCode[1]);

            string ptwebqq  = http.GetCookie("ptwebqq").ToString();

            return ptwebqq;

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

        public List<object> getUinAndPsessionid(string ptwebqq)
        {
            List<object> result = new List<object>();

            string loginstr = "{\"ptwebqq\":\""+ ptwebqq + "\",\"clientid\": 53999199, \"psessionid\": \"\",\"status\": \"online\"}";
            Debug.Write(loginstr);

            IDictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("r", loginstr);

            try
            {
                var content = http.POST(ApiUrl.Get_Uin_And_Psessionid[0], ApiUrl.Get_Uin_And_Psessionid[1], "", parameters);

                Debug.Write(content);

                JObject JoGetDate = (JObject)JsonConvert.DeserializeObject(content);


                result[1] = JoGetDate["result"]["psessionid"].ToString();
                Debug.Write(result[0]);
                result[2] = Convert.ToInt32(JoGetDate["result"]["uin"]);
                Debug.Write(result[1]);
                result[0] = http.GetCookie("skey").ToString();
                Debug.Write(result[2]);
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
