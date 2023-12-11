namespace ComListener
{
    /// <summary>
    /// Represents the response from a serial device operation,
    /// including success status, error messages, and the results.
    /// </summary>
    public class DeviceResponse
    {
        const string separator = "|";

        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public string Weight { get; set; }
        public string Height { get; set; }
        public string BMI { get; set; }


        private DeviceResponse() { }

        internal static DeviceResponse Create(string weight = null, string height = null, string bmi = null)
        {
            return new DeviceResponse()
            {
                Success = true,
                Weight = weight,
                Height = height,
                BMI = bmi
            };
        }

        internal static DeviceResponse CreateError(string errorMessage)
        {
            return new DeviceResponse()
            {
                Success = false,
                ErrorMessage = errorMessage
            };
        }

        public override string ToString()
        {
            return Success ?
                $"W{Weight}{separator}H{Height}{separator}B{BMI}" :
                $"Error: {ErrorMessage}";
        }
    }
}
