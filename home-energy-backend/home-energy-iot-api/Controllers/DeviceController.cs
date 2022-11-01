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
        public IActionResult Create([FromBody] Device device)
        {
            try
            {
                _deviceManager.Create(device);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Problem("Erro ao criar o dispositivo: " + ex.Message);
            }
        }

        [HttpPut]
        [Route("Update")]
        [Authorize]
        public IActionResult Update([FromBody] Device device)
        {
            try
            {
                _deviceManager.Update(device);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Problem("Erro ao atualizar o dispositivo: " + ex.Message);
            }
        }

        [HttpDelete]
        [Route("Delete/{id}")]
        [Authorize]
        public IActionResult Delete(int id)
        {
            try
            {
                _deviceManager.Delete(id);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Problem("Erro ao deletar o dispositivo: " + ex.Message);
            }
        }

        [HttpGet]
        [Route("Get/{id}")]
        [Authorize]
        public IActionResult Get(int id)
        {
            try
            {
                var device = _deviceManager.Get(id);

                return Ok(device);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Problem("Erro ao buscar o dispositivo: " + ex.Message);
            }
        }

        [HttpGet]
        [Route("GetAll")]
        [Authorize]
        public IActionResult GetAll()
        {
            try
            {
                var devices = _deviceManager.GetAll();

                return Ok(devices);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Problem("Erro ao buscar os dispositivos: " + ex.Message);
            }
        }

        [HttpGet]
        [Route("GetByHouseId/{id}")]
        [Authorize]
        public IActionResult GetByHouseId(int id)
        {
            try
            {
                var devices = _deviceManager.GetByHouseId(id);

                return Ok(devices);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Problem("Erro ao buscar os dispositivos: " + ex.Message);
            }
        }

        [HttpGet]
        [Route("Exists/{deviceIdentificationCode}")]
        [Authorize]
        public IActionResult Exists(string deviceIdentificationCode)
        {
            try
            {
                bool HasDevice = _deviceManager.Exists(deviceIdentificationCode);
                return Ok(new { result = HasDevice });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Problem("Erro ao checar dispositivo: " + ex.Message);
            }
        }
    }
}