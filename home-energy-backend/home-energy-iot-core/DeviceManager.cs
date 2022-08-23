﻿using home_energy_iot_core.Interfaces;
using home_energy_iot_entities;
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
                ValidadeDevice(device);

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

        public Task<Device> Get(int id)
        {
            try
            {
                if (id < 0)
                    throw new ArgumentOutOfRangeException(nameof(id), $"Id do dispositivo inválido: [{id}].");

                _logger.LogInformation($"Buscando o Dispositivo com Id [{id}].");

                var device = _deviceManagerRepository.Get(id);

                if (device != null)
                {
                    _logger.LogInformation($"Dispositivo Id [{id}] encontrado. Retornando resultado.");
                    return Task.FromResult(device);
                }

                var notFound = $"Dispositivo Id [{id}] não encontrado.";

                _logger.LogError(notFound);
                throw new Exception(notFound);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao buscar o Dispositivo Id [{id}].");
                throw;
            }
        }

        public Task<IEnumerable<Device>> GetAll()
        {
            try
            {
                _logger.LogInformation("Buscando Dispositivos na base de dados.");

                var devices = _deviceManagerRepository.GetAll();

                if (devices.Count > 0)
                    return Task.FromResult<IEnumerable<Device>>(devices);

                var message = "Nenhum Dispositivo encontrado.";

                _logger.LogInformation(message);
                throw new Exception(message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao buscar os Dispositivos.");
                throw;
            }
        }

        private void ValidadeDevice(Device device)
        {
            if (device is null)
                throw new ArgumentNullException(nameof(device), "Dispositivo nulo.");
        }
    }
}