using System.Drawing;
using System.Drawing.Imaging;
using GeminiApi.Services;

namespace 螢幕擷取工具;
public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();
    }

    // --- 擷取畫面按鈕的行為 ---
    private void btnCaptureScreen_Click(object sender, EventArgs e)
    {
        // 1. 隱藏主視窗，避免擷取到自己
        this.Hide();
        // 短暫延遲確保視窗完全隱藏 (有時需要)
        System.Threading.Thread.Sleep(200);

        // 2. 擷取整個螢幕 (包含所有螢幕)
        Bitmap screenBitmap = CaptureFullScreen();

        if (screenBitmap != null)
        {
            // 3. 顯示一個全螢幕、半透明的表單，用於選擇區域
            using (ScreenCaptureForm captureForm = new ScreenCaptureForm(screenBitmap))
            {
                if (captureForm.ShowDialog() == DialogResult.OK)
                {
                    // 4. 如果使用者成功選擇了一個區域
                    Rectangle selectedArea = captureForm.SelectedRectangle;

                    // 5. 從完整螢幕截圖中，根據選擇的區域裁剪出圖片
                    if (selectedArea.Width > 0 && selectedArea.Height > 0)
                    {
                        Bitmap capturedImage = new Bitmap(selectedArea.Width, selectedArea.Height);
                        using (Graphics g = Graphics.FromImage(capturedImage))
                        {
                            g.DrawImage(screenBitmap,
                                        new Rectangle(0, 0, selectedArea.Width, selectedArea.Height), // 目的矩形 (從(0,0)開始畫)
                                        selectedArea, // 來源矩形 (從完整截圖中擷取的部分)
                                        GraphicsUnit.Pixel);
                        }

                        // 6. 將擷取的圖片顯示在 PictureBox 上
                        //    先釋放舊圖片 (如果有的話)
                        pictureBoxCanvas.Image?.Dispose();
                        pictureBoxCanvas.Image = capturedImage;
                    }
                }
            }
            // 釋放完整螢幕截圖資源
            screenBitmap.Dispose();
        }
        else
        {
            MessageBox.Show("無法擷取螢幕畫面。", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }


        // 7. 重新顯示主視窗
        this.Show();
        this.Activate(); // 確保視窗獲得焦點
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
                // 清除背景（透明）
                g.Clear(Color.Transparent);

                foreach (Screen screen in Screen.AllScreens)
                {
                    using (Bitmap screenCapture = new Bitmap(
                        screen.Bounds.Width,
                        screen.Bounds.Height,
                        PixelFormat.Format32bppArgb))
                    {
                        using (Graphics screenGraphics = Graphics.FromImage(screenCapture))
                        {
                            screenGraphics.CopyFromScreen(
                                screen.Bounds.X,
                                screen.Bounds.Y,
                                0,
                                0,
                                screen.Bounds.Size,
                                CopyPixelOperation.SourceCopy);
                        }

                        g.DrawImage(
                            screenCapture,
                            new Rectangle(
                                screen.Bounds.X - totalBounds.X,
                                screen.Bounds.Y - totalBounds.Y,
                                screen.Bounds.Width,
                                screen.Bounds.Height));
                    }
                }
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
        }
    }

}

// --- 用於選擇擷取區域的輔助表單 ---
internal class ScreenCaptureForm : Form
{
    private Point _startPoint;
    private Rectangle _selectionRectangle;
    private bool _isDragging = false;
    private readonly Bitmap _backgroundBitmap; // 儲存傳入的完整螢幕截圖

    public Rectangle SelectedRectangle => _selectionRectangle;

    public ScreenCaptureForm(Bitmap background)
    {
        _backgroundBitmap = background;

        this.FormBorderStyle = FormBorderStyle.None;
        this.WindowState = FormWindowState.Normal; // 不要最大化
        this.StartPosition = FormStartPosition.Manual;
        Rectangle totalBounds = Rectangle.Empty;
        foreach (Screen screen in Screen.AllScreens)
        {
            totalBounds = Rectangle.Union(totalBounds, screen.Bounds);
        }
        this.Bounds = totalBounds;
        this.TopMost = true;
        this.Cursor = Cursors.Cross; // 設定滑鼠指標為十字
        this.DoubleBuffered = true; // 減少繪圖閃爍

        // 設定背景為傳入的螢幕截圖 (模擬透明效果)
        this.BackgroundImage = _backgroundBitmap;
        this.BackgroundImageLayout = ImageLayout.None; // 不要縮放背景圖

        this.MouseDown += CaptureForm_MouseDown;
        this.MouseMove += CaptureForm_MouseMove;
        this.MouseUp += CaptureForm_MouseUp;
        this.Paint += CaptureForm_Paint;
        this.KeyDown += CaptureForm_KeyDown; // 允許按 ESC 取消
    }

    private void CaptureForm_MouseDown(object sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left)
        {
            _startPoint = e.Location;
            _isDragging = true;
            _selectionRectangle = new Rectangle(_startPoint, Size.Empty); // 重置矩形
        }
    }

    private void CaptureForm_MouseMove(object sender, MouseEventArgs e)
    {
        if (_isDragging)
        {
            // 計算目前的矩形範圍 (確保寬高為正)
            int x = Math.Min(_startPoint.X, e.X);
            int y = Math.Min(_startPoint.Y, e.Y);
            int width = Math.Abs(_startPoint.X - e.X);
            int height = Math.Abs(_startPoint.Y - e.Y);
            _selectionRectangle = new Rectangle(x, y, width, height);

            // 觸發重繪，更新選擇框
            this.Invalidate();
        }
    }

    private void CaptureForm_MouseUp(object sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left)
        {
            _isDragging = false;

            // 如果選擇區域有效 (寬高大於0)，則設定結果並關閉表單
            if (_selectionRectangle.Width > 0 && _selectionRectangle.Height > 0)
            {
                this.DialogResult = DialogResult.OK;
            }
            else // 否則視為取消
            {
                this.DialogResult = DialogResult.Cancel;
            }
            this.Close();
        }
    }

    private void CaptureForm_Paint(object sender, PaintEventArgs e)
    {
        // 繪製半透明遮罩效果 和 清晰的選取區域
        // 1. 先繪製一個半透明的黑色遮罩覆蓋整個畫面
        using (SolidBrush semiTransparentBrush = new SolidBrush(Color.FromArgb(120, 0, 0, 0))) // Alpha=120 的黑色
        {
            e.Graphics.FillRectangle(semiTransparentBrush, this.ClientRectangle);
        }

        // 2. 如果正在拖曳，將選擇區域的部分用原始背景圖"蓋掉"半透明遮罩，使其變清晰
        if (_isDragging && _selectionRectangle.Width > 0 && _selectionRectangle.Height > 0)
        {
            e.Graphics.DrawImage(
                _backgroundBitmap,      // 來源圖 (完整截圖)
                _selectionRectangle,    // 目的區域 (在表單上繪製的位置和大小)
                _selectionRectangle,    // 來源區域 (從完整截圖中擷取的區域)
                GraphicsUnit.Pixel
            );

            // 3. 在清晰區域周圍繪製一個紅色邊框，標示選擇範圍
            using (Pen borderPen = new Pen(Color.Red, 1))
            {
                e.Graphics.DrawRectangle(borderPen, _selectionRectangle);
            }
        }
    }

    private void CaptureForm_KeyDown(object sender, KeyEventArgs e)
    {
        // 按下 ESC 鍵取消擷取
        if (e.KeyCode == Keys.Escape)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }

    // 覆寫 Dispose 方法以釋放背景 Bitmap
    protected override void Dispose(bool disposing)
    {
        // 注意：此處不應 Dispose _backgroundBitmap，因為它是從 Form1 傳入的，
        // Form1 會負責 Dispose 它。如果在這裡 Dispose，Form1 可能會出錯。
        base.Dispose(disposing);
    }
}

