﻿using home_energy_iot_core.Interfaces;
using home_energy_iot_entities.Entities;
using home_energy_iot_repository.Interfaces;
using Microsoft.Extensions.Logging;

namespace home_energy_iot_core
{
    public class HouseManager : IHouseManager
    {
        private ILogger<HouseManager> _logger;
        private IHouseManagerRepository _houseManagerRepository;

        public HouseManager(ILogger<HouseManager> logger, IHouseManagerRepository houseManagerRepository)
        {
            _logger = logger;
            _houseManagerRepository = houseManagerRepository;
        }

        public async Task Create(House house)
        {
            try
            {
                ValidateHouse(house);

                _logger.LogInformation($"Criando Casa: [{house.Name}].");

                await _houseManagerRepository.Create(house);

                _logger.LogInformation($"Casa [{house.Name}] criada com sucesso.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar a Casa.");
                throw;
            }
        }

        public async Task Update(House house)
        {
            try
            {
                ValidateHouse(house);

                _logger.LogInformation($"Atualizando Casa Id [{house.Id}].");

                await _houseManagerRepository.Update(house);

                _logger.LogInformation($"Casa Id [{house.Id}] atualizada com sucesso.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar a Casa.");
                throw;
            }
        }

        public async Task Delete(House house)
        {
            try
            {
                ValidateHouse(house);

                _logger.LogInformation($"Deletando Casa Id [{house.Id}].");

                await _houseManagerRepository.Delete(house);

                _logger.LogInformation($"Casa Id [{house.Id}] deletada com sucesso.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao deletar a Casa.");
                throw;
            }
        }

        public Task<House> Get(int id)
        {
            try
            {
                if (id < 0)
                    throw new ArgumentOutOfRangeException(nameof(id), $"Id [{id}] da Casa inválido.");

                _logger.LogInformation($"Buscando a Casa com Id [{id}].");

                var house = _houseManagerRepository.Get(id);

                if (house != null)
                {
                    _logger.LogInformation($"Casa Id [{id}] encontrada. Retornando resultado.");
                    return house;
                }

                var errorMessage = $"Casa com Id [{id}] não encontrada.";

                _logger.LogInformation(errorMessage);
                throw new Exception(errorMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao buscar a Casa Id [{id}].");
                throw;
            }
        }

        public Task<IEnumerable<House>> GetAll()
        {
            try
            {
                _logger.LogInformation("Buscando Casas na base de dados.");

                var houses = _houseManagerRepository.GetAll().Result.ToList();

                if (houses.Count > 0)
                {
                    _logger.LogInformation("Retornando as Casas encontradas.");
                    return Task.FromResult<IEnumerable<House>>(houses);
                }

                var message = "Nenhuma Casa encontrada.";

                _logger.LogInformation(message);
                throw new Exception(message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao buscar as Casas.");
                throw;
            }
        }

        private void ValidateHouse(House house)
        {
            if (house is null)
                throw new ArgumentNullException(nameof(house), "Casa nula.");
        }
    }
}