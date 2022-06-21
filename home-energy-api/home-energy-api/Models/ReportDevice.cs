namespace home_energy_api.Models
{
    public class ReportDevice
    {
        public int IdReportDevice { get; set; }
        public int IdDevice { get; set; }
        public DateTime DtReport { get; set; }
        public decimal ValueTotal { get; set; }
    }
}
