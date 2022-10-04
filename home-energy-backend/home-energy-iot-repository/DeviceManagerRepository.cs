using home_energy_iot_entities;
using home_energy_iot_entities.Entities;
using home_energy_iot_repository.Interfaces;
using Microsoft.EntityFrameworkCore;

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

        public async Task<Device> Get(int id)
        {
             var result = await _dataBaseContext.Devices.FindAsync(id);

             return result ?? new Device();
        }

        public async Task<List<Device>> GetAll()
        {
            return _dataBaseContext.Devices.ToList();
        }

        public async Task<List<Device>> GetByHouseId(int id)
        {
            return _dataBaseContext.Devices.Where(x => x.IdHouse == id).ToList();
        }

        public async Task<bool> Exists(string deviceid)
        {
            return _dataBaseContext.Devices.FirstOrDefault(x => x.IdentificationCode == deviceid) != null;
        }
    }
}