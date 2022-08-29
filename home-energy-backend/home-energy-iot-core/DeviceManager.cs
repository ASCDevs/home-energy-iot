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

        public async Task Create(Device device)
        {
            try
            {
                if(device is null)
                    throw new ArgumentNullException(nameof(device), "Objeto do Dispositivo Nulo.");

                ValidadeDevice(device);

                _logger.LogInformation($"Criando Dispositivo: [{device.Name}].");

                await _deviceManagerRepository.Create(device);

                _logger.LogInformation($"Dispositivo [{device.Name}] criado com sucesso.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar o Dispositivo.");
                throw;
            }
        }

        public async Task Update(Device device)
        {
            try
            {
                if (device is null)
                    throw new ArgumentNullException(nameof(device), "Objeto do Dispositivo Nulo.");

                ValidateDeviceId(device.Id);
                ValidadeDevice(device);

                _logger.LogInformation($"Atualizando Dispositivo Id [{device.Id}].");

                await _deviceManagerRepository.Update(device);

                _logger.LogInformation($"Dispositivo Id [{device.Id}] atualizado com sucesso.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar o Dispositivo.");
                throw;
            }
        }

        public async Task Delete(Device device)
        {
            try
            {
                if (device is null)
                    throw new ArgumentNullException(nameof(device), "Objeto do Dispositivo Nulo.");

                ValidateDeviceId(device.Id);

                _logger.LogInformation($"Deletando Dispositivo Id [{device.Id}].");

                await _deviceManagerRepository.Delete(device);

                _logger.LogInformation($"Dispositivo Id [{device.Id}] deletado com sucesso.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao deletar o Dispositivo.");
                throw;
            }
        }

        public async Task<Device> Get(int id)
        {
            try
            {
                ValidateDeviceId(id);

                _logger.LogInformation($"Buscando o Dispositivo com Id [{id}].");

                var device = await _deviceManagerRepository.Get(id);

                if (device.Id > 0)
                {
                    _logger.LogInformation($"Dispositivo Id [{id}] encontrado. Retornando resultado.");
                    return device;
                }

                var notFoundMessage = $"Dispositivo Id [{id}] não encontrado.";
                 
                _logger.LogError(notFoundMessage);
                throw new EntityNotFoundException(notFoundMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao buscar o Dispositivo Id [{id}].");
                throw;
            }
        }

        public async Task<List<Device>> GetAll()
        {
            try
            {
                _logger.LogInformation("Buscando Dispositivos na base de dados.");

                var devices = _deviceManagerRepository.GetAll().Result.ToList();

                if (devices.Count > 0)
                {
                    _logger.LogInformation("Retornando os dispositivos encontrados.");
                    return devices;
                }

                var notFoundMessage = "Nenhum Dispositivo encontrado.";

                _logger.LogInformation(notFoundMessage);
                throw new EntityNotFoundException(notFoundMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao buscar os Dispositivos.");
                throw;
            }
        }

        private void ValidadeDevice(Device device)
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