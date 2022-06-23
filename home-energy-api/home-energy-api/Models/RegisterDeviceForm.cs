namespace home_energy_api.Models
{
    public class RegisterDeviceForm
    {

        public int IdDevice { get; set; }
        public int IdHouseUer { get; set; }
        public string NameDevice { get; set; }
        public string? DescDevice { get; set; }
    }
}
