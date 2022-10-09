using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using home_energy_iot_core.Helpers.Interfaces;
using home_energy_iot_core.Interfaces;
using home_energy_iot_core.Login;
using home_energy_iot_entities.Entities;
using home_energy_iot_repository.Interfaces;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Moq;
using Xunit;

namespace home_energy_iot_tests
{
    public class LoginServiceTests
    {
        private readonly Mock<IHasher> _hasher;

        public LoginServiceTests()
        {
            _hasher = new Mock<IHasher>();
        }

        [Fact]
        public void InvalidPasswordTest()
        {
            var providedPassword = "123";

            var user = new User
            {
                Password = "SenhaComHash",
                SaltPassword = "SaltDaSenha"
            };

            var senhaReal = "SenhaDoUsuario";

            _hasher.Setup(x => x.GenerateHash(providedPassword, user.SaltPassword))
                .Returns(senhaReal);

            var instance = GetInstance();

            var result = instance.ValidPassword(providedPassword, user);

            Assert.False(result);

            _hasher.Verify();
        }

        [Fact]
        public void ValidPasswordTest()
        {
            var providedPassword = "123";

            var user = new User
            {
                Password = "SenhaComHash",
                SaltPassword = "SaltDaSenha"
            };

            var senhaReal = "SenhaComHash";

            _hasher.Setup(x => x.GenerateHash(providedPassword, user.SaltPassword))
                .Returns(senhaReal);

            var instance = GetInstance();

            var result = instance.ValidPassword(providedPassword, user);

            Assert.True(result);

            _hasher.Verify();
        }

        public LoginService GetInstance()
        {
            return new LoginService(_hasher.Object);
        }
    }
}
