using home_energy_iot_api.Models;
using home_energy_iot_core.Interfaces;
using home_energy_iot_entities.Entities;
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
        public async Task<IActionResult> Create([FromBody] User user)
        {
            try
            {               
                await _userManager.Create(user);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest("Erro ao criar o usuário: " + ex.Message);
            }
        }

        [HttpPut]
        [Route("Update")]
        public async Task<IActionResult> Update([FromBody] User user)
        {
            try
            {
                await _userManager.Update(user);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest("Erro ao atualizar o usuário: " + ex.Message);
            }
        }

        [HttpGet]
        [Route("Get/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var user = await _userManager.Get(id);

                var result = FilterUserResult(user);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest("Erro ao buscar o usuário: " + ex.Message);
            }
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var users = await _userManager.GetAll();

                var usersFiltered = new List<UserView>();

                foreach (var user in users)
                    usersFiltered.Add(FilterUserResult(user));

                return Ok(usersFiltered);
            }
            catch (Exception ex)
            {
                return BadRequest("Erro ao buscar os usuários: " + ex.Message);
            }
        }

        private UserView FilterUserResult(User user)
        {
            if (user is null)
                throw new ArgumentNullException(nameof(user), "Não foi possível filtrar o usuário. Usuário Nulo.");

            _logger.LogInformation($"Filtrando as informações do Usuário Id [{user.Id}].");

            return new UserView
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                DtRegistration = user.RegisterDate
            };
        }
    }
}