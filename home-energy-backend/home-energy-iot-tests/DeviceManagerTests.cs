using home_energy_iot_core;
using home_energy_iot_core.Exceptions;
using home_energy_iot_entities.Entities;
using home_energy_iot_repository.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

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
                Id = 1,
                Name = "Computer",
                IdentificationCode = "PC:DS:48:FD",
                IdHouse = 1,
                Description = "New Computer",
                RegisterDate = DateTime.Now,
                Watts = 500
            };
        }

        #region Create

        [Fact]
        public void CreateDeviceNullDeviceTest()
        {
            Device device = null;

            var instance = GetInstance();

            Assert.Throws<ArgumentNullException>(() => instance.Create(device));
        }

        #region ValidateDeviceHouseId

        [Fact]
        public void CreateDeviceIdHouseZeroTest()
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

            Assert.Throws<InvalidEntityNumericValueException>(() => instance.Create(device));
        }

        [Fact]
        public void CreateDeviceIdHouseNegativeTest()
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

            Assert.Throws<InvalidEntityNumericValueException>(() => instance.Create(device));
        }

        #endregion

        #region ValidateDeviceName

        [Fact]
        public void CreateDeviceNullNameTest()
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

            Assert.Throws<InvalidEntityTextValueException>(() => instance.Create(device));
        }

        [Fact]
        public void CreateDeviceEmptyNameTest()
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

            Assert.Throws<InvalidEntityTextValueException>(() => instance.Create(device));
        }

        [Fact]
        public void CreateDeviceEmptySpaceNameTest()
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

            Assert.Throws<InvalidEntityTextValueException>(() => instance.Create(device));
        }

        #endregion

        #region ValidateDeviceIdentificationCode

        [Fact]
        public void CreateDeviceNullIdentificationCodeTest()
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

            Assert.Throws<InvalidEntityTextValueException>(() => instance.Create(device));
        }

        [Fact]
        public void CreateDeviceEmptyIdentificationCodeTest()
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

            Assert.Throws<InvalidEntityTextValueException>(() => instance.Create(device));
        }

        [Fact]
        public void CreateDeviceWhiteSpaceIdentificationCodeTest()
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

            Assert.Throws<InvalidEntityTextValueException>(() => instance.Create(device));
        }

        #endregion

        #region ValidateDeviceDescription

        [Fact]
        public void CreateDeviceNullDescriptionTest()
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

            Assert.Throws<InvalidEntityTextValueException>(() => instance.Create(device));
        }

        [Fact]
        public void CreateDeviceEmptyDescriptionTest()
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

            Assert.Throws<InvalidEntityTextValueException>(() => instance.Create(device));
        }

        [Fact]
        public void CreateDeviceWhiteSpaceDescriptionTest()
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

            Assert.Throws<InvalidEntityTextValueException>(() => instance.Create(device));
        }

        #endregion

        #region ValidateDeviceWatts

        [Fact]
        public void CreateDeviceZeroWattsTest()
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

            Assert.Throws<InvalidEntityNumericValueException>(() => instance.Create(device));
        }

        [Fact]
        public void CreateDeviceNegativeWattsTest()
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

            Assert.Throws<InvalidEntityNumericValueException>(() => instance.Create(device));
        }

        #endregion

        [Fact]
        public void CreateDeviceSuccessTest()
        {
            var device = _deviceMock;

            _deviceManagerRepository.Setup(x => x.Create(device)).Verifiable();

            var instance = GetInstance();

            instance.Create(device);

            _deviceManagerRepository.Verify();
        }

        #endregion

        #region Update


        [Fact]
        public void UpdateDeviceNullTest()
        {
            Device device = null;

            var instance = GetInstance();

            Assert.Throws<ArgumentNullException>(() => instance.Update(device));
        }

        #region ValidaveDeviceId

        [Fact]
        public void UpdateDeviceIdZeroTest()
        {
            var device = new Device
            {
                Id = 0
            };

            var instance = GetInstance();

            Assert.Throws<InvalidEntityNumericValueException>(() => instance.Update(device));
        }

        [Fact]
        public void UpdateDeviceIdNegativeTest()
        {
            var device = new Device
            {
                Id = -1
            };

            var instance = GetInstance();

            Assert.Throws<InvalidEntityNumericValueException>(() => instance.Update(device));
        }

        #endregion

        #region ValidateDeviceHouseId

        [Fact]
        public void UpdateDeviceIdHouseZeroTest()
        {
            var device = new Device
            {
                Id = _deviceMock.Id,
                IdHouse = 0,
                Name = _deviceMock.Name,
                IdentificationCode = _deviceMock.IdentificationCode,
                Description = _deviceMock.Description,
                RegisterDate = _deviceMock.RegisterDate,
                Watts = _deviceMock.Watts
            };

            var instance = GetInstance();

            Assert.Throws<InvalidEntityNumericValueException>(() => instance.Update(device));
        }

        [Fact]
        public void UpdateDeviceIdHouseNegativeTest()
        {
            var device = new Device
            {
                Id = _deviceMock.Id,
                IdHouse = -1,
                Name = _deviceMock.Name,
                IdentificationCode = _deviceMock.IdentificationCode,
                Description = _deviceMock.Description,
                RegisterDate = _deviceMock.RegisterDate,
                Watts = _deviceMock.Watts
            };

            var instance = GetInstance();

            Assert.Throws<InvalidEntityNumericValueException>(() => instance.Update(device));
        }

        #endregion

        #region ValidateDeviceName

        [Fact]
        public void UpdateDeviceNullNameTest()
        {
            var device = new Device
            {
                Id = _deviceMock.Id,
                IdHouse = _deviceMock.IdHouse,
                Name = null,
                IdentificationCode = _deviceMock.IdentificationCode,
                Description = _deviceMock.Description,
                RegisterDate = _deviceMock.RegisterDate,
                Watts = _deviceMock.Watts
            };

            var instance = GetInstance();

            Assert.Throws<InvalidEntityTextValueException>(() => instance.Update(device));
        }

        [Fact]
        public void UpdateDeviceEmptyNameTest()
        {
            var device = new Device
            {
                Id = _deviceMock.Id,
                IdHouse = _deviceMock.IdHouse,
                Name = "",
                IdentificationCode = _deviceMock.IdentificationCode,
                Description = _deviceMock.Description,
                RegisterDate = _deviceMock.RegisterDate,
                Watts = _deviceMock.Watts
            };

            var instance = GetInstance();

            Assert.Throws<InvalidEntityTextValueException>(() => instance.Update(device));
        }

        [Fact]
        public void UpdateDeviceEmptySpaceNameTest()
        {
            var device = new Device
            {
                Id = _deviceMock.Id,
                IdHouse = _deviceMock.IdHouse,
                Name = " ",
                IdentificationCode = _deviceMock.IdentificationCode,
                Description = _deviceMock.Description,
                RegisterDate = _deviceMock.RegisterDate,
                Watts = _deviceMock.Watts
            };

            var instance = GetInstance();

            Assert.Throws<InvalidEntityTextValueException>(() => instance.Update(device));
        }

        #endregion

        #region ValidateDeviceIdentificationCode

        [Fact]
        public void UpdateDeviceNullIdentificationCodeTest()
        {
            var device = new Device
            {
                Id = _deviceMock.Id,
                IdHouse = _deviceMock.IdHouse,
                Name = _deviceMock.Name,
                IdentificationCode = null,
                Description = _deviceMock.Description,
                RegisterDate = _deviceMock.RegisterDate,
                Watts = _deviceMock.Watts
            };

            var instance = GetInstance();

            Assert.Throws<InvalidEntityTextValueException>(() => instance.Update(device));
        }

        [Fact]
        public void UpdateDeviceEmptyIdentificationCodeTest()
        {
            var device = new Device
            {
                Id = _deviceMock.Id,
                IdHouse = _deviceMock.IdHouse,
                Name = _deviceMock.Name,
                IdentificationCode = "",
                Description = _deviceMock.Description,
                RegisterDate = _deviceMock.RegisterDate,
                Watts = _deviceMock.Watts
            };

            var instance = GetInstance();

            Assert.Throws<InvalidEntityTextValueException>(() => instance.Update(device));
        }

        [Fact]
        public void UpdateDeviceWhiteSpaceIdentificationCodeTest()
        {
            var device = new Device
            {
                Id = _deviceMock.Id,
                IdHouse = _deviceMock.IdHouse,
                Name = _deviceMock.Name,
                IdentificationCode = " ",
                Description = _deviceMock.Description,
                RegisterDate = _deviceMock.RegisterDate,
                Watts = _deviceMock.Watts
            };

            var instance = GetInstance();

            Assert.Throws<InvalidEntityTextValueException>(() => instance.Update(device));
        }

        #endregion

        #region ValidateDeviceDescription

        [Fact]
        public void UpdateDeviceNullDescriptionTest()
        {
            var device = new Device
            {
                Id = _deviceMock.Id,
                IdHouse = _deviceMock.IdHouse,
                Name = _deviceMock.Name,
                IdentificationCode = _deviceMock.IdentificationCode,
                Description = null,
                RegisterDate = _deviceMock.RegisterDate,
                Watts = _deviceMock.Watts
            };

            var instance = GetInstance();

            Assert.Throws<InvalidEntityTextValueException>(() => instance.Update(device));
        }

        [Fact]
        public void UpdateDeviceEmptyDescriptionTest()
        {
            var device = new Device
            {
                Id = _deviceMock.Id,
                IdHouse = _deviceMock.IdHouse,
                Name = _deviceMock.Name,
                IdentificationCode = _deviceMock.IdentificationCode,
                Description = "",
                RegisterDate = _deviceMock.RegisterDate,
                Watts = _deviceMock.Watts
            };

            var instance = GetInstance();

            Assert.Throws<InvalidEntityTextValueException>(() => instance.Update(device));
        }

        [Fact]
        public void UpdateDeviceWhiteSpaceDescriptionTest()
        {
            var device = new Device
            {
                Id = _deviceMock.Id,
                IdHouse = _deviceMock.IdHouse,
                Name = _deviceMock.Name,
                IdentificationCode = _deviceMock.IdentificationCode,
                Description = " ",
                RegisterDate = _deviceMock.RegisterDate,
                Watts = _deviceMock.Watts
            };

            var instance = GetInstance();

            Assert.Throws<InvalidEntityTextValueException>(() => instance.Update(device));
        }

        #endregion

        #region ValidateDeviceWatts

        [Fact]
        public void UpdateDeviceZeroWattsTest()
        {
            var device = new Device
            {
                Id = _deviceMock.Id,
                IdHouse = _deviceMock.IdHouse,
                Name = _deviceMock.Name,
                IdentificationCode = _deviceMock.IdentificationCode,
                Description = _deviceMock.Description,
                RegisterDate = _deviceMock.RegisterDate,
                Watts = 0
            };

            var instance = GetInstance();

            Assert.Throws<InvalidEntityNumericValueException>(() => instance.Update(device));
        }

        [Fact]
        public void UpdateDeviceNegativeWattsTest()
        {
            var device = new Device
            {
                Id = _deviceMock.Id,
                IdHouse = _deviceMock.IdHouse,
                Name = _deviceMock.Name,
                IdentificationCode = _deviceMock.IdentificationCode,
                Description = _deviceMock.Description,
                RegisterDate = _deviceMock.RegisterDate,
                Watts = -1
            };

            var instance = GetInstance();

            Assert.Throws<InvalidEntityNumericValueException>(() => instance.Update(device));
        }

        #endregion


        [Fact]
        public void UpdateDeviceSuccessTest()
        {
            var device = new Device
            {
                Id = _deviceMock.Id,
                IdHouse = _deviceMock.IdHouse,
                Name = _deviceMock.Name,
                IdentificationCode = _deviceMock.IdentificationCode,
                Description = _deviceMock.Description,
                RegisterDate = _deviceMock.RegisterDate,
                Watts = _deviceMock.Watts
            };

            _deviceManagerRepository.Setup(x => x.Update(device)).Verifiable();

            var instance = GetInstance();

             instance.Update(device);

            _deviceManagerRepository.Verify();
        }

        #endregion

        #region Delete

        [Fact]
        public void DeleteDeviceNegativeIdTest()
        {
            var id = -1;

            var instance = GetInstance();

            Assert.Throws<InvalidEntityNumericValueException>(() => instance.Delete(id));
        }

        [Fact]
        public void DeleteDeviceZeroIdTest()
        {
            var id = 0;

            var instance = GetInstance();

            Assert.Throws<InvalidEntityNumericValueException>(() => instance.Delete(id));
        }

        [Fact]
        public void DeleteDeviceSuccessTest()
        {
            var id = 1;

            _deviceManagerRepository.Setup(x => x.Delete(id)).Verifiable();

            var instance = GetInstance();

             instance.Delete(id);

            _deviceManagerRepository.Verify();
        }

        #endregion

        #region Get

        [Fact]
        public void GetDeviceZeroIdTest()
        {
            var id = 0;

            var instance = GetInstance();

            Assert.Throws<InvalidEntityNumericValueException>(() => instance.Get(id));
        }

        [Fact]
        public void GetDeviceNegativeIdTest()
        {
            var id = -1;

            var instance = GetInstance();

            Assert.Throws<InvalidEntityNumericValueException>(() => instance.Get(id));
        }

        [Fact]
        public void GetDeviceReturnCountZeroTest()
        {
            int id = 1;

            var device = new Device();

            _deviceManagerRepository.Setup(x => x.Get(id)).Returns(device);

            var instance = GetInstance();

            Assert.Throws<EntityNotFoundException>(() => instance.Get(id));

            _deviceManagerRepository.Verify();
        }

        [Fact]
        public void GetDeviceSuccessTest()
        {
            int id = 1;

            var device = _deviceMock;

            _deviceManagerRepository.Setup(x => x.Get(id)).Returns(device);

            var instance = GetInstance();

            var result =  instance.Get(id);

            Assert.Equal(device, result);

            _deviceManagerRepository.Verify();
        }

        #endregion

        #region GetAll

        [Fact]
        public void GetAllDevicesEmptyTest()
        {

            var devices = new List<Device>();

            _deviceManagerRepository.Setup(x => x.GetAll())
                .Returns(devices).Verifiable();

            var instance = GetInstance();

            Assert.Throws<EntityNotFoundException>(() => instance.GetAll());

            _deviceManagerRepository.Verify();
        }

        [Fact]
        public void GetAllDevicesSuccessTest()
        {
            var devices = new List<Device>
            {
                _deviceMock,
                _deviceMock
            };

            _deviceManagerRepository.Setup(x => x.GetAll())
                .Returns(devices).Verifiable();

            var instance = GetInstance();

            var result = instance.GetAll();

            Assert.Equal(devices.Count, result.Count);

            _deviceManagerRepository.Verify();
        }

        #endregion

        #region GetByHouseId

        [Fact]
        public void GetDeviceByIdNegativeIdTest()
        {
            var id = -1;

            var instance = GetInstance();

            Assert.Throws<InvalidEntityNumericValueException>(() => instance.GetByHouseId(id));
        }

        [Fact]
        public void GetDeviceByIdZeroIdTest()
        {
            var id = 0;

            var instance = GetInstance();

            Assert.Throws<InvalidEntityNumericValueException>(() => instance.GetByHouseId(id));
        }

        [Fact]
        public void GetDeviceByIdDevicesNotFoundTest()
        {
            var id = 1;

            var devices = new List<Device>();

            _deviceManagerRepository.Setup(x => x.GetByHouseId(id))
                .Returns(devices).Verifiable();

            var instance = GetInstance();

            Assert.Throws<EntityNotFoundException>(() => instance.GetByHouseId(id));

            _deviceManagerRepository.Verify();
        }

        [Fact]
        public void GetDeviceByIdDevicesSuccessTest()
        {
            var id = 1;

            var devices = new List<Device>
            {
                _deviceMock,
                _deviceMock
            };

            _deviceManagerRepository.Setup(x => x.GetByHouseId(id))
                .Returns(devices).Verifiable();

            var instance = GetInstance();

            var result = instance.GetByHouseId(id);

            Assert.Equal(devices.Count, result.Count);

            _deviceManagerRepository.Verify();
        }

        #endregion

        #region Exists

        [Fact]
        public void DeviceExistsNullIdentificationCodeTest()
        {
            string deviceIdentificationCode = null;

            var instance = GetInstance();

            Assert.Throws<InvalidEntityTextValueException>(() => instance.Exists(deviceIdentificationCode));
        }

        [Fact]
        public void DeviceExistsEmptyIdentificationCodeTest()
        {
            string deviceIdentificationCode = "";

            var instance = GetInstance();

            Assert.Throws<InvalidEntityTextValueException>(() => instance.Exists(deviceIdentificationCode));
        }

        [Fact]
        public void DeviceExistsWhiteSpaceIdentificationCodeTest()
        {
            string deviceIdentificationCode = " ";

            var instance = GetInstance();

            Assert.Throws<InvalidEntityTextValueException>(() => instance.Exists(deviceIdentificationCode));
        }

        [Fact]
        public void DeviceExistsTrueTest()
        {
            string deviceIdentificationCode = "ABC123";

            _deviceManagerRepository.Setup(x => x.Exists(deviceIdentificationCode))
                .Returns(true).Verifiable();

            var instance = GetInstance();

            var result = instance.Exists(deviceIdentificationCode);

            Assert.True(result);

            _deviceManagerRepository.Verify();
        }

        [Fact]
        public void DeviceExistsFalseTest()
        {
            string deviceIdentificationCode = "ABC123";

            _deviceManagerRepository.Setup(x => x.Exists(deviceIdentificationCode))
                .Returns(false).Verifiable();

            var instance = GetInstance();

            var result = instance.Exists(deviceIdentificationCode);

            Assert.False(result);

            _deviceManagerRepository.Verify();
        }

        #endregion

        public DeviceManager GetInstance()
        {
            return new DeviceManager(_logger.Object, _deviceManagerRepository.Object);
        }
    }
}