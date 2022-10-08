using home_energy_iot_entities.Entities;

namespace home_energy_iot_repository.Interfaces
{
    public interface IHouseManagerRepository
    {
        void Create(House house);

        void Update(House house);

        void Delete(int id);

        House Get(int id);

        List<House> GetAll();

        List<House> GetByUserId(int id);

        double GetHouseBaseKwhByDeviceIdentificationCode(string deviceIdentificationCode);
    }
}
