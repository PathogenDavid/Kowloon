using System.Runtime.InteropServices;

namespace RpiWs281x
{
    public static class Native
    {
        private const string libws2811 = "libws2811.so";

        /// <summary>Initialize buffers/hardware</summary>
        [DllImport(libws2811)]
        public static extern ws2811_return_t ws2811_init(ref ws2811_t ws2811);

        /// <summary>Tear it all down</summary>
        [DllImport(libws2811)]
        public static extern void ws2811_fini(ref ws2811_t ws2811);

        /// <summary>Send LEDs off to hardware</summary>
        [DllImport(libws2811)]
        public static extern ws2811_return_t ws2811_render(ref ws2811_t ws2811);

        /// <summary>Wait for DMA completion</summary>
        [DllImport(libws2811)]
        public static extern ws2811_return_t ws2811_wait(ref ws2811_t ws2811);
    }
}
