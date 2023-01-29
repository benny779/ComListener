using ComListener;
using System;
using System.IO.Ports;

namespace ComListenerTester
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("===========================");
            Console.WriteLine("Note: ATD is set to port 10");
            Console.WriteLine("===========================");

            string[] ports = SerialPort.GetPortNames();
            Console.WriteLine("Available COM ports:");
            foreach (string p in ports)
            {
                Console.WriteLine(p);
            }
            Console.WriteLine("===========================");
            Console.WriteLine();

            Console.WriteLine("COM Port:");
            int port = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Scale type (Default is \"1\"):");
            int type = Convert.ToInt32(Console.ReadLine());

            Console.Clear();
            Console.WriteLine("Selected port: " + port);
            Console.WriteLine("Selected type: " + type);
            Console.WriteLine();


            Scales s = new Scales();
            for (int i = 0; i < 100; i++)
            {
                string str = s.Scale(port, type);
                Console.WriteLine(str);
                Console.Write("");
                Console.ReadKey();
            }
        }
    }
}
