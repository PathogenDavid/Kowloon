using System;

namespace Kowloon.Core
{
    public class ChaserPattern : IRenderer
    {
        private readonly KowloonController Controller;

        internal ChaserPattern(KowloonController controller)
            => Controller = controller;

        public void Render(bool isFirstFrame)
        {
            int currentPart = (Controller.FrameNumber / 6) % 10;
            Span<int> leds = Controller.Leds;

            for (int i = 0; i < leds.Length; i++)
            {
                leds[i] = currentPart < 5 ? 0x00FFFFFF : 0x00000000;

                currentPart++;

                if (currentPart >= 10)
                { currentPart = 0; }
            }
        }
    }
}
