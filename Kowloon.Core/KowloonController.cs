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
        public double FrameTimeSeconds { get; private set; } = 0.0;

        private readonly Thread RenderThread;
        private volatile bool IsRunning = true;

        public Span<int> Leds => LedString.Leds;

        public IRenderer PrimaryRenderer { get; set; }
        public IRenderer OverrideRenderer { get; set; } = null;

        public KowloonController()
        {
            LedString = LedString.Create(240);
            MaximumFrameRate = 60;
            MinimumFrameTime = TimeSpan.FromSeconds(1.0 / ((double)MaximumFrameRate));

            RenderThread = new Thread(RenderThreadEntry);
            RenderThread.Name = "Render Thread";
            RenderThread.Start();
        }

        private void RenderThreadEntry()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            IRenderer lastRenderer = null;

            while (IsRunning)
            {
                FrameNumber++;
                FrameTimeSeconds = stopwatch.Elapsed.TotalSeconds;
                stopwatch.Restart();

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

                // Render the current frame buffer to the LED string
                LedString.Render();

                // Sleep for the remaining frame time
                TimeSpan sleepTime = MinimumFrameTime - stopwatch.Elapsed;
                if (sleepTime > TimeSpan.FromMilliseconds(1))
                { Thread.Sleep(sleepTime); }
            }
        }

        public void Dispose()
        {
            IsRunning = false;
            RenderThread.Join();
            LedString.Dispose();
        }
    }
}
