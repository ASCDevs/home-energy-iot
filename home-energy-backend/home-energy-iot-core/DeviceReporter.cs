using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using home_energy_iot_core.Interfaces;
using home_energy_iot_entities;
using home_energy_iot_entities.Entities;
using Microsoft.Extensions.Logging;

namespace home_energy_iot_core
{
    public class DeviceReporter : IDeviceReporter
    {
        private ILogger _logger;

        private DataBaseContext _context;

        public DeviceReporter(ILogger logger)
        {
            _logger = logger;
        }

        public async Task Report(DeviceReport device)
        {
            try
            {
                if (device is null)
                    throw new ArgumentNullException(nameof(device), "Dispositivo nulo.");

                if (ReportExists(device))
                {
                    _logger.LogInformation($"Atualizando o Report do Dispositivo com Id [{device.Id}]");
                    _context.DevicesReports.Update(device);
                    return;
                }

                _logger.LogInformation($"Adicionando Report do Dispositivo com Id [{device.Id}]");
                await _context.AddAsync(device);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                _logger.LogError("Erro ao reportar o Dispositivo");
                throw;
            }
        }

        private bool ReportExists(DeviceReport device)
        {
            _logger.LogInformation($"Buscando o Dispositivo com Id [{device.Id}]");

            var result = _context.DevicesReports.ToList().Find(x => x.IdDevice == device.Id);

            return result != null;
        }
    }
}