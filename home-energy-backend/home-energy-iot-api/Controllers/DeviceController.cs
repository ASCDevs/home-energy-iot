using home_energy_iot_core.Interfaces;
using home_energy_iot_entities.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace home_energy_iot_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceController : ControllerBase
    {
        private IDeviceManager _deviceManager;
        private ILogger<DeviceController> _logger;

        public DeviceController(IDeviceManager deviceManager, ILogger<DeviceController> logger)
        {
            _deviceManager = deviceManager;
            _logger = logger;
        }

        [HttpPost]
        [Route("Create")]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] Device device)
        {
            try
            {
                await _deviceManager.Create(device);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest("Erro ao criar o dispositivo: " + ex.Message);
            }
        }

        [HttpPut]
        [Route("Update")]
        [Authorize]
        public async Task<IActionResult> Update([FromBody] Device device)
        {
            try
            {
                await _deviceManager.Update(device);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest("Erro ao atualizar o dispositivo: " + ex.Message);
            }
        }

        [HttpDelete]
        [Route("Delete")]
        [Authorize]
        public async Task<IActionResult> Delete([FromBody] Device device)
        {
            try
            {
                await _deviceManager.Delete(device);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest("Erro ao deletar o dispositivo: " + ex.Message);
            }
        }

        [HttpGet]
        [Route("Get/{id}")]
        [Authorize]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var device = await _deviceManager.Get(id);

                return Ok(device);
            }
            catch (Exception ex)
            {
                return BadRequest("Erro ao buscar o dispositivo: " + ex.Message);
            }
        }

        [HttpGet]
        [Route("GetAll")]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var devices = await _deviceManager.GetAll();

                return Ok(devices);
            }
            catch (Exception ex)
            {
                return BadRequest("Erro ao buscar os dispositivos: " + ex.Message);
            }
        }

        [HttpGet]
        [Route("GetByHouseId/{id}")]
        [Authorize]
        public async Task<IActionResult> GetByHouseId(int id)
        {
            try
            {
                var devices = await _deviceManager.GetByHouseId(id);

                return Ok(devices);
            }
            catch (Exception ex)
            {
                return BadRequest("Erro ao buscar os dispositivos: " + ex.Message);
            }
        }

        [HttpGet]
        [Route("Exists/{deviceid}")]
        [Authorize]
        public async Task<IActionResult> Exists(string deviceid)
        {
            try
            {
                bool HasDevice = await _deviceManager.Exists(deviceid);
                return Ok(new { result = HasDevice });
            }
            catch (Exception ex)
            {
                return BadRequest("Erro ao checar dispositivo: " + ex.Message);
            }
        }
    }
}