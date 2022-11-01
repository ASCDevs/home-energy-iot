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

        public void Create(Device device)
        {
            _dataBaseContext.Devices.Add(device);
            _dataBaseContext.SaveChanges();
        }

        public void Update(Device device)
        {
            _dataBaseContext.Devices.Update(device);
            _dataBaseContext.SaveChanges();
        }

        public void Delete(int id)
        {
            var device = new Device
            {
                Id = id
            };

            _dataBaseContext.Devices.Remove(device);
            _dataBaseContext.SaveChanges();
        }

        public Device Get(int id)
        {
             var result = _dataBaseContext.Devices.Find(id);

             return result;
        }

        public List<Device> GetAll()
        {
            return _dataBaseContext.Devices.ToList();
        }

        public List<Device> GetByHouseId(int id)
        {
            return _dataBaseContext.Devices.Where(x => x.IdHouse == id).ToList();
        }

        public bool Exists(string deviceIdentificationCode)
        {
            return _dataBaseContext.Devices.FirstOrDefault(x => x.IdentificationCode == deviceIdentificationCode) != null;
        }
    }
}