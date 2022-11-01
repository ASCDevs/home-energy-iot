using home_energy_iot_core.Interfaces;
using home_energy_iot_entities.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace home_energy_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HouseController : ControllerBase
    {
        private IHouseManager _houseManager;

        public HouseController(IHouseManager houseManager)
        {
            _houseManager = houseManager;
        }

        [HttpPost]
        [Route("Create")]
        [Authorize]
        public IActionResult Create([FromBody] House house)
        {
            try
            {
                _houseManager.Create(house);

                return Ok();
            }
            catch (Exception ex)
            {
                return Problem("Erro ao criar a casa: " + ex.Message);
            }
        }

        [HttpPut]
        [Route("Update")]
        [Authorize]
        public IActionResult Update([FromBody] House house)
        {
            try
            {
                _houseManager.Update(house);

                return Ok();
            }
            catch (Exception ex)
            {
                return Problem("Erro ao atualizar a casa: " + ex.Message);
            }
        }

        [HttpDelete]
        [Route("Delete/{id}")]
        [Authorize]
        public IActionResult Delete(int id)
        {
            try
            {
                _houseManager.Delete(id);

                return Ok();
            }
            catch (Exception ex)
            {
                return Problem("Erro ao deletar a casa: " + ex.Message);
            }
        }

        [HttpGet]
        [Route("Get/{id}")]
        [Authorize]
        public IActionResult Get(int id)
        {
            try
            {
                var house = _houseManager.Get(id);

                return Ok(house);
            }
            catch (Exception ex)
            {
                return Problem("Erro ao buscar a casa: " + ex.Message);
            }
        }

        [HttpGet]
        [Route("GetAll")]
        [Authorize]
        public IActionResult GetAll()
        {
            try
            {
                var houses = _houseManager.GetAll();

                return Ok(houses);
            }
            catch (Exception ex)
            {
                return Problem("Erro ao buscar as casas: " + ex.Message);
            }
        }

        [HttpGet]
        [Route("GetByUserId/{id}")]
        [Authorize]
        public IActionResult GetByUserId(int id)
        {
            try
            {
                var houses = _houseManager.GetByUserId(id);

                return Ok(houses);
            }
            catch (Exception ex)
            {
                return Problem("Erro ao buscar as casas: " + ex.Message);
            }
        }
    }
}