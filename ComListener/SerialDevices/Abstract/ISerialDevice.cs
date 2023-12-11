namespace ComListener.SerialDevices.Abstract
{
    internal interface ISerialDevice
    {
        DeviceID ID { get; }
        string Name { get; }

        string ComPort { get; set; }
        string ErrorString { get; }

        DeviceResponse Read();
        DeviceResponse Parse(string value);
        bool IsValidResponse(string value);
        DeviceResponse TestConnection();
    }
}
