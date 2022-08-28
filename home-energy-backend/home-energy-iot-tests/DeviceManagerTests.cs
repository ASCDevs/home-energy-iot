using Xunit;
using Moq;
using Moq.EntityFrameworkCore;
using home_energy_iot_core;
using home_energy_iot_core.Exceptions;
using Microsoft.Extensions.Logging;
using home_energy_iot_entities;
using home_energy_iot_entities.Entities;
using home_energy_iot_repository.Interfaces;

namespace home_energy_iot_tests
{
    public class DeviceManagerTests
    {
        private readonly Mock<ILogger<DeviceManager>> _logger;
        private readonly Mock<IDeviceManagerRepository> _deviceManagerRepository;
        private readonly Device _deviceMock;

        public DeviceManagerTests()
        {
            _logger = new Mock<ILogger<DeviceManager>>(MockBehavior.Loose);
            _deviceManagerRepository = new Mock<IDeviceManagerRepository>();

            _deviceMock = new Device
            {
                Name = "Computer",
                IdentificationCode = "PC:DS:48:FD",
                IdHouse = 1,
                Description = "New Computer",
                RegisterDate = DateTime.Now,
                Watts = 500
            };
        }

        #region Create
        
        #region ValidateDeviceHouseId

        [Fact]
        public async void CreateDeviceIdHouseZeroTest()
        {
            var device = new Device
            {
                IdHouse = 0,
                Name = _deviceMock.Name,
                IdentificationCode = _deviceMock.IdentificationCode,
                Description = _deviceMock.Description,
                RegisterDate = _deviceMock.RegisterDate,
                Watts = _deviceMock.Watts
            };

            var instance = GetInstance();

            await Assert.ThrowsAsync<InvalidEntityNumericValueException>(() => instance.Create(device));
        }

        [Fact]
        public async void CreateDeviceIdHouseNegativeTest()
        {
            var device = new Device
            {
                IdHouse = -1,
                Name = _deviceMock.Name,
                IdentificationCode = _deviceMock.IdentificationCode,
                Description = _deviceMock.Description,
                RegisterDate = _deviceMock.RegisterDate,
                Watts = _deviceMock.Watts
            };

            var instance = GetInstance();

            await Assert.ThrowsAsync<InvalidEntityNumericValueException>(() => instance.Create(device));
        }

        #endregion

        #region ValidateDeviceName

        [Fact]
        public async void CreateDeviceNullNameTest()
        {
            var device = new Device
            {
                IdHouse = _deviceMock.IdHouse,
                Name = null,
                IdentificationCode = _deviceMock.IdentificationCode,
                Description = _deviceMock.Description,
                RegisterDate = _deviceMock.RegisterDate,
                Watts = _deviceMock.Watts
            };

            var instance = GetInstance();

            await Assert.ThrowsAsync<InvalidEntityTextValueException>(() => instance.Create(device));
        }

        [Fact]
        public async void CreateDeviceEmptyNameTest()
        {
            var device = new Device
            {
                IdHouse = _deviceMock.IdHouse,
                Name = "",
                IdentificationCode = _deviceMock.IdentificationCode,
                Description = _deviceMock.Description,
                RegisterDate = _deviceMock.RegisterDate,
                Watts = _deviceMock.Watts
            };

            var instance = GetInstance();

            await Assert.ThrowsAsync<InvalidEntityTextValueException>(() => instance.Create(device));
        }

        [Fact]
        public async void CreateDeviceEmptySpaceNameTest()
        {
            var device = new Device
            {
                IdHouse = _deviceMock.IdHouse,
                Name = " ",
                IdentificationCode = _deviceMock.IdentificationCode,
                Description = _deviceMock.Description,
                RegisterDate = _deviceMock.RegisterDate,
                Watts = _deviceMock.Watts
            };

            var instance = GetInstance();

            await Assert.ThrowsAsync<InvalidEntityTextValueException>(() => instance.Create(device));
        }

        #endregion

        #region ValidateDeviceIdentificationCode

        [Fact]
        public async void CreateDeviceNullIdentificationCodeTest()
        {
            var device = new Device
            {
                IdHouse = _deviceMock.IdHouse,
                Name = _deviceMock.Name,
                IdentificationCode = null,
                Description = _deviceMock.Description,
                RegisterDate = _deviceMock.RegisterDate,
                Watts = _deviceMock.Watts
            };

            var instance = GetInstance();

            await Assert.ThrowsAsync<InvalidEntityTextValueException>(() => instance.Create(device));
        }

        [Fact]
        public async void CreateDeviceEmptyIdentificationCodeTest()
        {
            var device = new Device
            {
                IdHouse = _deviceMock.IdHouse,
                Name = _deviceMock.Name,
                IdentificationCode = "",
                Description = _deviceMock.Description,
                RegisterDate = _deviceMock.RegisterDate,
                Watts = _deviceMock.Watts
            };

            var instance = GetInstance();

            await Assert.ThrowsAsync<InvalidEntityTextValueException>(() => instance.Create(device));
        }

        [Fact]
        public async void CreateDeviceWhiteSpaceIdentificationCodeTest()
        {
            var device = new Device
            {
                IdHouse = _deviceMock.IdHouse,
                Name = _deviceMock.Name,
                IdentificationCode = " ",
                Description = _deviceMock.Description,
                RegisterDate = _deviceMock.RegisterDate,
                Watts = _deviceMock.Watts
            };

            var instance = GetInstance();

            await Assert.ThrowsAsync<InvalidEntityTextValueException>(() => instance.Create(device));
        }

        #endregion

        #region ValidateDeviceDescription

        [Fact]
        public async void CreateDeviceNullDescriptionTest()
        {
            var device = new Device
            {
                IdHouse = _deviceMock.IdHouse,
                Name = _deviceMock.Name,
                IdentificationCode = _deviceMock.IdentificationCode,
                Description = null,
                RegisterDate = _deviceMock.RegisterDate,
                Watts = _deviceMock.Watts
            };

            var instance = GetInstance();

            await Assert.ThrowsAsync<InvalidEntityTextValueException>(() => instance.Create(device));
        }

        [Fact]
        public async void CreateDeviceEmptyDescriptionTest()
        {
            var device = new Device
            {
                IdHouse = _deviceMock.IdHouse,
                Name = _deviceMock.Name,
                IdentificationCode = _deviceMock.IdentificationCode,
                Description = "",
                RegisterDate = _deviceMock.RegisterDate,
                Watts = _deviceMock.Watts
            };

            var instance = GetInstance();

            await Assert.ThrowsAsync<InvalidEntityTextValueException>(() => instance.Create(device));
        }

        [Fact]
        public async void CreateDeviceWhiteSpaceDescriptionTest()
        {
            var device = new Device
            {
                IdHouse = _deviceMock.IdHouse,
                Name = _deviceMock.Name,
                IdentificationCode = _deviceMock.IdentificationCode,
                Description = " ",
                RegisterDate = _deviceMock.RegisterDate,
                Watts = _deviceMock.Watts
            };

            var instance = GetInstance();

            await Assert.ThrowsAsync<InvalidEntityTextValueException>(() => instance.Create(device));
        }

        #endregion

        #region ValidateDeviceWatts

        [Fact]
        public async void CreateDeviceZeroWattsTest()
        {
            var device = new Device
            {
                IdHouse = _deviceMock.IdHouse,
                Name = _deviceMock.Name,
                IdentificationCode = _deviceMock.IdentificationCode,
                Description = _deviceMock.Description,
                RegisterDate = _deviceMock.RegisterDate,
                Watts = 0
            };

            var instance = GetInstance();

            await Assert.ThrowsAsync<InvalidEntityNumericValueException>(() => instance.Create(device));
        }

        [Fact]
        public async void CreateDeviceNegativeWattsTest()
        {
            var device = new Device
            {
                IdHouse = _deviceMock.IdHouse,
                Name = _deviceMock.Name,
                IdentificationCode = _deviceMock.IdentificationCode,
                Description = _deviceMock.Description,
                RegisterDate = _deviceMock.RegisterDate,
                Watts = -1
            };

            var instance = GetInstance();

            await Assert.ThrowsAsync<InvalidEntityNumericValueException>(() => instance.Create(device));
        }

        #endregion

        [Fact]
        public async void CreateDeviceNullDeviceTest()
        {
            Device device = null;

            var instance = GetInstance();

            await Assert.ThrowsAsync<ArgumentNullException>(() => instance.Create(device));
        }

        [Fact]
        public async void CreateDeviceSuccessTest()
        {
            var device = _deviceMock;

            _deviceManagerRepository.Setup(x => x.Create(device)).Verifiable();

            var instance = GetInstance();

            await instance.Create(device);

            _deviceManagerRepository.Verify();
        }

        #endregion

        public DeviceManager GetInstance()
        {
            return new DeviceManager(_logger.Object, _deviceManagerRepository.Object);
        }
    }
}