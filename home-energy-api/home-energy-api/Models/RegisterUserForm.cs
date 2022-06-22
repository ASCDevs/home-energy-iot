namespace home_energy_api.Models
{
    public class RegisterUserForm
    {
        public int IdUser { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string UserCPF { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

    }
}
