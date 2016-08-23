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

                Debug.Write("[*] 正在等待扫码 ....");
                OnGetQRCodeImage?.Invoke(QRImg);
                //----------2.检查扫描
                while (true)
                {
                    Thread.Sleep(1000);
                    var authresult = api.GetAuthStatus();
                    if (authresult.Contains("成功"))
                    {
                        // 登录成功

                        string redirect_url = authresult.Split(new string[] { "\'" }, StringSplitOptions.None)[5];

                        MessageBox.Show(redirect_url);

                        //----------2.获取ptwebqq
                        api.GetPtwebqq(redirect_url);

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
                    else if (authresult.Contains("二维码已失效"))
                    {
                        Debug.Write("[*] 正在生成二维码 ....");

                        OnGetQRCodeImage?.Invoke(QRImg);
                    }

                }

                //----------2.获取vfwebqq
                Cookies.vfwebqq = api.GetVfwebqq(Cookies.ptwebqq);

                api.getUinAndPsessionid(Cookies.ptwebqq);
                IsLogin = true;
            } while (!IsLogin);
            OnLoginSucess?.Invoke();



        }

    }
}
