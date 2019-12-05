﻿using RpiWs281x;
using System;
using static RpiWs281x.Native;

namespace Kowloon.LedControl
{
    internal class RaspberryPiLedString : LedString
    {
        private ws2811_t NativeString;
        public unsafe override Span<int> Leds => new Span<int>(NativeString.channel0.leds, NativeString.channel0.count);

        public RaspberryPiLedString(int ledCount)
        {
            if (ledCount <= 0)
            { throw new ArgumentOutOfRangeException(nameof(ledCount), "There must be at least 1 LED in the string."); }

            NativeString = new ws2811_t()
            {
                freq = ws2811_t.WS2811_TARGET_FREQ,
                dmanum = 9, // Documentation recommends 10, but we had issues with 10 causing SD card access timeouts and random freezes.
                channel0 = new ws2811_channel_t()
                {
                    gpionum = 18,
                    count = ledCount,
                    brightness = 255,
                    strip_type = ws2811_strip_type.WS2811_STRIP_GRB
                }
            };

            ws2811_init(ref NativeString).ThrowIfError();
        }

        public override void Render()
            => ws2811_render(ref NativeString);

        protected override void Dispose(bool disposing)
        {
            ws2811_fini(ref NativeString);
            NativeString = default;
        }
    }
}
