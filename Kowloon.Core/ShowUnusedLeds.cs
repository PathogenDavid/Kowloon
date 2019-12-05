using System;

namespace Kowloon.Core
{
    public class ShowUnusedLeds : IRenderer
    {
        private readonly KowloonController Controller;

        internal ShowUnusedLeds(KowloonController controller)
            => Controller = controller;

        public void Render(bool isFirstFrame)
        {
            Span<int> leds = Controller.Leds;
            leds.Fill(0xFFFFFF);

            foreach ((int startIndex, int endIndex) in KowloonConfig.ApartmentRanges)
            {
                // Skip invalid apartment definitions
                if (!KowloonConfig.IsValidRange(startIndex, endIndex, leds.Length))
                { continue; }

                // Erase the LEDs corrosponding to the apartment
                for (int i = startIndex; i <= endIndex; i++)
                { leds[i] = 0; }
            }
        }
    }
}
