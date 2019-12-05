namespace RpiWs281x
{
    public static class Ws2811ReturnCodeExtensions
    {
        /// <summary>Returns a string representation of the given return state.</summary>
        /// <remarks>This is equivalent to the native function ws2811_get_return_t_str, except it doesn't return an empty string when the return code is invalid.</remarks>
        public static string GetErrorDescription(this ws2811_return_t errorCode)
            => errorCode switch
            {
                ws2811_return_t.WS2811_SUCCESS => "Success",
                ws2811_return_t.WS2811_ERROR_GENERIC => "Generic failure",
                ws2811_return_t.WS2811_ERROR_OUT_OF_MEMORY => "Out of memory",
                ws2811_return_t.WS2811_ERROR_HW_NOT_SUPPORTED => "Hardware revision is not supported",
                ws2811_return_t.WS2811_ERROR_MEM_LOCK => "Memory lock failed",
                ws2811_return_t.WS2811_ERROR_MMAP => "mmap() failed",
                ws2811_return_t.WS2811_ERROR_MAP_REGISTERS => "Unable to map registers into userspace",
                ws2811_return_t.WS2811_ERROR_GPIO_INIT => "Unable to initialize GPIO",
                ws2811_return_t.WS2811_ERROR_PWM_SETUP => "Unable to initialize PWM",
                ws2811_return_t.WS2811_ERROR_MAILBOX_DEVICE => "Failed to create mailbox device",
                ws2811_return_t.WS2811_ERROR_DMA => "DMA error",
                ws2811_return_t.WS2811_ERROR_ILLEGAL_GPIO => "Selected GPIO not possible",
                ws2811_return_t.WS2811_ERROR_PCM_SETUP => "Unable to initialize PCM",
                ws2811_return_t.WS2811_ERROR_SPI_SETUP => "Unable to initialize SPI",
                ws2811_return_t.WS2811_ERROR_SPI_TRANSFER => "SPI transfer error",
                _ => $"Unknown Error {(int)errorCode}"
            };

        public static void ThrowIfError(this ws2811_return_t errorCode)
        {
            if (errorCode != ws2811_return_t.WS2811_SUCCESS)
            { throw new Ws2811Exception(errorCode); }
        }
    }
}
