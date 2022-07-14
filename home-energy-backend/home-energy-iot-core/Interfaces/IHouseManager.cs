using home_energy_iot_entities.Entities;

namespace home_energy_iot_core.Interfaces
{
    public interface IHouseManager
    {
        Task CreateHouse(House house);
        Task UpdateHouse(House house);
        Task DeleteHouse(House house);
        Task<House> GetHouse(int id);
        Task<IEnumerable<House>> GetHouses();
    }
}
