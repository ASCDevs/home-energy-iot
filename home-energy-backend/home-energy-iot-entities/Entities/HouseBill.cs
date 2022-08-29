using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace home_energy_iot_entities.Entities
{
    [Table("HouseBill")]
    public class HouseBill
    {
        [Key]
        public int Id { get; set; }
        public int IdHouse { get; set; }
        public decimal MonthBill { get; set; }
        public decimal YearBill { get; set; }
        public decimal TariffBill { get; set; }
        public decimal ValuePerKWH { get; set; }
        public decimal BaseKWH { get; set; }
    }
}