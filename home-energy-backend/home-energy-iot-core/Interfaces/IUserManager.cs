using home_energy_iot_entities.Entities;

namespace home_energy_iot_core.Interfaces
{
    public interface IUserManager
    {
        Task Create(User user);
        Task Update(User user);
        Task ChangePassword(User user);
        Task<User> Get(int id);
        Task<IEnumerable<User>> GetAll();
    }
}
