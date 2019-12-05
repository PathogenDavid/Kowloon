using System;

namespace Kowloon.Core
{
    public class SolidWhitePattern : IRenderer
    {
        private readonly KowloonController Controller;

        internal SolidWhitePattern(KowloonController controller)
            => Controller = controller;

        public void Render(bool isFirstFrame)
        {
            if (isFirstFrame)
            {
                Span<int> leds = Controller.Leds;
                for (int i = 0; i < leds.Length; i++)
                { leds[i] = 0x00FFFFFF; }
            }
        }
    }
}
