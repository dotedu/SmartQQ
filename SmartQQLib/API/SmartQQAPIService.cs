using SmartQQLib.API.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartQQLib.API.Tool;
using Newtonsoft.Json;

namespace SmartQQLib.API
{
    internal class SmartQQAPIService
    {
        private static HttpClient http;

        internal SmartQQAPIService(HttpClient httpClient)
        {
            http = httpClient;

    }

        private static UniversalTool tool = new UniversalTool();

        internal long TimeStampGetQR;

        public static Random rd = new Random();
        double Random_DT = rd.NextDouble();
        int Random_T = rd.Next();

        /// <summary>
        /// 获得登录二维码
        /// </summary>
        /// <param name="GetQRCodeImage"></param>
        /// <returns></returns>
        internal Image GetQRCodeImage()
        {
            string url = "https://ssl.ptlogin2.qq.com/ptqrshow";

            IDictionary<string, object> getParam = new Dictionary<string, object>();

            getParam.Add("appid", 501004106);
            getParam.Add("e", 0);
            getParam.Add("l", 'M');
            getParam.Add("s", 5);
            getParam.Add("d", 72);
            getParam.Add("v", 4);
            getParam.Add("t", Random_DT);
            TimeStampGetQR = tool.GetTimeStamp(DateTime.Now);

            return http.GetImage(url, null, getParam);
           
        }


        /// <summary>
        /// 获得二维码验证结果
        /// </summary>
        /// <param name="GetQRCodeImage"></param>
        /// <returns></returns>
        internal string GetAuthStatus()
        {
            string query_string_ul = "http%3A%2F%2Fw.qq.com%2Fproxy.html%3Flogin2qq%3D1%26webqq_type%3D10";

            object query_string_action = "0 - 0 -"+ (tool.GetTimeStamp(DateTime.Now) - TimeStampGetQR);

            string url = "https://ssl.ptlogin2.qq.com/ptqrlogin";

            IDictionary<string, object> getParam = new Dictionary<string, object>();

            getParam.Add("webqq_type", 10);
            getParam.Add("remember_uin", 1);
            getParam.Add("login2qq", 1);
            getParam.Add("aid", Base.g_appid);
            getParam.Add("u1", query_string_ul);
            getParam.Add("h", 1);
            getParam.Add("ptredirect", 0);
            getParam.Add("ptlang", 2052);
            getParam.Add("daid", Base.g_daid);
            getParam.Add("from_ui", 1);
            getParam.Add("pttype", 1);
            getParam.Add("dumy", "");
            getParam.Add("fp", "loginerroralert");
            getParam.Add("action", query_string_action);
            getParam.Add("mibao_css", Base.g_mibao_css);
            getParam.Add("t", 1);
            getParam.Add("g", 1);
            getParam.Add("js_type", 0);
            getParam.Add("js_ver", Base.g_pt_version);
            getParam.Add("login_sig", "");
            getParam.Add("pt_randsalt", Base.isRandSalt);
            //Debug.Write(url);

            string referer = "https://ui.ptlogin2.qq.com/cgi-bin/login?daid=164&target=self&style=16&mibao_css=m_webqq&appid=501004106&enable_qlogin=0&no_verifyimg=1 &s_url=http%3A%2F%2Fw.qq.com%2Fproxy.html&f_url=loginerroralert &strong_login=1&login_state=10&t=20131024001";

            string result = http.GET(url,referer, getParam);
            Debug.Write(result);
            
            http.ListCookie();

            return result;
        }

        /// <summary>
        /// 获得ptwebqq
        /// </summary>
        /// <param name="AuthRedirect"></param>
        /// <returns></returns>
        internal string GetPtwebqq(string redirect_url)
        {
            string url = redirect_url;

            string referer = "http://s.web2.qq.com/proxy.html?v=20130916001&callback=1&id=1";


            try
            {
                http.GET(url, referer, null);

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
        internal string GetVfwebqq(string ptwebqq)
        {
            string url = "http://s.web2.qq.com/api/getvfwebqq";
            

            string referer = "http://s.web2.qq.com/proxy.html?v=20130916001&callback=1&id=1";

            IDictionary<string, object> getParam = new Dictionary<string, object>();

            getParam.Add("ptwebqq", ptwebqq);
            getParam.Add("clientid",Base.clientid);
            getParam.Add("psessionid", "");
            getParam.Add("t", tool.GetTimeStamp(DateTime.Now));


            try
            {
                string result = http.GET(url, referer, getParam);

                
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
        /// 获取UIN和Psessionid
        /// </summary>
        /// <param name="ptwebqq"></param>
        /// <returns></returns>
        internal string getUinAndPsessionid(string ptwebqq)
        {

            string url = "http://d1.web2.qq.com/channel/login2";


            string referer = "http://d1.web2.qq.com/proxy.html?v=20151105001&callback=1&id=2";



            string PostJson = "{{\"ptwebqq\":\"{0}\",\"clientid\":{1}, \"psessionid\": \"\",\"status\": \"{2}\"}}";
            PostJson = string.Format(PostJson, ptwebqq, Base.clientid, Base.status);

            Debug.Write(PostJson);

            IDictionary<string, string> postdata = new Dictionary<string, string>();
            postdata.Add("r", PostJson);

            try
            {
                var content = http.POST(url, referer, "", postdata);
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
 
        
        /// <summary>
        /// 从cookie文件登录
        /// </summary>
        /// <param name="ptwebqq"></param>
        /// <param name="status"></param>
        /// <param name="skey"></param>
        /// <param name="uin"></param>
        /// <param name="p_skey"></param>
        /// <param name="p_uin"></param>
        /// <returns></returns>
        internal string ReLoginByCookie(string ptwebqq, string status, string skey, string uin, string p_skey, string p_uin)
        {
            http.AddCookie("ptwebqq", ptwebqq, "/", "qq.com");
            http.AddCookie("skey", skey, "/", "qq.com");
            http.AddCookie("uin", uin, "/", "qq.com");
            http.AddCookie("p_skey", p_skey, "/", "web2.qq.com");
            http.AddCookie("p_uin", p_uin, "/", "web2.qq.com");


            string url = "http://d1.web2.qq.com/channel/login2";


            string referer = "http://d1.web2.qq.com/proxy.html?v=20151105001&callback=1&id=2";






            string PostJson = "{{\"ptwebqq\":\"{0}\",\"clientid\":{1}, \"psessionid\": \"\",\"status\": \"{2}\"}}";
            PostJson = string.Format(PostJson, ptwebqq, Base.clientid, status);

            Debug.Write(PostJson);

            IDictionary<string, string> postdata = new Dictionary<string, string>();
            postdata.Add("r", PostJson);

            try
            {
                Debug.Write("尝试重新登录");
                var content = http.POST(url, referer, "", postdata);

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

        /// <summary>
        /// 修改登录状态
        /// </summary>
        /// <param name="state"></param>
        /// <param name="psessionid"></param>
        internal void Change_State(string state, string psessionid)
        {
            string url = "http://s.web2.qq.com/api/get_self_info2";


            string referer = "http://s.web2.qq.com/proxy.html?v=20130916001&callback=1&id=1";


            IDictionary<string, object> getParam = new Dictionary<string, object>();

            getParam.Add("newstatus", state);
            getParam.Add("clientid", Base.clientid);
            getParam.Add("psessionid", psessionid);
            getParam.Add("t", Random_DT);

            try
            {
                string result = http.GET(url, referer, getParam);


                http.ListCookie();

            }
            catch (Exception e)
            {

                Debug.WriteLine(e);
            }
        }

        /// <summary>
        /// 获取本人信息
        /// </summary>
        /// <returns></returns>
        internal string GetUserInfo()
        {
            string url = "http://s.web2.qq.com/api/get_self_info2";


            string referer = "http://s.web2.qq.com/proxy.html?v=20130916001&callback=1&id=1";

            try
            {
                string result = http.GET(url, referer, null);


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
        /// 接收消息
        /// </summary>
        /// <param name="ptwebqq"></param>
        /// <param name="psessionid"></param>
        /// <returns></returns>
        internal string recv_message(string ptwebqq, string psessionid)
        {
            string url = "http://d1.web2.qq.com/channel/poll2";


            string referer = "http://d1.web2.qq.com/proxy.html?v=20151105001&callback=1&id=2";


            
            string PostJson = "{{ \"ptwebqq\":\"{0}\",\"clientid\":{1},\"psessionid\":\"{2}\",\"key\":\"\"}}";
            PostJson = string.Format(PostJson, ptwebqq, Base.clientid, psessionid);

            Debug.Write(PostJson);

            IDictionary<string, string> postdata = new Dictionary<string, string>();
            postdata.Add("r", PostJson);

            try
            {
                var content = http.POST(url, referer, "", postdata);
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

    
        //----------------------发送消息--------



        //--------   ----待添加-------------------





        /// <summary>
        /// 获取QQ头像
        /// </summary>
        /// <param name="qqnum"></param>
        /// <returns></returns>
        public Image GetUserLogo(int qqnum)
        {
            string url = "http://q1.qlogo.cn/g";


            IDictionary<string, object> getParam = new Dictionary<string, object>();

            getParam.Add("b", "qq");
            getParam.Add("nk", qqnum);
            getParam.Add("s", 5);

            return http.GetImage(url, "", getParam);
        }


        /// <summary>
        /// 清除cookie
        /// </summary>
        internal void ClearCookies()
        {
            http.HttpHelp();
        }


        /// <summary>
        /// 设置cookie共享
        /// </summary>
        /// <param name="p_skey"></param>
        /// <param name="p_uin"></param>
        internal void CookieProxy(string p_skey, string p_uin)
        {
            http.AddCookie("p_skey", p_skey, "/", "w.qq.com");
            http.AddCookie("p_uin", p_uin, "/", "w.qq.com");
            http.AddCookie("p_skey", p_skey, "/", "qun.qq.com");
            http.AddCookie("p_uin", p_uin, "/", "qun.qq.com");

            http.ListCookie();
        }


        /// <summary>
        /// 获取指定cookies
        /// </summary>
        /// <param name="namelist"></param>
        /// <returns></returns>
        internal List<string> GetCookies(string[] namelist)
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




        internal string ReadTextFile(string path, string file)
        {
            if (File.Exists(Path.Combine(path, file)))
            {
                string result = File.ReadAllText(Path.Combine(path, file));
                return result;
            }
            else
            {
                Debug.Write(Path.Combine(path, file));
                Debug.Write("文件不存在");

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                return null;
            }
        }

    }
}
