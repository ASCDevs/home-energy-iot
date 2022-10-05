﻿using home_energy_iot_entities;
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

        public async Task Create(House house)
        {
            await _dataBaseContext.Houses.AddAsync(house);
            await _dataBaseContext.SaveChangesAsync();
        }

        public async Task Update(House house)
        {
            _dataBaseContext.Houses.Update(house);
            await _dataBaseContext.SaveChangesAsync();
        }

        public async Task Delete(House house)
        {
            _dataBaseContext.Houses.Remove(house);
            await _dataBaseContext.SaveChangesAsync();
        }

        public async Task<House> Get(int id)
        {
            return _dataBaseContext.Houses.Find(id);
        }

        public async Task<IEnumerable<House>> GetAll()
        {
            return _dataBaseContext.Houses.ToList();
        }

        public async Task<IEnumerable<House>> GetByUserId(int id)
        {
            return _dataBaseContext.Houses.Where(x => x.IdUser == id).ToList();
        }

        //TODO: Buscar o valor do kwh hora
        //query exemplo para criar o LINQ
        //
        // select[h].[ValuePerKWH], [d].[IdentificationCode] from device d
        // inner join house h on d.IdHouse where d.IdentificationCode = 'HU:34:DS4:D1'
        public double GetHouseBaseKwhByDeviceIdentificationCode(string deviceIdentificationCode)
        {
            throw new NotImplementedException();
        }
    }
}