using System;

namespace Kowloon.Core
{
    public class SingleLedTest : IRenderer, IRangedTest
    {
        private readonly KowloonController Controller;

        public int MinimumValue => 0;
        public int MaximumValue => Controller.Leds.Length;
        public int Value { get; set; }

        internal SingleLedTest(KowloonController controller)
            => Controller = controller;

        public void Render(bool isFirstFrame)
        {
            Span<int> leds = Controller.Leds;
            leds.Clear();

            if (Value >= MinimumValue && Value < MaximumValue)
            {
                byte channelValue = (byte)((Math.Abs(Math.Sin(Controller.Timestamp)) * 0.75 + 0.25) * 255.0);
                int color = channelValue | channelValue << 8 | channelValue << 16;
                leds[Value] = color;
            }
        }
    }
}
