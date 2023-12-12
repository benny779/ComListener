using ComListener.SerialDevices.Abstract;
using System.Text.RegularExpressions;

namespace ComListener.SerialDevices
{
    internal class Device1 : SerialDeviceBaseWriteRead
    {
        private readonly DeviceID _id = DevicesRepository.GetByID(1);
        public override DeviceID ID => _id;
        public override string Name => "H650 Baby and Neonatal Scales";

        // <ESC>R<ESC>W00.000<ESC>H000.0<ESC>B00.0<ESC>Nm<ESC>E
        static readonly Regex regex = new Regex(
               @"^\x1B[R]\x1B[W][\d\s.]{6}\x1B[H][\d\s.]{5}\x1B[B][\d\s.]{4}\x1B[N][mc]\x1B.$",
               RegexOptions.IgnoreCase);
        public override Regex Regex => regex;


        static readonly string esc = Utils.FromAscii(27);
        protected override string PortRequestString => $"{esc}R{esc}E";

        public Device1() : base()
        {
        }

        public Device1(string defaultPort) : base(defaultPort)
        {
        }


        const string overloadedScaleValue = "9999.9";
        const int weightSubstringStart = 4;
        const int weightSubstringLength = 6;
        const int heightSubstringStart = 12;
        const int heightSubstringLength = 5;
        const int bmiSubstringStart = 19;
        const int bmiSubstringLength = 4;
        public override DeviceResponse Parse(string value)
        {
            var weight = value.Substring(weightSubstringStart, weightSubstringLength).Trim();
            if (weight == overloadedScaleValue)
                weight = null;

            var height = value.Substring(heightSubstringStart, heightSubstringLength).Trim();
            var bmi = value.Substring(bmiSubstringStart, bmiSubstringLength).Trim();

            return DeviceResponse.Create(weight, height, bmi);
        }
    }
}
