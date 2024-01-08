using ComListener.CustomExceptions;
using ComListener.SerialDevices;
using ComListener.SerialDevices.Abstract;
using System;
using System.Linq;

namespace ComListener
{
    public class SerialDeviceManager
    {
        ISerialDevice device;


        /// <summary>
        /// Sets the serial device identified by the provided deviceId.
        /// </summary>
        /// <param name="deviceId">An integer representing the unique identifier of the desired device.</param>
        /// <param name="defaultPort">An optional parameter specifying the default COM port to be used
        /// if the repository fails to auto-detect the connected port.</param>
        /// <param name="useDefaultPort">Indicating whether to force the use of the defaultPort,
        /// skipping the repository auto-detection even if available.</param>
        /// <exception cref="UnsupportedDeviceException">Thrown when the provided deviceId does not correspond to a supported device.</exception>
        public void SetDevice(int deviceId, string defaultPort = null, bool useDefaultPort = false)
        {
            device = GetDevice(deviceId, defaultPort, useDefaultPort);
        }


        public DeviceResponse TestConnection() => SerialDeviceAction(device => device.TestConnection());
        public string TestConnectionAsString() => TestConnection().ToString();


        /// <summary>
        /// Reads data from the managed serial device.
        /// </summary>
        /// <returns></returns>
        public DeviceResponse Read() => SerialDeviceAction(device => device.Read());

        /// <inheritdoc cref="Read"/>
        public string ReadAsString() => Read().ToString();


        public DeviceResponse RandomRead() => DeviceResponse.CreateTest();
        public string RandomReadAsString() => RandomRead().ToString();


        public string GetConnectedDeviceIDs()
        {
            return string.Join("|",
                DevicesRepository.Devices
                .Where(d => !string.IsNullOrEmpty(Utils.GetPortByID(d.Value.VID, d.Value.PID)))
                .Select(d => d.Key));
        }

        public string GetConnectedDevicesVidPidIDs()
        {
            return string.Join("|",
                    DevicesRepository.Devices
                    .Where(d => !string.IsNullOrEmpty(Utils.GetPortByID(d.Value.VID, d.Value.PID)))
                    .Select(d => $"{d.Value.VID},{d.Value.PID}"));
        }

        public string GetConnectedDevicesIdAndPort()
        {
            return string.Join("|",
                DevicesRepository.Devices
                .Select(d => new
                {
                    Id = d.Key,
                    Port = Utils.GetPortByID(d.Value.VID, d.Value.PID)
                })
                .Where(d => !string.IsNullOrEmpty(d.Port))
                .Select(d => $"{d.Id},{d.Port}"));
        }


        private static ISerialDevice GetDevice(int deviceId, string defaultPort = null, bool useDefaultPort = false)
        {
            if (deviceId == 1)
                return new Device1(defaultPort, useDefaultPort);

            if (deviceId == 2)
                return new Device2(defaultPort, useDefaultPort);

            throw new UnsupportedDeviceException(deviceId);
        }

        private TResult SerialDeviceAction<TResult>(Func<ISerialDevice, TResult> action)
        {
            if (device is null)
                throw new DeviceNotSetException();

            return action(device);
        }
    }
}
