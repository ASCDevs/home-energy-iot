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

        public DeviceReportController(IDeviceReporter deviceReporter)
        {
            _deviceReporter = deviceReporter;
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
                return BadRequest(ex.Message);
            }
        }
    }
}
