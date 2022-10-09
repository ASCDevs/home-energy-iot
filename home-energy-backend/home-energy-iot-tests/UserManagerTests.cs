using home_energy_iot_core;
using home_energy_iot_core.Helpers.Interfaces;
using home_energy_iot_repository.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace home_energy_iot_tests
{
    public class UserManagerTests
    {
        private readonly Mock<IUserManagerRepository> _userReporitoryMock;
        private readonly Mock<ILogger<UserManager>> _logger;
        private readonly Mock<IHasher> _hasher;

        public UserManagerTests()
        {
            _userReporitoryMock = new Mock<IUserManagerRepository>();
            _logger = new Mock<ILogger<UserManager>>();
            _hasher = new Mock<IHasher>();
        }

        [Fact]
        

        private UserManager GetInstance()
        {
            return new UserManager(_logger.Object, _hasher.Object, _userReporitoryMock.Object);
        }
    }
}