namespace ComListener.SerialDevices.Abstract
{
    internal static class SerialDeviceExtensions
    {
        public static bool UpdateComPort(this ISerialDevice device)
        {
            var comPort = Utils.GetPortByID(device.ID.VID, device.ID.PID, false);
            if (string.IsNullOrEmpty(comPort))
                return false;

            device.ComPort = comPort;
            return true;
        }
    }
}
