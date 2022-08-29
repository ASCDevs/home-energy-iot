using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace home_energy_iot_entities.Entities
{
    [Table("DeviceReport")]
    public class DeviceReport
    {
        [Key]
        public int Id { get; set; }
        public string IdentificationCode { get; set; }
        public DateTime ReportDate { get; set; }
        public decimal WattsUsage{ get; set; }
    }
}