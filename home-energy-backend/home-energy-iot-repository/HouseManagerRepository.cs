using home_energy_iot_entities;
using home_energy_iot_entities.Entities;
using home_energy_iot_repository.Interfaces;

namespace home_energy_iot_repository
{
    public class HouseManagerRepository : IHouseManagerRepository
    {
        private DataBaseContext _dataBaseContext;

        public HouseManagerRepository(DataBaseContext dataBaseContext)
        {
            _dataBaseContext = dataBaseContext;
        }

        public void Create(House house)
        {
            _dataBaseContext.Houses.Add(house);
            _dataBaseContext.SaveChanges();
        }

        public void Update(House house)
        {
            _dataBaseContext.Houses.Update(house);
            _dataBaseContext.SaveChanges();
        }

        public void Delete(int id)
        {
            var house = new House
            {
                Id = id
            };

            _dataBaseContext.Houses.Remove(house);
            _dataBaseContext.SaveChanges();
        }

        public House Get(int id)
        {
            return _dataBaseContext.Houses.Find(id);
        }

        public List<House> GetAll()
        {
            return _dataBaseContext.Houses.ToList();
        }

        public List<House> GetByUserId(int id)
        {
            return _dataBaseContext.Houses.Where(x => x.IdUser == id).ToList();
        }

        public double GetHouseBaseKwhByDeviceIdentificationCode(string deviceIdentificationCode)
        {
            double kwhValue = 0;

            var query = from device in _dataBaseContext.Devices
                join house in _dataBaseContext.Houses
                    on device.IdHouse equals house.Id where device.IdentificationCode == deviceIdentificationCode
                        select new { result = house.ValuePerKWH };

            foreach (var result in query)
            {
                kwhValue = result.result;
            }

            return kwhValue;
        }
    }
}