using RJCP.IO.Ports;
using System;
using System.Threading;

namespace ComListener.SerialDevices.Abstract
{
    internal abstract class SerialDeviceBaseWriteRead: SerialDeviceBase
    {
        protected virtual int MaxReadLoops => 100;
        protected virtual int SleepBetweenReadLoopsMs => 10;
        protected abstract string PortRequestString { get; }

        protected SerialDeviceBaseWriteRead()
        {
        }

        protected SerialDeviceBaseWriteRead(string defaultPort) : base(defaultPort)
        {
        }

        public override DeviceResponse Read()
        {
            try
            {
                using (var serialPort = new SerialPortStream(ComPort))
                {
                    serialPort.OpenDirect();
                    serialPort.Write(PortRequestString);

                    var response = string.Empty;
                    for (int i = 0; i < MaxReadLoops; i++)
                    {
                        Thread.Sleep(SleepBetweenReadLoopsMs);

                        response = serialPort.ReadExisting();

                        if (IsValidResponse(response))
                            return Parse(response);
                    }
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
