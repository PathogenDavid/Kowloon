using System;
using System.Collections.Generic;
using System.Text;

namespace Kowloon.Core
{
    internal static class ColorHelper
    {
        public static int GetRainbowColor(double hue)
        {
            hue /= 60.0;
            byte c = 255;
            byte x = (byte)((1.0 - Math.Abs(hue % 2.0 - 1.0)) * 255.0);

            if (hue <= 1.0)
            { return c << 16 | x << 8; }
            else if (hue <= 2.0)
            { return x << 16 | c << 8; }
            else if (hue <= 3.0)
            { return c << 8 | x; }
            else if (hue <= 4.0)
            { return x << 8 | c; }
            else if (hue <= 5.0)
            { return x << 16 | c; }
            else
            { return c << 16 | x; }
        }
    }
}
