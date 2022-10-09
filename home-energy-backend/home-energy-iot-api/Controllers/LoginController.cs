using home_energy_api.Authentication;
using home_energy_api.Models;
using home_energy_iot_api.Models;
using home_energy_iot_core.Interfaces;
using home_energy_iot_core.Login;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace home_energy_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private ILoginService _loginService;
        private ITokenService _tokenService;
        private IUserManager _userManager;
        private ILogger<LoginController> _logger;

        public LoginController(ILoginService loginService, ITokenService tokenService, IUserManager userManager, ILogger<LoginController> logger)
        {
            _loginService = loginService;
            _tokenService = tokenService;
            _userManager = userManager;
            _logger = logger;
        }

        [HttpPost]
        [Route("Authenticate")]
        [AllowAnonymous]
        public ActionResult<dynamic> Login([FromBody] LoginModel login)
        {
            try
            {
                _logger.LogInformation($"Buscando o usuário [{login.Username}].");

                var userReturned = _userManager.GetByUsername(login.Username);

                if (userReturned != null && _loginService.ValidPassword(login.Password, userReturned))
                {
                    _logger.LogInformation($"Usuário [{login.Username}] encontrado e gerando token de autenticação.");

                    var token = _tokenService.GenerateToken(userReturned);

                    return new
                    {
                        user = new UserModel
                        {
                            Id = userReturned.Id,
                            Name = userReturned.Name
                        },
                        userToken = token
                    };
                }

                var message = $"Usuário ou senha inválidos.";

                _logger.LogError(message);
                throw new Exception(message);
            }
            catch (Exception ex)
            {
                return BadRequest("Erro ao autenticar o usuário: " + ex.Message);
            }
        }
    }
}