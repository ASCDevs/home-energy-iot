using home_energy_api.Core.Models;
using home_energy_iot_api.Core.Models;
using home_energy_iot_core.Exceptions;
using home_energy_iot_core.Helpers.Interfaces;
using home_energy_iot_core.Interfaces;
using home_energy_iot_core.Login.Authentication;
using home_energy_iot_entities.Entities;
using Microsoft.Extensions.Logging;

namespace home_energy_iot_core.Login
{
    public class LoginService : ILoginService
    {
        private IHasher _hasher;
        private IUserManager _userManager;
        private ILogger<LoginService> _logger;
        private ITokenService _tokenService;

        public LoginService(IHasher hasher, IUserManager userManager, ILogger<LoginService> logger, ITokenService tokenService)
        {
            _hasher = hasher;
            _userManager = userManager;
            _logger = logger;
            _tokenService = tokenService;
        }

        public UserLoginModel Login(LoginModel loginModel)
        {
            try
            {
                ValidateLoginModel(loginModel);

                _logger.LogInformation($"Buscando o usuário [{loginModel.Username}].");

                var userReturned = _userManager.GetByUsername(loginModel.Username);

                if (ValidPassword(loginModel.Password, userReturned))
                {
                    _logger.LogInformation($"Usuário [{loginModel.Username}] encontrado e gerando token de autenticação.");

                    var token = _tokenService.GenerateToken(userReturned);

                    return new UserLoginModel
                    {
                        User = new UserModel
                        {
                            Id = userReturned.Id,
                            Name = userReturned.Name
                        },
                        UserToken = token
                    };
                }

                var message = "Erro ao realizar o login.";

                _logger.LogError(message);
                throw new LoginFailedException(message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
        }

        private bool ValidPassword(string providedPassword, User user)
        {
            if (_hasher.GenerateHash(providedPassword, user.SaltPassword) == user.Password)
                return true;

            return false;
        }


        private void ValidateLoginModel(LoginModel loginModel)
        {
            if (loginModel is null)
                throw new ArgumentNullException(nameof(loginModel), "Objeto de login nulo.");

            if (string.IsNullOrWhiteSpace(loginModel.Username))
                throw new Exception("Username nulo ou vazio.");

            if (string.IsNullOrWhiteSpace(loginModel.Password))
                throw new Exception("Senha nula ou vazia.");
        }
    }
}