using ComListener;
using ComListener.CustomExceptions;
using System;
using System.IO.Ports;
using System.Linq;

namespace ComListenerTester
{
    class Program
    {
        static void Main(string[] args)
        {
            const string lineDelimiter = "===========================";
            var deviceManager = new SerialDeviceManager();

            while (true)
            {
                Console.Clear();

                Console.WriteLine(lineDelimiter);
                Console.WriteLine("Note: ATD is set to port 10");
                Console.WriteLine(lineDelimiter);
                Console.WriteLine();

                Console.WriteLine("Available Device Types:");
                Console.WriteLine("1: H650 Baby and Neonatal Scales");
                Console.WriteLine("2: Healthweigh™ Ultrasonic Physician BMI Scale - H120");
                Console.WriteLine();
                Console.WriteLine(lineDelimiter);
                Console.WriteLine();

                var ports = SerialPort.GetPortNames();
                Console.WriteLine("Available COM ports:");
                foreach (string p in ports)
                {
                    Console.WriteLine(p);
                }
                Console.WriteLine();
                Console.WriteLine(lineDelimiter);
                Console.WriteLine();

                Console.WriteLine("Connected devices found:");
                var connectedDevices = deviceManager
                    .GetConnectedDevicesIdAndPort()
                    .Split('|')
                    .Select(d =>
                    {
                        var details = d.Split(',');
                        return $"Device Id: {details[0]}, Port: {details[1]}";
                    });
                foreach (var device in connectedDevices)
                {
                    Console.WriteLine(device);
                }
                Console.WriteLine();
                Console.WriteLine(lineDelimiter);
                Console.WriteLine();
                Console.WriteLine();

                Console.WriteLine("Device ID:");
                int deviceId = int.Parse(Console.ReadLine());
                Console.WriteLine("Default COM Port: (to be used if the tool doesn't detect itself)");
                Console.Write("COM");
                string defaultPort = $"COM{Console.ReadLine()}";

                Console.Clear();
                Console.WriteLine("Selected device: " + deviceId);
                Console.WriteLine("Selected port: " + defaultPort);
                Console.WriteLine();

                try
                {
                    deviceManager.SetDevice(deviceId, defaultPort);
                    while (true)
                    {
                        Console.WriteLine(deviceManager.ReadAsString());
                        Console.Write("");
                        Console.ReadKey();
                    }
                }
                catch (UnsupportedDeviceException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (DeviceNotSetException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }

                Console.ReadLine();
            }
        }
    }
}
