using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 螢幕擷取工具
{
    // ResultForm.cs
    public partial class ResultForm : Form
    {
        public ResultForm(string resultText)
        {
            InitializeComponent();
            txtResult.Text = resultText;
        }

        // 按鈕動態效果
        private void btnCopy_MouseEnter(object sender, EventArgs e)
        {
            btnCopy.BackColor = Color.FromArgb(0, 86, 179);
        }

        private void btnCopy_MouseLeave(object sender, EventArgs e)
        {
            btnCopy.BackColor = Color.FromArgb(0, 123, 255);
        }

        // 複製功能
        private void btnCopy_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtResult.Text))
            {
                Clipboard.SetText(txtResult.Text);
                ShowToast("✓ 已複製到剪貼簿");
            }
        }

        // Toast提示
        private void ShowToast(string message)
        {
            var lblToast = new Label
            {
                Text = message,
                ForeColor = Color.Green,
                BackColor = Color.WhiteSmoke,
                AutoSize = false,
                Size = new Size(140, 25),
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(btnCopy.Left - 150, btnCopy.Top)
            };

            Controls.Add(lblToast);
            lblToast.BringToFront();

            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer { Interval = 1500 };
            timer.Tick += (s, e) =>
            {
                Controls.Remove(lblToast);
                lblToast.Dispose();
                timer.Dispose();
            };
            timer.Start();
        }
    }
}
