using System.ComponentModel.DataAnnotations;

namespace home_energy_api.Models.Db
{
    public class User
    {
        [Key]
        public int IdUser { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string UserCPF { get; set; }
        public string Password { get; set; }
        public DateTime DtRegistration { get; set; }
        public DateTime DtInactivation { get; set; }

    }
}
