using home_energy_iot_core.Exceptions;
using home_energy_iot_core.Interfaces;
using home_energy_iot_entities.Entities;
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

        public void Report(DeviceReport report)
        {
            try
            {
                ValidateDeviceReport(report);

                _logger.LogInformation($"Adicionando Report do Dispositivo com Código de identificação [{report.IdentificationCode}] com [{report.WattsUsage}] Watts de uso.");

                report.ReportDate = DateTime.UtcNow.AddHours(-3);

                _deviceReporterRepository.Report(report);

                _logger.LogInformation($"Report do Dispositivo com Código de identificação [{report.IdentificationCode}] adicionado com sucesso.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao reportar o Dispositivo.");
                throw;
            }
        }

        private void ValidateDeviceReport(DeviceReport report)
        {
            if (report is null)
                throw new ArgumentNullException(nameof(report), "Dispositivo nulo.");

            if (report.WattsUsage < 0)
                throw new InvalidEntityNumericValueException($"Valor de consumo em potência(W) do report inválido. Valor recebido [{report.WattsUsage}].");

            if (string.IsNullOrWhiteSpace(report.IdentificationCode))
                throw new InvalidEntityTextValueException("Código de identificação do dispositivo nulo ou vazio.");
        }
    }
}