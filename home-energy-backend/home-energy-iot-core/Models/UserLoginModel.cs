using home_energy_iot_api.Core.Models;

namespace home_energy_api.Core.Models
{
    public class UserLoginModel
    {
        public UserModel User { get; set; }
        public string UserToken { get; set; }
    }
}
