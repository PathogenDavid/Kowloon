using System;
using System.Runtime.InteropServices;

namespace Kowloon.LedControl
{
    public abstract class LedString : IDisposable
    {
        public abstract Span<int> Leds { get; }

        public abstract byte Brightness { get; set; }

        public abstract void Render();

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        { }

        ~LedString()
            => Dispose(false);

        public static LedString Create(int ledCount)
        {
            // If the platform is Linux on ARM32 or ARM64, assume we're running on a Raspberry Pi.
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) && (RuntimeInformation.ProcessArchitecture == Architecture.Arm || RuntimeInformation.ProcessArchitecture == Architecture.Arm64))
            { return new RaspberryPiLedString(ledCount); }

            // If we don't support driving LEDs on this platform, just return a dummy LED string
            Console.Error.WriteLine("Platform does not support LED string. Using null LED controller.");
            return new NullLedString(ledCount);
        }
    }
}
