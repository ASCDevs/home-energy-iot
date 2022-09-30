﻿using home_energy_iot_core.Interfaces;
using home_energy_iot_core.Models;
using home_energy_iot_repository.Interfaces;
using Microsoft.Extensions.Logging;

namespace home_energy_iot_core
{
    public class DeviceConsumptionReader : IDeviceConsumptionReader
    {
        private readonly ILogger<DeviceConsumptionReader> _logger;
        private readonly IDeviceConsumptionReaderRepository _deviceReportReaderRepository;

        public DeviceConsumptionReader(ILogger<DeviceConsumptionReader> logger, IDeviceConsumptionReaderRepository deviceReportReaderRepository)
        {
            _logger = logger;
            _deviceReportReaderRepository = deviceReportReaderRepository;
        }

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

                    var wattsTotal = Convert.ToDouble(reports.Sum(x => x.WattsUsage));

                    var initialDate = reports[0].ReportDate;
                    var finalDate = reports[reports.Count - 1].ReportDate;

                    var consumption = new DeviceConsumption
                    {
                        IdentificationCode = deviceIdentificationCode,
                        ConsumptionInReal = CalculateWattsToReal(wattsTotal, initialDate, finalDate, reports.Count),
                        ConsumptionInWatts = wattsTotal,
                        ConsumptionDates = reports.Select(x => x.ReportDate).ToList(),
                        InitialDate = initialDate,
                        FinalDate = finalDate
                    };

                    return consumption;
                }

                _logger.LogInformation($"Nenhum report encontrado para o Dispositivo [{deviceIdentificationCode}].");

                return new DeviceConsumption();
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

                _logger.LogInformation($"Iniciando busca dos reports entre {initialDate} - {finalDate} de Dispositivos com o Código de identificação [{deviceIdentificationCode}].");

                var reports = _deviceReportReaderRepository.GetDeviceConsumptionBetweenDates(deviceIdentificationCode, initialDate, finalDate);

                _logger.LogInformation($"Total de reports encontrados para o Dispositivo [{deviceIdentificationCode}]: {reports.Count}.");

                if (reports.Count > 0)
                {
                    var wattsTotal = Convert.ToDouble(reports.Sum(x => x.WattsUsage));

                    var consumption = new DeviceConsumption
                    {
                        IdentificationCode = deviceIdentificationCode,
                        ConsumptionInReal = CalculateWattsToReal(wattsTotal, initialDate, finalDate, reports.Count),
                        ConsumptionInWatts = wattsTotal,
                        ConsumptionDates = reports.Select(x => x.ReportDate).ToList(),
                        InitialDate = initialDate,
                        FinalDate = finalDate
                    };

                    return consumption;
                }

                _logger.LogInformation($"Nenhum report encontrado para o Dispositivo [{deviceIdentificationCode}].");

                return new DeviceConsumption();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar os reports de dispositivos.");
                throw;
            }
        }
        
        private double CalculateWattsToReal(double watts, DateTime initialDate, DateTime finalDate, double totalSecondsUsage)
        {
            var totalHours = totalSecondsUsage / 3600;

            var kwhPrice = 0.80;

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