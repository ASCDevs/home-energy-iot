﻿using home_energy_iot_entities.Entities;

namespace home_energy_iot_repository.Interfaces
{
    public interface IHouseManagerRepository
    {
        Task Create(House house);

        Task Update(House house);

        Task Delete(int id);

        Task<House> Get(int id);

        Task<List<House>> GetAll();

        Task<List<House>> GetByUserId(int id);

        double GetHouseBaseKwhByDeviceIdentificationCode(string deviceIdentificationCode);
    }
}
