using home_energy_api.Core.Models;
using home_energy_iot_core.Exceptions;
using home_energy_iot_core.Helpers.Interfaces;
using home_energy_iot_core.Interfaces;
using home_energy_iot_core.Login;
using home_energy_iot_core.Login.Authentication;
using home_energy_iot_entities.Entities;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace home_energy_iot_tests
{
    public class LoginServiceTests
    {
        private readonly Mock<IHasher> _hasher;
        private readonly Mock<IUserManager> _userManager;
        private readonly Mock<ILogger<LoginService>> _logger;
        private readonly Mock<ITokenService> _tokenService;

        private LoginModel _loginModelMock;

        public LoginServiceTests()
        {
            _hasher = new Mock<IHasher>();
            _userManager = new Mock<IUserManager>();
            _logger = new Mock<ILogger<LoginService>>();
            _tokenService = new Mock<ITokenService>();

            _loginModelMock = new LoginModel
            {
                Username = "admin",
                Password = "admin"
            };
        }

        [Fact]
        public void LoginModelNullTest()
        {
            LoginModel loginModel = null;

            var instance = GetInstance();

            Assert.Throws<ArgumentNullException>(() => instance.Login(loginModel));
        }

        [Fact]
        public void LoginModelUsernameNullTest()
        {
            var loginModel = _loginModelMock;

            loginModel.Username = null;

            var instance = GetInstance();

            Assert.Throws<Exception>(() => instance.Login(loginModel));
        }

        [Fact]
        public void LoginModelUsernameEmptyTest()
        {
            var loginModel = _loginModelMock;

            loginModel.Username = "";

            var instance = GetInstance();

            Assert.Throws<Exception>(() => instance.Login(loginModel));
        }

        [Fact]
        public void LoginModelUsernameWhiteSpaceTest()
        {
            var loginModel = _loginModelMock;

            loginModel.Username = " ";

            var instance = GetInstance();

            Assert.Throws<Exception>(() => instance.Login(loginModel));
        }

        [Fact]
        public void LoginModelPasswordNullTest()
        {
            var loginModel = _loginModelMock;

            loginModel.Password = null;

            var instance = GetInstance();

            Assert.Throws<Exception>(() => instance.Login(loginModel));
        }

        [Fact]
        public void LoginModelPasswordEmptyTest()
        {
            var loginModel = _loginModelMock;

            loginModel.Password = "";

            var instance = GetInstance();

            Assert.Throws<Exception>(() => instance.Login(loginModel));
        }

        [Fact]
        public void LoginModelPasswordWhiteSpaceTest()
        {
            var loginModel = _loginModelMock;

            loginModel.Password = " ";

            var instance = GetInstance();

            Assert.Throws<Exception>(() => instance.Login(loginModel));
        }

        [Fact]
        public void InvalidLoginPasswordTest()
        {
            var loginModel = new LoginModel
            {
                Username = "admin",
                Password = "SenhaErrada"
            };

            var user = new User
            {
                Password = "HashSenhaCerta",
                SaltPassword = "SaltDaSenha"
            };

            var hashGerado = "HashSenhaErrada";

            _userManager.Setup(x => x.GetByUsername(loginModel.Username)).Returns(user);

            _hasher.Setup(x => x.GenerateHash(loginModel.Password, user.SaltPassword))
                .Returns(hashGerado);

            var instance = GetInstance();

            Assert.Throws<LoginFailedException>(() => instance.Login(loginModel));

            _userManager.Verify();
            _hasher.Verify();
        }

        [Fact]
        public void ValidLoginPasswordTest()
        {
            var loginModel = new LoginModel
            {
                Username = "admin",
                Password = "SenhaCerta"
            };

            var user = new User
            {
                Id = 1,
                Name = "administrator",
                Password = "HashSenhaCerta",
                SaltPassword = "SaltDaSenha"
            };

            var hashGerado = "HashSenhaCerta";

            _userManager.Setup(x => x.GetByUsername(loginModel.Username)).Returns(user);

            _hasher.Setup(x => x.GenerateHash(loginModel.Password, user.SaltPassword))
                .Returns(hashGerado);

            _tokenService.Setup(x => x.GenerateToken(user)).Returns("token");

            var instance = GetInstance();

            var result = instance.Login(loginModel);

            Assert.NotNull(result);

            _userManager.Verify();
            _hasher.Verify();
            _tokenService.Verify();
        }

        public LoginService GetInstance()
        {
            return new LoginService(_hasher.Object, _userManager.Object, _logger.Object, _tokenService.Object);
        }
    }
}
