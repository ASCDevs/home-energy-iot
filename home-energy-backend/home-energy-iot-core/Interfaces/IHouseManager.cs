using home_energy_iot_entities.Entities;

namespace home_energy_iot_core.Interfaces
{
    public interface IHouseManager
    {
        Task Create(House house);

        Task Update(House house);

        Task Delete(House house);

        Task<House> Get(int id);

        Task<IEnumerable<House>> GetAll();
    }
}