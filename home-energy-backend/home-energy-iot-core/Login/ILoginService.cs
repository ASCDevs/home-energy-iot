using home_energy_api.Core.Models;
using home_energy_iot_entities.Entities;

namespace home_energy_iot_core.Login
{
    public interface ILoginService
    {
        UserLoginModel Login(LoginModel loginModel);
    }
}