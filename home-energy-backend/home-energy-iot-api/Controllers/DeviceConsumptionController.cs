using home_energy_api.Models;
using home_energy_iot_core.Interfaces;
using home_energy_iot_entities.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace home_energy_iot_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceConsumptionController : ControllerBase
    {
        private IDeviceConsumptionReader _deviceReportReader;
        private ILogger _logger;

        public DeviceConsumptionController(IDeviceConsumptionReader deviceReportReader, ILogger logger)
        {
            _deviceReportReader = deviceReportReader;
            _logger = logger;
        }

        [HttpGet]
        [Route("GetDeviceConsumptionTotalValue/{deviceIdentificationCode}")]
        [Authorize]
        public IActionResult GetDeviceConsumptionTotalValue(string deviceIdentificationCode)
        {
            try
            {
                var result = _deviceReportReader.GetDeviceConsumptionTotalValue(deviceIdentificationCode);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Problem("Erro buscar os reports do dispositivo: " + ex.Message);
            }
        }

        [HttpPost]
        [Route("GetConsumptionValueBetweenDates")]
        [Authorize]
        public IActionResult GetDeviceConsumptionValueBetweenDates([FromBody] ReportFilterModel reportFilter)
        {
            try
            {
                var result = _deviceReportReader.GetDeviceConsumptionValueBetweenDates(reportFilter.DeviceIdentificationCode, reportFilter.initialDate, reportFilter.finalDate);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Problem("Erro buscar os reports do dispositivo: " + ex.Message);
            }
        }
    }
}