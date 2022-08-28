using home_energy_iot_entities;
using home_energy_iot_entities.Entities;
using home_energy_iot_repository.Interfaces;

namespace home_energy_iot_repository
{
    public class DeviceManagerRepository : IDeviceManagerRepository
    {
        private readonly DataBaseContext _dataBaseContext;
        public DeviceManagerRepository(DataBaseContext dataBaseContext)
        {
            _dataBaseContext = dataBaseContext;
        }

        public async Task Create(Device device)
        {
            await _dataBaseContext.Devices.AddAsync(device);
            await _dataBaseContext.SaveChangesAsync();
        }

        public async Task Update(Device device)
        {
            _dataBaseContext.Devices.Update(device);
            await _dataBaseContext.SaveChangesAsync();
        }

        public async Task Delete(Device device)
        {
            _dataBaseContext.Devices.Remove(device);
            await _dataBaseContext.SaveChangesAsync();
        }

        public Device Get(int id)
        {
            return _dataBaseContext.Devices.Find(id);
        }

        public List<Device> GetAll()
        {
            return _dataBaseContext.Devices.ToList();
        }
    }
}