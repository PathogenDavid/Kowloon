namespace RpiWs281x
{
    public unsafe struct ws2811_channel_t
    {
        /// <summary>GPIO Pin with PWM alternate function, 0 if unused</summary>
        public int gpionum;
        /// <summary>Invert output signal</summary>
        public int invert;
        /// <summary>Number of LEDs, 0 if channel is unused</summary>
        public int count;
        /// <summary>Strip color layout</summary>
        public ws2811_strip_type strip_type;
        /// <summary>LED buffers, allocated by driver based on count</summary>
        /// <remarks>LEDs are stored as 0xWWRRGGBB</remarks>
        public uint* leds;
        /// <summary>Brightness value between 0 and 255</summary>
        public byte brightness;
        /// <summary>White shift value</summary>
        public byte wshift;
        /// <summary>Red shift value</summary>
        public byte rshift;
        /// <summary>Green shift value</summary>
        public byte gshift;
        /// <summary>Blue shift value</summary>
        public byte bshift;
        /// <summary>Gamma correction table</summary>
        public byte* gamma;
    }
}
