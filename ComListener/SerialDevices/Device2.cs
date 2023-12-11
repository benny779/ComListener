using ComListener.SerialDevices.Abstract;
using System.Text.RegularExpressions;

namespace ComListener.SerialDevices
{
    internal class Device2 : SerialDeviceBaseRead
    {
        private readonly DeviceID _id = DevicesRepository.GetByID(2);
        public override DeviceID ID => _id;
        public override string Name => "Healthweigh™ Ultrasonic Physician BMI Scale - H120";

        static readonly Regex regex = new Regex(
              @"^(\d{2}\.\d{4}\.\d{3}\.\d{1}m)$",
              RegexOptions.IgnoreCase);
        public override Regex Regex => regex;


        public Device2() : base()
        {
        }

        public Device2(string defaultPort) : base(defaultPort)
        {
        }



        const int weightSubstringStart = 0;
        const int weightSubstringLength = 6;
        const int heightSubstringStart = 6;
        const int heightSubstringLength = 5;
        const int bmiSubstringStart = 11;
        const int bmiSubstringLength = 4;
        public override DeviceResponse Parse(string value)
        {
            //   59.8168.021.2m
            // wwwwwwhhhhhbbbb
            value = value.Trim().PadLeft(16, '0');

            var weight = value.Substring(weightSubstringStart, weightSubstringLength);
            var height = value.Substring(heightSubstringStart, heightSubstringLength);
            var bmi = value.Substring(bmiSubstringStart, bmiSubstringLength);

            return DeviceResponse.Create(weight, height, bmi);
        }
    }
}
