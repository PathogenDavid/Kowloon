using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Kowloon.LedControl.Simulator
{
    internal class LedStringForm : Form
    {
        private const int LedsPerLine = 50;
        private const int LedSize = 20;

        private readonly WinFormsLedString LedString;

        private SolidBrush Brush = new SolidBrush(Color.White);

        public LedStringForm(WinFormsLedString ledString)
        {
            LedString = ledString;

            int width = LedsPerLine * LedSize;
            int height = ((LedString.Leds.Length + LedsPerLine - 1) / LedsPerLine) * LedSize;
            ClientSize = new Size(width, height);

            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            DoubleBuffered = true;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Text = $"Frame: {LedString.FrameNumber}";
            Graphics g = e.Graphics;
            g.Clear(Color.FromArgb(0x1a, 0x1a, 0x1a));

            Span<int> leds = LedString.Leds;
            for (int i = 0; i < leds.Length; i++)
            {
                int row = i / LedsPerLine;
                int col = i % LedsPerLine;
                int x = col * LedSize;
                int y = row * LedSize;

                Brush.Color = Color.FromArgb(leds[i] | unchecked((int)0xFF000000));
                g.FillRectangle(Brush, x, y, LedSize, LedSize);
            }

            if (LedString.Brightness < 255)
            {
                Brush.Color = Color.FromArgb(255 - LedString.Brightness, Color.Black);
                g.FillRectangle(Brush, 0, 0, ClientSize.Width, ClientSize.Height);
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            if (!LedString.IsDisposing)
            { e.Cancel = true; }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            { Brush.Dispose(); }
        }
    }
}
