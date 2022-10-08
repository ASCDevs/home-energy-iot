using home_energy_iot_entities.Entities;

namespace home_energy_iot_repository.Interfaces
{
    public interface IDeviceManagerRepository
    {
        void Create(Device device);
        void Update(Device device);
        void Delete(int id);
        Device Get(int id);
        List<Device> GetAll();
        List<Device> GetByHouseId(int id);
        bool Exists(string deviceIdentificationCode);
    }
}
