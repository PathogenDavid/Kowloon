using System;
using System.Threading;
using System.Windows.Forms;

namespace Kowloon.LedControl.Simulator
{
    public class WinFormsLedString : LedString
    {
        private readonly int[] _Leds;
        public override Span<int> Leds => _Leds;
        public override byte Brightness { get; set; } = 255;

        internal int FrameNumber { get; private set; } = 0;

        private readonly LedStringForm Form;
        private readonly Thread UiThread;

        public bool IsDisposing { get; private set; } = false;

        public WinFormsLedString(int ledCount)
        {
            _Leds = new int[ledCount];
            Form = new LedStringForm(this);

            UiThread = new Thread(UiThreadEntry);
            UiThread.SetApartmentState(ApartmentState.STA);
            UiThread.Start();

            while (!Form.Visible)
            { }
        }

        public override void Render()
        {
            FrameNumber++;
            Form.Invoke(new Action(() => Form.Invalidate()));
        }

        private void UiThreadEntry()
        {
            UiThread.Name = "Simulator UI Thread";
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(Form);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                IsDisposing = true;
                Form.Invoke(new Action(() => Form.Close()));
                UiThread.Join();
            }
        }
    }
}
