using home_energy_iot_core.Exceptions;
using home_energy_iot_core.Interfaces;
using home_energy_iot_entities.Entities;
using home_energy_iot_repository.Interfaces;
using Microsoft.Extensions.Logging;

namespace home_energy_iot_core
{
    public class DeviceManager : IDeviceManager
    {
        private ILogger<DeviceManager> _logger;
        private IDeviceManagerRepository _deviceManagerRepository;

        public DeviceManager(ILogger<DeviceManager> logger, IDeviceManagerRepository deviceManagerRepository)
        {
            _logger = logger;
            _deviceManagerRepository = deviceManagerRepository;
        }

        public void Create(Device device)
        {
            try
            {
                if(device is null)
                    throw new ArgumentNullException(nameof(device), "Objeto do Dispositivo Nulo.");

                ValidateDevice(device);

                _logger.LogInformation($"Criando Dispositivo: [{device.Name}].");

                device.RegisterDate = DateTime.UtcNow.AddHours(-3);

                _deviceManagerRepository.Create(device);

                _logger.LogInformation($"Dispositivo [{device.Name}] criado com sucesso.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar o Dispositivo.");
                throw;
            }
        }

        public void Update(Device device)
        {
            try
            {
                if (device is null)
                    throw new ArgumentNullException(nameof(device), "Objeto do Dispositivo Nulo.");

                ValidateDeviceId(device.Id);
                ValidateDevice(device);

                _logger.LogInformation($"Atualizando Dispositivo Id [{device.Id}].");

                _deviceManagerRepository.Update(device);

                _logger.LogInformation($"Dispositivo Id [{device.Id}] atualizado com sucesso.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar o Dispositivo.");
                throw;
            }
        }

        public void Delete(int id)
        {
            try
            {
                ValidateDeviceId(id);

                _logger.LogInformation($"Deletando Dispositivo Id [{id}].");

                _deviceManagerRepository.Delete(id);

                _logger.LogInformation($"Dispositivo Id [{id}] deletado com sucesso.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao deletar o Dispositivo.");
                throw;
            }
        }

        public Device Get(int id)
        {
            try
            {
                ValidateDeviceId(id);

                _logger.LogInformation($"Buscando o Dispositivo com Id [{id}].");

                var device = _deviceManagerRepository.Get(id);

                if (device?.Id > 0)
                {
                    _logger.LogInformation($"Dispositivo Id [{id}] encontrado. Retornando resultado.");
                    return device;
                }

                throw new EntityNotFoundException($"Dispositivo Id [{id}] não encontrado.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao buscar o Dispositivo Id [{id}].");
                throw;
            }
        }

        public List<Device> GetAll()
        {
            try
            {
                _logger.LogInformation("Buscando Dispositivos na base de dados.");

                var devices = _deviceManagerRepository.GetAll();

                if (devices?.Count > 0)
                {
                    _logger.LogInformation("Retornando os dispositivos encontrados.");
                    return devices;
                }

                throw new EntityNotFoundException("Nenhum Dispositivo encontrado.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao buscar os Dispositivos.");
                throw;
            }
        }

        public List<Device> GetByHouseId(int id)
        {
            try
            {
                ValidateDeviceId(id);

                _logger.LogInformation($"Buscando Dispositivos na base de dados da Casa Id [{id}].");

                var devices = _deviceManagerRepository.GetByHouseId(id);

                if (devices?.Count > 0)
                {
                    _logger.LogInformation($"Retornando os dispositivos encontrados para a Casa Id [{id}].");
                    return devices;
                }

                throw new EntityNotFoundException($"Nenhum Dispositivo encontrado para a Casa Id [{id}].");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao buscar os Dispositivos.");
                throw;
            }
        }

        public bool Exists(string deviceIdentificationCode)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(deviceIdentificationCode))
                    throw new InvalidEntityTextValueException("Código de identificação do dispositivo nulo ou vazio.");

                _logger.LogInformation($"Verificando a existência do Dispositivo com código de identificação [{deviceIdentificationCode}]");

                var result = _deviceManagerRepository.Exists(deviceIdentificationCode);

                if (result)
                {
                    _logger.LogInformation($"Dispositivo com código de identificação [{deviceIdentificationCode}] encontrado.");
                    return result;
                }

                _logger.LogInformation($"Dispositivo com código de identificação [{deviceIdentificationCode}] não encontrado.");
                return result;
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, $"Erro ao buscar os Dispositivos.");
                throw;
            }
        }

        private void ValidateDevice(Device device)
        {
            if(device.IdHouse <= 0)
                throw new InvalidEntityNumericValueException($"Id de referência à casa do dispositivo inválido: [{device.IdHouse}].");

            if (string.IsNullOrWhiteSpace(device.Name))
                throw new InvalidEntityTextValueException("Nome do dispositivo nulo ou vazio.");

            if (string.IsNullOrWhiteSpace(device.IdentificationCode))
                throw new InvalidEntityTextValueException("Código de identificação do dispositivo nulo ou vazio.");

            if (string.IsNullOrWhiteSpace(device.Description))
                throw new InvalidEntityTextValueException("Descrição do dispositivo nula ou vazia.");

            if(device.Watts <= 0)
                throw new InvalidEntityNumericValueException("Potência (Watts) do dispositivo inválida.");
        }

        private void ValidateDeviceId(int id)
        {
            if (id <= 0)
                throw new InvalidEntityNumericValueException($"Id do dispositivo inválido: [{id}].");
        }
    }
}