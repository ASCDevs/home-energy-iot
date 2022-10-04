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
        public async void CreateDeviceNullDeviceTest()
        {
            Device device = null;

            var instance = GetInstance();

            await Assert.ThrowsAsync<ArgumentNullException>(() => instance.Create(device));
        }

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
        public async void CreateDeviceSuccessTest()
        {
            var device = _deviceMock;

            _deviceManagerRepository.Setup(x => x.Create(device)).Verifiable();

            var instance = GetInstance();

            await instance.Create(device);

            _deviceManagerRepository.Verify();
        }

        #endregion

        #region Update


        [Fact]
        public async void UpdateDeviceNullTest()
        {
            Device device = null;

            var instance = GetInstance();

            await Assert.ThrowsAsync<ArgumentNullException>(() => instance.Update(device));
        }

        #region ValidaveDeviceId

        [Fact]
        public async void UpdateDeviceIdZeroTest()
        {
            var device = new Device
            {
                Id = 0
            };

            var instance = GetInstance();

            await Assert.ThrowsAsync<InvalidEntityNumericValueException>(() => instance.Update(device));
        }

        [Fact]
        public async void UpdateDeviceIdNegativeTest()
        {
            var device = new Device
            {
                Id = -1
            };

            var instance = GetInstance();

            await Assert.ThrowsAsync<InvalidEntityNumericValueException>(() => instance.Update(device));
        }

        #endregion

        #region ValidateDeviceHouseId

        [Fact]
        public async void UpdateDeviceIdHouseZeroTest()
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

            await Assert.ThrowsAsync<InvalidEntityNumericValueException>(() => instance.Update(device));
        }

        [Fact]
        public async void UpdateDeviceIdHouseNegativeTest()
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

            await Assert.ThrowsAsync<InvalidEntityNumericValueException>(() => instance.Update(device));
        }

        #endregion

        #region ValidateDeviceName

        [Fact]
        public async void UpdateDeviceNullNameTest()
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

            await Assert.ThrowsAsync<InvalidEntityTextValueException>(() => instance.Update(device));
        }

        [Fact]
        public async void UpdateDeviceEmptyNameTest()
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

            await Assert.ThrowsAsync<InvalidEntityTextValueException>(() => instance.Update(device));
        }

        [Fact]
        public async void UpdateDeviceEmptySpaceNameTest()
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

            await Assert.ThrowsAsync<InvalidEntityTextValueException>(() => instance.Update(device));
        }

        #endregion

        #region ValidateDeviceIdentificationCode

        [Fact]
        public async void UpdateDeviceNullIdentificationCodeTest()
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

            await Assert.ThrowsAsync<InvalidEntityTextValueException>(() => instance.Update(device));
        }

        [Fact]
        public async void UpdateDeviceEmptyIdentificationCodeTest()
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

            await Assert.ThrowsAsync<InvalidEntityTextValueException>(() => instance.Update(device));
        }

        [Fact]
        public async void UpdateDeviceWhiteSpaceIdentificationCodeTest()
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

            await Assert.ThrowsAsync<InvalidEntityTextValueException>(() => instance.Update(device));
        }

        #endregion

        #region ValidateDeviceDescription

        [Fact]
        public async void UpdateDeviceNullDescriptionTest()
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

            await Assert.ThrowsAsync<InvalidEntityTextValueException>(() => instance.Update(device));
        }

        [Fact]
        public async void UpdateDeviceEmptyDescriptionTest()
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

            await Assert.ThrowsAsync<InvalidEntityTextValueException>(() => instance.Update(device));
        }

        [Fact]
        public async void UpdateDeviceWhiteSpaceDescriptionTest()
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

            await Assert.ThrowsAsync<InvalidEntityTextValueException>(() => instance.Update(device));
        }

        #endregion

        #region ValidateDeviceWatts

        [Fact]
        public async void UpdateDeviceZeroWattsTest()
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

            await Assert.ThrowsAsync<InvalidEntityNumericValueException>(() => instance.Update(device));
        }

        [Fact]
        public async void UpdateDeviceNegativeWattsTest()
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

            await Assert.ThrowsAsync<InvalidEntityNumericValueException>(() => instance.Update(device));
        }

        #endregion


        [Fact]
        public async void UpdateDeviceSuccessTest()
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

            await instance.Update(device);

            _deviceManagerRepository.Verify();
        }

        #endregion

        #region Delete

        [Fact]
        public async void DeleteDeviceNullDeviceTest()
        {
            Device device = null;

            var instance = GetInstance();

            await Assert.ThrowsAsync<ArgumentNullException>(() => instance.Delete(device));
        }

        #region ValidateDeviceId 

        [Fact]
        public async void DeleteDeviceIdZeroTest()
        {
            var device = new Device
            {
                Id = 0
            };

            var instance = GetInstance();

            await Assert.ThrowsAsync<InvalidEntityNumericValueException>(() => instance.Delete(device));
        }

        [Fact]
        public async void DeleteDeviceIdNegativeTest()
        {
            var device = new Device
            {
                Id = -1
            };

            var instance = GetInstance();

            await Assert.ThrowsAsync<InvalidEntityNumericValueException>(() => instance.Delete(device));
        }

        #endregion

        [Fact]
        public async void DeleteDeviceSuccessTest()
        {
            var device = new Device
            {
                Id = _deviceMock.Id
            };

            _deviceManagerRepository.Setup(x => x.Delete(device)).Verifiable();

            var instance = GetInstance();

            await instance.Delete(device);

            _deviceManagerRepository.Verify();
        }

        #endregion

        #region Get

        [Fact]
        public async void GetDeviceZeroIdTest()
        {
            var id = 0;

            var instance = GetInstance();

            await Assert.ThrowsAsync<InvalidEntityNumericValueException>(() => instance.Get(id));
        }

        [Fact]
        public async void GetDeviceNegativeIdTest()
        {
            var id = -1;

            var instance = GetInstance();

            await Assert.ThrowsAsync<InvalidEntityNumericValueException>(() => instance.Get(id));
        }

        [Fact]
        public async void GetDeviceNullTest()
        {
            int id = 1;
            Task<Device> device = Task.FromResult(new Device());

            _deviceManagerRepository.Setup(x => x.Get(id)).Returns(device);

            var instance = GetInstance();

            await Assert.ThrowsAsync<EntityNotFoundException>(() => instance.Get(id));

            _deviceManagerRepository.Verify();
        }

        [Fact]
        public async void GetDeviceSuccessTest()
        {
            int id = 1;
            Task<Device> device = Task.FromResult(_deviceMock);

            _deviceManagerRepository.Setup(x => x.Get(id)).Returns(device);

            var instance = GetInstance();

            await instance.Get(id);

            _deviceManagerRepository.Verify();
        }

        #endregion

        #region GetAll

        [Fact]
        public async void GetAllDevicesEmptyTest()
        {

            Task<List<Device>> devices = Task.FromResult(new List<Device>());

            _deviceManagerRepository.Setup(x => x.GetAll()).Returns(devices);

            var instance = GetInstance();

            await Assert.ThrowsAsync<EntityNotFoundException>(() => instance.GetAll());

            _deviceManagerRepository.Verify();
        }

        [Fact]
        public async void GetAllDevicesSuccessTest()
        {
            Task<List<Device>> devices = Task.FromResult(new List<Device>
            {
                _deviceMock,
                _deviceMock
            });

            _deviceManagerRepository.Setup(x => x.GetAll()).Returns(devices);

            var instance = GetInstance();

            await instance.GetAll();

            _deviceManagerRepository.Verify();
        }

        #endregion

        #region GetByHouseId

        

        #endregion

        #region Exists

        [Fact]
        public async void DeviceExistsNullIdentificationCodeTest()
        {
            string deviceIdentificationCode = null;

            var instance = GetInstance();

            await Assert.ThrowsAsync<InvalidEntityTextValueException>(() => instance.Exists(deviceIdentificationCode));
        }

        [Fact]
        public async void DeviceExistsEmptyIdentificationCodeTest()
        {
            string deviceIdentificationCode = "";

            var instance = GetInstance();

            await Assert.ThrowsAsync<InvalidEntityTextValueException>(() => instance.Exists(deviceIdentificationCode));
        }

        [Fact]
        public async void DeviceExistsWhiteSpaceIdentificationCodeTest()
        {
            string deviceIdentificationCode = " ";

            var instance = GetInstance();

            await Assert.ThrowsAsync<InvalidEntityTextValueException>(() => instance.Exists(deviceIdentificationCode));
        }

        [Fact]
        public void DeviceExistsTrueTest()
        {
            string deviceIdentificationCode = "ABC123";

            _deviceManagerRepository.Setup(x => x.Exists(deviceIdentificationCode))
                .Returns(Task.FromResult(true)).Verifiable();

            var instance = GetInstance();

            var result = instance.Exists(deviceIdentificationCode).Result;

            Assert.True(result);

            _deviceManagerRepository.Verify();
        }

        [Fact]
        public void DeviceExistsFalseTest()
        {
            string deviceIdentificationCode = "ABC123";

            _deviceManagerRepository.Setup(x => x.Exists(deviceIdentificationCode))
                .Returns(Task.FromResult(false)).Verifiable();

            var instance = GetInstance();

            var result = instance.Exists(deviceIdentificationCode).Result;

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