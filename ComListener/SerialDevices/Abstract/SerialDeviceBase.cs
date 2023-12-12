using RJCP.IO.Ports;
using System;
using System.Text.RegularExpressions;

namespace ComListener.SerialDevices.Abstract
{
    internal abstract class SerialDeviceBase : ISerialDevice
    {
        public abstract DeviceID ID { get; }
        public abstract string Name { get; }

        public string ComPort { get; set; }
        public string ErrorString => "Error receiving data";

        public abstract Regex Regex { get; }


        public SerialDeviceBase()
        {
            this.UpdateComPort();
        }
        public SerialDeviceBase(string defaultPort)
        {
            if (!this.UpdateComPort())
                ComPort = defaultPort;
        }


        public abstract DeviceResponse Read();
        public abstract DeviceResponse Parse(string value);

        public virtual bool IsValidResponse(string value)
        {
            return !string.IsNullOrEmpty(value) && Regex.IsMatch(value);
        }

        public virtual DeviceResponse TestConnection()
        {
            return Utils.TestSerialPortConnection(ComPort);
        }
    }
}
