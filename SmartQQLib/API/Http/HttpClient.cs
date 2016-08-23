using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace SmartQQLib.API.Http
{

    public class HttpClient
    {
        /// <summary>
        /// 访问服务器时的cookies
        /// </summary>
        private CookieContainer mCookiesContainer;
        /// <summary>
        /// 向服务器发送get请求  返回服务器回复数据
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>       
        public byte[] GETbyte(string url, string referer)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.UserAgent = ApiUrl.UserAgent;
                request.ContentType = ApiUrl.ContentType;
                request.Referer = referer;
                request.Method = "get";

                if (mCookiesContainer == null)
                {
                    mCookiesContainer = new CookieContainer();
                }

                request.CookieContainer = mCookiesContainer;  //启用cookie

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();


                Stream response_stream = response.GetResponseStream();

                int count = (int)response.ContentLength;
                int offset = 0;
                byte[] buf = new byte[count];
                while (count > 0)  //读取返回数据
                {
                    int n = response_stream.Read(buf, offset, count);
                    if (n == 0) break;
                    count -= n;
                    offset += n;
                }
                return buf;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
                return new byte[] { 0 };
            }
        }

        public string GET(string url, string referer)
        {
            try
            {

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.UserAgent = ApiUrl.UserAgent;
            request.ContentType = ApiUrl.ContentType;
            request.Referer = referer;
            request.Method = "get";

            if (mCookiesContainer == null)
            {
                mCookiesContainer = new CookieContainer();
            }

            request.CookieContainer = mCookiesContainer;  //启用cookie

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            if (response.StatusCode== HttpStatusCode.NotFound)
            {                
                response = (HttpWebResponse)request.GetResponse();

            }
            //MessageBox.Show(response);
            StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
            return sr.ReadToEnd();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
                return "";
            }

        }

        public string GET_UTF8String(string url, string referer)
        {
            byte[] bytes = this.GETbyte(url, referer);
            string utf8str = Encoding.UTF8.GetString(bytes);
            return utf8str;
        }






        /// <summary>
        /// 向服务器发送post请求 返回服务器回复数据
        /// </summary>
        /// <param name="url"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        public byte[] POST(string url, string referer, string body)
        {
            try
            {
                byte[] request_body = Encoding.UTF8.GetBytes(body);
                

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.UserAgent = ApiUrl.UserAgent;
                request.ContentType = ApiUrl.ContentType;
                request.Referer = referer;
                request.Method = "post";
                request.ContentLength = request_body.Length;

                Stream request_stream = request.GetRequestStream();

                request_stream.Write(request_body, 0, request_body.Length);

                if (mCookiesContainer == null)
                {
                    mCookiesContainer = new CookieContainer();
                }
                request.CookieContainer = mCookiesContainer;  //启用cookie
                
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream response_stream = response.GetResponseStream();

                int count = (int)response.ContentLength;
                int offset = 0;
                byte[] buf = new byte[count];
                while (count > 0)  //读取返回数据
                {
                    int n = response_stream.Read(buf, offset, count);
                    if (n == 0) break;
                    count -= n;
                    offset += n;
                }
                return buf;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
                return new byte[] { 0 };
            }
        }

        public string POST_UTF8String(string url, string referer, string body)
        {
            byte[] bytes = this.POST(url, referer, body);
            string utf8str = Encoding.UTF8.GetString(bytes);
            return utf8str;
        }

        /// <summary>
        /// 获取指定cookie
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetCookie(string name)
        {
            List<Cookie> cookies = GetAllCookies(mCookiesContainer);
            foreach (Cookie c in cookies)
            {
                if (c.Name == name)
                {
                    return (c.ToString()).Remove(0, name.Length + 1);
                }
            }
            return null;
        }

        private static List<Cookie> GetAllCookies(CookieContainer cc)
        {
            List<Cookie> lstCookies = new List<Cookie>();

            Hashtable table = (Hashtable)cc.GetType().InvokeMember("m_domainTable",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.GetField |
                System.Reflection.BindingFlags.Instance, null, cc, new object[] { });

            foreach (object pathList in table.Values)
            {
                SortedList lstCookieCol = (SortedList)pathList.GetType().InvokeMember("m_list",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.GetField
                    | System.Reflection.BindingFlags.Instance, null, pathList, new object[] { });
                foreach (CookieCollection colCookies in lstCookieCol.Values)
                    foreach (Cookie c in colCookies) lstCookies.Add(c);
            }
            return lstCookies;
        }

        public string UrlEncode(string str)
        {
            StringBuilder sb = new StringBuilder();
            byte[] byStr = System.Text.Encoding.UTF8.GetBytes(str); //默认是System.Text.Encoding.Default.GetBytes(str)
            for (int i = 0; i < byStr.Length; i++)
            {
                sb.Append(@"%" + Convert.ToString(byStr[i], 16));
            }

            return (sb.ToString());
        }
    }
}
