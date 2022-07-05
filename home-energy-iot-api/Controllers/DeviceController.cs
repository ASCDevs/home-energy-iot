using home_energy_iot_api.Models;
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
        private readonly ILogger<DeviceController> _logger;
        private DataBaseContext _context;

        public DeviceController(ILogger<DeviceController> logger, DataBaseContext dbContext)
        {
            _logger = logger;
            _context = dbContext;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> AddOrUpdateDevice([FromBody] RegisterDeviceForm deviceForm)
        {
            //Adicionar autenticação de rota, para usuários autorizados
            try
            {
                //Add User
                if(deviceForm.IdDevice == 0)
                {
                    _logger.LogInformation("Adding a new Device");

                    Device deviceNew = new Device 
                    { 
                        Id = deviceForm.IdDevice,
                        Name = deviceForm.NameDevice,
                        Description = deviceForm.DescDevice,
                        RegisterDate = DateTime.Now,
                        IdHouse = deviceForm.IdHouseUer
                    };

                    if (_context.Devices.Add(deviceNew) == null) throw new Exception("Não foi possível adicionar o novo dispositivo");
                    _context.SaveChanges();

                    _logger.LogInformation("New device has been successfully added.");
                    return Ok(new { message = "Dispositivo adicionado com sucesso" });
                }

                _logger.LogInformation("Updating a device.");

                Device deviceUpdate = new Device
                {
                        Id = deviceForm.IdDevice,
                        Name = deviceForm.NameDevice,
                        Description = deviceForm.DescDevice,
                        IdHouse = deviceForm.IdHouseUer
                };

                if(_context.Devices.Update(deviceUpdate) == null) throw new Exception("Erro ao atualizar o dispositivo");
                _context.SaveChanges();

                _logger.LogInformation("Device has been successfully updated.");
                return Ok(new { message = "Dispositivo foi atualizado com sucesso!"});
                
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message});
            }
        }

        // GET: api/<DeviceController>
        [HttpGet]
        [Route("AllDevices")]
        public async Task<IActionResult> GetDevices()
        {
            try
            {
                _logger.LogInformation("Querying devices");

                List<DeviceView> devices = _context.Devices.AsNoTracking().Select(x => new DeviceView 
                { 
                    IdDevice = x.Id,
                    NameDevice = x.Name,
                    DescDevice = x.Description,
                    DtRegistration = x.RegisterDate.ToString("dd/MM/yyyy"),
                    IdHouseUser = x.IdHouse
                }).ToList();
                if (devices.Count() == 0) throw new Exception("Nenhum dispositivo encontrado");

                return Ok(devices);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message});
            }
        }

        // GET api/<DeviceController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDeviceById(int id)
        {
            try
            {
                _logger.LogInformation("Querying device by id.");

                Device? deviceDb = _context.Devices.Find(id);
                if (deviceDb == null) throw new Exception("Dispositivo não encontrado.");

                DeviceView deviceView = new DeviceView
                {
                    IdDevice = deviceDb.Id,
                    NameDevice = deviceDb.Name,
                    DescDevice = deviceDb.Description,
                    DtRegistration = deviceDb.RegisterDate.ToString("dd/MM/yyyy"),
                    IdHouseUser = deviceDb.IdHouse
                };

                return Ok(deviceView);

            }
            catch (Exception ex)
            {
                _logger.LogInformation("Error on get devive by id. > " + ex.Message);
                return BadRequest(new { message = ex.Message});
            }
        }

        [HttpPost]
        [Route("Auth")]
        public async Task<IActionResult> AuthDevice()
        {
            return Ok("Autenticação de dispositivo em construção");
        }


    }
}
