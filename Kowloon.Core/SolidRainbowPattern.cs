using System;

namespace Kowloon.Core
{
    public class SolidRainbowPattern : IRenderer
    {
        private readonly KowloonController Controller;

        internal SolidRainbowPattern(KowloonController controller)
            => Controller = controller;

        public void Render(bool isFirstFrame)
        {
            int color = ColorHelper.GetRainbowColor((Controller.Timestamp * 100.0) % 360.0);
            Controller.Leds.Fill(color);
        }
    }
}
