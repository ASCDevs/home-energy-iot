using home_energy_iot_core.Interfaces;
using home_energy_iot_entities;
using home_energy_iot_entities.Entities;
using Microsoft.AspNetCore.Mvc;

namespace home_energy_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HouseController : ControllerBase
    {
        private ILogger<HouseController> _logger;
        private IHouseManager _houseManager;

        private DataBaseContext _context;

        public HouseController(ILogger<HouseController> logger, IHouseManager houseManager, DataBaseContext context)
        {
            _logger = logger;
            _houseManager = houseManager;
            _context = context;
        }

        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> AddHouse([FromBody] House house)
        {
            try
            {
                await _houseManager.CreateHouse(house);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("Update")]
        public async Task<IActionResult> UpdateHouse([FromBody] House house)
        {
            try
            {
                await _houseManager.UpdateHouse(house);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> DeleteHouse([FromBody] House house)
        {
            try
            {
                await _houseManager.DeleteHouse(house);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("Get/{id}")]
        public async Task<IActionResult> GetHouse(int id)
        {
            try
            {
                var house = await _houseManager.GetHouse(id);
                
                return Ok(house);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetHouse()
        {
            try
            {
                var houses = await _houseManager.GetHouses();

                return Ok(houses);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
