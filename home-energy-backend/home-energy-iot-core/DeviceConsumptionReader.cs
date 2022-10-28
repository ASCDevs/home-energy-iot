using home_energy_iot_core.Exceptions;
using home_energy_iot_core.Interfaces;
using home_energy_iot_core.Models;
using home_energy_iot_repository.Interfaces;
using Microsoft.Extensions.Logging;

namespace home_energy_iot_core
{
    public class DeviceConsumptionReader : IDeviceConsumptionReader
    {
        private readonly ILogger<DeviceConsumptionReader> _logger;
        private readonly IDeviceConsumptionReaderRepository _deviceReportReaderRepository;
        private readonly IHouseManagerRepository _houseManagerRepository;

        public DeviceConsumptionReader(ILogger<DeviceConsumptionReader> logger, IDeviceConsumptionReaderRepository deviceReportReaderRepository, IHouseManagerRepository houseManagerRepository)
        {
            _logger = logger;
            _deviceReportReaderRepository = deviceReportReaderRepository;
            _houseManagerRepository = houseManagerRepository;
        }

        //select identificationCode as device, min(reportDate) as minDate, max(reportDate) as maxDate, avg(wattsUsage) as mediaWatts, count(1) as quantidadeRegistros from deviceReport where identificationCode = 'ABC123' group by identificationCode
        public DeviceConsumption GetDeviceConsumptionTotalValue(string deviceIdentificationCode)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(deviceIdentificationCode))
                    throw new ArgumentNullException("Código de identificação do Dispositivo inválido.");

                _logger.LogInformation($"Iniciando busca de todos os reports de Dispositivos com o Código de identificação [{deviceIdentificationCode}].");

                var reports = _deviceReportReaderRepository.GetDeviceConsumption(deviceIdentificationCode);
                
                if (reports?.Count > 0)
                {
                    _logger.LogInformation($"Total de reports encontrados para o Dispositivo [{deviceIdentificationCode}]: {reports.Count}.");

                    var wattsTotal = Convert.ToDouble(reports.Average(x => x.WattsUsage));

                    var initialDate = reports[0].ReportDate;
                    var finalDate = reports[reports.Count - 1].ReportDate;

                    var kwhPrice =
                        _houseManagerRepository.GetHouseBaseKwhByDeviceIdentificationCode(deviceIdentificationCode);

                    var consumption = new DeviceConsumption
                    {
                        IdentificationCode = deviceIdentificationCode,
                        ConsumptionInReal = CalculateWattsToReal(kwhPrice, wattsTotal, initialDate, finalDate, reports.Count),
                        ConsumptionInWatts = wattsTotal,
                        ConsumptionDates = reports.Select(x => x.ReportDate).ToList(),
                        InitialDate = initialDate,
                        FinalDate = finalDate
                    };

                    return consumption;
                }

                var notFoundMessage = $"Nenhum report encontrado para o Dispositivo [{deviceIdentificationCode}].";

                _logger.LogInformation(notFoundMessage);
                throw new EntityNotFoundException(notFoundMessage);
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
                if (string.IsNullOrWhiteSpace(deviceIdentificationCode))
                    throw new ArgumentNullException("Código de identificação do Dispositivo inválido.");

                if (finalDate < initialDate)
                    throw new Exception("Período inválido. A data final deve ser maior que a inicial.");

                _logger.LogInformation($"Iniciando busca dos reports entre {initialDate} - {finalDate} de Dispositivos com o Código de identificação [{deviceIdentificationCode}].");

                var reports = _deviceReportReaderRepository.GetDeviceConsumptionValueBetweenDates(deviceIdentificationCode, initialDate, finalDate);
                
                if (reports.Count > 0)
                {
                    _logger.LogInformation($"Total de reports encontrados para o Dispositivo [{deviceIdentificationCode}]: {reports.Count}.");

                    var wattsTotal = Convert.ToDouble(reports.Average(x => x.WattsUsage));

                    var kwhPrice = _houseManagerRepository.GetHouseBaseKwhByDeviceIdentificationCode(deviceIdentificationCode);

                    var consumption = new DeviceConsumption
                    {
                        IdentificationCode = deviceIdentificationCode,
                        ConsumptionInReal = CalculateWattsToReal(kwhPrice, wattsTotal, initialDate, finalDate, reports.Count),
                        ConsumptionInWatts = wattsTotal,
                        ConsumptionDates = reports.Select(x => x.ReportDate).ToList(),
                        InitialDate = initialDate,
                        FinalDate = finalDate
                    };

                    return consumption;
                }

                var notFoundMessage = $"Nenhum report encontrado para o Dispositivo [{deviceIdentificationCode}].";

                _logger.LogInformation(notFoundMessage);
                throw new EntityNotFoundException(notFoundMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar os reports de dispositivos.");
                throw;
            }
        }
        
        private double CalculateWattsToReal(double kwhPrice, double watts, DateTime initialDate, DateTime finalDate, double totalSecondsUsage)
        {
            var totalHours = totalSecondsUsage / 3600;

            var result =  (watts * totalHours / 1000) * kwhPrice;

            _logger.LogInformation(
                "Cálculo de consumo efetuado. Valores utilizados na fórmula: \n" +
                $"Total de horas de consumo entre {initialDate} e {finalDate}: {totalHours} \n" +
                $"Média de Watts do período: {watts} \n" +
                $"Valor do kWh: {kwhPrice} \n" +
                $"Resultado do cálculo: R${String.Format("{0:0.00}", result)}");

            return result;
        }
    }
}