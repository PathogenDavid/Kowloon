using System;
using RpiWs281x;
using static RpiWs281x.Native;
using System.Threading;

namespace CyberTest
{
    public static class Program
    {
        public static unsafe void Main(string[] args)
        {
            ws2811_t ledString = new ws2811_t()
            {
                freq = ws2811_t.WS2811_TARGET_FREQ,
                dmanum = 9,
                channel0 = new ws2811_channel_t()
                {
                    gpionum = 18,
                    count = 240,
                    brightness = 255,
                    strip_type = ws2811_strip_type.WS2811_STRIP_GRB
                }
            };

            Console.WriteLine("Initializing...");
            ws2811_init(ref ledString).ThrowIfError();

            Console.WriteLine("Filling LED colors...");
            for (int i = 0; i < ledString.channel0.count; i++)
            { ledString.channel0.leds[i] = 0x00FFFFFF; }

            Console.WriteLine("Rendering!");
            ws2811_render(ref ledString).ThrowIfError();

            int frameNum = 0;
            char[] spinner = { '-', '\\', '|', '/' };
            Random random = new Random();
            while (true)
            {
                uint color = 0x00FFFFFF;
                for (int i = 0; i < ledString.channel0.count; i++)
                {
                    if ((i % 3) == 0)
                    {
                        double roll = random.NextDouble();
                        if (roll < 0.33)
                        { color = 0x00FFFFFF; }
                        else if (roll > 0.66)
                        { color = 0x00F24D00; }
                        else
                        { color = 0x001EC7AB; }
                    }

                    ledString.channel0.leds[i] = color;
                }

                char currentSpinner = spinner[frameNum % spinner.Length];
                Console.Write($"    {currentSpinner} A {frameNum}                \r");
                Console.Out.Flush();
                ws2811_render(ref ledString).ThrowIfError();
                Console.Write($"    {currentSpinner} B {frameNum}                \r");
                Console.Out.Flush();

                Thread.Sleep(1000 / 30);
                frameNum++;

                if (Console.KeyAvailable && Console.ReadKey().Key == ConsoleKey.Q)
                { break; }
            }

            Console.WriteLine();
            Console.WriteLine("Finalizing...");
            ws2811_fini(ref ledString);
        }
    }
}
