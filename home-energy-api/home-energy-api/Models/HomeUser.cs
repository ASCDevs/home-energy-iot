namespace home_energy_api.Models
{
    public class HomeUser
    {
        public int IdHome { get; set; }
        public int IdUser { get; set; }
        public string HouseName { get; set; }
        public string? TypeAddress { get; set; }
        public string? NameAddress { get; set; }
        public string? NumberAddress { get; set; }
        public DateTime DtRegistration { get; set; }
        public int PeriodDaysReport { get; set; }
    }
}
