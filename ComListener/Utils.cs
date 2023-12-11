using Microsoft.Win32;
using System;
using System.Text.RegularExpressions;

namespace ComListener
{
    public static class Utils
    {
        public static string GetPortByID(string VID, string PID)
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
                            return (string)registryKey5.GetValue("PortName");
                        }
                    }
                }
            }

            return null;
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
    }
}
