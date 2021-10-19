using System;
using System.IO;
using System.IO.Ports;
using System.Threading;
using System.Text.RegularExpressions;

using RJCP.IO.Ports;


namespace ComListner
{
    class PortDataReceived
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Port number required");
                //return "Port number required";
            }
            else
            {
                string comPort = "COM" + args[0];
                string weight = GetWeight(comPort);

                if (weight == "Error receiving data")
                {
                    Console.WriteLine("Error receiving data");
                }
                else
                {
                    //Console.WriteLine(weight);
                    Console.WriteLine(weight.Substring(4, 6));
                    Console.ReadKey();
                    //return weight.Substring(4, 6);
                }
            }
        }


        static string GetWeight(string port)
        {
            var mySerialPort = new SerialPortStream(port);
            try
            {
                mySerialPort.OpenDirect();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadKey();
                //return ex.Message;
            }

            byte[] data = FromHex("1b-52-1b-45");
            string sendStr = System.Text.Encoding.ASCII.GetString(data);
            try
            {
                mySerialPort.Write(sendStr);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadKey();
                //return ex.Message;
            }

            string indata = null;
            Regex rgx = new Regex(@"^\u001b[R]\u001b[W]\d{2}[.]\d{3}\u001b[H]\d{3}[.]\d\u001b[B]\d{2}[.]\d\u001b[N][m]\u001b[E]$");
            for (int i = 0; i <= 8; i++)
            {
                Thread.Sleep(250);
                indata = mySerialPort.ReadExisting();
                if (indata != "" && indata != null)
                {
                    if (rgx.IsMatch(indata))
                    {
                        break;
                    }
                }
            }
            if (indata == null)
                "Error receiving data"


            mySerialPort.Close();

            //Console.WriteLine(indata);
            //Console.WriteLine("Press any key to continue...");
            //Console.ReadKey();
            return indata;
        }

            

            




            /*
            string[] serialPorts = SerialPort.GetPortNames();
            foreach (string serialPort in serialPorts)
                Console.WriteLine(serialPort);
            //Console.ReadKey();



            SerialPort mySerialPort = new SerialPort("COM2");

            mySerialPort.BaudRate = 9600;
            mySerialPort.Parity = Parity.None;
            mySerialPort.StopBits = StopBits.One;
            mySerialPort.DataBits = 8;
            mySerialPort.Handshake = Handshake.None;

            try
            {
                mySerialPort.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Console.ReadKey();
                Environment.Exit(1);
            }
            

            byte[] data = FromHex("1b-52-1b-45");
            string sendStr = System.Text.Encoding.ASCII.GetString(data);
            mySerialPort.Write(sendStr);

            string indata = null;
            Regex rgx = new Regex(@"^\u001b[R]\u001b[W]\d{2}[.]\d{3}\u001b[H]\d{3}[.]\d\u001b[B]\d{2}[.]\d\u001b[N][m]\u001b[E]$");
            for (int i = 0; i <= 8; i++)
            {
                Thread.Sleep(250);
                indata = mySerialPort.ReadExisting();
                if (indata != "")
                {
                    if (rgx.IsMatch(indata))
                    {
                        break;
                    }
                }
            }

            mySerialPort.Close();

            Console.WriteLine(indata);
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            

            //string str = ".R.W00.108.H000.0.B00.0.Nm.E";

            //byte[] data = FromHex("1b-52-1b-57-30-30-2e-33-30-31-1b-48-30-30-30-2e-30-1b-42-30-30-2e-30-1b-4e-6d-1b-45");
            //string s = System.Text.Encoding.ASCII.GetString(data);

            //Regex rgx = new Regex(@"^\u001b[R]\u001b[W]\d{2}[.]\d{3}\u001b[H]\d{3}[.]\d\u001b[B]\d{2}[.]\d\u001b[N][m]\u001b[E]$");

            //mySerialPort.Write(s);

            //Console.WriteLine(rgx.IsMatch(s) ? 1 : 0);
            //Console.ReadKey();
            */
        //}


        static byte[] FromHex(string hex)
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
