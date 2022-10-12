using home_energy_api.Core.Models;
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

        public LoginController(ILoginService loginService)
        {
            _loginService = loginService;
        }

        [HttpPost]
        [Route("Authenticate")]
        [AllowAnonymous]
        public ActionResult<dynamic> Login([FromBody] LoginModel login)
        {
            try
            {
                var loginInfo = _loginService.Login(login);

                return loginInfo;
            }
            catch (Exception ex)
            {
                return BadRequest("Erro ao autenticar o usuário: " + ex.Message);
            }
        }
    }
}