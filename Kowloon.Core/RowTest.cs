using System;

namespace Kowloon.Core
{
    public class RowTest : IRenderer, IRangedTest
    {
        private readonly KowloonController Controller;

        public int MinimumValue => 0;
        public int MaximumValue => KowloonConfig.RowDescriptions.Count;
        public int Value { get; set; }

        internal RowTest(KowloonController contorller)
            => Controller = contorller;

        public void Render(bool isFirstFrame)
        {
            Span<int> leds = Controller.Leds;
            leds.Clear();

            if (Value < MinimumValue || Value >= MaximumValue)
            { return; }

            // Find the first apartment in the row
            int firstApartment = 0;
            for (int i = 0; i < Value; i++)
            { firstApartment += KowloonConfig.RowDescriptions[i].Width; }

            // Find the last apartment in the row
            int lastApartment = firstApartment + KowloonConfig.RowDescriptions[Value].Width - 1;

            // If the first apartment is invalid, error the display and abort
            if (firstApartment >= KowloonConfig.ApartmentRanges.Count)
            {
                leds.Fill(0x00FF0000);
                return;
            }

            // If the last apartment index is invalid, error the display and cap it
            if (lastApartment >= KowloonConfig.ApartmentRanges.Count)
            {
                leds.Fill(0x00FF0000);
                lastApartment = KowloonConfig.ApartmentRanges.Count - 1;
            }

            // Color the apartments in the row
            for (int apartmentIndex = firstApartment; apartmentIndex <= lastApartment; apartmentIndex++)
            {
                (int startIndex, int endIndex) = KowloonConfig.ApartmentRanges[apartmentIndex];

                // Skip invalid apartments
                if (!KowloonConfig.IsValidRange(startIndex, endIndex, leds.Length))
                { continue; }

                for (int ledIndex = startIndex; ledIndex <= endIndex; ledIndex++)
                { leds[ledIndex] = 0x00FFFFFF; }
            }
        }
    }
}
