using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace home_energy_iot_entities.Entities
{
    [Table("DeviceReport")]
    public class DeviceReport
    {
        [Key]
        public int Id { get; set; }
        public int IdDevice { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime LastDate { get; set; }
        public decimal Consumption { get; set; }
    }
}
