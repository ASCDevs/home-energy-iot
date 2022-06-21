namespace home_energy_api.Models
{
    public class Device
    {
        public int IdDevice { get; set; }
        public int IdHomeUser { get; set; }
        public string? NameDevice { get; set; }
        public string? DescDevice { get; set; }
        public bool Online { get; set; }
        public DateTime DtRegistration { get; set; }
        public DateTime DtInactivation { get; set; }
        

    }
}
