using home_energy_iot_entities.Entities;

namespace home_energy_iot_repository.Interfaces
{
    public interface IDeviceManagerRepository
    {
        Task Create(Device device);
        Task Update(Device device);
        Task Delete(Device device);
        Task<Device> Get(int id);
        Task<List<Device>> GetAll();
        Task<List<Device>> GetByHouseId(int id);
        Task<bool> Exists(string deviceid);
    }
}
