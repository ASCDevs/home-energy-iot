using home_energy_iot_api.Models;
using home_energy_iot_core.Interfaces;
using home_energy_iot_entities.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace home_energy_iot_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IUserManager _userManager;

        public UserController(ILogger<UserController> logger, IUserManager userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }

        [HttpPost]
        [Route("Create")]
        public IActionResult Create([FromBody] User user)
        {
            try
            {               
                _userManager.Create(user);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest("Erro ao criar o usuário: " + ex.Message);
            }
        }

        [HttpGet]
        [Route("Get/{id}")]
        [Authorize]
        public IActionResult Get(int id)
        {
            try
            {
                var user = _userManager.Get(id);

                var result = FilterUserResult(user);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest("Erro ao buscar o usuário: " + ex.Message);
            }
        }

        private UserModel FilterUserResult(User user)
        {
            if (user is null)
                throw new ArgumentNullException(nameof(user), "Não foi possível filtrar o usuário. Usuário Nulo.");

            _logger.LogInformation($"Filtrando as informações do Usuário Id [{user.Id}].");

            return new UserModel
            {
                Id = user.Id,
                Name = user.Name,
            };
        }
    }
}