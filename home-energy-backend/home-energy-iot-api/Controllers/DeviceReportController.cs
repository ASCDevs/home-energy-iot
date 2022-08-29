using home_energy_api.Models;
using home_energy_iot_core.Interfaces;
using home_energy_iot_entities.Entities;
using Microsoft.AspNetCore.Mvc;

namespace home_energy_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceReportController : ControllerBase
    {
        private IDeviceReporter _deviceReporter;
        private IDeviceConsumptionReader _deviceReportReader;

        public DeviceReportController(IDeviceReporter deviceReporter, IDeviceConsumptionReader deviceReportReader)
        {
            _deviceReporter = deviceReporter;
            _deviceReportReader = deviceReportReader;
        }

        [HttpPost]
        [Route("Report")]
        public async Task<IActionResult> ReportDevice([FromBody] DeviceReport device)
        {
            try
            {
                await _deviceReporter.Report(device);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest("Erro reportar o dispositivo: " + ex.Message);
            }
        }

        [HttpGet]
        [Route("GetConsumption/{deviceIdentificationCode}")]
        public async Task<IActionResult> GetConsumption(string deviceIdentificationCode)
        {
            try
            {
                var result = _deviceReportReader.GetDeviceConsumptionTotalValue(deviceIdentificationCode);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest("Erro buscar os reports do dispositivo: " + ex.Message);
            }
        }

        [HttpPost]
        [Route("GetConsumptionBetweenDates")]
        public async Task<IActionResult> GetDeviceConsumptionValueBetweenDates([FromBody] ReportFilter reportFilter)
        {
            try
            {
                var result = _deviceReportReader.GetDeviceConsumptionValueBetweenDates(reportFilter.DeviceIdentificationCode, reportFilter.initialDate, reportFilter.finalDate);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest("Erro buscar os reports do dispositivo: " + ex.Message);
            }
        }
    }
}