namespace home_energy_api.Models
{
    public class HouseBill
    {
        public int IdHouseBill { get; set; }
        public int IdHome { get; set; }
        public int MonthBill { get; set; }
        public int YearBill { get; set; }
        public decimal TariffBill { get; set; }
        public decimal ValuePerKWH { get; set; }
        public decimal BaseKWH { get; set; }    

    }
}
