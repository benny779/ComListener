using RJCP.IO.Ports;
using System;

namespace ComListener.SerialDevices.Abstract
{
    internal abstract class SerialDeviceBaseRead : SerialDeviceBase
    {
        protected SerialDeviceBaseRead()
        {
        }

        protected SerialDeviceBaseRead(string defaultPort) : base(defaultPort)
        {
        }


        public override DeviceResponse Read()
        {
            try
            {
                using (var serialPort = new SerialPortStream(ComPort))
                {
                    serialPort.OpenDirect();

                    var response = serialPort.ReadExisting();

                    if (IsValidResponse(response))
                        return Parse(response);
                }
            }
            catch (Exception ex)
            {
                return DeviceResponse.CreateError(ex.Message);
            }

            return DeviceResponse.CreateError(ErrorString);
        }
    }
}
