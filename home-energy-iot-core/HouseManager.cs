using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using home_energy_iot_core.Interfaces;
using home_energy_iot_entities;
using home_energy_iot_entities.Entities;
using Microsoft.Extensions.Logging;

namespace home_energy_iot_core
{
    internal class HouseManager : IHouseManager
    {
        private ILogger<HouseManager> _logger;

        private DataBaseContext _context;

        public HouseManager(ILogger<HouseManager> logger, DataBaseContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task CreateHouse(House house)
        {
            try
            {
                if (house is null)
                    throw new ArgumentNullException(nameof(house), "casa nula.");

                _logger.LogInformation("Salvando a casa na base de dados");

                await _context.Houses.AddAsync(house);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Casa criada com sucesso.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar a casa.");
                throw;
            }
        }

        public async Task UpdateHouse(House house)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteHouse(House house)
        {
            throw new NotImplementedException();
        }

        public async Task<House> GetHouse(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<House>> GetHouses()
        {
            throw new NotImplementedException();
        }
    }
}
