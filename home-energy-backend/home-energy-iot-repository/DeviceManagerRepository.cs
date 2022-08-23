using home_energy_iot_entities;
using home_energy_iot_entities.Entities;
using home_energy_iot_repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace home_energy_iot_repository
{
    public class DeviceManagerRepository : IDeviceManagerRepository
    {
        private readonly DataBaseContext _dbContext;
        public DeviceManagerRepository(DataBaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Create(Device device)
        {
            await _dbContext.Devices.AddAsync(device);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Update(Device device)
        {
            _dbContext.Devices.Update(device);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(Device device)
        {
            _dbContext.Devices.Remove(device);
            await _dbContext.SaveChangesAsync();
        }

        public Device Get(int id)
        {
            return _dbContext.Devices.Find(id);
        }

        public List<Device> GetAll()
        {
            return _dbContext.Devices.ToList();
        }
    }
}