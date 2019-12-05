using System;

namespace Kowloon.Core
{
    public class ApartmentManager : IRenderer
    {
        private readonly KowloonController Controller;
        private readonly Random Random = new Random();

        /// <summary>The color of each apartment</summary>
        /// <remarks>Negative numbers are indices into the pallete. Positive are specific colors. 0 is always black.</remarks>
        public readonly int[] ApartmentColors = new int[KowloonConfig.ApartmentRanges.Count];

        /// <summary>The current palette</summary>
        /// <remarks>The 0th color must always be black.</remarks>
        private int[] CurrentPalette =
        {
            0,
            0x00FFFFFF,
            0x00F24D00,
            0x001EC7AB
        };

        internal ApartmentManager(KowloonController controller)
        {
            Controller = controller;
            ScrambleColors();
        }

        public void ScrambleColors()
        {
            for (int i = 0; i < ApartmentColors.Length; i++)
            { ApartmentColors[i] = -Random.Next(CurrentPalette.Length); }
        }

        public void CycleApartment(int apartmentIndex)
        {
            if (apartmentIndex < 0 || apartmentIndex >= ApartmentColors.Length)
            { throw new ArgumentOutOfRangeException(nameof(apartmentIndex), "The specified apartment index is invalid."); }

            int oldColor = ApartmentColors[apartmentIndex];

            // If the apartment is a custom color, just turn it off
            if (oldColor > 0)
            {
                ApartmentColors[apartmentIndex] = 0;
                return;
            }

            // Otherwise advance to the next color in the palette
            int newColor = oldColor - 1;
            if (newColor <= -CurrentPalette.Length)
            { newColor = 0; }

            ApartmentColors[apartmentIndex] = newColor;
        }

        public void Render(bool isFirstFrame)
        {
            Span<int> leds = Controller.Leds;
            leds.Clear();

            for (int apartmentIndex = 0; apartmentIndex < ApartmentColors.Length; apartmentIndex++)
            {
                // Determine the apartment color
                int color = ApartmentColors[apartmentIndex];

                if (color == 0)
                { continue; }

                if (color < 0)
                {
                    color = -color % CurrentPalette.Length;
                    color = CurrentPalette[color];
                }

                // Get the apartment LED range
                (int firstLedIndex, int lastLedIndex) = KowloonConfig.ApartmentRanges[apartmentIndex];

                if (!KowloonConfig.IsValidRange(firstLedIndex, lastLedIndex, leds.Length))
                { continue; }

                // Color the apartment
                for (int ledIndex = firstLedIndex; ledIndex <= lastLedIndex; ledIndex++)
                { leds[ledIndex] = color; }
            }
        }
    }
}
