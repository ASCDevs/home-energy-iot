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

        public DeviceConsumptionController(IDeviceConsumptionReader deviceReportReader)
        {
            _deviceReportReader = deviceReportReader;
        }

        [HttpGet]
        [Route("GetDeviceConsumptionTotalValue/{deviceIdentificationCode}")]
        [Authorize]
        public async Task<IActionResult> GetDeviceConsumptionTotalValue(string deviceIdentificationCode)
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
        [Authorize]
        public async Task<IActionResult> GetDeviceConsumptionValueBetweenDates([FromBody] ReportFilterModel reportFilter)
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