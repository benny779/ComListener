using Microsoft.Win32;
using RJCP.IO.Ports;
using System;
using System.Text.RegularExpressions;

namespace ComListener
{
    public static class Utils
    {
        private static readonly Random random = new Random();

        public static string GetPortByID(string VID, string PID, bool testConnection = true)
        {
            string pattern = $"^VID_{VID}.PID_{PID}";
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);

            var localMachine = Registry.LocalMachine;
            var registryKey = localMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Enum");
            var subKeyNames = registryKey.GetSubKeyNames();

            foreach (var name in subKeyNames)
            {
                var registryKey2 = registryKey.OpenSubKey(name);
                var subKeyNames2 = registryKey2.GetSubKeyNames();
                foreach (var text in subKeyNames2)
                {
                    if (regex.Match(text).Success)
                    {
                        var registryKey3 = registryKey2.OpenSubKey(text);
                        var subKeyNames3 = registryKey3.GetSubKeyNames();

                        if (subKeyNames3.Length > 0)
                        {
                            string name2 = subKeyNames3[0];
                            var registryKey4 = registryKey3.OpenSubKey(name2);
                            var registryKey5 = registryKey4.OpenSubKey("Device Parameters");

                            var portName = (string)registryKey5.GetValue("PortName");
                            if (!testConnection || TestSerialPortConnection(portName).Success)
                                return portName;
                        }
                    }
                }
            }

            return null;
        }


        public static DeviceResponse TestSerialPortConnection(string portName)
        {
            try
            {
                using (var serialPort = new SerialPortStream(portName))
                {
                    serialPort.OpenDirect();
                }

                return DeviceResponse.Create();
            }
            catch (Exception ex)
            {
                return DeviceResponse.CreateError(ex.Message);
            }
        }


        public static byte[] FromHex(string hex)
        {
            hex = hex.Replace("-", "");
            var raw = new byte[hex.Length / 2];
            for (int i = 0; i < raw.Length; i++)
            {
                raw[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
            }
            return raw;
        }


        public static bool IsHexadecimal(string input)
        {
            foreach (char c in input)
            {
                if (!char.IsDigit(c) && !(c >= 'a' && c <= 'f') && !(c >= 'A' && c <= 'F'))
                {
                    return false;
                }
            }

            return true;
        }


        public static string FromAscii(int asciiCode)
        {
            return Convert.ToChar(asciiCode).ToString();
        }


        public static double GetRandomWeight()
        {
            const double minWeight = 3.0;
            const double maxWeight = 120.0;

            return minWeight + (maxWeight - minWeight) * random.NextDouble();
        }

        public static double GetRandomHeight()
        {
            const double minHeight = 80.0;
            const double maxHeight = 190.0;

            return minHeight + (maxHeight - minHeight) * random.NextDouble();
        }

        public static double GetRandomBMI(double weight, double height)
        {
            return weight / Math.Pow(height / 100, 2);
        }
    }
}
