using home_energy_iot_entities.Entities;

namespace home_energy_iot_core.Login
{
    public interface ILoginService
    {
        User GetUser(string username);

        bool ValidPassword(string providedPassword, User user);
    }
}