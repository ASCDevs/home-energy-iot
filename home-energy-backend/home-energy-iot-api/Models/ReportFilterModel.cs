namespace home_energy_api.Models
{
    public class ReportFilterModel
    {
        public string DeviceIdentificationCode { get; set; }
        public DateTime initialDate { get; set; }
        public DateTime finalDate { get; set; }
    }
}
