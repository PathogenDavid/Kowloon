using System;
using System.Collections.Generic;
using System.Text;

namespace Kowloon.Core
{
    public class ApartmentTest : IRenderer, IRangedTest
    {
        private readonly KowloonController Controller;

        public int MinimumValue => 0;
        public int MaximumValue => KowloonConfig.ApartmentRanges.Count;
        public int Value { get; set; }

        internal ApartmentTest(KowloonController controller)
            => Controller = controller;

        public void Render(bool isFirstFrame)
        {
            Span<int> leds = Controller.Leds;
            leds.Clear();

            if (Value < MinimumValue || Value >= MaximumValue)
            { return; }

            byte channelValue = (byte)((Math.Abs(Math.Sin(Controller.Timestamp)) * 0.75 + 0.25) * 255.0);
            int color = channelValue | channelValue << 8 | channelValue << 16;

            (int startIndex, int endIndex) = KowloonConfig.ApartmentRanges[Value];

            // Color lamp red for invalid apartment definition
            if (!KowloonConfig.IsValidRange(startIndex, endIndex, leds.Length))
            {
                leds.Fill(color & 0xFF0000);
                return;
            }

            // Light up the apartment
            for (int i = startIndex; i <= endIndex; i++)
            { leds[i] = color; }
        }
    }
}
