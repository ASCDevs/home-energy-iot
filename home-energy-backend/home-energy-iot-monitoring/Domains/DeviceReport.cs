namespace home_energy_iot_monitoring.Domains
{
    public class DeviceReport
    {
        public int Id { get; set; }
        public string IdentificationCode { get; set; }
        public DateTime ReportDate { get; set; }
        public string WattsUsage { get; set; }

        public DeviceReport(string energyVale, string deviceId)
        {
            WattsUsage = energyVale;
            IdentificationCode = deviceId;
            ReportDate = DateTime.Now;
        }
    }
}
