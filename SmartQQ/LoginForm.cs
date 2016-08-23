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
                    
                    MessageBox.Show("ptwebqq="+SmartQQLib.API.Cookies.ptwebqq);
                    MessageBox.Show("vfwebqq=" + SmartQQLib.API.Cookies.vfwebqq);
                    MessageBox.Show("skey=" + SmartQQLib.API.Cookies.skey);
                    MessageBox.Show("psessionid=" + SmartQQLib.API.Cookies.psessionid);
                    MessageBox.Show("uin=" + SmartQQLib.API.Cookies.uin.ToString());
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
