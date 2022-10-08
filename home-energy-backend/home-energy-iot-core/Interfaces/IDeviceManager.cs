using home_energy_iot_entities.Entities;

namespace home_energy_iot_core.Interfaces
{
    public interface IDeviceManager
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