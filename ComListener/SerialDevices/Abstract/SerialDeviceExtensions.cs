namespace ComListener.SerialDevices.Abstract
{
    internal static class SerialDeviceExtensions
    {
        public static bool UpdateComPort(this ISerialDevice device, bool testConnection)
        {
            var comPort = Utils.GetPortByID(device.ID.VID, device.ID.PID, testConnection);
            if (string.IsNullOrEmpty(comPort))
                return false;

            device.ComPort = comPort;
            return true;
        }
    }
}
