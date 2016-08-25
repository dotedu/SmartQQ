using SmartQQLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SmartQQ
{
    public partial class LoginForm : Form
    {
        SmartQQClient qc;
        public LoginForm()
        {
            InitializeComponent();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            qc = new SmartQQClient();

            qc.BeginReLogin = () => {
                RunInMainthread(() => {
                    //QrCodePicture.Image = UserLogo;
                    LoginLabel.Text = "自动登录中";
                });
            };
            qc.ReLoginFail = () => {
                RunInMainthread(() => {
                    //QrCodePicture.Image = UserLogo;
                    LoginLabel.Text = "自动登录失败，正在获取二维码";
                });
            };

            qc.OnGetQRCodeImage = (image) => {
                RunInMainthread(() => {
                    QrCodePicture.Image = image;
                    LoginLabel.Text = "使用QQ手机版扫描二维码";
                });
            };
            qc.OnVerifyImage = () => {
                RunInMainthread(() => {
                    //QrCodePicture.Image = UserLogo;
                    LoginLabel.Text = "二维码已扫描，等待授权";
                });
            };
            qc.OnVerifySucess = () => {
                RunInMainthread(() => {
                    LoginLabel.Text = "已确认,正在登陆....";
                });
            };

            qc.OnLoginSucess = () => {
                RunInMainthread(() => {
                    LoginLabel.Text = "登录成功";

                    //MessageBox.Show("ptwebqq="+qc.LoginCookies.ptwebqq);
                    //MessageBox.Show("vfwebqq=" + qc.LoginCookies.vfwebqq);
                    //MessageBox.Show("skey=" + qc.LoginCookies.skey);
                    //MessageBox.Show("psessionid=" + qc.loingresult.psessionid);
                    //MessageBox.Show("uin=" + qc.LoginCookies.uin.ToString());
                });
            };


            RunAsync(() => {
                if (File.Exists(Application.StartupPath + "\\cookie.data"))
                {
                    qc.ReLink();

                }
                else
                {
                    qc.Run();
                }
            });

            //MessageBox.Show(SmartQQLib.API.Cookies.ptwebqq);

        }

        void RunAsync(Action action)
        {
            ((Action)(delegate () {
                action?.Invoke();
            })).BeginInvoke(null, null);
        }

        void RunInMainthread(Action action)
        {
            this.BeginInvoke((Action)(delegate () {
                action?.Invoke();
            }));
        }

    }
}
