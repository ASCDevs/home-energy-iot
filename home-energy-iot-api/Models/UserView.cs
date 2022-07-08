namespace home_energy_iot_api.Models
{
    public class UserView
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public DateTime DtRegistration { get; set; }
    }
}
