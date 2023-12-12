using RJCP.IO.Ports;
using System;
using System.Threading;

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
                using (var wait = new ManualResetEvent(false))
                {
                    serialPort.OpenDirect();

                    string response = string.Empty;

                    serialPort.DataReceived += (s, e) =>
                    {
                        response = serialPort.ReadExisting();
                        wait.Set();
                    };

                    wait.WaitOne();

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
