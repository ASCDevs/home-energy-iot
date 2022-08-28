using home_energy_iot_core.Interfaces;
using home_energy_iot_entities.Entities;
using Microsoft.AspNetCore.Mvc;

namespace home_energy_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HouseController : ControllerBase
    {
        private IHouseManager _houseManager;
        private ILogger<HouseController> _logger;

        public HouseController(IHouseManager houseManager, ILogger<HouseController> logger)
        {
            _houseManager = houseManager;
            _logger = logger;
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create([FromBody] House house)
        {
            try
            {
                await _houseManager.Create(house);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest("Erro ao criar a casa: " + ex.Message);
            }
        }

        [HttpPut]
        [Route("Update")]
        public async Task<IActionResult> Update([FromBody] House house)
        {
            try
            {
                await _houseManager.Update(house);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest("Erro ao atualizar a casa: " + ex.Message);
            }
        }

        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> Delete([FromBody] House house)
        {
            try
            {
                await _houseManager.Delete(house);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest("Erro ao deletar a casa: " + ex.Message);
            }
        }

        [HttpGet]
        [Route("Get/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var house = await _houseManager.Get(id);

                return Ok(house);
            }
            catch (Exception ex)
            {
                return BadRequest("Erro ao buscar a casa: " + ex.Message);
            }
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var houses = await _houseManager.GetAll();

                return Ok(houses);
            }
            catch (Exception ex)
            {
                return BadRequest("Erro ao buscar as casas: " + ex.Message);
            }
        }
    }
}