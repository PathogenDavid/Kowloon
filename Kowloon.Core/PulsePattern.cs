using System;

namespace Kowloon.Core
{
    public class PulsePattern : IRenderer
    {
        private readonly KowloonController Controller;
        private const double Range = 0.9;

        internal PulsePattern(KowloonController controller)
            => Controller = controller;

        public void Render(bool isFirstFrame)
        {
            byte value = (byte)((Math.Abs(Math.Sin(Controller.Timestamp)) * Range + (1.0 - Range)) * 255.0);
            int color = value | value << 8 | value << 16;
            Controller.Leds.Fill(color);
        }
    }
}
