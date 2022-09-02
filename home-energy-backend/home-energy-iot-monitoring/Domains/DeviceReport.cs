namespace home_energy_iot_monitoring.Domains
{
    public class DeviceReport
    {
        public int Id { get; set; }
        public string IdentificationCode { get; set; }
        public DateTime ReportDate { get; set; }
        public decimal WattsUsage { get; set; }

        public DeviceReport(string energyVale, string deviceId)
        {
            WattsUsage = DecimalConverter(energyVale);
            IdentificationCode = deviceId;
            ReportDate = DateTime.Now;
        }

        private decimal DecimalConverter(string value)
        {
            if (value.Contains("."))
            {
                value = value.Replace(".", ",");
            }

            return Convert.ToDecimal(value);
        }
    }
}
