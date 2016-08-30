using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SmartQQLib.API;
using SmartQQLib.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SmartQQLib
{
    public class SmartQQClient
    {
        public SmartQQClient()
        {
            api = new SmartQQAPIService(new HttpProvider());
        }
        private SmartQQAPIService api = null;

        public static string cookiesFileName = "cookie.data";

        public bool IsLogin { get; private set; }

        internal Cookies LoginCookies;


        //callback
        public Action<Image> OnGetQRCodeImage;
        public Action OnScanImage;
        //public Action<Image> OnVerifyImage;

        public Action BeginReLogin;
        public Action ReLoginFail;

        public Action OnVerifyImage;
        public Action OnVerifySucess;
        public Action OnLoginSucess;
        public Action OnInitComplate;
        public Action OnLoginOut;


        /// <summary>
        /// 运行QQClient主逻辑,推荐放在独立的线程中执行这个方法
        /// </summary>

        // ----------0.利用cookie文件登录

        public void ReLink()
        {
            string loginuser = api.ReadTextFile(Environment.CurrentDirectory, "user\\user.ini");
            Debug.Write(loginuser);

            if (!string.IsNullOrEmpty(loginuser))
            {

                if (File.Exists(Path.Combine(Environment.CurrentDirectory + "\\user\\" + loginuser, cookiesFileName)))
                {

                    LoginCookies = JsonConvert.DeserializeObject<Cookies>(File.ReadAllText(Path.Combine(Environment.CurrentDirectory + "\\user\\" + loginuser, cookiesFileName)));
                    BeginReLogin?.Invoke();
                    string Ptwebqq = LoginCookies.ptwebqq;
                    string Status = LoginCookies.status;
                    string Skey = LoginCookies.skey;
                    string Uin = LoginCookies.uin;
                    string P_skey = LoginCookies.p_skey;
                    string P_uin = LoginCookies.p_uin;

                    if (Ptwebqq != "")
                    {
                        string ReLoginResult = api._login_by_cookie(Ptwebqq, Status, Skey, Uin, P_skey, P_uin);
                        if (ReLoginResult != null)
                        {
                            JObject ReLogin = (JObject)JsonConvert.DeserializeObject(ReLoginResult);
                            if (ReLogin["retcode"].ToString() == "0")
                            {
                                LoginResult.psessionid = ReLogin["result"]["psessionid"].ToString();
                                Debug.Write("psessionid=" + LoginResult.psessionid);

                                LoginResult.qq = ReLogin["result"]["uin"].ToString();
                                Debug.Write("uin=" + LoginResult.qq);

                                LoginResult.ptwebqq = Ptwebqq;

                                string[] namelist = { "skey", "uin", "p_skey", "p_uin" };

                                List<string> CookieList = api.GetCookies(namelist);


                                LoginCookies.skey = CookieList[0].ToString();
                                Debug.Write("skey=" + LoginCookies.skey);

                                LoginCookies.uin = CookieList[1].ToString();
                                Debug.Write("uin=" + LoginCookies.uin);

                                LoginCookies.p_skey = CookieList[2].ToString();
                                Debug.Write("p_skey=" + LoginCookies.p_skey);

                                LoginCookies.p_uin = CookieList[3].ToString();
                                Debug.Write("p_uin=" + LoginCookies.p_uin);

                                api.CookieProxy(LoginCookies.p_skey, LoginCookies.p_uin);
                                IsLogin = true;
                                OnLoginSucess?.Invoke();
                            }
                            else
                            {
                                Debug.Write("返回值错误 errmsg:error!!!,retcode:100001");
                                ReLoginFail?.Invoke();

                                Login();
                            }
                        }
                        else
                        {
                            Debug.Write("服务器无返回");
                        }
                    }
                    else
                    {
                        Debug.Write("Cookie文件无效");
                        ReLoginFail?.Invoke();

                        Login();
                    }
                }
                else
                {
                    Debug.Write("Cookie文件不存在");
                    ReLoginFail?.Invoke();

                    Login();
                }

            }
            else
            {
                Debug.Write("user.ini文件无效");
                ReLoginFail?.Invoke();
                Login();
            }

        }

        public void Login()
        {
            // 启动流程
            // 1.获取二维码
            // 2.检查扫描
            // 3.获取ptwebqq
            // 4.获取vfwebqq
            // 5.获取psessionid和uin、skey
            LoginCookies = new Cookies();

            Debug.Write("清除Cookies.");
            api.ClearCookies();

            do
            {
                // ----------1.获取二维码

                Debug.Write("[*] 正在生成二维码 ....");
                var QRImg = api._get_qrcode_image();
                if (QRImg != null)
                {
                    Debug.Write("成功\n");
                }

                Debug.Write("[*] 正在等待扫码 ....");
                OnGetQRCodeImage?.Invoke(QRImg);
                //----------2.检查扫描
                while (true)
                {
                    Thread.Sleep(1000);
                    var authresult = api._get_authstatus();

                    if (authresult.Contains("成功"))
                    {
                        // 扫描成功
                        string redirect_url = authresult.Split(new string[] { "\'" }, StringSplitOptions.None)[5];
                        Debug.Write("跳转URL");
                        Debug.Write(redirect_url);

                        //----------2.获取ptwebqq
                        LoginCookies.ptwebqq = api._get_ptwebqq(redirect_url);
                        LoginResult.ptwebqq = LoginCookies.ptwebqq;

                        Debug.Write("ptwebqq=" + LoginResult.ptwebqq);

                        Debug.Write("已获授权\n");
                        //IsLogin = true;

                        OnVerifySucess?.Invoke();
                        break;
                    }
                    else if (authresult.Contains("二维码认证中"))
                    {
                        OnVerifyImage?.Invoke();

                    }
                    else if (authresult.Contains("二维码未失效"))
                    {
                        OnScanImage?.Invoke();

                    }
                    else if (authresult.Contains("二维码已失效")|| authresult==null)
                    {
                        Debug.Write("[*] 重新生成二维码 ....");
                        var QRImgReGet = api._get_qrcode_image();
                        if (QRImg != null)
                        {
                            Debug.Write("成功\n");
                        }

                        OnGetQRCodeImage?.Invoke(QRImgReGet);
                    }

                }
                //----------2.获取vfwebqq

                JObject Jovfwebqq = (JObject)JsonConvert.DeserializeObject(api._get_vfwebqq(LoginResult.ptwebqq));

                LoginCookies.vfwebqq = Jovfwebqq["result"]["vfwebqq"].ToString();
                LoginResult.vfwebqq = LoginCookies.vfwebqq;

                Debug.Write("vfwebqq=" + LoginResult.vfwebqq);
                if (LoginResult.vfwebqq == null)
                    return;
                //----------登录

                string strLoginCallback = api._login(LoginResult.ptwebqq);
                if (strLoginCallback != null&& strLoginCallback.Contains("psessionid"))
                {
                    JObject JoUinAndPsessionid = (JObject)JsonConvert.DeserializeObject(strLoginCallback);

                    LoginResult.psessionid = JoUinAndPsessionid["result"]["psessionid"].ToString();
                    Debug.Write("psessionid=" + LoginResult.psessionid);
                    LoginResult.qq = JoUinAndPsessionid["result"]["uin"].ToString();
                    Debug.Write("uin=" + LoginResult.qq);

                    string[] namelist = { "skey", "uin", "p_skey", "p_uin" };

                    List<string> CookieList = api.GetCookies(namelist);


                    LoginCookies.skey = CookieList[0].ToString();
                    Debug.Write("skey=" + LoginCookies.skey);

                    LoginCookies.uin = CookieList[1].ToString();
                    Debug.Write("uin=" + LoginCookies.uin);

                    LoginCookies.p_skey = CookieList[2].ToString();
                    Debug.Write("p_skey=" + LoginCookies.p_skey);

                    LoginCookies.p_uin = CookieList[3].ToString();
                    Debug.Write("p_uin=" + LoginCookies.p_uin);


                    if (!Directory.Exists(Environment.CurrentDirectory + "\\user\\" + LoginResult.qq))
                    {
                        Directory.CreateDirectory(Environment.CurrentDirectory + "\\user\\" + LoginResult.qq);
                    }

                    File.WriteAllText(Path.Combine(Environment.CurrentDirectory + "\\user\\" + LoginResult.qq, cookiesFileName), JsonConvert.SerializeObject(LoginCookies, Formatting.Indented));

                    Debug.Write("跨域共享cookie");
                    //跨域共享cookie
                    api.CookieProxy(LoginCookies.p_skey, LoginCookies.p_uin);
                    IsLogin = true;
                }
            } while (!IsLogin);
            OnLoginSucess?.Invoke();

        }

        /// <summary>
        /// 注销当前账号
        /// </summary>
        public void Logout()
        {
            //api.ClearCookies();
            OnLoginOut?.Invoke();
            Debug.Write("账号已注销");
            File.Delete(Path.Combine(Environment.CurrentDirectory + "\\user\\" + LoginResult.qq, cookiesFileName));
        }

        /// <summary>
        /// 更改在线状态
        /// </summary>
        /// <param name="state">online|callme|away|busy|silent|hidden|offline</param>
        public void ChangeState(string state)
        {
            api._change_state(state, LoginResult.psessionid);
        }


        public void Poll2()
        {
            api._recv_message(LoginResult.ptwebqq, LoginResult.psessionid);
        }
    }
}


                


