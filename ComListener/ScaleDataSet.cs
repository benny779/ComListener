using Microsoft.Win32;
using System.Text.RegularExpressions;

namespace ComListener
{
    internal class ScaleDataSet
    {
        public string deviceVID { get; set; }
        public string devicePID { get; set; }
        public string comPort { get; set; }
        public string sendStr { get; set; }
        public Regex rgx { get; set; }
        public int weightSubStrStart { get; set; }
        public int weightSubStrEnd { get; set; }
        public int heightSubStrStart { get; set; }
        public int heightSubStrEnd { get; set; }

        public bool UpdateComPort()
        {
            comPort = GetPortByID(deviceVID, devicePID);
            if (comPort != null)
            {
                return true;
            }

            return false;
        }

        private static string GetPortByID(string VID, string PID)
        {
            string pattern = $"^VID_{VID}.PID_{PID}";
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            RegistryKey localMachine = Registry.LocalMachine;
            RegistryKey registryKey = localMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Enum");
            string[] subKeyNames = registryKey.GetSubKeyNames();
            foreach (string name in subKeyNames)
            {
                RegistryKey registryKey2 = registryKey.OpenSubKey(name);
                string[] subKeyNames2 = registryKey2.GetSubKeyNames();
                foreach (string text in subKeyNames2)
                {
                    if (regex.Match(text).Success)
                    {
                        RegistryKey registryKey3 = registryKey2.OpenSubKey(text);
                        string[] subKeyNames3 = registryKey3.GetSubKeyNames();
                        int num = 0;
                        if (num < subKeyNames3.Length)
                        {
                            string name2 = subKeyNames3[num];
                            RegistryKey registryKey4 = registryKey3.OpenSubKey(name2);
                            RegistryKey registryKey5 = registryKey4.OpenSubKey("Device Parameters");
                            return (string)registryKey5.GetValue("PortName");
                        }
                    }
                }
            }

            return null;
        }
    }
}
