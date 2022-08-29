using home_energy_iot_core.Interfaces;
using home_energy_iot_core.Models;
using home_energy_iot_repository.Interfaces;
using Microsoft.Extensions.Logging;

namespace home_energy_iot_core
{
    public class DeviceReportReader : IDeviceReportReader
    {
        private readonly ILogger<DeviceReportReader> _logger;
        private readonly IDeviceReportReaderRepository _deviceReportReaderRepository;

        public DeviceReportReader(ILogger<DeviceReportReader> logger, IDeviceReportReaderRepository deviceReportReaderRepository)
        {
            _logger = logger;
            _deviceReportReaderRepository = deviceReportReaderRepository;
        }

        public DeviceConsumption GetDeviceConsumptionTotalValue(string deviceIdentificationCode)
        {
            try
            {
                _logger.LogInformation($"Iniciando busca de todos os reports de Dispositivos com o Código de identificação [{deviceIdentificationCode}].");

                var reports = _deviceReportReaderRepository.GetDeviceConsumption(deviceIdentificationCode);

                _logger.LogInformation($"Total de reports encontrados para o Dispositivo [{deviceIdentificationCode}]: {reports.Count}.");

                if (reports.Count > 0)
                {
                    var wattsTotal = Convert.ToDouble(reports.Sum(x => x.WattsUsage));

                    var initialDate = reports[0].ReportDate;
                    var finalDate = reports[reports.Count - 1].ReportDate;

                    var consumption = new DeviceConsumption
                    {
                        IdentificationCode = deviceIdentificationCode,
                        ConsumptionInReal = CalculateWattsToReal(wattsTotal, initialDate, finalDate),
                        ConsumptionInWatts = wattsTotal,
                        InitialDate = initialDate,
                        FinalDate = finalDate
                    };

                    return consumption;
                }

                _logger.LogInformation($"Nenhum report encontrado para o Dispositivo [{deviceIdentificationCode}].");
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar os reports de dispositivos.");
                throw;
            }
        }

        public DeviceConsumption GetDeviceConsumptionValueBetweenDates(string deviceIdentificationCode, 
            DateTime initialDate,
            DateTime finalDate)
        {
            try
            {
                _logger.LogInformation($"Iniciando busca dos reports entre {initialDate} - {finalDate} de Dispositivos com o Código de identificação [{deviceIdentificationCode}].");

                var reports = _deviceReportReaderRepository.GetDeviceConsumptionBetweenDates(deviceIdentificationCode, initialDate, finalDate);

                _logger.LogInformation($"Total de reports encontrados para o Dispositivo [{deviceIdentificationCode}]: {reports.Count}.");

                if (reports.Count > 0)
                {
                    var wattsTotal = Convert.ToDouble(reports.Sum(x => x.WattsUsage));

                    var consumption = new DeviceConsumption
                    {
                        IdentificationCode = deviceIdentificationCode,
                        ConsumptionInReal = CalculateWattsToReal(wattsTotal, initialDate, finalDate),
                        ConsumptionInWatts = wattsTotal,
                        InitialDate = initialDate,
                        FinalDate = finalDate
                    };

                    return consumption;
                }

                _logger.LogInformation($"Nenhum report encontrado para o Dispositivo [{deviceIdentificationCode}].");
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar os reports de dispositivos.");
                throw;
            }
        }

        private double CalculateWattsToReal(double watts, DateTime initialDate, DateTime finalDate)
        {
            var totalHours = (finalDate - initialDate).TotalHours;

            var kwhPrice = 0.80;

            var kwhTotalConsumption = (watts * totalHours / 1000) * kwhPrice;

            return kwhTotalConsumption;
        }
    }
}