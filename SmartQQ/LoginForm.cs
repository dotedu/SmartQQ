using SmartQQLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
                    
                    MessageBox.Show("ptwebqq="+qc.LoginCookies.ptwebqq);
                    MessageBox.Show("vfwebqq=" + qc.LoginCookies.vfwebqq);
                    MessageBox.Show("skey=" + qc.LoginCookies.skey);
                    MessageBox.Show("psessionid=" + qc.LoginCookies.psessionid);
                    MessageBox.Show("uin=" + qc.LoginCookies.uin.ToString());
                });
            };


            RunAsync(() => {
                qc.Run();
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
