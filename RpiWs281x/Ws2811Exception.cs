using System;

namespace RpiWs281x
{
    public class Ws2811Exception : Exception
    {
        public ws2811_return_t ErrorCode { get; }

        public Ws2811Exception(ws2811_return_t errorCode)
            : base(errorCode.GetErrorDescription())
        {
            if (errorCode == ws2811_return_t.WS2811_SUCCESS)
            { throw new ArgumentException("The specified error code is not an error.", nameof(errorCode)); }

            ErrorCode = errorCode;
        }
    }
}
