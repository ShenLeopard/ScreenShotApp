using GeminiApi.Services;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace 螢幕擷取工具;
public partial class Form1 : Form
{
    // Windows API for global hotkey
    [DllImport("user32.dll")]
    private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

    [DllImport("user32.dll")]
    private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

    private const int HOTKEY_ID = 1;
    private const uint MOD_CONTROL = 0x0002;
    private const uint MOD_SHIFT = 0x0004;
    private const uint VK_F1 = 0x70; // F1 key

    private System.Windows.Forms.Timer delayTimer;
    private System.Windows.Forms.Timer countdownTimer;
    private int delaySeconds = 3;
    private int remainingSeconds = 0;
    private CountdownForm countdownForm;

    public Form1()
    {
        InitializeComponent();

        // 初始化延遲計時器 (只觸發一次)
        delayTimer = new System.Windows.Forms.Timer();
        delayTimer.Tick += DelayTimer_Tick;

        // 初始化倒數計時器 (每秒更新一次 UI)
        countdownTimer = new System.Windows.Forms.Timer();
        countdownTimer.Interval = 1000;
        countdownTimer.Tick += CountdownTimer_Tick;

        // 註冊全域快捷鍵 Ctrl+Shift+F1
        RegisterHotKey(this.Handle, HOTKEY_ID, MOD_CONTROL | MOD_SHIFT, VK_F1);

        // 綁定延遲秒數變更事件
        numericUpDownDelay.ValueChanged += (s, e) =>
        {
            delaySeconds = (int)numericUpDownDelay.Value;
            UpdateDelayInfo();
        };

        UpdateDelayInfo();
    }

    // 倒數計時器邏輯
    private void CountdownTimer_Tick(object sender, EventArgs e)
    {
        remainingSeconds--;

        if (remainingSeconds > 0)
        {
            countdownForm?.UpdateCountdown(remainingSeconds);
        }
    }

    private void UpdateDelayInfo()
    {
        if (delaySeconds == 0)
        {
            lblDelayInfo.Text = "⚡ 立即截圖模式";
            lblDelayInfo.ForeColor = Color.Green;
        }
        else
        {
            lblDelayInfo.Text = $"⏱️ {delaySeconds} 秒後自動截圖";
            lblDelayInfo.ForeColor = Color.OrangeRed;
        }
    }

    protected override void WndProc(ref Message m)
    {
        const int WM_HOTKEY = 0x0312;

        if (m.Msg == WM_HOTKEY && m.WParam.ToInt32() == HOTKEY_ID)
        {
            StartDelayedCapture();
        }

        base.WndProc(ref m);
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        UnregisterHotKey(this.Handle, HOTKEY_ID);
        base.OnFormClosing(e);
    }

    private void StartDelayedCapture()
    {
        if (delaySeconds > 0)
        {
            // 隱藏主視窗
            this.Hide();

            remainingSeconds = delaySeconds;

            // 建立倒數視窗
            countdownForm = new CountdownForm(remainingSeconds);

            // !!! 關鍵修改：訂閱取消事件 !!!
            countdownForm.RequestCancel += (s, e) => CancelCapture();

            countdownForm.Show();

            // 啟動計時器
            delayTimer.Interval = delaySeconds * 1000;
            delayTimer.Start();
            countdownTimer.Start();
        }
        else
        {
            PerformCapture();
        }
    }
    private void CancelCapture()
    {
        // 1. 停止所有計時器 (這就是之前缺少的關鍵！)
        delayTimer.Stop();
        countdownTimer.Stop();

        // 2. 關閉倒數視窗
        if (countdownForm != null && !countdownForm.IsDisposed)
        {
            countdownForm.Close();
            countdownForm = null;
        }

        // 3. 恢復主視窗顯示
        this.Show();
        this.Activate(); // 確保主視窗回到最上層並取得焦點
    }

    private void DelayTimer_Tick(object sender, EventArgs e)
    {
        delayTimer.Stop();
        countdownTimer.Stop();

        countdownForm?.Close();
        countdownForm = null;

        PerformCapture();
    }

    // --- 延遲截圖按鈕 ---
    private void btnCaptureScreen_Click(object sender, EventArgs e)
    {
        StartDelayedCapture();
    }


    // --- 立即截圖按鈕 ---
    private void btnCaptureNow_Click(object sender, EventArgs e)
    {
        PerformCapture();
    }

    // 實際執行截圖的方法
    private void PerformCapture()
    {
        // 1. 確保視窗隱藏 (如果是立即截圖模式，視窗還在，所以這裡要 Hide)
        // 如果是延遲模式，StartDelayedCapture 已經 Hide 過了，再 Hide 一次也沒問題
        if (this.Visible)
        {
            this.Hide();
            System.Threading.Thread.Sleep(200); // 等待隱藏動畫
        }
        else
        {
            // 延遲模式下視窗早已隱藏，稍微等待確保倒數視窗完全關閉
            System.Threading.Thread.Sleep(100);
        }

        // 2. 擷取整個螢幕
        Bitmap screenBitmap = CaptureFullScreen();

        if (screenBitmap != null)
        {
            // 3. 顯示選擇區域表單
            using (ScreenCaptureForm captureForm = new ScreenCaptureForm(screenBitmap))
            {
                if (captureForm.ShowDialog() == DialogResult.OK)
                {
                    Rectangle selectedArea = captureForm.SelectedRectangle;

                    if (selectedArea.Width > 0 && selectedArea.Height > 0)
                    {
                        Bitmap capturedImage = new Bitmap(selectedArea.Width, selectedArea.Height);
                        using (Graphics g = Graphics.FromImage(capturedImage))
                        {
                            g.DrawImage(screenBitmap,
                                        new Rectangle(0, 0, selectedArea.Width, selectedArea.Height),
                                        selectedArea,
                                        GraphicsUnit.Pixel);
                        }

                        pictureBoxCanvas.Image?.Dispose();
                        pictureBoxCanvas.Image = capturedImage;
                    }
                }
            }
            screenBitmap.Dispose();
        }
        else
        {
            MessageBox.Show("無法擷取螢幕畫面。", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        // 4. 恢復視窗狀態 - [優化] 移除多餘的 WindowState 設定
        this.Show();
        this.Activate(); // 關鍵：讓視窗回到前景並取得焦點
    }

    // 🔧 修正 #7: 改善多螢幕截圖支援
    private Bitmap CaptureFullScreen()
    {
        try
        {
            Rectangle totalBounds = Rectangle.Empty;
            foreach (Screen screen in Screen.AllScreens)
            {
                totalBounds = Rectangle.Union(totalBounds, screen.Bounds);
            }

            Bitmap screenBitmap = new Bitmap(totalBounds.Width, totalBounds.Height, PixelFormat.Format32bppArgb);
            using (Graphics g = Graphics.FromImage(screenBitmap))
            {
                g.Clear(Color.Transparent);
                g.CopyFromScreen(totalBounds.Location, new Point(0, 0), totalBounds.Size, CopyPixelOperation.SourceCopy);
            }
            return screenBitmap;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"截圖時發生錯誤: {ex.Message}");
            return null;
        }
    }

    // --- 辨識文字按鈕 ---
    private async void btnRecognizeText_Click(object sender, EventArgs e)
    {
        if (pictureBoxCanvas.Image == null)
        {
            MessageBox.Show("請先擷取畫面。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        try
        {
            this.Cursor = Cursors.WaitCursor;
            btnRecognizeText.Enabled = false;
            btnCaptureScreen.Enabled = false;
            btnCaptureNow.Enabled = false;

            using (Bitmap imageToRecognize = new Bitmap(pictureBoxCanvas.Image))
            {
                await GeminiApiService.CallGeminiApiAsync(imageToRecognize);
            }
        }
        finally
        {
            this.Cursor = Cursors.Default;
            btnRecognizeText.Enabled = true;
            btnCaptureScreen.Enabled = true;
            btnCaptureNow.Enabled = true;
        }
    }
}

// --- 用於選擇擷取區域的輔助表單 ---
internal class ScreenCaptureForm : Form
{
    private Point _startPoint;
    private Rectangle _selectionRectangle;
    private bool _isDragging = false;
    private readonly Bitmap _backgroundBitmap;
    private Cursor _customCursor; // 🔧 修正 #5: 儲存自訂游標以便釋放

    public Rectangle SelectedRectangle => _selectionRectangle;

    public ScreenCaptureForm(Bitmap background)
    {
        _backgroundBitmap = background;

        this.FormBorderStyle = FormBorderStyle.None;
        this.WindowState = FormWindowState.Normal;
        this.StartPosition = FormStartPosition.Manual;
        Rectangle totalBounds = Rectangle.Empty;
        foreach (Screen screen in Screen.AllScreens)
        {
            totalBounds = Rectangle.Union(totalBounds, screen.Bounds);
        }
        this.Bounds = totalBounds;
        this.TopMost = true;

        // 🔧 修正 #5: 儲存游標參考
        _customCursor = CreateHighContrastCrossCursor();
        this.Cursor = _customCursor;

        this.DoubleBuffered = true;
        this.BackgroundImage = _backgroundBitmap;
        this.BackgroundImageLayout = ImageLayout.None;

        this.MouseDown += CaptureForm_MouseDown;
        this.MouseMove += CaptureForm_MouseMove;
        this.MouseUp += CaptureForm_MouseUp;
        this.Paint += CaptureForm_Paint;
        this.KeyDown += CaptureForm_KeyDown;

        // 🔧 修正 #9: 顯示操作提示
        this.Load += (s, e) =>
        {
            // 可選: 在視窗上顯示提示文字
            ShowInstructions();
        };
    }

    // 🔧 修正 #9: 新增操作說明
    private void ShowInstructions()
    {
        // 在螢幕中央上方顯示提示
        using (Graphics g = this.CreateGraphics())
        {
            string instruction = "拖曳滑鼠選擇區域 | 按 ESC 取消";
            Font font = new Font("微軟正黑體", 14, FontStyle.Bold);
            SizeF textSize = g.MeasureString(instruction, font);

            Point location = new Point(
                (this.Width - (int)textSize.Width) / 2,
                30
            );

            // 繪製半透明背景
            using (SolidBrush bgBrush = new SolidBrush(Color.FromArgb(200, 0, 0, 0)))
            {
                g.FillRectangle(bgBrush,
                    location.X - 10,
                    location.Y - 5,
                    textSize.Width + 20,
                    textSize.Height + 10
                );
            }

            // 繪製文字
            using (SolidBrush textBrush = new SolidBrush(Color.White))
            {
                g.DrawString(instruction, font, textBrush, location);
            }

            font.Dispose();
        }
    }

    // 🔧 修正 #5: 改善游標建立與釋放
    private Cursor CreateHighContrastCrossCursor()
    {
        const int size = 32;
        Bitmap bmp = new Bitmap(size, size, PixelFormat.Format32bppArgb);

        using (Graphics g = Graphics.FromImage(bmp))
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(Color.Transparent);

            int thickness = 3;
            int half = size / 2;

            using (Pen penBorder = new Pen(Color.Black, thickness + 2))
            {
                g.DrawLine(penBorder, half, 0, half, size);
                g.DrawLine(penBorder, 0, half, size, half);
            }
            using (Pen penInner = new Pen(Color.White, thickness))
            {
                g.DrawLine(penInner, half, 1, half, size - 2);
                g.DrawLine(penInner, 1, half, size - 2, half);
            }
        }

        IntPtr hIcon = bmp.GetHicon();
        Cursor cursor = new Cursor(hIcon);

        // 🔧 修正 #5: 釋放 Bitmap
        bmp.Dispose();

        // 注意: hIcon 應該在 Cursor 不再使用時釋放，但 .NET 會自動處理

        return cursor;
    }

    private void CaptureForm_MouseDown(object sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left)
        {
            _startPoint = e.Location;
            _isDragging = true;
            _selectionRectangle = new Rectangle(_startPoint, Size.Empty);
        }
    }

    private void CaptureForm_MouseMove(object sender, MouseEventArgs e)
    {
        if (_isDragging)
        {
            Rectangle prevRect = _selectionRectangle;

            int x = Math.Min(_startPoint.X, e.X);
            int y = Math.Min(_startPoint.Y, e.Y);
            int width = Math.Abs(_startPoint.X - e.X);
            int height = Math.Abs(_startPoint.Y - e.Y);
            _selectionRectangle = new Rectangle(x, y, width, height);

            Rectangle invalidateRect = Rectangle.Union(prevRect, _selectionRectangle);
            invalidateRect.Inflate(2, 2);

            this.Invalidate(invalidateRect);
        }
    }

    private void CaptureForm_MouseUp(object sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left)
        {
            _isDragging = false;

            if (_selectionRectangle.Width > 0 && _selectionRectangle.Height > 0)
            {
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                this.DialogResult = DialogResult.Cancel;
            }
            this.Close();
        }
    }

    private void CaptureForm_Paint(object sender, PaintEventArgs e)
    {
        using (SolidBrush semiTransparentBrush = new SolidBrush(Color.FromArgb(120, 0, 0, 0)))
        {
            e.Graphics.FillRectangle(semiTransparentBrush, this.ClientRectangle);
        }

        if (_isDragging && _selectionRectangle.Width > 0 && _selectionRectangle.Height > 0)
        {
            e.Graphics.DrawImage(
                _backgroundBitmap,
                _selectionRectangle,
                _selectionRectangle,
                GraphicsUnit.Pixel
            );

            using (Pen borderPen = new Pen(Color.Red, 2))
            {
                e.Graphics.DrawRectangle(borderPen, _selectionRectangle);
            }

            // 🔧 額外改善: 顯示選取區域尺寸
            if (_selectionRectangle.Width > 50 && _selectionRectangle.Height > 50)
            {
                string sizeText = $"{_selectionRectangle.Width} × {_selectionRectangle.Height}";
                using (Font font = new Font("Arial", 10))
                using (SolidBrush bgBrush = new SolidBrush(Color.FromArgb(180, 0, 0, 0)))
                using (SolidBrush textBrush = new SolidBrush(Color.White))
                {
                    SizeF textSize = e.Graphics.MeasureString(sizeText, font);
                    Point textPos = new Point(
                        _selectionRectangle.X + 5,
                        _selectionRectangle.Y + 5
                    );

                    e.Graphics.FillRectangle(bgBrush,
                        textPos.X,
                        textPos.Y,
                        textSize.Width + 4,
                        textSize.Height + 2
                    );
                    e.Graphics.DrawString(sizeText, font, textBrush, textPos);
                }
            }
        }
    }

    private void CaptureForm_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Escape)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }

    // 🔧 修正 #5: 正確釋放游標資源
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _customCursor?.Dispose();
        }
        base.Dispose(disposing);
    }
}

// --- 倒數提示視窗 ---
internal class CountdownForm : Form
{
    private Label lblCountdown;
    private Label lblHint; // 新增提示文字

    // 定義一個事件，當使用者想取消時觸發
    public event EventHandler RequestCancel;

    public CountdownForm(int initialSeconds)
    {
        // 視窗設定
        this.FormBorderStyle = FormBorderStyle.None;
        this.StartPosition = FormStartPosition.Manual;
        this.Size = new Size(200, 120); // 🔧 增加高度以容納說明文字
        this.TopMost = true;
        this.BackColor = Color.FromArgb(40, 40, 40);
        this.Opacity = 0.9;
        this.ShowInTaskbar = false; // 不顯示在工作列

        // 設定位置在螢幕右下角
        Rectangle workingArea = Screen.PrimaryScreen.WorkingArea;
        this.Location = new Point(
            workingArea.Right - this.Width - 20,
            workingArea.Bottom - this.Height - 20
        );
        this.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));

        // 1. 倒數標籤
        lblCountdown = new Label();
        lblCountdown.Text = $"{initialSeconds}";
        lblCountdown.Font = new Font("Segoe UI", 48, FontStyle.Bold); // 改用現代字體
        lblCountdown.ForeColor = Color.White;
        lblCountdown.TextAlign = ContentAlignment.MiddleCenter;
        lblCountdown.Dock = DockStyle.Top;
        lblCountdown.Height = 80;
        this.Controls.Add(lblCountdown);

        // 2. 新增提示標籤 (讓使用者知道可以點擊取消)
        lblHint = new Label();
        lblHint.Text = "(點擊以取消)";
        lblHint.Font = new Font("微軟正黑體", 9, FontStyle.Regular);
        lblHint.ForeColor = Color.Gray;
        lblHint.TextAlign = ContentAlignment.TopCenter;
        lblHint.Dock = DockStyle.Fill;
        this.Controls.Add(lblHint);

        // 3. 綁定點擊事件 (點擊表單或標籤都要觸發)
        this.Click += TriggerCancel;
        lblCountdown.Click += TriggerCancel;
        lblHint.Click += TriggerCancel;
    }
    private void TriggerCancel(object sender, EventArgs e)
    {
        // 觸發事件通知主視窗
        RequestCancel?.Invoke(this, EventArgs.Empty);
    }

    [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
    private static extern IntPtr CreateRoundRectRgn(
        int nLeftRect, int nTopRect, int nRightRect, int nBottomRect,
        int nWidthEllipse, int nHeightEllipse);

    public void UpdateCountdown(int seconds)
    {
        if (lblCountdown != null && !this.IsDisposed)
        {
            lblCountdown.Text = $"{seconds}";
        }
    }
}