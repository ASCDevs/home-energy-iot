using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace home_energy_api.Models.Db
{
    public class ReportDevice
    {
        [Key]
        public int IdReportDevice { get; set; }
        [ForeignKey("Device")]
        public int IdDevice { get; set; }
        public DateTime DtReport { get; set; }
        public decimal ValueTotal { get; set; }
    }
}
