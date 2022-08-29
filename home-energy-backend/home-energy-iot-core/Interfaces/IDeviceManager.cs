using home_energy_iot_entities.Entities;

namespace home_energy_iot_core.Interfaces
{
    public interface IDeviceManager
    {
        Task Create(Device device);

        Task Update(Device device);

        Task Delete(Device device);

        Task<Device> Get(int id);

        Task<List<Device>> GetAll();
    }
}