using ComListener.SerialDevices;
using System.Collections.Generic;
using System.Linq;

namespace ComListener
{
    internal static class DevicesRepository
    {
        private static Dictionary<int, DeviceID> _devices =
            new Dictionary<int, DeviceID>()
            {
                { 1, DeviceID.Create("1FB6", "1589") },
                { 2, DeviceID.Create("1FB6", "0002") },
            };

        public static Dictionary<int, DeviceID> Devices => _devices;

        public static DeviceID GetByID(int id)
        {
            return Devices[id];
        }

        public static DeviceID GetByVidPid(string vid, string pid)
        {
            return Devices
                .FirstOrDefault(d => d.Value.VID == vid && d.Value.PID == pid)
                .Value;
        }
    }
}

