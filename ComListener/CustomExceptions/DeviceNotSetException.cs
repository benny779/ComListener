using System;

namespace ComListener.CustomExceptions
{
    public class DeviceNotSetException : Exception
    {
        public DeviceNotSetException()
            : base("Device not set. Use the 'SetDevice' method to set the device.")
        {
        }
    }
}
