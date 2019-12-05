using System;
using System.Diagnostics;

namespace Kowloon.Core
{
    public class SolidColorCyclePattern : IRenderer
    {
        private readonly KowloonController Controller;
        private readonly Stopwatch Stopwatch = new Stopwatch();
        private static int[] Colors = { 0x00FFFFFF, 0x00FF0000, 0x0000FF00, 0x000000FF };
        private int ColorIndex = 0;

        internal SolidColorCyclePattern(KowloonController controller)
        {
            Controller = controller;
            Stopwatch.Start();
        }

        public void Render(bool isFirstFrame)
        {
            // Only update once every 1 second
            if (!isFirstFrame && Stopwatch.ElapsedMilliseconds < 1000)
            { return; }

            Stopwatch.Restart();

            // Render the frame
            if (isFirstFrame)
            { ColorIndex = 0; }
            else
            { ColorIndex = (ColorIndex + 1) % Colors.Length; }

            Controller.Leds.Fill(Colors[ColorIndex]);
        }
    }
}
