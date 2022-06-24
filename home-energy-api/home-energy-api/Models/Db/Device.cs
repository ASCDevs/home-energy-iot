using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace home_energy_iot_api.Models.Db
{
    public class Device
    {
        [Key]
        public int IdDevice { get; set; }
        [ForeignKey("HouseUser")]
        public int IdHouseUser { get; set; }
        public string? NameDevice { get; set; }
        public string? DescDevice { get; set; }
        public DateTime DtRegistration { get; set; }
        public DateTime DtInactivation { get; set; }
    }
}