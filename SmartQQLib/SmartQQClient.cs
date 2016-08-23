using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SmartQQLib.API;
using SmartQQLib.API.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
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
            api = new SmartQQAPIService(new API.Http.HttpClient());
        }
        private SmartQQAPIService api = null;

        public Cookies LoginCookies = new Cookies();

        public bool IsLogin { get; private set; }



        //callback
        public Action<Image> OnGetQRCodeImage;
        public Action OnScanImage;
        //public Action<Image> OnVerifyImage;
        public Action OnVerifyImage;
        public Action OnVerifySucess;
        public Action OnLoginSucess;
        public Action OnInitComplate;


        /// <summary>
        /// 运行QQClient主逻辑,推荐放在独立的线程中执行这个方法
        /// </summary>

        public void Run()
        {
            // 启动流程
            // 1.获取二维码
            // 2.检查扫描
            // 3.获取ptwebqq
            // 4.获取vfwebqq
            // 5.获取psessionid和uin、skey


            // ----------1.获取二维码

            do
            {
                Debug.Write("[*] 正在生成二维码 ....");
                var QRImg = api.GetQRCodeImage();
                if (QRImg != null)
                {
                    Debug.Write("成功\n");
                }
                else
                {
                    return;
                }

                Debug.Write("[*] 正在等待扫码 ....");
                OnGetQRCodeImage?.Invoke(QRImg);
                //----------2.检查扫描
                while (true)
                {
                    Thread.Sleep(1000);
                    var authresult = api.GetAuthStatus();

                    if (authresult.Contains("成功"))
                    {
                        // 扫描成功
                        string redirect_url = authresult.Split(new string[] { "\'" }, StringSplitOptions.None)[5];
                        Debug.Write(redirect_url);

                        //----------2.获取ptwebqq
                        LoginCookies.ptwebqq = api.GetPtwebqq(redirect_url);
                        Debug.Write("ptwebqq=" + LoginCookies.ptwebqq);

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
                    else if (authresult.Contains("二维码已失效")| authresult==null)
                    {
                        Debug.Write("[*] 重新生成二维码 ....");
                        var QRImgReGet = api.GetQRCodeImage();
                        if (QRImg != null)
                        {
                            Debug.Write("成功\n");
                        }

                        OnGetQRCodeImage?.Invoke(QRImgReGet);
                    }

                }


                //----------2.获取vfwebqq

                JObject Jovfwebqq = (JObject)JsonConvert.DeserializeObject(api.GetVfwebqq(LoginCookies.ptwebqq));

                LoginCookies.vfwebqq = Jovfwebqq["result"]["vfwebqq"].ToString();


                Debug.Write("vfwebqq=" + LoginCookies.vfwebqq);
                if (LoginCookies.vfwebqq == null)
                    return;

                string LoginResult = api.getUinAndPsessionid(LoginCookies.ptwebqq);
                if (LoginResult != null)
                {
                    JObject JoUinAndPsessionid = (JObject)JsonConvert.DeserializeObject(LoginResult);

                    LoginCookies.psessionid = JoUinAndPsessionid["result"]["psessionid"].ToString();
                    Debug.Write("psessionid=" + LoginCookies.psessionid);
                    LoginCookies.uin = JoUinAndPsessionid["result"]["uin"].ToString();
                    Debug.Write("uin=" + LoginCookies.uin);

                    string[] namelist ={ "skey","p_skey","p_uin"};

                    List<string> CookieList = api.GetCookies(namelist);


                    LoginCookies.skey = CookieList[0].ToString();
                    Debug.Write("skey=" + LoginCookies.skey);
                    LoginCookies.p_skey = CookieList[1].ToString();
                    Debug.Write("p_skey=" + LoginCookies.p_skey);
                    LoginCookies.p_uin = CookieList[2].ToString();
                    Debug.Write("p_uin=" + LoginCookies.p_uin);
                    IsLogin = true;
                }

            } while (!IsLogin);
            Debug.Write("跨域共享cookie");
            //跨域共享cookie
            api.CookieProxy(LoginCookies.p_skey, LoginCookies.p_uin);
            OnLoginSucess?.Invoke();

        }

    }
}


                


