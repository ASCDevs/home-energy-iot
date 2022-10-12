using home_energy_iot_entities.Entities;

namespace home_energy_iot_core.Login.Authentication
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}