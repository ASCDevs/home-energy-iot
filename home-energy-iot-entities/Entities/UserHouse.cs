using System.ComponentModel.DataAnnotations.Schema;

namespace home_energy_iot_entities.Entities
{
    [Table("UserHouse")]
    public class UserHouse
    {
        public int Id { get; set; }
        public int IdUser { get; set; }
        public string Name { get; set; }
        public string TypeAddress { get; set; }
        public string NameAddress { get; set; }
        public int NumberAddress { get; set; }
        public DateTime RegisterDate { get; set; }
        public int PeriodDaysReport { get; set; }
    }
}
