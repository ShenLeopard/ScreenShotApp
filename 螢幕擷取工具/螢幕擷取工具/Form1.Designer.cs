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
            // 初始化控制項
            this.panelHeader = new System.Windows.Forms.Panel();
            this.btnCaptureNow = new System.Windows.Forms.Button();
            this.groupBoxDelay = new System.Windows.Forms.GroupBox();
            this.btnCaptureScreen = new System.Windows.Forms.Button();
            this.lblDelayInfo = new System.Windows.Forms.Label();
            this.numericUpDownDelay = new System.Windows.Forms.NumericUpDown();
            this.panelFooter = new System.Windows.Forms.Panel();
            this.btnRecognizeText = new System.Windows.Forms.Button();
            this.panelCanvasContainer = new System.Windows.Forms.Panel();
            this.pictureBoxCanvas = new System.Windows.Forms.PictureBox();

            // 開始佈局設定
            this.panelHeader.SuspendLayout();
            this.groupBoxDelay.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDelay)).BeginInit();
            this.panelFooter.SuspendLayout();
            this.panelCanvasContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCanvas)).BeginInit();
            this.SuspendLayout();

            // 
            // Form1 設定 (現代深色風格)
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(32, 32, 32); // 深色背景
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.White;
            this.Name = "Form1";
            this.Text = "AI 螢幕文字擷取助手";
            this.MinimumSize = new System.Drawing.Size(600, 500);

            // 
            // [區域 1] 頂部面板 (放置截圖功能)
            // 
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Height = 100;
            this.panelHeader.BackColor = System.Drawing.Color.FromArgb(45, 45, 48);
            this.panelHeader.Padding = new System.Windows.Forms.Padding(20);
            this.panelHeader.Controls.Add(this.btnCaptureNow); // 立即截圖放左邊
            this.panelHeader.Controls.Add(this.groupBoxDelay); // 延遲設定放右邊

            // 
            // btnCaptureNow (立即截圖 - 主按鈕)
            // 
            this.btnCaptureNow.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnCaptureNow.Width = 220;
            this.btnCaptureNow.Text = "⚡ 立即截圖";
            this.btnCaptureNow.BackColor = System.Drawing.Color.FromArgb(0, 120, 215); // 鮮豔藍色
            this.btnCaptureNow.ForeColor = System.Drawing.Color.White;
            this.btnCaptureNow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCaptureNow.FlatAppearance.BorderSize = 0;
            this.btnCaptureNow.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.btnCaptureNow.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCaptureNow.Click += new System.EventHandler(this.btnCaptureNow_Click);

            // 
            // groupBoxDelay (延遲功能區塊 - 用群組框隔開，避免誤觸)
            // 
            this.groupBoxDelay.Dock = System.Windows.Forms.DockStyle.Right;
            this.groupBoxDelay.Width = 420;
            this.groupBoxDelay.Text = "⏳ 延遲模式";
            this.groupBoxDelay.ForeColor = System.Drawing.Color.LightGray;
            this.groupBoxDelay.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.groupBoxDelay.Controls.Add(this.lblDelayInfo);
            this.groupBoxDelay.Controls.Add(this.numericUpDownDelay);
            this.groupBoxDelay.Controls.Add(this.btnCaptureScreen);

            // 
            // numericUpDownDelay (秒數)
            // 
            this.numericUpDownDelay.Location = new System.Drawing.Point(15, 35);
            this.numericUpDownDelay.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.numericUpDownDelay.Size = new System.Drawing.Size(60, 32);
            this.numericUpDownDelay.Minimum = 0;
            this.numericUpDownDelay.Maximum = 10;
            this.numericUpDownDelay.Value = 3;
            this.numericUpDownDelay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;

            // 
            // btnCaptureScreen (延遲截圖按鈕 - 放在秒數旁邊)
            // 
            this.btnCaptureScreen.Location = new System.Drawing.Point(90, 30);
            this.btnCaptureScreen.Size = new System.Drawing.Size(140, 40);
            this.btnCaptureScreen.Text = "開始倒數";
            this.btnCaptureScreen.BackColor = System.Drawing.Color.FromArgb(60, 60, 60); // 深灰色，降低視覺權重
            this.btnCaptureScreen.ForeColor = System.Drawing.Color.White;
            this.btnCaptureScreen.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCaptureScreen.FlatAppearance.BorderSize = 0;
            this.btnCaptureScreen.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCaptureScreen.Click += new System.EventHandler(this.btnCaptureScreen_Click);

            // 
            // lblDelayInfo (狀態文字)
            // 
            this.lblDelayInfo.AutoSize = true;
            this.lblDelayInfo.Location = new System.Drawing.Point(245, 40);
            this.lblDelayInfo.Text = "3秒後";
            this.lblDelayInfo.ForeColor = System.Drawing.Color.Orange;

            // 
            // [區域 2] 底部面板 (放置辨識功能)
            // 這裡的策略是：把「辨識」放到最下面，讓它與「截圖」物理隔離
            // 
            this.panelFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelFooter.Height = 80;
            this.panelFooter.BackColor = System.Drawing.Color.FromArgb(40, 40, 40);
            this.panelFooter.Padding = new System.Windows.Forms.Padding(100, 15, 100, 15); // 左右留白，讓按鈕置中
            this.panelFooter.Controls.Add(this.btnRecognizeText);

            // 
            // btnRecognizeText (辨識文字 - 巨大的確認按鈕)
            // 
            this.btnRecognizeText.Dock = System.Windows.Forms.DockStyle.Fill; // 填滿底部中央
            this.btnRecognizeText.Text = "🔍 分析圖片並辨識文字"; // 加長文字，增加可點擊區域
            this.btnRecognizeText.BackColor = System.Drawing.Color.SeaGreen; // 綠色，代表「執行/通過」
            this.btnRecognizeText.ForeColor = System.Drawing.Color.White;
            this.btnRecognizeText.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRecognizeText.FlatAppearance.BorderSize = 0;
            this.btnRecognizeText.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.btnRecognizeText.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRecognizeText.Click += new System.EventHandler(this.btnRecognizeText_Click);

            // 
            // [區域 3] 中間圖片區 (自動填滿剩餘空間)
            // 
            this.panelCanvasContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelCanvasContainer.Padding = new System.Windows.Forms.Padding(20); // 圖片與邊框的距離
            this.panelCanvasContainer.Controls.Add(this.pictureBoxCanvas);

            // 
            // pictureBoxCanvas
            // 
            this.pictureBoxCanvas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxCanvas.BackColor = System.Drawing.Color.FromArgb(50, 50, 50); // 稍微亮一點的背景，區分畫布
            this.pictureBoxCanvas.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxCanvas.BorderStyle = System.Windows.Forms.BorderStyle.None; // 移除老舊邊框

            // 
            // 加入控制項 (注意順序，Dock 屬性依賴加入順序)
            // 
            this.Controls.Add(this.panelCanvasContainer); // 中間
            this.Controls.Add(this.panelHeader);          // 上方
            this.Controls.Add(this.panelFooter);          // 下方

            // 結束佈局
            this.panelHeader.ResumeLayout(false);
            this.groupBoxDelay.ResumeLayout(false);
            this.groupBoxDelay.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDelay)).EndInit();
            this.panelFooter.ResumeLayout(false);
            this.panelCanvasContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCanvas)).EndInit();
            this.ResumeLayout(false);
        }
        #endregion

        private System.Windows.Forms.Panel panelHeader; // 頂部面板
        private System.Windows.Forms.Panel panelFooter; // 底部面板
        private System.Windows.Forms.Panel panelCanvasContainer; // 圖片容器(做邊框用)
        private System.Windows.Forms.Button btnCaptureScreen;
        private System.Windows.Forms.Button btnCaptureNow;
        private System.Windows.Forms.Button btnRecognizeText;
        private System.Windows.Forms.PictureBox pictureBoxCanvas;
        private System.Windows.Forms.NumericUpDown numericUpDownDelay;
        private System.Windows.Forms.Label lblDelayInfo;
        private System.Windows.Forms.GroupBox groupBoxDelay; // 改用 GroupBox 包裝延遲功能
    }
}
