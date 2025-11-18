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
    private System.Windows.Forms.Timer countdownTimer; // 新增倒數計時器
    private int delaySeconds = 3; // 預設延遲3秒
    private int remainingSeconds = 0; // 剩餘秒數
    private CountdownForm countdownForm; // 倒數顯示表單

    public Form1()
    {
        InitializeComponent();

        // 初始化延遲計時器
        delayTimer = new System.Windows.Forms.Timer();
        delayTimer.Tick += DelayTimer_Tick;

        // 初始化倒數計時器 (每秒更新一次)
        countdownTimer = new System.Windows.Forms.Timer();
        countdownTimer.Interval = 1000; // 1秒
        countdownTimer.Tick += CountdownTimer_Tick;

        // 註冊全域快捷鍵 Ctrl+Shift+F1
        RegisterHotKey(this.Handle, HOTKEY_ID, MOD_CONTROL | MOD_SHIFT, VK_F1);

        // 綁定延遲秒數變更事件
        numericUpDownDelay.ValueChanged += (s, e) =>
        {
            delaySeconds = (int)numericUpDownDelay.Value;
            UpdateDelayInfo();
        };

        // 初始化顯示
        UpdateDelayInfo();
    }

    private void CountdownTimer_Tick(object sender, EventArgs e)
    {
        remainingSeconds--;

        if (remainingSeconds > 0)
        {
            // 更新倒數顯示
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
            // 快捷鍵被按下,開始延遲截圖
            StartDelayedCapture();
        }

        base.WndProc(ref m);
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        // 取消註冊快捷鍵
        UnregisterHotKey(this.Handle, HOTKEY_ID);
        base.OnFormClosing(e);
    }

    private void StartDelayedCapture()
    {
        if (delaySeconds > 0)
        {
            // 設定剩餘秒數
            remainingSeconds = delaySeconds;

            // 顯示倒數提示視窗 (在螢幕右下角)
            countdownForm = new CountdownForm(remainingSeconds);
            countdownForm.Show();

            // 這裡不再需要最小化視窗，因為 PerformCapture() 會處理隱藏

            // 啟動延遲計時器
            delayTimer.Interval = delaySeconds * 1000;
            delayTimer.Start();

            // 啟動倒數計時器
            countdownTimer.Start();
        }
        else
        {
            // 無延遲,立即截圖
            PerformCapture();
        }
    }

    private void DelayTimer_Tick(object sender, EventArgs e)
    {
        delayTimer.Stop();
        countdownTimer.Stop();

        // 關閉倒數視窗
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
        // 1. 隱藏主視窗
        this.Hide();
        System.Threading.Thread.Sleep(200);

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

        // 4. 恢復視窗狀態 - 確保視窗完全顯示並獲得焦點
        this.WindowState = FormWindowState.Normal;
        this.Show();
        this.BringToFront(); // 將視窗帶到最前面
        this.Activate();     // 啟動視窗並獲得焦點
        this.Focus();        // 確保視窗有焦點
    }

    // 輔助方法：擷取完整螢幕
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
    private Rectangle _prevSelectionRectangle = Rectangle.Empty;

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
        this.Cursor = CreateHighContrastCrossCursor();
        this.DoubleBuffered = true;
        this.BackgroundImage = _backgroundBitmap;
        this.BackgroundImageLayout = ImageLayout.None;

        this.MouseDown += CaptureForm_MouseDown;
        this.MouseMove += CaptureForm_MouseMove;
        this.MouseUp += CaptureForm_MouseUp;
        this.Paint += CaptureForm_Paint;
        this.KeyDown += CaptureForm_KeyDown;
    }

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
        return new Cursor(hIcon);
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
            _prevSelectionRectangle = _selectionRectangle;
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

            using (Pen borderPen = new Pen(Color.Red, 1))
            {
                e.Graphics.DrawRectangle(borderPen, _selectionRectangle);
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

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
    }
}

// --- 倒數提示視窗 ---
internal class CountdownForm : Form
{
    private Label lblCountdown;

    public CountdownForm(int initialSeconds)
    {
        // 視窗設定
        this.FormBorderStyle = FormBorderStyle.None;
        this.StartPosition = FormStartPosition.Manual;
        this.Size = new Size(200, 100);
        this.TopMost = true;
        this.BackColor = Color.FromArgb(40, 40, 40);
        this.Opacity = 0.9;

        // 設定位置在螢幕右下角
        Rectangle workingArea = Screen.PrimaryScreen.WorkingArea;
        this.Location = new Point(
            workingArea.Right - this.Width - 20,
            workingArea.Bottom - this.Height - 20
        );

        // 倒數標籤
        lblCountdown = new Label();
        lblCountdown.Dock = DockStyle.Fill;
        lblCountdown.Text = $"{initialSeconds}";
        lblCountdown.Font = new Font("微軟正黑體", 48, FontStyle.Bold);
        lblCountdown.ForeColor = Color.White;
        lblCountdown.TextAlign = ContentAlignment.MiddleCenter;
        this.Controls.Add(lblCountdown);

        // 加上圓角效果 (optional)
        this.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
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