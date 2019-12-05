using System;

namespace Kowloon.Core
{
    public class RainbowPattern : IRenderer
    {
        private readonly KowloonController Controller;

        internal RainbowPattern(KowloonController controller)
            => Controller = controller;

        public void Render(bool isFirstFrame)
        {
            double hue = Controller.Timestamp * 100.0;
            Span<int> leds = Controller.Leds;
            double hueStep = 360.0 / leds.Length;

            for (int i = 0; i < leds.Length; i++)
            {
                hue = (hue + hueStep) % 360.0;
                leds[i] = ColorHelper.GetRainbowColor(hue);
            }
        }
    }
}
