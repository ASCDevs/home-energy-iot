using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace home_energy_api.Models.Db
{
    public class HouseUser
    {
        [Key]
        public int IdHouseUser { get; set; }
        [ForeignKey("User")]
        public int IdUser { get; set; }
        public string HouseName { get; set; }
        public string? TypeAddress { get; set; }
        public string? NameAddress { get; set; }
        public string? NumberAddress { get; set; }
        public DateTime DtRegistration { get; set; }
        public int PeriodDaysReport { get; set; }
    }
}
