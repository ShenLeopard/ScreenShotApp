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
            this.btnCaptureNow = new System.Windows.Forms.Button();
            this.pictureBoxCanvas = new System.Windows.Forms.PictureBox();
            this.numericUpDownDelay = new System.Windows.Forms.NumericUpDown();
            this.lblDelayInfo = new System.Windows.Forms.Label();
            this.lblHotkeyInfo = new System.Windows.Forms.Label();
            this.groupBoxSettings = new System.Windows.Forms.GroupBox();
            this.labelDelaySeconds = new System.Windows.Forms.Label();

            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCanvas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDelay)).BeginInit();
            this.groupBoxSettings.SuspendLayout();
            this.SuspendLayout();

            // 
            // groupBoxSettings 設定群組
            // 
            this.groupBoxSettings.Controls.Add(this.numericUpDownDelay);
            this.groupBoxSettings.Controls.Add(this.labelDelaySeconds);
            this.groupBoxSettings.Controls.Add(this.lblDelayInfo);
            this.groupBoxSettings.Controls.Add(this.lblHotkeyInfo);
            this.groupBoxSettings.Location = new System.Drawing.Point(12, 12);
            this.groupBoxSettings.Name = "groupBoxSettings";
            this.groupBoxSettings.Size = new System.Drawing.Size(300, 130);
            this.groupBoxSettings.TabIndex = 0;
            this.groupBoxSettings.TabStop = false;
            this.groupBoxSettings.Text = "📋 截圖設定";

            // 
            // labelDelaySeconds 延遲標籤
            // 
            this.labelDelaySeconds.AutoSize = true;
            this.labelDelaySeconds.Location = new System.Drawing.Point(15, 30);
            this.labelDelaySeconds.Name = "labelDelaySeconds";
            this.labelDelaySeconds.Size = new System.Drawing.Size(80, 15);
            this.labelDelaySeconds.TabIndex = 0;
            this.labelDelaySeconds.Text = "延遲時間:";
            this.labelDelaySeconds.Font = new System.Drawing.Font("微軟正黑體", 9F);

            // 
            // numericUpDownDelay 延遲秒數選擇器
            // 
            this.numericUpDownDelay.Location = new System.Drawing.Point(100, 27);
            this.numericUpDownDelay.Name = "numericUpDownDelay";
            this.numericUpDownDelay.Size = new System.Drawing.Size(60, 25);
            this.numericUpDownDelay.TabIndex = 1;
            this.numericUpDownDelay.Minimum = 0;
            this.numericUpDownDelay.Maximum = 10;
            this.numericUpDownDelay.Value = 3;
            this.numericUpDownDelay.Font = new System.Drawing.Font("微軟正黑體", 9F);

            // 
            // lblDelayInfo 延遲資訊標籤
            // 
            this.lblDelayInfo.AutoSize = true;
            this.lblDelayInfo.Location = new System.Drawing.Point(165, 30);
            this.lblDelayInfo.Name = "lblDelayInfo";
            this.lblDelayInfo.Size = new System.Drawing.Size(120, 15);
            this.lblDelayInfo.TabIndex = 2;
            this.lblDelayInfo.Text = "⏱️ 3 秒後自動截圖";
            this.lblDelayInfo.ForeColor = System.Drawing.Color.OrangeRed;
            this.lblDelayInfo.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Bold);

            // 
            // lblHotkeyInfo 快捷鍵資訊標籤
            // 
            this.lblHotkeyInfo.Location = new System.Drawing.Point(15, 65);
            this.lblHotkeyInfo.Name = "lblHotkeyInfo";
            this.lblHotkeyInfo.Size = new System.Drawing.Size(270, 50);
            this.lblHotkeyInfo.TabIndex = 3;
            this.lblHotkeyInfo.Text = "💡 使用方式:\r\n按下 Ctrl+Shift+F1 快捷鍵\r\n或點擊「擷取畫面」按鈕";
            this.lblHotkeyInfo.ForeColor = System.Drawing.Color.FromArgb(0, 102, 204);
            this.lblHotkeyInfo.Font = new System.Drawing.Font("微軟正黑體", 9F);

            //
            // btnCaptureScreen 擷取畫面按鈕 (延遲)
            //
            this.btnCaptureScreen.Location = new System.Drawing.Point(330, 25);
            this.btnCaptureScreen.Name = "btnCaptureScreen";
            this.btnCaptureScreen.Size = new System.Drawing.Size(140, 50);
            this.btnCaptureScreen.TabIndex = 4;
            this.btnCaptureScreen.Text = "⏱️ 延遲截圖";
            this.btnCaptureScreen.UseVisualStyleBackColor = true;
            this.btnCaptureScreen.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Bold);
            this.btnCaptureScreen.BackColor = System.Drawing.Color.FromArgb(255, 152, 0);
            this.btnCaptureScreen.ForeColor = System.Drawing.Color.White;
            this.btnCaptureScreen.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCaptureScreen.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCaptureScreen.Click += new System.EventHandler(this.btnCaptureScreen_Click);

            //
            // btnCaptureNow 立即截圖按鈕 (新增)
            //
            this.btnCaptureNow.Location = new System.Drawing.Point(490, 25);
            this.btnCaptureNow.Name = "btnCaptureNow";
            this.btnCaptureNow.Size = new System.Drawing.Size(140, 50);
            this.btnCaptureNow.TabIndex = 5;
            this.btnCaptureNow.Text = "⚡ 立即截圖";
            this.btnCaptureNow.UseVisualStyleBackColor = true;
            this.btnCaptureNow.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Bold);
            this.btnCaptureNow.BackColor = System.Drawing.Color.FromArgb(76, 175, 80);
            this.btnCaptureNow.ForeColor = System.Drawing.Color.White;
            this.btnCaptureNow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCaptureNow.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCaptureNow.Click += new System.EventHandler(this.btnCaptureNow_Click);

            //
            // btnRecognizeText 辨識文字按鈕
            //
            this.btnRecognizeText.Location = new System.Drawing.Point(650, 25);
            this.btnRecognizeText.Name = "btnRecognizeText";
            this.btnRecognizeText.Size = new System.Drawing.Size(140, 50);
            this.btnRecognizeText.TabIndex = 6;
            this.btnRecognizeText.Text = "🔍 辨識文字";
            this.btnRecognizeText.UseVisualStyleBackColor = true;
            this.btnRecognizeText.Font = new System.Drawing.Font("微軟正黑體", 10F, System.Drawing.FontStyle.Bold);
            this.btnRecognizeText.BackColor = System.Drawing.Color.FromArgb(33, 150, 243);
            this.btnRecognizeText.ForeColor = System.Drawing.Color.White;
            this.btnRecognizeText.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRecognizeText.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRecognizeText.Click += new System.EventHandler(this.btnRecognizeText_Click);

            //
            // pictureBoxCanvas 圖片顯示區
            //
            this.pictureBoxCanvas.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBoxCanvas.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxCanvas.Location = new System.Drawing.Point(12, 155);
            this.pictureBoxCanvas.Name = "pictureBoxCanvas";
            this.pictureBoxCanvas.Size = new System.Drawing.Size(776, 403);
            this.pictureBoxCanvas.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxCanvas.TabIndex = 7;
            this.pictureBoxCanvas.TabStop = false;
            this.pictureBoxCanvas.BackColor = System.Drawing.Color.WhiteSmoke;

            //
            // Form1
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 570);
            this.Controls.Add(this.btnRecognizeText);
            this.Controls.Add(this.btnCaptureNow);
            this.Controls.Add(this.btnCaptureScreen);
            this.Controls.Add(this.groupBoxSettings);
            this.Controls.Add(this.pictureBoxCanvas);
            this.Name = "Form1";
            this.Text = "螢幕文字擷取器 - Screen Text Capture";
            this.MinimumSize = new System.Drawing.Size(800, 400);

            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCanvas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDelay)).EndInit();
            this.groupBoxSettings.ResumeLayout(false);
            this.groupBoxSettings.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Button btnCaptureScreen;
        private System.Windows.Forms.Button btnCaptureNow;
        private System.Windows.Forms.Button btnRecognizeText;
        private System.Windows.Forms.PictureBox pictureBoxCanvas;
        private System.Windows.Forms.NumericUpDown numericUpDownDelay;
        private System.Windows.Forms.Label lblDelayInfo;
        private System.Windows.Forms.Label lblHotkeyInfo;
        private System.Windows.Forms.GroupBox groupBoxSettings;
        private System.Windows.Forms.Label labelDelaySeconds;
    }
}