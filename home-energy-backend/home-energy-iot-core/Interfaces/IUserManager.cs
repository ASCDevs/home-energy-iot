using home_energy_iot_entities.Entities;

namespace home_energy_iot_core.Interfaces
{
    public interface IUserManager
    {
        void Create(User user);

        void Update(User user);

        User Get(int id);

        List<User> GetAll();
    }
}