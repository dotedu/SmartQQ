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
            // 1.登陆
            // 2.初始化
            // 3.开启系统通知
            // 4.获得联系人列表
            // 5.进入同步主循环


            // ----------1.登陆

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
                //检查扫描
                while (true)
                {
                    
                    var authresult = api.GetAuthStatus();
                    MessageBox.Show(authresult);
                    AuthResult AuthStatus = new AuthResult();
                    if (authresult.Contains("成功"))
                    {
                        // 登录成功
                        var redirectResult = api.AuthRedirect(AuthStatus.redirect_uri);
                        AuthStatus.authStatus = "已获授权";
                        AuthStatus.StatusCode = 3;
                        //User Currentuser = new User();
                        //Currentuser.qqNum=api.GetCurrentQQ().qqNum;
 //                       var UserLogo = api.GetUserLogo(api.GetCurrentQQ().qqNum);
                        Debug.Write("已获授权\n");
                        IsLogin = true;

                        OnVerifySucess?.Invoke();
                        break;
                    }
                    else if (authresult.Contains("二维码认证中"))
                    {
                        AuthStatus.authStatus = "二维码已扫描，等待授权";
                        AuthStatus.StatusCode = 2;
                        //OnVerifyImage?.Invoke(UserLogo);
                        OnVerifyImage?.Invoke();

                    }
                    else if (authresult.Contains("二维码未失效"))
                    {
                        OnScanImage?.Invoke();
                        AuthStatus.authStatus = "等待二维码扫描及授权";
                        AuthStatus.StatusCode = 1;

                    }
                    else if (authresult.Contains("65"))
                    {
                        Debug.Write("[*] 正在生成二维码 ....");

                        OnGetQRCodeImage?.Invoke(QRImg);
                    }
                }

            } while (!IsLogin);




        }



    }
    }
