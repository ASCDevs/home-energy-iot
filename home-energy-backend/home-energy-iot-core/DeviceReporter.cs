using home_energy_iot_core.Interfaces;
using home_energy_iot_entities;
using home_energy_iot_entities.Entities;
using home_energy_iot_repository;
using home_energy_iot_repository.Interfaces;
using Microsoft.Extensions.Logging;

namespace home_energy_iot_core
{
    public class DeviceReporter : IDeviceReporter
    {
        private ILogger<DeviceReporter> _logger;
        private IDeviceReporterRepository _deviceReporterRepository;

        public DeviceReporter(ILogger<DeviceReporter> logger, IDeviceReporterRepository deviceReporterRepository)
        {
            _logger = logger;
            _deviceReporterRepository = deviceReporterRepository;
        }

        public async Task Report(DeviceReport device)
        {
            try
            {
                if (device is null)
                    throw new ArgumentNullException(nameof(device), "Dispositivo nulo.");

                _logger.LogInformation($"Adicionando Report do Dispositivo com Id [{device.IdDevice}].");

                await _deviceReporterRepository.Report(device);

                _logger.LogInformation($"Report do Dispositivo com Id [{device.IdDevice}] adicionado com sucesso.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao reportar o Dispositivo com Id [{device.IdDevice}].");
                throw;
            }
        }
    }
}