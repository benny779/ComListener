namespace ComListener.SerialDevices
{
    internal class Device2 : Device1
    {
        private readonly DeviceID _id = DevicesRepository.GetByID(2);
        public override DeviceID ID => _id;
        public override string Name => "Healthweigh™ Ultrasonic Physician BMI Scale - H120";

        public Device2() : base()
        {
        }

        public Device2(string defaultPort) : base(defaultPort)
        {
        }
    }
}
