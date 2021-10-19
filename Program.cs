using System;
using System.Threading;
using System.Text.RegularExpressions;
using System.Diagnostics;

using RJCP.IO.Ports;


namespace ComListener
{
    public class Scales
    {
        //static void Main()
        //{
        //    Scales s = new Scales();
        //    for (int i = 0; i < 100; i++)
        //    {
        //        string currWeight = s.Scale(10, 1);
        //        Console.WriteLine(currWeight);
        //        Console.ReadKey();
        //    }
        //}


        public string Scale(int port, int type)
        {
            Trace.WriteLine("scale()", "ComListener");
            string comPort = "COM" + port;
            scaleDataSet scaleSettings = ScaleSettings(type);
            if (scaleSettings is null)
            {
                return "Invalid scale type";
            }

            string weight = GetWeight(comPort, scaleSettings);
            return weight;
        }





        private string GetWeight(string port, scaleDataSet scaleSettings)
        {
            Trace.WriteLine("GetWeight()", "ComListener");
            string s = "General error";   // null

            try
            {
                s = "Error receiving data";   // null

                using (var mySerialPort = new SerialPortStream(port))
                {
                    Trace.WriteLine("OpenDirect()", "ComListener");
                    mySerialPort.OpenDirect();

                    Trace.WriteLine("Write(" + scaleSettings.sendStr + ")", "ComListener");
                    mySerialPort.Write(scaleSettings.sendStr);

                    string indata = null;
                    for (int i = 0; i <= 100; i++)         // get weight from scale
                    {
                        Thread.Sleep(10);
                        Trace.WriteLine("ReadExisting() [" + i + "]", "ComListener");
                        indata = mySerialPort.ReadExisting();
                        if (indata != "" && indata != null)
                        {
                            Trace.WriteLine("indata is not null", "ComListener");
                            if (scaleSettings.rgx.IsMatch(indata))
                            {
                                Trace.WriteLine("indata: " + indata, "ComListener");
                                s = "W" + indata.Substring(scaleSettings.weightSubStrStart, scaleSettings.weightSubStrEnd);
                                break;                    // retrive proper weight
                            }
                            else
                            {
                                Trace.WriteLine("indata (" + indata + ") does not comply with regex legality", "ComListener");
                            }
                        }
                    }

                    Trace.WriteLine("Close()", "ComListener");
                    mySerialPort.Close();
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Exception(" + ex.Message + ")", "ComListener");
                s = ex.Message;
            }

            return s;
        }


        private scaleDataSet ScaleSettings(int type)
        {
            if (type == 1)
            {
                Trace.WriteLine("ScaleSettings(type: " + type + ")", "ComListener");
                scaleDataSet scaleData = new scaleDataSet();
                byte[] sendStrByte = FromHex("1b-52-1b-45");
                string sendStr = System.Text.Encoding.ASCII.GetString(sendStrByte);
                scaleData.sendStr = sendStr;
                string rgxStr = @"^\u001b[R]\u001b[W]\d{2}[.]\d{3}\u001b[H]\d{3}[.]\d\u001b[B]\d{2}[.]\d\u001b[N][m]\u001b[E]$";
                scaleData.rgx = new Regex(rgxStr);
                scaleData.weightSubStrStart = 4;
                scaleData.weightSubStrEnd = 6;
                //scaleData.heightSubStrStart = 4;
                //scaleData.heightSubStrEnd = 6;
                return scaleData;
            }
            return null;
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


    
    

    class scaleDataSet
    {
        public string sendStr { get; set; }
        public Regex rgx { get; set; }
        public int weightSubStrStart { get; set; }
        public int weightSubStrEnd { get; set; }
        public int heightSubStrStart { get; set; }
        public int heightSubStrEnd { get; set; }
    }


}
    
