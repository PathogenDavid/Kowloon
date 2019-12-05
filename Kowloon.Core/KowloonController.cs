using System;
using Kowloon.LedControl;
using System.Diagnostics;
using System.Threading;

namespace Kowloon.Core
{
    internal sealed class KowloonController : IDisposable
    {
        private readonly LedString LedString;
        private readonly int MaximumFrameRate;
        private readonly TimeSpan MinimumFrameTime;

        public int FrameNumber { get; private set; } = 0;
        /// <summary>The time elapsed since the beginning of the previous frame, in seconds.</summary>
        public double FrameTime { get; private set; } = 0.0;
        /// <summary>The time elapsed since the beginning of the render, in seconds.</summary>
        public double Timestamp { get; private set; } = 0.0;

        private readonly Thread RenderThread;
        private volatile bool IsRunning = true;

        public Span<int> Leds => LedString.Leds;

        public byte Brightness
        {
            get => LedString.Brightness;
            set => LedString.Brightness = value;
        }

        public IRenderer PrimaryRenderer { get; set; }
        public IRenderer OverrideRenderer { get; set; } = null;

        public KowloonController()
        {
            LedString = LedString.Create(199);
            MaximumFrameRate = 60;
            MinimumFrameTime = TimeSpan.FromSeconds(1.0 / ((double)MaximumFrameRate));

            RenderThread = new Thread(RenderThreadEntry);
            RenderThread.Name = "Render Thread";
            RenderThread.Start();
        }

        private void RenderThreadEntry()
        {
            Stopwatch frameStopwatch = new Stopwatch();
            Stopwatch totalStopwatch = new Stopwatch();
            frameStopwatch.Start();
            totalStopwatch.Start();
            IRenderer lastRenderer = null;

            while (IsRunning)
            {
                FrameNumber++;
                FrameTime = frameStopwatch.Elapsed.TotalSeconds;
                frameStopwatch.Restart();
                Timestamp = totalStopwatch.Elapsed.TotalSeconds;

                // Determine the current renderer
                IRenderer renderer = OverrideRenderer ?? PrimaryRenderer;
                bool isFirstFrame = false;

                // If the renderer has changed, clear the LED strip to prevent colors from previous renderers from sticking around
                if (renderer != lastRenderer)
                {
                    lastRenderer = renderer;
                    Leds.Clear();
                    isFirstFrame = true;
                }

                // Render a frame
                renderer?.Render(isFirstFrame);

                // The last LED in the string seems to be broken, make sure it's always off.
                Leds[Leds.Length - 1] = 0;

                // Render the current frame buffer to the LED string
                LedString.Render();

                // Sleep for the remaining frame time
                TimeSpan sleepTime = MinimumFrameTime - frameStopwatch.Elapsed;
                if (sleepTime > TimeSpan.FromMilliseconds(1))
                { Thread.Sleep(sleepTime); }
            }
        }

        public void Dispose()
        {
            IsRunning = false;
            RenderThread.Join();
            Leds.Clear();
            LedString.Render();
            LedString.Dispose();
        }
    }
}
