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

        private readonly double[] FlickerTimers = new double[KowloonConfig.ApartmentRanges.Count];

        public event Action ApartmentColorsChanged;

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

            ApartmentColorsChanged?.Invoke();
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

            ApartmentColorsChanged?.Invoke();
        }

        public int GetColor(int apartmentIndex)
        {
            if (apartmentIndex < 0 || apartmentIndex >= ApartmentColors.Length)
            { return 0; }

            // Determine the apartment color
            int color = ApartmentColors[apartmentIndex];

            if (color < 0)
            {
                color = -color % CurrentPalette.Length;
                color = CurrentPalette[color];
            }

            return color;
        }

        public string GetCssColor(int apartmentIndex)
            => $"#{GetColor(apartmentIndex) & 0xFFFFFF:X6}";

        public void Render(bool isFirstFrame)
        {
            Span<int> leds = Controller.Leds;
            leds.Clear();

            for (int apartmentIndex = 0; apartmentIndex < ApartmentColors.Length; apartmentIndex++)
            {
                int color = GetColor(apartmentIndex);

                // If the apartment is off, skip processing it
                if (color == 0)
                { continue; }

                // Get the apartment LED range
                (int firstLedIndex, int lastLedIndex) = KowloonConfig.ApartmentRanges[apartmentIndex];

                if (!KowloonConfig.IsValidRange(firstLedIndex, lastLedIndex, leds.Length))
                { continue; }

                // If this apartment isn't flickering, it has a random chance to start
                if (FlickerTimers[apartmentIndex] <= 0.0)
                {
                    if (Random.NextDouble() > 0.998)
                    { FlickerTimers[apartmentIndex] = 0.1 + Random.NextDouble() * 3.0; }
                }
                // If it is flickering, reduce the flicker timer and apply the flicker effect
                else if (color != 0x00F24D00) // Don't flicker the orange, it looks weird.
                {
                    FlickerTimers[apartmentIndex] -= Controller.FrameTime;

                    int flickerAmount = Random.Next(0, 48);
                    int r = color >> 16 & 0xFF;
                    int g = color >> 8 & 0xFF;
                    int b = color & 0xFF;
                    r -= flickerAmount;
                    g -= flickerAmount;
                    b -= flickerAmount;
                    r &= 0xFF;
                    g &= 0xFF;
                    b &= 0xFF;
                    color = r << 16 | g << 8 | b;
                }

                // Color the apartment
                for (int ledIndex = firstLedIndex; ledIndex <= lastLedIndex; ledIndex++)
                { leds[ledIndex] = color; }
            }
        }
    }
}
