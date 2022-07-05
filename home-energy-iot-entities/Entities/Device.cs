using System.ComponentModel.DataAnnotations.Schema;

namespace home_energy_iot_entities.Entities
{
    [Table("Device")]
    public class Device
    {
        public int Id { get; set; }
        public int IdHouse { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime RegisterDate { get; set; }
        public decimal Watts { get; set; }
    }
}
