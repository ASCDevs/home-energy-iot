namespace home_energy_api.Models
{
    public class DeviceView
    {
        public int IdDevice { get; set; }
        public int IdHouseUser { get; set; }
        public string? NameDevice { get; set; }
        public string? DescDevice { get; set; }
        public string DtRegistration { get; set; }
    }
}
