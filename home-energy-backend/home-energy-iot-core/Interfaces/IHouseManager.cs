using home_energy_iot_entities.Entities;

namespace home_energy_iot_core.Interfaces
{
    public interface IHouseManager
    {
        void Create(House house);

        void Update(House house);

        void Delete(int id);

        House Get(int id);

        List<House> GetAll();

        List<House> GetByUserId(int id);
    }
}