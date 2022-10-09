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
                if(house is null)
                    throw new ArgumentNullException(nameof(house), "Objeto da Casa Nulo.");

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

                ValidateDeviceId(house.Id);

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
                if (id <= 0)
                    throw new InvalidEntityNumericValueException($"Id do usuário [{id}] inválido.");

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
            if (house.IdUser <= 0)
                throw new InvalidEntityNumericValueException($"Id de referência ao usuário inválido: [{house.IdUser}]");

            if (string.IsNullOrWhiteSpace(house.Name))
                throw new InvalidEntityTextValueException("Nome da Casa nulo ou vazio.");

            if(string.IsNullOrWhiteSpace(house.TypeAddress))
                throw new InvalidEntityTextValueException("Tipo do endereço nulo ou vazio.");

            if (string.IsNullOrWhiteSpace(house.NameAddress))
                throw new InvalidEntityTextValueException("Nome do endereço nulo ou vazio.");

            if(house.NumberAddress <= 0)
                throw new InvalidEntityNumericValueException($"Número do endereço inválido [{house.NumberAddress}].");

            if (house.PeriodDaysReport <= 0)
                throw new InvalidEntityNumericValueException($"Período de report inválido [{house.NumberAddress}].");

            if (house.ValuePerKWH <= 0)
                throw new InvalidEntityNumericValueException($"Valor do KWH inválido [{house.ValuePerKWH}].");
        }

        private void ValidateDeviceId(int id)
        {
            if (id <= 0)
                throw new InvalidEntityNumericValueException($"Id da Casa inválido: [{id}].");
        }
    }
}