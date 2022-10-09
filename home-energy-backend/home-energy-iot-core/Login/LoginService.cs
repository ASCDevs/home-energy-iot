using home_energy_iot_core.Helpers.Interfaces;
using home_energy_iot_core.Interfaces;
using home_energy_iot_entities;
using home_energy_iot_entities.Entities;

namespace home_energy_iot_core.Login
{
    public class LoginService : ILoginService
    {
        private IHasher _hasher;

        public LoginService(IHasher hasher)
        {
            _hasher = hasher;
        }

        public bool ValidPassword(string providedPassword, User user)
        {
            if (_hasher.GenerateHash(providedPassword, user.SaltPassword) == user.Password)
                return true;

            return false;
        }
    }
}