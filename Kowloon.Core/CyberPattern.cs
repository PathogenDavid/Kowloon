using System;
using System.Diagnostics;

namespace Kowloon.Core
{
    public class CyberPattern : IRenderer
    {
        private readonly KowloonController Controller;
        private readonly Random Random = new Random();
        private Stopwatch Stopwatch = new Stopwatch();

        internal CyberPattern(KowloonController controller)
        {
            Controller = controller;
            Stopwatch.Start();
        }

        public void Render(bool isFirstFrame)
        {
            // If less than 5 seconds have passed since we last rendered, don't do anything to avoid over-animating
            if (!isFirstFrame && Stopwatch.Elapsed.TotalSeconds < 5.0)
            { return; }

            Stopwatch.Restart();

            // Render the frame
            Span<int> leds = Controller.Leds;
            int color = 0x00FFFFFF;
            for (int i = 0; i < leds.Length; i++)
            {
                if ((i % 3) == 0)
                {
                    double roll = Random.NextDouble();
                    if (roll < 0.33)
                    { color = 0x00FFFFFF; }
                    else if (roll > 0.66)
                    { color = 0x00F24D00; }
                    else
                    { color = 0x001EC7AB; }
                }

                leds[i] = color;
            }
        }
    }
}
