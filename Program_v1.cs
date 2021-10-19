using System;
using System.Threading;
using System.Text.RegularExpressions;

using RJCP.IO.Ports;


namespace ComListener
{
    public class Scales
    {
        static void Main()
        {
            Scales s = new Scales();
            Console.WriteLine(s.Scale("1"));
            Console.ReadKey();
        }

        public string Scale(string port = "1")
        {
            string comPort = "COM" + port;
            string weight = GetWeight(comPort);
            return weight;
        }


        private string GetWeight(string port)
        {
            string s = "General error";   // null

            try
            {
                s = "Error receiving data";   // null

                var mySerialPort = new SerialPortStream(port);

                mySerialPort.OpenDirect();

                byte[] data = FromHex("1b-52-1b-45");
                string sendStr = System.Text.Encoding.ASCII.GetString(data);
                mySerialPort.Write(sendStr);

                string indata = null;
                Regex rgx = new Regex(@"^\u001b[R]\u001b[W]\d{2}[.]\d{3}\u001b[H]\d{3}[.]\d\u001b[B]\d{2}[.]\d\u001b[N][m]\u001b[E]$");
                for (int i = 0; i <= 100; i++)         // get weight from scale
                {
                    Thread.Sleep(10);
                    indata = mySerialPort.ReadExisting();
                    if (indata != "" && indata != null)
                    {
                        if (rgx.IsMatch(indata))
                        {
                            s = "W" + indata.Substring(4, 6);
                            break;                    // retrive proper weight
                        }
                    }
                }
                //if (s.Substring(0, 1) != "W")
                    //s = "Error receiving data";

                mySerialPort.Close();
            }
            catch (Exception ex)
            {
                s = ex.Message;
            }


            return s;
        }

            
        private static byte[] FromHex(string hex)
        {
            hex = hex.Replace("-", "");
            byte[] raw = new byte[hex.Length / 2];
            for (int i = 0; i < raw.Length; i++)
            {
                raw[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
            }
            return raw;
        }

    }
}
