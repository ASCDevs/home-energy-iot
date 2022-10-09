using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace home_energy_iot_entities.Entities
{
    [Table("User")]
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? SaltPassword { get; set; }
        public string? CPF { get; set; }
        public string? Email { get; set; }
        public DateTime RegisterDate { get; set; }
    }
}