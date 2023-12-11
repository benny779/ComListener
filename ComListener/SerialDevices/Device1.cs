using ComListener.SerialDevices.Abstract;
using System.Text.RegularExpressions;

namespace ComListener.SerialDevices
{
    internal class Device1 : SerialDeviceBaseWriteRead
    {
        private readonly DeviceID _id = DevicesRepository.GetByID(1);
        public override DeviceID ID => _id;
        public override string Name => "H650 Baby and Neonatal Scales";

        // <ESC>R<ESC>W00.000<ESC>H000.0<ESC>B00.0<ESC>Nm<ESC><ESC>E
        static readonly Regex regex = new Regex(
               @"^\x1B[R]\x1B[W]\d{2}[.]\d{3}\x1B[H]\d{3}[.]\d\x1B[B]\d{2}[.]\d\x1B[N][mc]\x1B[E]$",
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


        const int weightSubstringStart = 4;
        const int weightSubstringLength = 6;
        public override DeviceResponse Parse(string value)
        {
            var weight = value.Substring(weightSubstringStart, weightSubstringLength);

            return DeviceResponse.Create(weight);
        }
    }
}
