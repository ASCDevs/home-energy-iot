using home_energy_iot_core.Exceptions;
using home_energy_iot_core.Interfaces;
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

        public void Create(House house)
        {
            try
            {
                ValidateHouse(house);

                _logger.LogInformation($"Criando Casa: [{house.Name}].");

                house.RegisterDate = DateTime.Now;

                _houseManagerRepository.Create(house);

                _logger.LogInformation($"Casa [{house.Name}] criada com sucesso.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar a Casa.");
                throw;
            }
        }

        public void Update(House house)
        {
            try
            {
                ValidateHouse(house);

                _logger.LogInformation($"Atualizando Casa Id [{house.Id}].");

                _houseManagerRepository.Update(house);

                _logger.LogInformation($"Casa Id [{house.Id}] atualizada com sucesso.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar a Casa.");
                throw;
            }
        }

        public void Delete(int id)
        {
            try
            {
                ValidateDeviceId(id);

                _logger.LogInformation($"Deletando Casa Id [{id}].");

                _houseManagerRepository.Delete(id);

                _logger.LogInformation($"Casa Id [{id}] deletada com sucesso.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao deletar a Casa.");
                throw;
            }
        }

        public House Get(int id)
        {
            try
            {
                ValidateDeviceId(id);

                _logger.LogInformation($"Buscando a Casa com Id [{id}].");

                var house = _houseManagerRepository.Get(id);

                if (house?.Id > 0)
                {
                    _logger.LogInformation($"Casa Id [{id}] encontrada. Retornando resultado.");
                    return house;
                }

                var errorMessage = $"Casa com Id [{id}] não encontrada.";

                _logger.LogInformation(errorMessage);
                throw new EntityNotFoundException(errorMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao buscar a Casa Id [{id}].");
                throw;
            }
        }

        public List<House> GetAll()
        {
            try
            {
                _logger.LogInformation("Buscando Casas na base de dados.");

                var houses = _houseManagerRepository.GetAll();

                if (houses?.Count > 0)
                {
                    _logger.LogInformation("Retornando as Casas encontradas.");
                    return houses;
                }

                var message = "Nenhuma Casa encontrada.";

                _logger.LogInformation(message);
                throw new EntityNotFoundException(message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao buscar as Casas.");
                throw;
            }
        }

        public List<House> GetByUserId(int id)
        {
            try
            {
                ValidateDeviceId(id);

                _logger.LogInformation($"Buscando as Casas do usuário Id [{id}].");

                var houses = _houseManagerRepository.GetByUserId(id);

                if (houses?.Count > 0)
                {
                    _logger.LogInformation("Retornando as Casas encontradas.");
                    return houses;
                }

                var message = $"Nenhuma Casa encontrada para o usuário Id [{id}].";

                _logger.LogInformation(message);
                throw new EntityNotFoundException(message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao buscas as Casas do usuário com Id [{id}].");
                throw;
            }
        }

        private void ValidateHouse(House house)
        {
            if (house is null)
                throw new ArgumentNullException(nameof(house), "Casa nula.");
        }

        private void ValidateDeviceId(int id)
        {
            if (id <= 0)
                throw new InvalidEntityNumericValueException($"Id da casa inválido: [{id}].");
        }
    }
}