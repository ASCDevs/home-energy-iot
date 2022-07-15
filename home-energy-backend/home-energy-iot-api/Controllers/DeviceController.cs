using home_energy_iot_api.Models;
using home_energy_iot_core.Interfaces;
using home_energy_iot_entities;
using home_energy_iot_entities.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace home_energy_iot_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceController : ControllerBase
    {
        private IDeviceManager _deviceManager;
        private ILogger<DeviceController> _logger;

        public DeviceController(ILogger<DeviceController> logger, IDeviceManager deviceManager)
        {
            _logger = logger;
            _deviceManager = deviceManager;
        }

        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> AddDevice([FromBody] Device device)
        {
            try
            {
                await _deviceManager.CreateDevice(device);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("Update")]
        public async Task<IActionResult> UpdateDevice([FromBody] Device device)
        {
            try
            {
                await _deviceManager.UpdateDevice(device);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> DeleteDevice([FromBody] Device device)
        {
            try
            {
                await _deviceManager.DeleteDevice(device);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("Get/{id}")]
        public async Task<IActionResult> GetDevice(int id)
        {
            try
            {
                var device = await _deviceManager.GetDevice(id);

                return Ok(device);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetDevice()
        {
            try
            {
                var devices = await _deviceManager.GetDevices();

                return Ok(devices);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}