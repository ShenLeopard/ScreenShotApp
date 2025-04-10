namespace 螢幕擷取工具
{
    partial class Form1
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnCaptureScreen = new System.Windows.Forms.Button();
            this.btnRecognizeText = new System.Windows.Forms.Button();
            this.pictureBoxCanvas = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCanvas)).BeginInit();
            this.SuspendLayout();
            //
            // btnCaptureScreen 擷取畫面按鈕
            //
            this.btnCaptureScreen.Location = new System.Drawing.Point(12, 12);
            this.btnCaptureScreen.Name = "btnCaptureScreen";
            this.btnCaptureScreen.Size = new System.Drawing.Size(120, 35);
            this.btnCaptureScreen.TabIndex = 0;
            this.btnCaptureScreen.Text = "擷取畫面";
            this.btnCaptureScreen.UseVisualStyleBackColor = true;
            this.btnCaptureScreen.Click += new System.EventHandler(this.btnCaptureScreen_Click);
            //
            // btnRecognizeText 辨識文字按鈕
            //
            this.btnRecognizeText.Location = new System.Drawing.Point(150, 12);
            this.btnRecognizeText.Name = "btnRecognizeText";
            this.btnRecognizeText.Size = new System.Drawing.Size(120, 35);
            this.btnRecognizeText.TabIndex = 1;
            this.btnRecognizeText.Text = "辨識文字";
            this.btnRecognizeText.UseVisualStyleBackColor = true;
            this.btnRecognizeText.Click += new System.EventHandler(this.btnRecognizeText_Click);
            //
            // pictureBoxCanvas
            //
            this.pictureBoxCanvas.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBoxCanvas.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxCanvas.Location = new System.Drawing.Point(12, 60);
            this.pictureBoxCanvas.Name = "pictureBoxCanvas";
            this.pictureBoxCanvas.Size = new System.Drawing.Size(776, 378);
            this.pictureBoxCanvas.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom; // 讓圖片縮放以符合畫布
            this.pictureBoxCanvas.TabIndex = 2;
            this.pictureBoxCanvas.TabStop = false;
            //
            // Form1
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F); // 字體和解析度根據需要調整
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.pictureBoxCanvas);
            this.Controls.Add(this.btnRecognizeText);
            this.Controls.Add(this.btnCaptureScreen);
            this.Name = "Form1";
            this.Text = "螢幕文字擷取器"; // 設定視窗標題
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCanvas)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCaptureScreen;
        private System.Windows.Forms.Button btnRecognizeText;
        private System.Windows.Forms.PictureBox pictureBoxCanvas;
    }
}
