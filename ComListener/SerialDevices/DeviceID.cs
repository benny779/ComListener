using ComListener.CustomExceptions;

namespace ComListener.SerialDevices
{
    internal class DeviceID
    {
        public string VID { get; set; }
        public string PID { get; set; }

        private DeviceID(string vid, string pid)
        {
            VID = vid;
            PID = pid;
        }

        public static DeviceID Create(string vid, string pid)
        {
            if (!IsValidVIDOrPID(vid) || !IsValidVIDOrPID(pid))
            {
                throw new InvalidDeviceIDException(vid, pid);
            }

            return new DeviceID(vid, pid);
        }


        private static bool IsValidVIDOrPID(string input)
        {
            return !string.IsNullOrEmpty(input) &&
                input.Length == 4 &&
                Utils.IsHexadecimal(input);
        }


        public override string ToString()
        {
            return $"^VID_{VID}.PID_{PID}";
        }
    }
}
