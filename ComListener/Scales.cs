using System;
using System.Threading;
using System.Text.RegularExpressions;
using System.Diagnostics;
using RJCP.IO.Ports;
using System.Text;

namespace ComListener
{
    public class Scales
    {
        private Logger _logger;
        public Scales()
        {
            _logger = new Logger();
        }


        public string Scale(int port, int type)
        {
            _logger.Log("scale()");
            string comPort = "COM" + port;
            ScaleDataSet scaleSettings = ScaleSettings(type);
            if (scaleSettings is null)
            {
                return "Invalid scale type";
            }

            string weight = GetWeight(comPort, scaleSettings);
            return weight;
        }





        private string GetWeight(string port, ScaleDataSet scaleSettings)
        {
            _logger.Log("GetWeight()");
            string text = "General error";   // null

            try
            {
                text = "Error receiving data";   // null

                using (var mySerialPort = new SerialPortStream((scaleSettings.comPort != null) ? scaleSettings.comPort : port))
                {
                    _logger.Log("OpenDirect()");
                    mySerialPort.OpenDirect();

                    _logger.Log("Write(" + scaleSettings.sendStr + ")");
                    mySerialPort.Write(scaleSettings.sendStr);

                    string indata = null;
                    for (int i = 0; i <= 100; i++)         // get weight from scale
                    {
                        Thread.Sleep(10);
                        _logger.Log("ReadExisting() [" + i + "]");
                        indata = mySerialPort.ReadExisting();
                        if (!string.IsNullOrEmpty(indata))
                        {
                            _logger.Log("indata is not null");
                            if (scaleSettings.rgx.IsMatch(indata))
                            {
                                _logger.Log("indata: " + indata);
                                text = "W" + indata.Substring(scaleSettings.weightSubStrStart, scaleSettings.weightSubStrEnd);
                                break;                    // retrive proper weight
                            }

                            _logger.Log("indata (" + indata + ") does not comply with regex legality");
                        }
                    }

                    _logger.Log("Close()");
                    mySerialPort.Close();
                }
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
                text = ex.Message;
            }

            return text;
        }


        private ScaleDataSet ScaleSettings(int type)
        {
            if (type == 1)
            {
                Trace.WriteLine("ScaleSettings(type: " + type + ")", "ComListener");
                ScaleDataSet scaleData = new ScaleDataSet();
                scaleData.deviceVID = "1FB6";
                scaleData.devicePID = "1589";
                scaleData.UpdateComPort();
                byte[] bytes = FromHex("1b-52-1b-45");
                scaleData.sendStr = Encoding.ASCII.GetString(bytes);
                string pattern = "^\\u001b[R]\\u001b[W]\\d{2}[.]\\d{3}\\u001b[H]\\d{3}[.]\\d\\u001b[B]\\d{2}[.]\\d\\u001b[N][m]\\u001b[E]$";
                scaleData.rgx = new Regex(pattern);
                scaleData.weightSubStrStart = 4;
                scaleData.weightSubStrEnd = 6;
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






}

