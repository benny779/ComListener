using System;

namespace ComListener.CustomExceptions
{
    internal class InvalidDeviceIDException : Exception
    {
        public InvalidDeviceIDException(string vid, string pid)
            : base($"The device ID is invalid. VID: {vid}, PID: {pid}")
        {
        }
    }
}
