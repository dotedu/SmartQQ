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
            RunAsync(() => {
                qc.Run();
            });

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
