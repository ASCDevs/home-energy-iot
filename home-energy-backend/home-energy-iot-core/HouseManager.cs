using home_energy_iot_core.Interfaces;
using home_energy_iot_entities;
using home_energy_iot_entities.Entities;
using Microsoft.Extensions.Logging;

namespace home_energy_iot_core
{
    public class HouseManager : IHouseManager
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
                ValidateHouse(house);

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
            try
            {
                ValidateHouse(house);

                _logger.LogInformation("Atualizando a casa na base de dados");

                _context.Houses.Update(house);

                await _context.SaveChangesAsync();

                _logger.LogInformation("Casa atualizada na base de dados");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar a casa.");
                throw;
            }
        }

        public async Task DeleteHouse(House house)
        {
            try
            {
                ValidateHouse(house);

                _logger.LogInformation($"Deletando casa com Id [{house.Id}] da base de dados");

                _context.Houses.Remove(house);

                await _context.SaveChangesAsync();

                _logger.LogInformation("Casa removida da base de dados");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao remover a casa.");
                throw;
            }
        }

        public Task<House> GetHouse(int id)
        {
            try
            {
                if (id < 0)
                    throw new ArgumentOutOfRangeException(nameof(id), "Id inválido.");

                _logger.LogInformation($"Buscando a casa com Id [{id}].");

                var house = _context.Houses.Find(id);

                if (house != null)
                    return Task.FromResult(house);

                var errorMessage = $"Casa com Id [{id}] não encontrado.";

                _logger.LogInformation(errorMessage);
                throw new Exception(errorMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao buscar a casa com Id [{id}].");
                throw;
            }
        }

        public async Task<IEnumerable<House>> GetHouses()
        {
            try
            {
                _logger.LogInformation("Buscando casas na base de dados.");

                var houses = _context.Houses.ToList();

                if (houses.Count > 0)
                    return houses;

                var message = "Nenhuma casa encontrada.";

                _logger.LogInformation(message);
                throw new Exception(message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao consultar as casas.");
                throw;
            }
        }

        private void ValidateHouse(House house)
        {
            if (house is null)
                throw new ArgumentNullException(nameof(house), "Casa nula.");

            //Incluir outras validações caso seja necessário.
        }
    }
}
