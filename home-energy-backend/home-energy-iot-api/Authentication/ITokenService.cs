using home_energy_iot_entities.Entities;

namespace home_energy_api.Authentication;

public interface ITokenService
{
    string GenerateToken(User user);
}