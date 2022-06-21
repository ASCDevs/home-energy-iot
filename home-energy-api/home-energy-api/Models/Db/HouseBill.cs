using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace home_energy_api.Models.Db
{
    public class HouseBill
    {
        [Key]
        public int IdHouseBill { get; set; }
        [ForeignKey("HouseUser")]
        public int IdHouseUser { get; set; }
        public int MonthBill { get; set; }
        public int YearBill { get; set; }
        public decimal TariffBill { get; set; }
        public decimal ValuePerKWH { get; set; }
        public decimal BaseKWH { get; set; }    

    }
}
