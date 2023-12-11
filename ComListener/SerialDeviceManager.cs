using ComListener.CustomExceptions;
using ComListener.SerialDevices;
using ComListener.SerialDevices.Abstract;
using System.Linq;

namespace ComListener
{
    public class SerialDeviceManager
    {
        ISerialDevice device;

        private SerialDeviceManager()
        {
        }

        /// <summary>
        /// Creates an instance of <see cref="SerialDeviceManager"/> configured with a specific device identified by the provided deviceId.
        /// </summary>
        /// <param name="deviceId">An integer representing the unique identifier of the desired device.</param>
        /// <param name="defaultPort">An optional parameter specifying the default COM port to be used
        /// if the repository fails to auto-detect the connected port.</param>
        /// <returns>A new instance of <see cref="SerialDeviceManager"/> with the specified device.</returns>
        /// <exception cref="UnsupportedDeviceException">Thrown when the provided deviceId does not correspond to a supported device.</exception>
        public static SerialDeviceManager Create(int deviceId, string defaultPort = null)
        {
            return new SerialDeviceManager()
            {
                device = GetDevice(deviceId, defaultPort)
            };

        }





        /// <summary>
        /// Reads data from the managed serial device.
        /// </summary>
        /// <returns></returns>
        public DeviceResponse Read() => device.Read();

        /// <inheritdoc cref="Read"/>
        public string ReadAsAstring() => Read().ToString();



        public static string GetConnectedDeviceIDs()
        {
            return string.Join("|",
                DevicesRepository.Devices
                .Where(d => !string.IsNullOrEmpty(Utils.GetPortByID(d.Value.VID, d.Value.PID)))
                .Select(d => d.Key));
        }

        public static string GetConnectedDevicesVidPidIDs()
        {
            return string.Join("|",
                    DevicesRepository.Devices
                    .Where(d => !string.IsNullOrEmpty(Utils.GetPortByID(d.Value.VID, d.Value.PID)))
                    .Select(d => $"{d.Value.VID},{d.Value.PID}"));
        }


        private static ISerialDevice GetDevice(int deviceId, string defaultPort = null)
        {
            if (deviceId == 1)
                return new Device1(defaultPort);

            if (deviceId == 2)
                return new Device2(defaultPort);

            throw new UnsupportedDeviceException(deviceId);
        }
    }
}
