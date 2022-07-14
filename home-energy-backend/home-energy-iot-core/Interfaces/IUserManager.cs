using home_energy_iot_entities.Entities;

namespace home_energy_iot_core.Interfaces
{
    public interface IUserManager
    {
        Task CreateUser(User user);
        Task UpdateUser(User user);
        Task ChangePassword(User user);
        Task<User> GetUser(int id);
        Task<IEnumerable<User>> GetUsers();
    }
}
