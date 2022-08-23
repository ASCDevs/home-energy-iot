using home_energy_iot_entities.Entities;

namespace home_energy_iot_repository.Interfaces
{
    public interface IDeviceManagerRepository
    {
        Task Create(Device device);
        Task Update(Device device);
        Task Delete(Device device);
        Device Get(int id);
        List<Device> GetAll();
    }
}
