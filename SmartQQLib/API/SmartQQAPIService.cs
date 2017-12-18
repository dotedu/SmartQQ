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
using SmartQQLib.Http;
using System.Net;
using System.Web;

namespace SmartQQLib.API
{
    internal class SmartQQAPIService
    {

        private static HttpProvider http;

        internal SmartQQAPIService(HttpProvider httpProvider)
        {
            http = httpProvider;

        }
        string qrsig;

        private HttpCookieType mCookieType = new HttpCookieType();

        private CookieCollection mCookieCollection = new CookieCollection();

        private static UniversalTool tool = new UniversalTool();

        internal long TimeStampGetQR;

        private static Random rd = new Random();
        double Random_DT = rd.NextDouble();
        int Random_T = rd.Next();

        private Image grcode;
        private Image UserFace;

        /// <summary>
        /// 获得登录二维码
        /// </summary>
        /// <param name="GetQRCodeImage"></param>
        /// <returns></returns>
        internal Image _get_qrcode_image()
        {

            HttpRequestParameter rp = new HttpRequestParameter();
            mCookieType.CookieCollection = mCookieCollection;
            IDictionary<string, object> getParam = new Dictionary<string, object>();
            getParam.Add("appid", 501004106);
            getParam.Add("e", 0);
            getParam.Add("l", 'M');
            getParam.Add("s", 5);
            getParam.Add("d", 72);
            getParam.Add("v", 4);
            getParam.Add("t", Random_DT);
            TimeStampGetQR = tool.GetTimeStamp(DateTime.Now);

            rp.Url = "https://ssl.ptlogin2.qq.com/ptqrshow";
            rp.Parameters = getParam;
            rp.Cookie = mCookieType;

            rp.ResponseEnum = HttpResponseEnum.Stream; // 说明服务端 响应的为 流
            rp.StreamAction = x => // x 代表响应 流 - stream
            {
                grcode = Image.FromStream(x);
            };
            try
            {
                HttpResponseParameter result = HttpProvider.Execute(rp);

                qrsig = http.GetCookie("qrsig");
                return grcode;
            }
            catch (Exception e)
            {

                Debug.WriteLine(e);
            }
            return null;

        }

        private static int hash33(string s)
        {
            int e = 0, n = s.Length;
            for (int i = 0; n > i; ++i) { 
            var s1 = e << 5;
                char s2 = Convert.ToChar(s.Substring(i, 1));
            var s3 = (int)(s2);
                e += s1+s3;
            }
            return 2147483647 & e;
        }

        /// <summary>
        /// 获得二维码验证结果
        /// </summary>
        /// <returns></returns>
        internal string _get_authstatus()
        {

            HttpRequestParameter rp = new HttpRequestParameter();
            mCookieType.CookieCollection = mCookieCollection;

            string query_string_ul = "https%3A%2F%2Fw.qq.com%2Fproxy.html%3Flogin2qq%3D1%26webqq_type%3D10&";

            object query_string_action = "0-0-"+ (tool.GetTimeStamp(DateTime.Now) - TimeStampGetQR);

            IDictionary<string, object> getParam = new Dictionary<string, object>();
            
            getParam.Add("ptqrtoken", hash33(qrsig));
            getParam.Add("webqq_type", 10);
            getParam.Add("remember_uin", 1);
            getParam.Add("login2qq", 1);
            getParam.Add("aid", 501004106);
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

            rp.Url = "https://ssl.ptlogin2.qq.com/ptqrlogin?" +
                                "ptqrtoken="+ hash33(qrsig) + "&webqq_type=10&remember_uin=1&login2qq=1&aid=501004106&" +
                                "u1=https%3A%2F%2Fw.qq.com%2Fproxy.html%3Flogin2qq%3D1%26webqq_type%3D10&" +
                                "ptredirect=0&ptlang=2052&daid=164&from_ui=1&pttype=1&dumy=&fp=loginerroralert&0-0-157510&" +
                                "mibao_css=m_webqq&t=undefined&g=1&js_type=0&js_ver=10184&login_sig=&pt_randsalt=3";
            //Debug.Write(url);
            


            //rp.Parameters = getParam;
            rp.Referer = "https://ui.ptlogin2.qq.com/cgi-bin/login?" +
                    "daid=164&target=self&style=16&mibao_css=m_webqq&appid=501004106&enable_qlogin=0&no_verifyimg=1&" +
                    "s_url=https%3A%2F%2Fw.qq.com%2Fproxy.html&f_url=loginerroralert&strong_login=1&login_state=10&t=20131024001";
            rp.Cookie = mCookieType;

            try
            {
                HttpResponseParameter result = HttpProvider.Execute(rp);
                Debug.Write(result.Body);
                return result.Body;
            }
            catch (Exception e)
            {

                Debug.WriteLine(e);
            }
            return "";
        }

        /// <summary>
        /// 获得ptwebqq
        /// </summary>
        /// <param name="redirect_url"></param>
        /// <returns></returns>
        internal string _get_ptwebqq(string redirect_url)
        {
            HttpRequestParameter rp = new HttpRequestParameter();

            rp.Url = redirect_url;
            rp.Referer = "http://s.web2.qq.com/proxy.html?v=20130916001&callback=1&id=1";
            rp.Cookie = mCookieType;
            mCookieType.CookieCollection = mCookieCollection;

            try
            {
                HttpResponseParameter result = HttpProvider.Execute(rp);
                //Cookie[] cookielist = new Cookie[];
                Debug.WriteLine(http.GetCookie("ptwebqq"));
                Debug.WriteLine(result.Cookie.CookieString);
                return http.GetCookie("ptwebqq");
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
        /// <param name="ptwebqq"></param>
        /// <returns></returns>
        internal string _get_vfwebqq(string ptwebqq)
        {
            HttpRequestParameter rp = new HttpRequestParameter();
            mCookieType.CookieCollection = mCookieCollection;

            IDictionary<string, object> getParam = new Dictionary<string, object>();

            getParam.Add("ptwebqq", ptwebqq);
            getParam.Add("clientid",Base.clientid);
            getParam.Add("psessionid", "");
            getParam.Add("t", tool.GetTimeStamp(DateTime.Now));

            rp.Url = "http://s.web2.qq.com/api/getvfwebqq";
            rp.Referer = "http://s.web2.qq.com/proxy.html?v=20130916001&callback=1&id=1";

            rp.Parameters = getParam;
            rp.Cookie = mCookieType;

            try
            {
                HttpResponseParameter result = HttpProvider.Execute(rp);
                Debug.WriteLine("显示vfwebqq结果");
                Debug.WriteLine(result.Body);
                return result.Body;
            }
            catch (Exception e)
            {

                Debug.WriteLine(e);
            }
            return "";

        }
        /// <summary>
        /// 获取UIN和Psessionid
        /// </summary>
        /// <param name="ptwebqq"></param>
        /// <returns></returns>
        internal string _login(string ptwebqq)
        {
            HttpRequestParameter rp = new HttpRequestParameter();
            mCookieType.CookieCollection = mCookieCollection;

            string PostJson = "{{\"ptwebqq\":\"{0}\",\"clientid\":{1}, \"psessionid\": \"\",\"status\": \"{2}\"}}";
            PostJson = string.Format(PostJson, ptwebqq, Base.clientid, Base.status);

            Debug.Write(PostJson);

            IDictionary<string, object> postdata = new Dictionary<string, object>();
            postdata.Add("r", PostJson);

            rp.Url = "http://d1.web2.qq.com/channel/login2";
            rp.Referer = "http://d1.web2.qq.com/proxy.html?v=20151105001&callback=1&id=2";
            rp.Method = HttpMethodEnum.Post;
            rp.Parameters = postdata;
            rp.Cookie = mCookieType;

            try
            {
                HttpResponseParameter result = HttpProvider.Execute(rp);
                Debug.Write("显示返回内容");
                Debug.Write(result.Body);
                return result.Body;

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
        internal string _login_by_cookie(string ptwebqq, string status, string skey, string uin, string p_skey, string p_uin)
        {
            mCookieCollection.Add(new Cookie("ptwebqq", ptwebqq, "/", "qq.com"));
            mCookieCollection.Add(new Cookie("skey", skey, "/", "qq.com"));
            mCookieCollection.Add(new Cookie("uin", uin, "/", "qq.com"));
            mCookieCollection.Add(new Cookie("p_skey", p_skey, "/", "web2.qq.com"));
            mCookieCollection.Add(new Cookie("p_uin", p_uin, "/", "web2.qq.com"));
            mCookieType.CookieCollection = mCookieCollection;

            HttpRequestParameter rp = new HttpRequestParameter();

            string PostJson = "{{\"ptwebqq\":\"{0}\",\"clientid\":{1}, \"psessionid\": \"\",\"status\": \"{2}\"}}";
            PostJson = string.Format(PostJson, ptwebqq, Base.clientid, Base.status);

            //Debug.Write(PostJson);

            IDictionary<string, object> postdata = new Dictionary<string, object>();
            postdata.Add("r", PostJson);

            rp.Url = "http://d1.web2.qq.com/channel/login2";
            rp.Referer = "http://d1.web2.qq.com/proxy.html?v=20151105001&callback=1&id=2";
            rp.Method = HttpMethodEnum.Post;
            rp.Parameters = postdata;
            rp.Cookie = mCookieType;

            try
            {
                Debug.Write("尝试重新登录");
                HttpResponseParameter result = HttpProvider.Execute(rp);
                Debug.WriteLine(http.GetCookie("ptwebqq"));

                Debug.Write("显示返回内容");
                Debug.Write(result.Body);
                return result.Body;

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
        internal void _change_state(string state, string psessionid)
        {
            HttpRequestParameter rp = new HttpRequestParameter();
            mCookieType.CookieCollection = mCookieCollection;

            IDictionary<string, object> getParam = new Dictionary<string, object>();

            getParam.Add("newstatus", state);
            getParam.Add("clientid", Base.clientid);
            getParam.Add("psessionid", psessionid);
            getParam.Add("t", Random_DT);

            rp.Url = "http://d1.web2.qq.com/channel/change_status2";
            rp.Referer = "http://s.web2.qq.com/proxy.html?v=20130916001&callback=1&id=1";

            rp.Parameters = getParam;
            rp.Cookie = mCookieType;

            try
            {
                HttpResponseParameter result = HttpProvider.Execute(rp);
                Debug.WriteLine("状态修改为:" + state);
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
        internal string _get_user_info()
        {
            IDictionary<string, object> getParam = new Dictionary<string, object>();

            getParam.Add("t", Random_DT);
            HttpRequestParameter rp = new HttpRequestParameter();
            mCookieType.CookieCollection = mCookieCollection;

            rp.Url = "http://s.web2.qq.com/api/get_self_info2";
            rp.Referer = "http://s.web2.qq.com/proxy.html?v=20130916001&callback=1&id=1";
            rp.Parameters = getParam;
            rp.Cookie = mCookieType;

            try
            {
                HttpResponseParameter result = HttpProvider.Execute(rp);

                return result.Body;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }

            return null;
        }

        /// <summary>
        /// 获取账号主显账户列表
        /// </summary>
        /// <param name="skey"></param>
        /// <param name="type">0：QQ主显账户列表|1：QQ邮箱账号列表</param>
        /// <returns></returns>
        internal string _get_info_acc(string skey, int type)
        {
            IDictionary<string, object> getParam = new Dictionary<string, object>();
            getParam.Add("type", type);
            getParam.Add("bkn", tool.GetBkn(skey).ToString());
            HttpRequestParameter rp = new HttpRequestParameter();
            mCookieType.CookieCollection = mCookieCollection;


            rp.Url = "http://accountadm.qq.com/cgi-bin/info_acc";
            rp.Parameters = getParam;
            rp.Cookie = mCookieType;

            try
            {
                HttpResponseParameter result = HttpProvider.Execute(rp);

                return result.Body;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }

            return null;
        }

        /// <summary>
        /// 获取QQ头像
        /// </summary>
        /// <param name="qqnum"></param>
        /// <returns></returns>
        public Image _get_user_face(int qqnum)
        {

            HttpRequestParameter rp = new HttpRequestParameter();
            mCookieType.CookieCollection = mCookieCollection;
            IDictionary<string, object> getParam = new Dictionary<string, object>();
            getParam.Add("b", "qq");
            getParam.Add("nk", qqnum);
            getParam.Add("s", 5);

            rp.Url = "http://q1.qlogo.cn/g";
            rp.Parameters = getParam;
            rp.Cookie = mCookieType;

            rp.ResponseEnum = HttpResponseEnum.Stream; // 说明服务端 响应的为 流
            rp.StreamAction = x => // x 代表响应 流 - stream
            {
                UserFace = Image.FromStream(x);
            };
            try
            {
                HttpResponseParameter result = HttpProvider.Execute(rp);
                return UserFace;
            }
            catch (Exception e)
            {

                Debug.WriteLine(e);
            }
            return null;
        }
        /// <summary>
        /// 最近消息
        /// </summary>
        /// <param name="vfwebqq"></param>
        /// <param name="psessionid"></param>
        /// <returns></returns>
        internal string _get_recent_info(string vfwebqq, string psessionid)
        {

            HttpRequestParameter rp = new HttpRequestParameter();
            mCookieType.CookieCollection = mCookieCollection;

            string PostJson = PostJson = "{{ \"vfwebqq\":\"{0}\",\"clientid\":{1},\"psessionid\":\"{2}\"}}";
            PostJson = string.Format(PostJson, vfwebqq, Base.clientid, psessionid);

            Debug.Write(PostJson);

            IDictionary<string, object> postdata = new Dictionary<string, object>();
            postdata.Add("r", PostJson);

            rp.Url = "http://d1.web2.qq.com/channel/get_recent_list2";
            rp.Referer = "http://d1.web2.qq.com/proxy.html?v=20151105001&callback=1&id=2";
            rp.Method = HttpMethodEnum.Post;
            rp.Parameters = postdata;
            rp.Cookie = mCookieType;

            try
            {
                HttpResponseParameter result = HttpProvider.Execute(rp);
                Debug.Write("显示返回内容");
                Debug.Write(result.Body);
                return result.Body;

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
        internal string _recv_message(string ptwebqq, string psessionid)
        {
            HttpRequestParameter rp = new HttpRequestParameter();
            mCookieType.CookieCollection = mCookieCollection;

            string PostJson = "{{ \"ptwebqq\":\"{0}\",\"clientid\":{1},\"psessionid\":\"{2}\",\"key\":\"\"}}";
            PostJson = string.Format(PostJson, ptwebqq, Base.clientid, psessionid);
            Debug.Write(PostJson);
            IDictionary<string, object> postdata = new Dictionary<string, object>(); ;
            postdata.Add("r", PostJson);
            rp.Url = "http://d1.web2.qq.com/channel/poll2";
            rp.Referer = "http://d1.web2.qq.com/proxy.html?v=20151105001&callback=1&id=2";
            rp.Method = HttpMethodEnum.Post;
            rp.Parameters = postdata;
            rp.Cookie = mCookieType;

            try
            {
                HttpResponseParameter result = HttpProvider.Execute(rp);
                Debug.Write("显示返回内容");
                Debug.Write(result.Body);
                return result.Body;

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
        ///获取好友列表(SMARTQQ)
        /// </summary>
        /// <param name="qq"></param>
        /// <param name="ptwebqq"></param>
        /// <param name="vfwebqq"></param>
        /// <returns></returns>
        internal string _get_user_friends(long qq, string ptwebqq, string vfwebqq)
        {
            string PostJson = "{{ \"hash\":\"{0}\",\"vfwebqq\":\"{1}\"}}";
            PostJson = string.Format(PostJson, tool.hash(qq, ptwebqq), vfwebqq);
            IDictionary<string, object> postdata = new Dictionary<string, object>(); ;
            postdata.Add("r", PostJson);


            HttpRequestParameter rp = new HttpRequestParameter();
            mCookieType.CookieCollection = mCookieCollection;
            //Debug.Write(PostJson);
            rp.Url = "http://s.web2.qq.com/api/get_user_friends2";
            rp.Referer = "http://s.web2.qq.com/proxy.html?v=20130916001&callback=1&id=1";
            rp.Method = HttpMethodEnum.Post;
            rp.Parameters = postdata;
            rp.Cookie = mCookieType;

            try
            {
                HttpResponseParameter result = HttpProvider.Execute(rp);
                Debug.Write("显示返回内容");
                Debug.Write(result.Body);
                return result.Body;

            }
            catch (Exception e)
            {

                Debug.WriteLine(e);
            }
            return null;

        }

        /// <summary>
        /// 获取QQ好友列表（qun.QQ.COM）
        /// </summary>
        /// <param name="skey"></param>
        /// <returns></returns>
        internal string _get_user_friends_ext(string skey)
        {
            string PostJson = tool.GetBkn(skey).ToString();
            IDictionary<string, object> postdata = new Dictionary<string, object>(); ;
            postdata.Add("kbn", PostJson);

            HttpRequestParameter rp = new HttpRequestParameter();
            mCookieType.CookieCollection = mCookieCollection;
            //Debug.Write(PostJson);
            rp.Url = "http://qun.qq.com/cgi-bin/qun_mgr/get_friend_list";
            rp.Referer = "http://qun.qq.com/member.html";
            rp.Method = HttpMethodEnum.Post;
            rp.Parameters = postdata;
            rp.Cookie = mCookieType;

            try
            {
                HttpResponseParameter result = HttpProvider.Execute(rp);
                Debug.Write("显示返回内容");
                Debug.Write(result.Body);
                return result.Body;

            }
            catch (Exception e)
            {

                Debug.WriteLine(e);
            }
            return null;
        }

        /// <summary>
        /// 获取好友信息
        /// </summary>
        /// <param name="tuin">获取好友列表时得到的uin</param>
        /// <param name="vfwebqq"></param>
        /// <param name="psessionid"></param>
        /// <returns></returns>
        internal string _get_friend_info(string tuin, string vfwebqq, string psessionid)
        {

            IDictionary<string, object> getParam = new Dictionary<string, object>();
            getParam.Add("tuin", tuin);
            getParam.Add("vfwebqq", vfwebqq);
            getParam.Add("clientid", Base.clientid);
            getParam.Add("psessionid", psessionid);
            getParam.Add("t", tool.GetTimeStamp(DateTime.Now));

            HttpRequestParameter rp = new HttpRequestParameter();
            mCookieType.CookieCollection = mCookieCollection;

            rp.Url = "http://s.web2.qq.com/api/get_friend_info2";
            rp.Referer = "http://s.web2.qq.com/proxy.html?v=20130916001&callback=1&id=1";
            rp.Parameters = getParam;
            rp.Cookie = mCookieType;

            try
            {
                HttpResponseParameter result = HttpProvider.Execute(rp);

                return result.Body;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
            return null;
        }


        /// <summary>
        /// 获取在线好友
        /// </summary>
        /// <param name="vfwebqq"></param>
        /// <param name="psessionid"></param>
        /// <returns></returns>
        internal string _get_friends_state(string vfwebqq, string psessionid)
        {
            IDictionary<string, object> getParam = new Dictionary<string, object>();
            getParam.Add("vfwebqq", vfwebqq);
            getParam.Add("clientid", Base.clientid);
            getParam.Add("psessionid", psessionid);
            getParam.Add("t", tool.GetTimeStamp(DateTime.Now));

            HttpRequestParameter rp = new HttpRequestParameter();
            mCookieType.CookieCollection = mCookieCollection;

            rp.Url = "http://d1.web2.qq.com/channel/get_online_buddies2";
            rp.Referer = "http://d1.web2.qq.com/proxy.html?v=20151105001&callback=1&id=2";
            rp.Parameters = getParam;
            rp.Cookie = mCookieType;

            try
            {
                HttpResponseParameter result = HttpProvider.Execute(rp);

                return result.Body;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
            return null;
        }

        /// <summary>
        /// 通过好友列表获取的uin获取好友QQ号
        /// </summary>
        /// <param name="tuin"></param>
        /// <param name="vfwebqq"></param>
        /// <returns></returns>
        internal string _get_qq_from_uin(string tuin, string vfwebqq)
        {
            IDictionary<string, object> getParam = new Dictionary<string, object>();
            getParam.Add("tuin", tuin);
            getParam.Add("type", 1);
            getParam.Add("vfwebqq", vfwebqq);
            getParam.Add("t", tool.GetTimeStamp(DateTime.Now));

            HttpRequestParameter rp = new HttpRequestParameter();
            mCookieType.CookieCollection = mCookieCollection;
            rp.Url = "http://s.web2.qq.com/api/get_friend_uin2";
            rp.Referer = "http://s.web2.qq.com/proxy.html?v=20130916001&callback=1&id=1";
            rp.Parameters = getParam;
            rp.Cookie = mCookieType;

            try
            {
                HttpResponseParameter result = HttpProvider.Execute(rp);

                return result.Body;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
            return null;
        }

        /// <summary>
        /// 个性签名获取
        /// </summary>
        /// <param name="tuin"></param>
        /// <param name="vfwebqq"></param>
        /// <returns></returns>
        internal string _get_single_long_nick(string tuin, string vfwebqq)
        {
            IDictionary<string, object> getParam = new Dictionary<string, object>();
            getParam.Add("tuin", tuin);
            getParam.Add("vfwebqq", vfwebqq);
            getParam.Add("t", tool.GetTimeStamp(DateTime.Now));

            HttpRequestParameter rp = new HttpRequestParameter();
            mCookieType.CookieCollection = mCookieCollection;
            rp.Url = "http://s.web2.qq.com/api/get_single_long_nick2";
            rp.Referer = "http://s.web2.qq.com/proxy.html?v=20130916001&callback=1&id=1";
            rp.Parameters = getParam;
            rp.Cookie = mCookieType;

            try
            {
                HttpResponseParameter result = HttpProvider.Execute(rp);

                return result.Body;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
            return null;
        }

        /// <summary>
        /// 获取群列表（SmartQQ）
        /// </summary>
        /// <param name="tuin"></param>
        /// <param name="ptwebqq"></param>
        /// <returns></returns>
        internal string _get_group_list_info(long qq, string ptwebqq, string vfwebqq)
        {
            string PostJson = "{{ \"hash\":\"{0}\",\"vfwebqq\":\"{1}\"}}";
            PostJson = string.Format(PostJson, tool.hash(qq, ptwebqq), vfwebqq);
            IDictionary<string, object> postdata = new Dictionary<string, object>(); ;
            postdata.Add("r", PostJson);

            HttpRequestParameter rp = new HttpRequestParameter();
            mCookieType.CookieCollection = mCookieCollection;
            rp.Url = "http://s.web2.qq.com/api/get_group_name_list_mask2";
            rp.Referer = "http://s.web2.qq.com/proxy.html?v=20130916001&callback=1&id=1";
            rp.Method = HttpMethodEnum.Post;
            rp.Parameters = postdata;
            rp.Cookie = mCookieType;

            try
            {
                HttpResponseParameter result = HttpProvider.Execute(rp);
                Debug.Write("显示返回内容");
                Debug.Write(result.Body);
                return result.Body;

            }
            catch (Exception e)
            {

                Debug.WriteLine(e);
            }
            return null;
        }

        /// <summary>
        /// 获取群列表（qun.qq.com）
        /// </summary>
        /// <param name="skey"></param>
        /// <returns></returns>
        internal string _get_group_list_info_ext(string skey)
        {
            IDictionary<string, object> postdata = new Dictionary<string, object>(); ;
            postdata.Add("kbn", tool.GetBkn(skey).ToString());

            HttpRequestParameter rp = new HttpRequestParameter();
            mCookieType.CookieCollection = mCookieCollection;
            rp.Url = "http://qun.qq.com/cgi-bin/qun_mgr/get_group_list";
            rp.Referer = "http://qun.qq.com/member.html";
            rp.Method = HttpMethodEnum.Post;
            rp.Parameters = postdata;
            rp.Cookie = mCookieType;

            try
            {
                HttpResponseParameter result = HttpProvider.Execute(rp);
                Debug.Write("显示返回内容");
                Debug.Write(result.Body);
                return result.Body;

            }
            catch (Exception e)
            {

                Debug.WriteLine(e);
            }
            return null;
        }

        /// <summary>
        /// 群信息（smartqq）
        /// </summary>
        /// <param name="gcode"></param>
        /// <param name="vfwebqq"></param>
        /// <returns></returns>
        internal string _get_group_info(string gcode, string vfwebqq)
        {
            string PostJson = "{{ \"gcode\":\"{0}\",\"vfwebqq\":\"{1}\",\"t\":\"{2}\"}}";
            PostJson = string.Format(PostJson, gcode, vfwebqq, tool.GetTimeStamp(DateTime.Now));
            IDictionary<string, object> postdata = new Dictionary<string, object>(); ;
            postdata.Add("r", PostJson);

            HttpRequestParameter rp = new HttpRequestParameter();
            mCookieType.CookieCollection = mCookieCollection;
            rp.Url = "http://s.web2.qq.com/api/get_group_info_ext2";
            rp.Referer = "http://s.web2.qq.com/proxy.html?v=20130916001&callback=1&id=1";
            rp.Method = HttpMethodEnum.Post;
            rp.Parameters = postdata;
            rp.Cookie = mCookieType;

            try
            {
                HttpResponseParameter result = HttpProvider.Execute(rp);
                Debug.Write("显示返回群信息");
                Debug.Write(result.Body);
                return result.Body;

            }
            catch (Exception e)
            {

                Debug.WriteLine(e);
            }
            return null;
        }
        /// <summary>
        /// 群信息（QUN）
        /// </summary>
        /// <param name="gc"></param>
        /// <param name="skey"></param>
        /// <returns></returns>
        internal string _get_group_info_ext(long gnumber, string skey)
        {
            IDictionary<string, object> postdata = new Dictionary<string, object>();
            postdata.Add("gc", gnumber);
            postdata.Add("st", 0);
            postdata.Add("end", 2000);
            postdata.Add("sort", 0);
            postdata.Add("kbn", tool.GetBkn(skey).ToString());

            HttpRequestParameter rp = new HttpRequestParameter();
            mCookieType.CookieCollection = mCookieCollection;
            rp.Url = "http://qun.qq.com/cgi-bin/qun_mgr/search_group_members";
            rp.Referer = "http://qun.qq.com/member.html";
            rp.Method = HttpMethodEnum.Post;
            rp.Parameters = postdata;
            rp.Cookie = mCookieType;

            try
            {
                HttpResponseParameter result = HttpProvider.Execute(rp);
                Debug.Write("显示返回内容");
                Debug.Write(result.Body);
                return result.Body;

            }
            catch (Exception e)
            {

                Debug.WriteLine(e);
            }
            return null;
        }

        /// <summary>
        /// 修改群信息 
        /// </summary>
        /// <param name="gnumber"></param>
        /// <param name="qq"></param>
        /// <param name="skey"></param>
        /// <param name="name"></param>
        /// <param name="classid"></param>
        /// <param name="classtag"></param>
        /// <param name="gintro"></param>
        /// <param name="grintro"></param>
        /// <returns>{"ec":0,"gc":579512715}</returns>
        internal string _set_group_info_new(long gnumber, long qq,string skey, string name, long classid,string classtag,string gintro,string grintro)
        {
            IDictionary<string, object> postdata = new Dictionary<string, object>();
            postdata.Add("src", "qinfo_v3");
            postdata.Add("gc", gnumber);
            postdata.Add("kbn", tool.GetBkn(skey).ToString());
            postdata.Add("classID", classid);
            postdata.Add("class", classtag);
            postdata.Add("fOthers", 1);
            postdata.Add("Name", name);
            postdata.Add("gIntro", gintro);
            postdata.Add("gRIntro", grintro);
            postdata.Add("Remark", 0);
            postdata.Add("nWeb", 1);

           HttpRequestParameter rp = new HttpRequestParameter();
            mCookieType.CookieCollection = mCookieCollection;
            rp.Url = "http://qinfo.clt.qq.com/cgi-bin/qun_info/set_group_info_new";
            rp.Referer = "http://qinfo.clt.qq.com/qinfo_v3/profile.html?groupuin="+ gnumber;
            rp.Method = HttpMethodEnum.Post;
            rp.Parameters = postdata;
            rp.Cookie = mCookieType;
            rp.Origin = "http://qinfo.clt.qq.com";


            try
            {
                HttpResponseParameter result = HttpProvider.Execute(rp);
                Debug.Write("显示返回内容");
                Debug.Write(result.Body);
                return result.Body;

            }
            catch (Exception e)
            {

                Debug.WriteLine(e);
            }
            return null;
        }
        /// <summary>
        /// 获取成员标签
        /// </summary>
        /// <param name="gnumber"></param>
        /// <param name="skey"></param>
        /// <returns></returns>
        internal string _get_member_tag(long gnumber, string skey)
        {
            IDictionary<string, object> getParam = new Dictionary<string, object>();
            getParam.Add("gc", gnumber);
            getParam.Add("kbn", tool.GetBkn(skey).ToString());

            HttpRequestParameter rp = new HttpRequestParameter();
            mCookieType.CookieCollection = mCookieCollection;
            rp.Url = "http://qinfo.clt.qq.com/cgi-bin/mem_tag/get_member_tag";
            rp.Referer = "http://qinfo.clt.qq.com/group_member_tags/index.html?groupuin=" + gnumber;
            rp.Parameters = getParam;
            rp.Cookie = mCookieType;

            try
            {
                HttpResponseParameter result = HttpProvider.Execute(rp);

                return result.Body;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
            return null;
        }
        /// <summary>
        /// 添加群成员标签
        /// </summary>
        /// <param name="gnumber"></param>
        /// <param name="skey"></param>
        /// <param name="tag">标签名</param>
        /// <returns></returns>
        internal string _add_member_tag(long gnumber, string skey,string tag)
        {
            IDictionary<string, object> postdata = new Dictionary<string, object>();
            postdata.Add("gc", gnumber);
            postdata.Add("kbn", tool.GetBkn(skey).ToString());
            postdata.Add("tag", tag);

            HttpRequestParameter rp = new HttpRequestParameter();
            mCookieType.CookieCollection = mCookieCollection;
            rp.Url = "http://qinfo.clt.qq.com/cgi-bin/mem_tag/add_member_tag";
            rp.Referer = "http://qinfo.clt.qq.com/group_member_tags/index.html?groupuin=" + gnumber;
            rp.Method = HttpMethodEnum.Post;
            rp.Parameters = postdata;
            rp.Cookie = mCookieType;
            rp.Origin = "http://qinfo.clt.qq.com";


            try
            {
                HttpResponseParameter result = HttpProvider.Execute(rp);
                Debug.Write("显示返回内容");
                Debug.Write(result.Body);
                return result.Body;

            }
            catch (Exception e)
            {

                Debug.WriteLine(e);
            }
            return null;
        }

        /// <summary>
        /// 删除群成员标签
        /// </summary>
        /// <param name="gnumber"></param>
        /// <param name="skey"></param>
        /// <param name="tag_id">标签ID</param>
        /// <returns></returns>
        internal string _del_member_tag(long gnumber, string skey, int tag_id)
        {
            IDictionary<string, object> postdata = new Dictionary<string, object>();
            postdata.Add("gc", gnumber);
            postdata.Add("kbn", tool.GetBkn(skey).ToString());
            postdata.Add("tag_id", tag_id);

            HttpRequestParameter rp = new HttpRequestParameter();
            mCookieType.CookieCollection = mCookieCollection;
            rp.Url = "http://qinfo.clt.qq.com/cgi-bin/mem_tag/del_member_tag";
            rp.Referer = "http://qinfo.clt.qq.com/group_member_tags/index.html?groupuin=" + gnumber;
            rp.Method = HttpMethodEnum.Post;
            rp.Parameters = postdata;
            rp.Cookie = mCookieType;
            rp.Origin = "http://qinfo.clt.qq.com";


            try
            {
                HttpResponseParameter result = HttpProvider.Execute(rp);
                Debug.Write("显示返回内容");
                Debug.Write(result.Body);
                return result.Body;

            }
            catch (Exception e)
            {

                Debug.WriteLine(e);
            }
            return null;
        }
        /// <summary>
        /// 设置成员标签
        /// </summary>
        /// <param name="qq"></param>
        /// <param name="gnumber"></param>
        /// <param name="skey"></param>
        /// <param name="tag_id">-1：取消标签</param>
        /// <returns></returns>
        internal string _set_group_mem_tag(long qq, long gnumber, string skey, int tag_id)
        {
            IDictionary<string, object> postdata = new Dictionary<string, object>();
            postdata.Add("uin_list", qq);
            postdata.Add("tag_id", tag_id);
            postdata.Add("gc", gnumber);
            postdata.Add("kbn", tool.GetBkn(skey).ToString());
            postdata.Add("src", "qinfo_v3");

            HttpRequestParameter rp = new HttpRequestParameter();
            mCookieType.CookieCollection = mCookieCollection;
            rp.Url = "http://qinfo.clt.qq.com/cgi-bin/qun_info/set_group_mem_tag";
            rp.Referer = "http://qinfo.clt.qq.com/qinfo_v3/member.html";
            rp.Method = HttpMethodEnum.Post;
            rp.Parameters = postdata;
            rp.Cookie = mCookieType;
            rp.Origin = "http://qinfo.clt.qq.com";


            try
            {
                HttpResponseParameter result = HttpProvider.Execute(rp);
                Debug.Write("显示返回内容");
                Debug.Write(result.Body);
                return result.Body;

            }
            catch (Exception e)
            {

                Debug.WriteLine(e);
            }
            return null;
        }


        /// <summary>
        /// 添加群标签
        /// </summary>
        /// <param name="gnumber">群号</param>
        /// <param name="skey"></param>
        /// <param name="op">1:增加|2：删除</param>
        /// <param name="tag"></param>
        /// <returns>{"ec":0,"gc":579512715,"md":"12081dbd5992b1343b2c63f8ef11bb6c"}|{"ec":0,"gc":579512715}</returns>
        internal string _set_group_more_cache(long gnumber, string skey, int op, string tag,string md)
        {
            IDictionary<string, object> postdata = new Dictionary<string, object>();
            postdata.Add("src", "qinfo_v3");
            postdata.Add("gc", gnumber);
            postdata.Add("kbn", tool.GetBkn(skey).ToString());
            postdata.Add("tag", tag);
            postdata.Add("op", op);
            postdata.Add("md", md);

            HttpRequestParameter rp = new HttpRequestParameter();
            mCookieType.CookieCollection = mCookieCollection;
            rp.Url = "http://qinfo.clt.qq.com/cgi-bin/qun_info/set_group_more_cache";
            rp.Referer = "http://qinfo.clt.qq.com/qinfo_v3/member.html?groupuin=" + gnumber;
            rp.Method = HttpMethodEnum.Post;
            rp.Parameters = postdata;
            rp.Cookie = mCookieType;
            rp.Origin = "http://qinfo.clt.qq.com";


            try
            {
                HttpResponseParameter result = HttpProvider.Execute(rp);
                Debug.Write("显示返回内容");
                Debug.Write(result.Body);
                return result.Body;

            }
            catch (Exception e)
            {

                Debug.WriteLine(e);
            }
            return null;
        }
        /// <summary>
        /// 修改群等级标签
        /// </summary>
        /// <param name="gnumber">群号</param>
        /// <param name="skey"></param>
        /// <param name="lvln1">等级1标签</param>
        /// <param name="lvln2">等级2标签</param>
        /// <param name="lvln3">等级3标签</param>
        /// <param name="lvln4">等级4标签</param>
        /// <param name="lvln5">等级5标签</param>
        /// <param name="lvln6">等级6标签</param>
        /// <returns></returns>
        internal string _set_group_level_info(long gnumber, string skey, string lvln1, string lvln2, string lvln3, string lvln4, string lvln5, string lvln6)
        {
            IDictionary<string, object> postdata = new Dictionary<string, object>();
            postdata.Add("lvln1", lvln1);
            postdata.Add("lvln2", lvln2);
            postdata.Add("lvln3", lvln3);
            postdata.Add("lvln4", lvln4);
            postdata.Add("lvln5", lvln5);
            postdata.Add("lvln6", lvln6);
            postdata.Add("gc", gnumber);
            postdata.Add("kbn", tool.GetBkn(skey).ToString());
            postdata.Add("src", "qinfo_v3");

            HttpRequestParameter rp = new HttpRequestParameter();
            mCookieType.CookieCollection = mCookieCollection;
            rp.Url = "http://qinfo.clt.qq.com/cgi-bin/qun_info/set_group_level_info";
            rp.Referer = "http://qinfo.clt.qq.com/qinfo_v3/grade.html";
            rp.Method = HttpMethodEnum.Post;
            rp.Parameters = postdata;
            rp.Cookie = mCookieType;
            rp.Origin = "http://qinfo.clt.qq.com";


            try
            {
                HttpResponseParameter result = HttpProvider.Execute(rp);
                Debug.Write("显示返回内容");
                Debug.Write(result.Body);
                return result.Body;

            }
            catch (Exception e)
            {

                Debug.WriteLine(e);
            }
            return null;
        }


        internal string _get_group_subscribe_status(long gnumber, string skey)
        {
            IDictionary<string, object> postdata = new Dictionary<string, object>();
            postdata.Add("gc", gnumber);
            postdata.Add("kbn", tool.GetBkn(skey).ToString());
            postdata.Add("scode_only", 1);
            
            HttpRequestParameter rp = new HttpRequestParameter();
            mCookieType.CookieCollection = mCookieCollection;
            rp.Url = "http://qun.qq.com/cgi-bin/group_subscribe/get_group_subscribe_status";
            rp.Referer = "http://web.qun.qq.com/announce/index.html";
            rp.Method = HttpMethodEnum.Post;
            rp.Parameters = postdata;
            rp.Cookie = mCookieType;
            rp.Origin = "http://web.qun.qq.com";


            try
            {
                HttpResponseParameter result = HttpProvider.Execute(rp);
                Debug.Write("显示返回内容");
                Debug.Write(result.Body);
                return result.Body;

            }
            catch (Exception e)
            {

                Debug.WriteLine(e);
            }
            return null;
        }
        /// <summary>
        /// 公告列表
        /// </summary>
        /// <param name="gnumber"></param>
        /// <param name="skey"></param>
        /// <param name="n">显示数量</param>
        /// <param name="s">未知，默认0</param>
        /// <returns></returns>
        internal string _get_announce_list(long gnumber, string skey,int n,int s)
        {
            IDictionary<string, object> postdata = new Dictionary<string, object>();
            postdata.Add("gc", gnumber);
            postdata.Add("kbn", tool.GetBkn(skey).ToString());
            postdata.Add("ft", 23);
            postdata.Add("n", n);
            postdata.Add("s", s);
            postdata.Add("i", 1);

            HttpRequestParameter rp = new HttpRequestParameter();
            mCookieType.CookieCollection = mCookieCollection;
            rp.Url = "http://web.qun.qq.com/cgi-bin/announce/get_t_list";
            rp.Referer = "http://web.qun.qq.com/announce/index.html";
            rp.Method = HttpMethodEnum.Post;
            rp.Parameters = postdata;
            rp.Cookie = mCookieType;
            rp.Origin = "http://web.qun.qq.com";


            try
            {
                HttpResponseParameter result = HttpProvider.Execute(rp);
                Debug.Write("显示返回内容");
                Debug.Write(result.Body);
                return result.Body;

            }
            catch (Exception e)
            {

                Debug.WriteLine(e);
            }
            return null;
        }

        /// <summary>
        /// 本群须知
        /// </summary>
        /// <param name="gnumber"></param>
        /// <param name="skey"></param>
        /// <param name="title"></param>
        /// <param name="text"></param>
        /// <param name="gsi"></param>
        /// <returns></returns>
        internal string _add_qun_notice(long gnumber, string skey, string title, string text, string gsi)
        {
            IDictionary<string, object> postdata = new Dictionary<string, object>();
            postdata.Add("kbn", tool.GetBkn(skey).ToString());
            postdata.Add("qid", gnumber);
            postdata.Add("title", title);
            postdata.Add("text", text);
            postdata.Add("gsi", gsi);

            HttpRequestParameter rp = new HttpRequestParameter();
            mCookieType.CookieCollection = mCookieCollection;
            rp.Url = "http://web.qun.qq.com/cgi-bin/announce/add_qun_instruction";
            rp.Referer = "http://web.qun.qq.com/announce/index.html";
            rp.Method = HttpMethodEnum.Post;
            rp.Parameters = postdata;
            rp.Cookie = mCookieType;
            rp.Origin = "http://web.qun.qq.com";


            try
            {
                HttpResponseParameter result = HttpProvider.Execute(rp);
                Debug.Write("显示返回内容");
                Debug.Write(result.Body);
                return result.Body;

            }
            catch (Exception e)
            {

                Debug.WriteLine(e);
            }
            return null;
        }

        /// <summary>
        /// 发布群公告
        /// </summary>
        /// <param name="gnumber"></param>
        /// <param name="skey"></param>
        /// <param name="title"></param>
        /// <param name="text"></param>
        /// <param name="gsi">获取群列表时获得</param>
        /// <returns></returns>
        internal string _add_announce_feed(long gnumber, string skey, string title, string text,string gsi)
        {
            IDictionary<string, object> postdata = new Dictionary<string, object>();
            postdata.Add("kbn", tool.GetBkn(skey).ToString());
            postdata.Add("qid", gnumber);
            postdata.Add("title", title);
            postdata.Add("text", text);
            postdata.Add("gsi", gsi);

            HttpRequestParameter rp = new HttpRequestParameter();
            mCookieType.CookieCollection = mCookieCollection;
            rp.Url = "http://web.qun.qq.com/cgi-bin/announce/add_qun_notice";
            rp.Referer = "http://web.qun.qq.com/announce/index.html";
            rp.Method = HttpMethodEnum.Post;
            rp.Parameters = postdata;
            rp.Cookie = mCookieType;
            rp.Origin = "http://web.qun.qq.com";


            try
            {
                HttpResponseParameter result = HttpProvider.Execute(rp);
                Debug.Write("显示返回内容");
                Debug.Write(result.Body);
                return result.Body;

            }
            catch (Exception e)
            {

                Debug.WriteLine(e);
            }
            return null;
        }

        /// <summary>
        /// 删除群公告
        /// </summary>
        /// <param name="gnumber"></param>
        /// <param name="skey"></param>
        /// <param name="fid">公告ID</param>
        /// <param name="gsi">获取群公告列表时获得</param>
        /// <returns>{"ec":0,"fid":"8ba98a220000000023afc157283d0c00","id":1,"lstm":1472313468}</returns>
        internal string _del_announce_feed(long gnumber, string skey, string fid, string gsi)
        {
            IDictionary<string, object> postdata = new Dictionary<string, object>();
            postdata.Add("fid", fid);
            postdata.Add("ft", 23);
            postdata.Add("kbn", tool.GetBkn(skey).ToString());
            postdata.Add("qid", gnumber);
            postdata.Add("op", 0);
            postdata.Add("gsi", gsi);

            HttpRequestParameter rp = new HttpRequestParameter();
            mCookieType.CookieCollection = mCookieCollection;
            rp.Url = "http://web.qun.qq.com/cgi-bin/announce/del_feed";
            rp.Referer = "http://web.qun.qq.com/announce/index.html";
            rp.Method = HttpMethodEnum.Post;
            rp.Parameters = postdata;
            rp.Cookie = mCookieType;
            rp.Origin = "http://web.qun.qq.com";


            try
            {
                HttpResponseParameter result = HttpProvider.Execute(rp);
                Debug.Write("显示返回内容");
                Debug.Write(result.Body);
                return result.Body;

            }
            catch (Exception e)
            {

                Debug.WriteLine(e);
            }
            return null;
        }

        /// <summary>
        /// 获取群关注
        /// </summary>
        /// <param name="gnumber"></param>
        /// <param name="skey"></param>
        /// <returns></returns>

        internal string _clear_red_point(long gnumber, string skey, string gsi)
        {
            IDictionary<string, object> postdata = new Dictionary<string, object>();
            postdata.Add("kbn", tool.GetBkn(skey).ToString());
            postdata.Add("qid", gnumber);
            postdata.Add("gsi", gsi);

            HttpRequestParameter rp = new HttpRequestParameter();
            mCookieType.CookieCollection = mCookieCollection;
            rp.Url = "http://web.qun.qq.com/cgi-bin/announce/clear_red_point";
            rp.Referer = "http://web.qun.qq.com/announce/index.html";
            rp.Method = HttpMethodEnum.Post;
            rp.Parameters = postdata;
            rp.Cookie = mCookieType;
            rp.Origin = "http://web.qun.qq.com";


            try
            {
                HttpResponseParameter result = HttpProvider.Execute(rp);
                Debug.Write("显示返回内容");
                Debug.Write(result.Body);
                return result.Body;

            }
            catch (Exception e)
            {

                Debug.WriteLine(e);
            }
            return null;
        }
        /// <summary>
        /// 获取特别关注设置
        /// </summary>
        /// <param name="gnumber"></param>
        /// <param name="skey"></param>
        /// <returns>{"ec":0,"flag":1,"gc":579512715,"uinls":[{"from":1,"n":"测试","u":43430833}],"wordls":[{"keyword":"红包"}]}</returns>
        internal string _get_concerned_list(long gnumber, string skey)
        {
            IDictionary<string, object> getParam = new Dictionary<string, object>();
            getParam.Add("gc", gnumber);
            getParam.Add("kbn", tool.GetBkn(skey).ToString());

            HttpRequestParameter rp = new HttpRequestParameter();
            mCookieType.CookieCollection = mCookieCollection;
            rp.Url = "http://qun.qq.com/cgi-bin/care/get_concerned_list";
            rp.Referer = "http://qun.qq.com/care/index.html?gc=" + gnumber;
            rp.Parameters = getParam;
            rp.Cookie = mCookieType;

            try
            {
                HttpResponseParameter result = HttpProvider.Execute(rp);

                return result.Body;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
            return null;
        }
        /// <summary>
        /// 设置特别关注
        /// </summary>
        /// <param name="gnumber"></param>
        /// <param name="skey"></param>
        /// <param name="addword"></param>
        /// <param name="adduin"></param>
        /// <param name="delword"></param>
        /// <param name="deluin"></param>
        /// <returns>{"count":0,"ec":0}</returns>
        internal string _set_concerned_list(long gnumber, string skey, List<string> addword, List<string> adduin, List<string> delword, List<string> deluin)
        {
            IDictionary<string, object> postdata = new Dictionary<string, object>();
            postdata.Add("gc", gnumber);
            postdata.Add("addword",tool.unicode(tool.Base64Encrypt(addword)));
            postdata.Add("adduin", tool.unicode(adduin));
            postdata.Add("delword", tool.unicode(tool.Base64Encrypt(delword)));
            postdata.Add("deluin", tool.unicode(deluin));
            postdata.Add("kbn", tool.GetBkn(skey).ToString());

            HttpRequestParameter rp = new HttpRequestParameter();
            mCookieType.CookieCollection = mCookieCollection;
            rp.Url = "http://qun.qq.com/cgi-bin/care/set_concerned_list";
            rp.Referer = "http://qun.qq.com/care/index.html?gc=" + gnumber;
            rp.Method = HttpMethodEnum.Post;
            rp.Parameters = postdata;
            rp.Cookie = mCookieType;
            rp.Origin = "http://web.qun.qq.com";


            try
            {
                HttpResponseParameter result = HttpProvider.Execute(rp);
                Debug.Write("显示返回内容");
                Debug.Write(result.Body);
                return result.Body;

            }
            catch (Exception e)
            {

                Debug.WriteLine(e);
            }
            return null;
        }

        /// <summary>
        /// 群最新消息聊天框提示
        /// </summary>
        /// <param name="gnumber"></param>
        /// <param name="skey"></param>
        /// <param name="fnum">显示数量</param>
        /// <returns></returns>
        internal string _get_data_new_notice(long gnumber, string skey,int fnum)
        {
            IDictionary<string, object> getParam = new Dictionary<string, object>();
            getParam.Add("qid", gnumber);
            getParam.Add("fnum", fnum);
            getParam.Add("bkn", tool.GetBkn(skey).ToString());

            HttpRequestParameter rp = new HttpRequestParameter();
            mCookieType.CookieCollection = mCookieCollection;
            rp.Url = "http://web.qun.qq.com/cgi-bin/notice/get_data_new";
            //rp.Referer = "http://qinfo.clt.qq.com/qinfo_v3/member.html?groupuin=" + gnumber;
            rp.Parameters = getParam;
            rp.Cookie = mCookieType;

            try
            {
                HttpResponseParameter result = HttpProvider.Execute(rp);

                return result.Body;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
            return null;
        }

        /// <summary>
        /// 邀请好友入群
        /// </summary>
        /// <param name="gnumber"></param>
        /// <param name="qq"></param>
        /// <param name="skey"></param>
        /// <returns></returns>
        /// 
        internal string _invite_friend(long gnumber, long qq, string skey)
        {
            IDictionary<string, object> postdata = new Dictionary<string, object>();
            postdata.Add("gc", gnumber);
            postdata.Add("ul", qq);
            postdata.Add("kbn", tool.GetBkn(skey).ToString());

            HttpRequestParameter rp = new HttpRequestParameter();
            mCookieType.CookieCollection = mCookieCollection;
            rp.Url = "http://qun.qq.com/cgi-bin/qun_mgr/add_group_member";
            rp.Referer = "http://qun.qq.com/member.html";
            rp.Method = HttpMethodEnum.Post;
            rp.Parameters = postdata;
            rp.Cookie = mCookieType;

            try
            {
                HttpResponseParameter result = HttpProvider.Execute(rp);
                Debug.Write("显示返回内容");
                Debug.Write(result.Body);
                return result.Body;

            }
            catch (Exception e)
            {

                Debug.WriteLine(e);
            }
            return null;
        }

        /// <summary>
        /// 删除群成员
        /// </summary>
        /// <param name="gnumber"></param>
        /// <param name="qq"></param>
        /// <param name="skey"></param>
        /// <returns></returns>
        internal string _kick_group_member(long gnumber, long qq, string skey)
        {
            IDictionary<string, object> postdata = new Dictionary<string, object>();
            postdata.Add("gc", gnumber);
            postdata.Add("ul", qq);
            postdata.Add("flag", 0);
            postdata.Add("kbn", tool.GetBkn(skey).ToString());

            HttpRequestParameter rp = new HttpRequestParameter();
            mCookieType.CookieCollection = mCookieCollection;
            rp.Url = "http://qun.qq.com/cgi-bin/qun_mgr/delete_group_member";
            rp.Referer = "http://qun.qq.com/member.html";
            rp.Method = HttpMethodEnum.Post;
            rp.Parameters = postdata;
            rp.Cookie = mCookieType;

            try
            {
                HttpResponseParameter result = HttpProvider.Execute(rp);
                Debug.Write("显示返回内容");
                Debug.Write(result.Body);
                return result.Body;

            }
            catch (Exception e)
            {

                Debug.WriteLine(e);
            }
            return null;
        }

        /// <summary>
        /// 群禁言
        /// </summary>
        /// <param name="gnumber"></param>
        /// <param name="qq"></param>
        /// <param name="time"></param>
        /// <param name="skey"></param>
        /// <returns></returns>
        internal string _shutup_group_member(long gnumber, long? qq, long? time, int? all_shutup, string skey)
        {
            IDictionary<string, object> postdata = new Dictionary<string, object>();
            string shutup_list = "[{ \"uin\":" + qq + ",\"t\":" + time + "}]";
            postdata.Add("gc", gnumber);
            if (all_shutup == null)
            {
                postdata.Add("shutup_list", shutup_list);
            }
            else
            {
                postdata.Add("all_shutup", all_shutup);
            }
            postdata.Add("kbn", tool.GetBkn(skey).ToString());

            HttpRequestParameter rp = new HttpRequestParameter();
            mCookieType.CookieCollection = mCookieCollection;
            rp.Url = "http://qinfo.clt.qq.com/cgi-bin/qun_info/set_group_shutup";
            rp.Referer = "http://qinfo.clt.qq.com/qinfo_v3/member.html";
            rp.Method = HttpMethodEnum.Post;
            rp.Parameters = postdata;
            rp.Cookie = mCookieType;

            try
            {
                HttpResponseParameter result = HttpProvider.Execute(rp);
                Debug.Write("显示返回内容");
                Debug.Write(result.Body);
                return result.Body;

            }
            catch (Exception e)
            {

                Debug.WriteLine(e);
            }
            return null;
        }

        /// <summary>
        /// 设置管理员（QUN API）
        /// </summary>
        /// <param name="gnumber"></param>
        /// <param name="qq"></param>
        /// <param name="op"></param>
        /// <param name="skey"></param>
        /// <returns></returns>
        internal string _set_group_admin_2(long gnumber, long qq, int op, string skey)
        {
            IDictionary<string, object> postdata = new Dictionary<string, object>();
            postdata.Add("gc", gnumber);
            postdata.Add("ul", qq);
            postdata.Add("op", op);
            postdata.Add("kbn", tool.GetBkn(skey).ToString());

            HttpRequestParameter rp = new HttpRequestParameter();
            mCookieType.CookieCollection = mCookieCollection;
            rp.Url = "http://qun.qq.com/cgi-bin/qun_mgr/set_group_admin";
            rp.Referer = "http://qun.qq.com/member.html";
            rp.Method = HttpMethodEnum.Post;
            rp.Parameters = postdata;
            rp.Cookie = mCookieType;
            //rp.Origin = "http://qinfo.clt.qq.com";

            try
            {
                HttpResponseParameter result = HttpProvider.Execute(rp);
                Debug.Write("显示返回内容");
                Debug.Write(result.Body);
                return result.Body;

            }
            catch (Exception e)
            {

                Debug.WriteLine(e);
            }
            return null;
        }
        /// <summary>
        /// 设置管理员（客户端API）
        /// </summary>
        /// <param name="gnumber"></param>
        /// <param name="qq"></param>
        /// <param name="op"></param>
        /// <param name="skey"></param>
        /// <returns></returns>
        internal string _set_group_admin(long gnumber, long qq, int op,string skey)
        {
            IDictionary<string, object> postdata = new Dictionary<string, object>();
            postdata.Add("u", qq);
            postdata.Add("op", op);
            postdata.Add("gc", gnumber);
            postdata.Add("kbn", tool.GetBkn(skey).ToString());
            postdata.Add("src", "qinfo_v3");

            HttpRequestParameter rp = new HttpRequestParameter();
            mCookieType.CookieCollection = mCookieCollection;
            rp.Url = "http://qinfo.clt.qq.com/cgi-bin/qun_info/set_group_admin";
            rp.Referer = "http://qinfo.clt.qq.com/qinfo_v3/member.html?groupuin="+ gnumber;
            rp.Method = HttpMethodEnum.Post;
            rp.Parameters = postdata;
            rp.Cookie = mCookieType;
            rp.Origin = "http://qinfo.clt.qq.com";


            try
            {
                HttpResponseParameter result = HttpProvider.Execute(rp);
                Debug.Write("显示返回内容");
                Debug.Write(result.Body);
                return result.Body;

            }
            catch (Exception e)
            {

                Debug.WriteLine(e);
            }
            return null;
        }

        /// <summary>
        /// 设置成员名片（QUN API）
        /// </summary>
        /// <param name="gnumber"></param>
        /// <param name="qq"></param>
        /// <param name="name"></param>
        /// <param name="skey"></param>
        /// <returns></returns>
        internal string _set_group_card_2(long gnumber, long qq, string name, string skey)
        {
            IDictionary<string, object> postdata = new Dictionary<string, object>();
            postdata.Add("gc", gnumber);
            postdata.Add("u", qq);
            postdata.Add("name", name);
            postdata.Add("kbn", tool.GetBkn(skey).ToString());

            HttpRequestParameter rp = new HttpRequestParameter();
            mCookieType.CookieCollection = mCookieCollection;
            rp.Url = "http://qun.qq.com/cgi-bin/qun_mgr/set_group_card";
            rp.Referer = "http://qun.qq.com/member.html";
            rp.Method = HttpMethodEnum.Post;
            rp.Parameters = postdata;
            rp.Cookie = mCookieType;
            //rp.Origin = "http://qinfo.clt.qq.com";


            try
            {
                HttpResponseParameter result = HttpProvider.Execute(rp);
                Debug.Write("显示返回内容");
                Debug.Write(result.Body);
                return result.Body;

            }
            catch (Exception e)
            {

                Debug.WriteLine(e);
            }
            return null;
        }

        /// <summary>
        /// 设置群名片（客户端API）
        /// </summary>
        /// <param name="gnumber"></param>
        /// <param name="qq"></param>
        /// <param name="name"></param>
        /// <param name="skey"></param>
        /// <returns></returns>
        internal string _set_group_card(long gnumber, long qq, string name, string skey)
        {
            IDictionary<string, object> postdata = new Dictionary<string, object>();
            postdata.Add("gc", gnumber);
            postdata.Add("u", qq);
            postdata.Add("name", name);
            postdata.Add("kbn", tool.GetBkn(skey).ToString());
            postdata.Add("src", "qinfo_v3");

            HttpRequestParameter rp = new HttpRequestParameter();
            mCookieType.CookieCollection = mCookieCollection;
            rp.Url = "http://qinfo.clt.qq.com/cgi-bin/qun_info/set_group_card";
            rp.Referer = "http://qinfo.clt.qq.com/qinfo_v3/member.html?groupuin=" + gnumber;
            rp.Method = HttpMethodEnum.Post;
            rp.Parameters = postdata;
            rp.Cookie = mCookieType;
            rp.Origin = "http://qinfo.clt.qq.com";


            try
            {
                HttpResponseParameter result = HttpProvider.Execute(rp);
                Debug.Write("显示返回内容");
                Debug.Write(result.Body);
                return result.Body;

            }
            catch (Exception e)
            {

                Debug.WriteLine(e);
            }
            return null;
        }
        internal string _get_group_info_all(long gnumber, string skey)
        {
            IDictionary<string, object> getParam = new Dictionary<string, object>();
            getParam.Add("gc", gnumber);
            getParam.Add("bkn", tool.GetBkn(skey).ToString());
            getParam.Add("src", "qinfo_v3");
            getParam.Add("_ti", tool.GetTimeStamp(DateTime.Now));

            HttpRequestParameter rp = new HttpRequestParameter();
            mCookieType.CookieCollection = mCookieCollection;
            rp.Url = "http://qinfo.clt.qq.com/cgi-bin/qun_info/get_group_info_all";
            rp.Referer = "http://qinfo.clt.qq.com/qinfo_v3/member.html?groupuin="+ gnumber;
            rp.Parameters = getParam;
            rp.Cookie = mCookieType;

            try
            {
                HttpResponseParameter result = HttpProvider.Execute(rp);

                return result.Body;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
            return null;
        }


        internal string _get_members_info_v1(long gnumber, string skey)
        {
            IDictionary<string, object> getParam = new Dictionary<string, object>();
            getParam.Add("friends", 1);
            getParam.Add("gc", gnumber);
            getParam.Add("bkn", tool.GetBkn(skey).ToString());
            getParam.Add("src", "qinfo_v3");
            getParam.Add("_ti", tool.GetTimeStamp(DateTime.Now));

            HttpRequestParameter rp = new HttpRequestParameter();
            mCookieType.CookieCollection = mCookieCollection;
            rp.Url = "http://qinfo.clt.qq.com/cgi-bin/qun_info/get_members_info_v1";
            rp.Referer = "http://qinfo.clt.qq.com/qinfo_v3/member.html?groupuin=" + gnumber;
            rp.Parameters = getParam;
            rp.Cookie = mCookieType;

            try
            {
                HttpResponseParameter result = HttpProvider.Execute(rp);

                return result.Body;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
            return null;
        }

        internal string _get_group_mem_tag(long gnumber, long qq, string skey)
        {
            IDictionary<string, object> postdata = new Dictionary<string, object>();
            postdata.Add("gc", gnumber);
            postdata.Add("kbn", tool.GetBkn(skey).ToString());
            postdata.Add("src", "qinfo_v3");

            HttpRequestParameter rp = new HttpRequestParameter();
            mCookieType.CookieCollection = mCookieCollection;
            rp.Url = "http://qinfo.clt.qq.com/cgi-bin/qun_info/get_group_mem_tag";
            rp.Referer = "http://qinfo.clt.qq.com/qinfo_v3/member.html?groupuin=" + gnumber;
            rp.Method = HttpMethodEnum.Post;
            rp.Parameters = postdata;
            rp.Cookie = mCookieType;
            rp.Origin = "http://qinfo.clt.qq.com";


            try
            {
                HttpResponseParameter result = HttpProvider.Execute(rp);
                Debug.Write("显示返回内容");
                Debug.Write(result.Body);
                return result.Body;

            }
            catch (Exception e)
            {

                Debug.WriteLine(e);
            }
            return null;
        }

        internal string _get_member_tag_flag(long gnumber, long qq, string skey)
        {
            IDictionary<string, object> postdata = new Dictionary<string, object>();
            postdata.Add("gc", gnumber);
            postdata.Add("kbn", tool.GetBkn(skey).ToString());
            postdata.Add("src", "qinfo_v3");

            HttpRequestParameter rp = new HttpRequestParameter();
            mCookieType.CookieCollection = mCookieCollection;
            rp.Url = "http://qinfo.clt.qq.com/cgi-bin/qun_info/get_member_tag_flag";
            rp.Referer = "http://qinfo.clt.qq.com/qinfo_v3/member.html?groupuin=" + gnumber;
            rp.Method = HttpMethodEnum.Post;
            rp.Parameters = postdata;
            rp.Cookie = mCookieType;
            rp.Origin = "http://qinfo.clt.qq.com";


            try
            {
                HttpResponseParameter result = HttpProvider.Execute(rp);
                Debug.Write("显示返回内容");
                Debug.Write(result.Body);
                return result.Body;

            }
            catch (Exception e)
            {

                Debug.WriteLine(e);
            }
            return null;
        }
        /// <summary>
        /// 群操作历史
        /// </summary>
        /// <param name="gnumber"></param>
        /// <param name="skey"></param>
        /// <returns></returns>
        internal string _get_group_member_log(long gnumber, string skey)
        {
            IDictionary<string, object> getParam = new Dictionary<string, object>();
            getParam.Add("gc", gnumber);
            getParam.Add("bkn", tool.GetBkn(skey).ToString());
            getParam.Add("src", "qinfo_v3");
            getParam.Add("_ti", tool.GetTimeStamp(DateTime.Now));

            HttpRequestParameter rp = new HttpRequestParameter();
            mCookieType.CookieCollection = mCookieCollection;
            rp.Url = "http://qinfo.clt.qq.com/cgi-bin/qun_info/get_sys_msg";
            rp.Referer = "http://qinfo.clt.qq.com/qinfo_v3/member-log.html";
            rp.Parameters = getParam;
            rp.Cookie = mCookieType;

            try
            {
                HttpResponseParameter result = HttpProvider.Execute(rp);

                return result.Body;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
            return null;
        }


        internal string _get_group_setting(long gnumber, string skey)
        {
            IDictionary<string, object> getParam = new Dictionary<string, object>();
            getParam.Add("gc", gnumber);
            getParam.Add("bkn", tool.GetBkn(skey).ToString());
            getParam.Add("src", "qinfo_v3");
            getParam.Add("_ti", tool.GetTimeStamp(DateTime.Now));

            HttpRequestParameter rp = new HttpRequestParameter();
            mCookieType.CookieCollection = mCookieCollection;
            rp.Url = "http://qinfo.clt.qq.com/cgi-bin/qun_info/get_group_setting";
            rp.Referer = "http://qinfo.clt.qq.com/qinfo_v3/setting.html?groupuin=" + gnumber;
            rp.Parameters = getParam;
            rp.Cookie = mCookieType;

            try
            {
                HttpResponseParameter result = HttpProvider.Execute(rp);

                return result.Body;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
            return null;
        }


        internal string _get_admin_auth(long gnumber, string skey)
        {
            IDictionary<string, object> getParam = new Dictionary<string, object>();
            getParam.Add("auth", 1);
            getParam.Add("gc", gnumber);
            getParam.Add("bkn", tool.GetBkn(skey).ToString());
            getParam.Add("src", "qinfo_v3");
            getParam.Add("_ti", tool.GetTimeStamp(DateTime.Now));

            HttpRequestParameter rp = new HttpRequestParameter();
            mCookieType.CookieCollection = mCookieCollection;
            rp.Url = "http://qinfo.clt.qq.com/cgi-bin/qun_info/get_admin_auth";
            rp.Referer = "http://qinfo.clt.qq.com/qinfo_v3/setting.html?groupuin=" + gnumber;
            rp.Parameters = getParam;
            rp.Cookie = mCookieType;

            try
            {
                HttpResponseParameter result = HttpProvider.Execute(rp);

                return result.Body;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
            return null;
        }

        /// <summary>
        /// 获取禁言设置（需管理员权限）
        /// </summary>
        /// <param name="gnumber"></param>
        /// <param name="skey"></param>
        /// <returns></returns>
        internal string _get_group_shutup(long gnumber, string skey)
        {
            IDictionary<string, object> getParam = new Dictionary<string, object>();
            getParam.Add("gc", gnumber);
            getParam.Add("bkn", tool.GetBkn(skey).ToString());
            getParam.Add("src", "qinfo_v3");
            getParam.Add("_ti", tool.GetTimeStamp(DateTime.Now));

            HttpRequestParameter rp = new HttpRequestParameter();
            mCookieType.CookieCollection = mCookieCollection;
            rp.Url = "http://qinfo.clt.qq.com/cgi-bin/qun_info/get_group_shutup";
            rp.Referer = "http://qinfo.clt.qq.com/qinfo_v3/setting.html?groupuin=" + gnumber;
            rp.Parameters = getParam;
            rp.Cookie = mCookieType;

            try
            {
                HttpResponseParameter result = HttpProvider.Execute(rp);

                return result.Body;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
            return null;
        }


        internal string _get_discu_info(string did, string vfwebqq, string psessionid)
        {


            IDictionary<string, object> getParam = new Dictionary<string, object>();
            getParam.Add("did", did);
            getParam.Add("vfwebqq", vfwebqq);
            getParam.Add("clientid", Base.clientid);
            getParam.Add("psessionid", psessionid);
            getParam.Add("t", tool.GetTimeStamp(DateTime.Now));

            HttpRequestParameter rp = new HttpRequestParameter();
            mCookieType.CookieCollection = mCookieCollection;
            rp.Url = "http://d1.web2.qq.com/channel/get_discu_info";
            rp.Referer = "http://d1.web2.qq.com/proxy.html?v=20151105001&callback=1&id=2";
            rp.Parameters = getParam;
            rp.Cookie = mCookieType;

            try
            {
                HttpResponseParameter result = HttpProvider.Execute(rp);

                return result.Body;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
            return null;
        }

        internal string _get_discus_list(string did, string vfwebqq, string psessionid)
        {


            IDictionary<string, object> getParam = new Dictionary<string, object>();
            getParam.Add("vfwebqq", vfwebqq);
            getParam.Add("clientid", Base.clientid);
            getParam.Add("psessionid", psessionid);
            getParam.Add("t", tool.GetTimeStamp(DateTime.Now));

            HttpRequestParameter rp = new HttpRequestParameter();
            mCookieType.CookieCollection = mCookieCollection;
            rp.Url = "http://s.web2.qq.com/api/get_discus_list";
            rp.Referer = "http://s.web2.qq.com/proxy.html?v=20130916001&callback=1&id=1";
            rp.Parameters = getParam;
            rp.Cookie = mCookieType;

            try
            {
                HttpResponseParameter result = HttpProvider.Execute(rp);

                return result.Body;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
            return null;
        }        /// <summary>
                 /// 清楚cookie
                 /// </summary>
        internal void ClearCookies()
        {
            http.InitCookies();
        }

        /// <summary>
        /// 设置cookie共享
        /// </summary>
        /// <param name="p_skey"></param>
        /// <param name="p_uin"></param>
        internal void CookieProxy(string p_skey, string p_uin)
        {
            mCookieCollection.Add(new Cookie("p_skey", p_skey, "/", "w.qq.com"));
            mCookieCollection.Add(new Cookie("p_uin", p_uin, "/", "w.qq.com"));
            mCookieCollection.Add(new Cookie("p_skey", p_skey, "/", "qun.qq.com"));
            mCookieCollection.Add(new Cookie("p_uin", p_uin, "/", "qun.qq.com"));
            mCookieCollection.Add(new Cookie("p_skey", p_skey, "/", "id.qq.com"));
            mCookieCollection.Add(new Cookie("p_uin", p_uin, "/", "id.qq.com"));
            mCookieCollection.Add(new Cookie("p_skey", p_skey, "/", "mail.qq.com"));
            mCookieCollection.Add(new Cookie("p_uin", p_uin, "/", "mail.qq.com"));
        }

        private void AddCookie(string name, string value, string path, string domain)
        {
            Cookie cookie = new Cookie(name, value, path, domain);
            mCookieCollection.Add(cookie);
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
