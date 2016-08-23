namespace SmartQQ
{
    partial class LoginForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.QrCodePicture = new System.Windows.Forms.PictureBox();
            this.LoginLabel = new System.Windows.Forms.Label();
            this.LogoPicture = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.QrCodePicture)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LogoPicture)).BeginInit();
            this.SuspendLayout();
            // 
            // QrCodePicture
            // 
            this.QrCodePicture.Location = new System.Drawing.Point(60, 30);
            this.QrCodePicture.Name = "QrCodePicture";
            this.QrCodePicture.Size = new System.Drawing.Size(165, 165);
            this.QrCodePicture.TabIndex = 0;
            this.QrCodePicture.TabStop = false;
            // 
            // LoginLabel
            // 
            this.LoginLabel.Location = new System.Drawing.Point(12, 211);
            this.LoginLabel.Name = "LoginLabel";
            this.LoginLabel.Size = new System.Drawing.Size(260, 23);
            this.LoginLabel.TabIndex = 1;
            this.LoginLabel.Text = "正在获取二维码";
            this.LoginLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LogoPicture
            // 
            this.LogoPicture.BackColor = System.Drawing.Color.White;
            this.LogoPicture.BackgroundImage = global::SmartQQ.Properties.Resources.icon_qq_QR;
            this.LogoPicture.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.LogoPicture.Location = new System.Drawing.Point(119, 89);
            this.LogoPicture.Name = "LogoPicture";
            this.LogoPicture.Size = new System.Drawing.Size(47, 47);
            this.LogoPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.LogoPicture.TabIndex = 2;
            this.LogoPicture.TabStop = false;
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.LogoPicture);
            this.Controls.Add(this.LoginLabel);
            this.Controls.Add(this.QrCodePicture);
            this.Name = "LoginForm";
            this.Text = "登陆框";
            this.Load += new System.EventHandler(this.LoginForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.QrCodePicture)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LogoPicture)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox QrCodePicture;
        private System.Windows.Forms.Label LoginLabel;
        private System.Windows.Forms.PictureBox LogoPicture;
    }
}

