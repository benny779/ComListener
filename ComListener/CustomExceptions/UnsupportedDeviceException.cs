using System;

namespace ComListener.CustomExceptions
{
    public class UnsupportedDeviceException : Exception
    {
        public UnsupportedDeviceException(int deviceId) 
            : base($"Device id '{deviceId}' is not supported.")
        {
        }
    }
}
