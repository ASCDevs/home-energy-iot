using home_energy_iot_entities.Entities;

namespace home_energy_iot_core.Login
{
    public interface ILoginService
    {
        bool ValidPassword(string providedPassword, User user);
    }
}