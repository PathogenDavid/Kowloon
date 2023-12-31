﻿using System;

namespace Kowloon.LedControl
{
    internal class NullLedString : LedString
    {
        private readonly int[] LedArray;
        public override Span<int> Leds => LedArray;

        public override byte Brightness { get; set; } = 255;

        public NullLedString(int ledCount)
            => LedArray = new int[ledCount];

        public override void Render()
        { }
    }
}
