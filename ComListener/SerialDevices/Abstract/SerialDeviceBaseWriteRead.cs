using RJCP.IO.Ports;
using System;
using System.IO;
using System.Threading;

namespace ComListener.SerialDevices.Abstract
{
    internal abstract class SerialDeviceBaseWriteRead : SerialDeviceBase
    {
        protected virtual int MaxReadLoops => 20;
        protected virtual int SleepBetweenReadLoopsMs => 15;
        protected abstract string PortRequestString { get; }

        protected SerialDeviceBaseWriteRead()
        {
        }

        protected SerialDeviceBaseWriteRead(string defaultPort, bool useDefaultPort)
            : base(defaultPort, useDefaultPort)
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
