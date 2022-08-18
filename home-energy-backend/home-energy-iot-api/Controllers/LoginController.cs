using home_energy_api.Authentication;
using home_energy_iot_api.Models;
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
        private ILogger<LoginController> _logger;

        public LoginController(ILoginService loginService, ITokenService tokenService, ILogger<LoginController> logger)
        {
            _loginService = loginService;
            _tokenService = tokenService;
            _logger = logger;
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public ActionResult<dynamic> Login(string username, string password)
        {
            try
            {
                _logger.LogInformation($"Buscando o usuário [{username}]");

                var userReturned = _loginService.GetUser(username);

                if (userReturned != null && _loginService.ValidPassword(password, userReturned))
                {
                    _logger.LogInformation($"Usuário [{username}] encontrado e gerando token de autenticação.");

                    var token = _tokenService.GenerateToken(userReturned);

                    return new
                    {
                        user = new UserView
                        {
                            Id = userReturned.Id,
                            Name = userReturned.Name,
                            Email = userReturned.Email,
                            DtRegistration = userReturned.RegisterDate
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
                return BadRequest(ex.Message);
            }
        }
    }
}