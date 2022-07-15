using home_energy_iot_entities.Entities;

namespace home_energy_iot_core.Interfaces
{
    public interface IDeviceManager
    {
        Task CreateDevice(Device device);
        Task UpdateDevice(Device device);
        Task DeleteDevice(Device device);
        Task<Device> GetDevice(int id);
        Task<IEnumerable<Device>> GetDevices();
    }
}