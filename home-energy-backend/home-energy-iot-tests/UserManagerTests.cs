using System.Runtime.CompilerServices;
using home_energy_iot_core;
using home_energy_iot_core.Exceptions;
using home_energy_iot_core.Helpers.Interfaces;
using home_energy_iot_entities.Entities;
using home_energy_iot_repository.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace home_energy_iot_tests
{
    public class UserManagerTests
    {
        private readonly Mock<IUserManagerRepository> _userManagerReporitoryMock;
        private readonly Mock<ILogger<UserManager>> _logger;
        private readonly Mock<IHasher> _hasher;

        private readonly User _userMock;

        public UserManagerTests()
        {
            _userManagerReporitoryMock = new Mock<IUserManagerRepository>();
            _logger = new Mock<ILogger<UserManager>>();
            _hasher = new Mock<IHasher>();

            _userMock = new User
            {
                Id = 1,
                Name = "John",
                Username = "john123",
                Password = "password123",
                SaltPassword = "saltPassword",
                Email = "john@john.com",
                CPF = "123.123.123-12",
                RegisterDate = DateTime.Now
            };
        }

        #region Create

        [Fact]
        public void CreateUserNullTest()
        {
            User user = null;

            var instance = GetInstance();

            Assert.Throws<ArgumentNullException>(() => instance.Create(user));
        }

        #region ValidateName

        [Fact]
        public void CreateUserNameNullTest()
        {
            User user = _userMock;

            user.Name = null;

            var instance = GetInstance();

            Assert.Throws<InvalidEntityTextValueException>(() => instance.Create(user));
        }

        [Fact]
        public void CreateUserNameEmptyTest()
        {
            User user = _userMock;

            user.Name = "";

            var instance = GetInstance();

            Assert.Throws<InvalidEntityTextValueException>(() => instance.Create(user));
        }

        [Fact]
        public void CreateUserNameWhiteSpaceTest()
        {
            User user = _userMock;

            user.Name = " ";

            var instance = GetInstance();

            Assert.Throws<InvalidEntityTextValueException>(() => instance.Create(user));
        }

        #endregion

        #region ValidateUsername

        [Fact]
        public void CreateUserUsernameNullTest()
        {
            User user = _userMock;

            user.Username = null;

            var instance = GetInstance();

            Assert.Throws<InvalidEntityTextValueException>(() => instance.Create(user));
        }

        [Fact]
        public void CreateUserUsernameEmptyTest()
        {
            User user = _userMock;

            user.Username = "";

            var instance = GetInstance();

            Assert.Throws<InvalidEntityTextValueException>(() => instance.Create(user));
        }

        [Fact]
        public void CreateUserUsernameWhiteSpaceTest()
        {
            User user = _userMock;

            user.Username = " ";

            var instance = GetInstance();

            Assert.Throws<InvalidEntityTextValueException>(() => instance.Create(user));
        }

        #endregion

        #region ValidatePassword

        [Fact]
        public void CreateUserPasswordNullTest()
        {
            User user = _userMock;

            user.Password = null;

            var instance = GetInstance();

            Assert.Throws<InvalidEntityTextValueException>(() => instance.Create(user));
        }

        [Fact]
        public void CreateUserPasswordEmptyTest()
        {
            User user = _userMock;

            user.Password = "";

            var instance = GetInstance();

            Assert.Throws<InvalidEntityTextValueException>(() => instance.Create(user));
        }

        [Fact]
        public void CreateUserPasswordWhiteSpaceTest()
        {
            User user = _userMock;

            user.Password = " ";

            var instance = GetInstance();

            Assert.Throws<InvalidEntityTextValueException>(() => instance.Create(user));
        }

        #endregion

        #region ValidateCPF

        [Fact]
        public void CreateUserCPFNullTest()
        {
            User user = _userMock;

            user.CPF = null;

            var instance = GetInstance();

            Assert.Throws<InvalidEntityTextValueException>(() => instance.Create(user));
        }

        [Fact]
        public void CreateUserCPFEmptyTest()
        {
            User user = _userMock;

            user.CPF = "";

            var instance = GetInstance();

            Assert.Throws<InvalidEntityTextValueException>(() => instance.Create(user));
        }

        [Fact]
        public void CreateUserCPFWhiteSpaceTest()
        {
            User user = _userMock;

            user.CPF = " ";

            var instance = GetInstance();

            Assert.Throws<InvalidEntityTextValueException>(() => instance.Create(user));
        }

        #endregion

        #region ValidateEmail

        [Fact]
        public void CreateUserEmailNullTest()
        {
            User user = _userMock;

            user.Email = null;

            var instance = GetInstance();

            Assert.Throws<InvalidEntityTextValueException>(() => instance.Create(user));
        }

        [Fact]
        public void CreateUserEmailEmptyTest()
        {
            User user = _userMock;

            user.Email = "";

            var instance = GetInstance();

            Assert.Throws<InvalidEntityTextValueException>(() => instance.Create(user));
        }

        [Fact]
        public void CreateUserEmailWhiteSpaceTest()
        {
            User user = _userMock;

            user.Email = " ";

            var instance = GetInstance();

            Assert.Throws<InvalidEntityTextValueException>(() => instance.Create(user));
        }

        #endregion

        [Fact]
        public void CreateUserSuccessTest()
        {
            var user = _userMock;

            var salt = "11111111111111111111";

            _hasher.Setup(x => x.CreateSalt(20))
                .Returns(salt).Verifiable();
            _hasher.Setup(x => x.GenerateHash(user.Password, salt))
                .Returns("HashGeradoAPartirDaSenhaEDoSalt").Verifiable();

            _userManagerReporitoryMock.Setup(x => x.Create(user)).Verifiable();

            var instance = GetInstance();

            instance.Create(user);

            _hasher.Verify();
            _userManagerReporitoryMock.Verify();
        }

        #endregion

        #region Get

        [Fact]
        public void GetUserIdNegativeTest()
        {
            var id = -1;

            var instance = GetInstance();

            Assert.Throws<InvalidEntityNumericValueException>(() => instance.Get(id));
        }

        [Fact]
        public void GetUserIdZeroTest()
        {
            var id = 0;

            var instance = GetInstance();

            Assert.Throws<InvalidEntityNumericValueException>(() => instance.Get(id));
        }

        [Fact]
        public void GetUserNotFoundTest()
        {
            var id = 2;

            User user = null;

            _userManagerReporitoryMock.Setup(x => x.Get(id))
                .Returns(user).Verifiable();

            var instance = GetInstance();

            Assert.Throws<EntityNotFoundException>(() => instance.Get(id));

            _userManagerReporitoryMock.Verify();
        }

        [Fact]
        public void GetUserSuccessTest()
        {
            var id = 1;
            var user = _userMock;

            _userManagerReporitoryMock.Setup(x => x.Get(id))
                .Returns(user).Verifiable();

            var instance = GetInstance();

            var result = instance.Get(id);

            Assert.Equal(id, result.Id);

            _userManagerReporitoryMock.Verify();
        }

        #endregion

        #region GetByUsername

        [Fact]
        public void GetUserUsernameNullTest()
        {
            string username = null;

            var instance = GetInstance();

            Assert.Throws<ArgumentNullException>(() => instance.GetByUsername(username));
        }

        [Fact]
        public void GetUserUsernameEmptyTest()
        {
            string username = "";

            var instance = GetInstance();

            Assert.Throws<ArgumentNullException>(() => instance.GetByUsername(username));
        }

        [Fact]
        public void GetUserUsernameWhiteSpaceTest()
        {
            string username = " ";

            var instance = GetInstance();

            Assert.Throws<ArgumentNullException>(() => instance.GetByUsername(username));
        }

        [Fact]
        public void GetUserUsernameNotFoundTest()
        {
            string username = "user";

            User user = null;

            _userManagerReporitoryMock.Setup(x => x.GetByUsername(username))
                .Returns(user).Verifiable();

            var instance = GetInstance();

            Assert.Throws<EntityNotFoundException>(() => instance.GetByUsername(username));

            _userManagerReporitoryMock.Verify();
        }

        [Fact]
        public void GetUserUsernameSuccessTest()
        {
            string username = "john123";

            var user = _userMock;

            _userManagerReporitoryMock.Setup(x => x.GetByUsername(username))
                .Returns(user).Verifiable();

            var instance = GetInstance();

            var result = instance.GetByUsername(username);

            Assert.Equal(username, result.Username);

            _userManagerReporitoryMock.Verify();
        }

        #endregion

        private UserManager GetInstance()
        {
            return new UserManager(_logger.Object, _hasher.Object, _userManagerReporitoryMock.Object);
        }
    }
}