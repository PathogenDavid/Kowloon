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
            { Controller.Leds.Fill(0xFFFFFF); }
        }
    }
}
