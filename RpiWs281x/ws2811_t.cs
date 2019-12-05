using System.Runtime.InteropServices;

namespace RpiWs281x
{
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct ws2811_t
    {
        /// <summary>time in µs before the next render can run</summary>
        public ulong render_wait_time;
        /// <summary>Private data for driver use</summary>
        private void* device;
        /// <summary>RPI Hardware Information</summary>
        private void* rpi_hw;
        /// <summary>Required output frequency</summary>
        public uint freq;
        /// <summary>DMA number _not_ already in use</summary>
        public int dmanum;
        /// <summary>LED channel 0</summary>
        public ws2811_channel_t channel0;
        /// <summary>LED channel 1</summary>
        public ws2811_channel_t channel1;

        public static readonly uint WS2811_TARGET_FREQ = 800000;
    }
}
