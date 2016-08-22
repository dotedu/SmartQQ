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
            ((System.ComponentModel.ISupportInitialize)(this.QrCodePicture)).BeginInit();
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
            this.LoginLabel.Text = "label1";
            this.LoginLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.LoginLabel);
            this.Controls.Add(this.QrCodePicture);
            this.Name = "LoginForm";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.LoginForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.QrCodePicture)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox QrCodePicture;
        private System.Windows.Forms.Label LoginLabel;
    }
}

