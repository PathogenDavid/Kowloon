using System;

namespace Kowloon.Core
{
    public class ApartmentManager : IRenderer
    {
        private readonly KowloonController Controller;
        private readonly Random Random = new Random();

        /// <summary>The color of each apartment</summary>
        /// <remarks>Negative numbers are indices into the pallete. Positive are specific colors. 0 is always black.</remarks>
        private readonly int[] ApartmentColors = new int[KowloonConfig.ApartmentRanges.Count];

        private readonly double[] FlickerTimers = new double[KowloonConfig.ApartmentRanges.Count];

        public event Action ApartmentColorsChanged;

        private Palette _CurrentPalette = KowloonConfig.Palettes[0];
        /// <summary>The current palette</summary>
        public Palette CurrentPalette
        {
            get => _CurrentPalette;
            set
            {
                if (value == null)
                { throw new ArgumentNullException(); }

                if (value == _CurrentPalette)
                { return; }

                _CurrentPalette = value;
                ScrambleLitColors(); // Scramble all lit apartments to use the new palette, will also dispatch ApartmentColorsChanged.
            }
        }

        internal ApartmentManager(KowloonController controller)
        {
            Controller = controller;
            ScrambleColors();
        }

        public void ScrambleColors()
        {
            for (int i = 0; i < ApartmentColors.Length; i++)
            { ApartmentColors[i] = -Random.Next(CurrentPalette.Count); }

            ApartmentColorsChanged?.Invoke();
        }

        public void ScrambleLitColors()
        {
            for (int i = 0; i < ApartmentColors.Length; i++)
            {
                if (ApartmentColors[i] != 0)
                { ApartmentColors[i] = -(Random.Next(CurrentPalette.Count - 1) + 1); }
            }

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
            int newIndex = (-oldColor + 1) % CurrentPalette.Count;
            ApartmentColors[apartmentIndex] = -newIndex;

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
                color = -color % CurrentPalette.Count;
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
                else
                {
                    FlickerTimers[apartmentIndex] -= Controller.FrameTime;

                    int flickerAmount = Random.Next(0, 48);
                    int r = color >> 16 & 0xFF;
                    int g = color >> 8 & 0xFF;
                    int b = color & 0xFF;
                    r = Math.Max(r - flickerAmount, 0) & 0xFF;
                    g = Math.Max(g - flickerAmount, 0) & 0xFF;
                    b = Math.Max(b - flickerAmount, 0) & 0xFF;
                    color = r << 16 | g << 8 | b;
                }

                // Color the apartment
                for (int ledIndex = firstLedIndex; ledIndex <= lastLedIndex; ledIndex++)
                { leds[ledIndex] = color; }
            }
        }
    }
}
