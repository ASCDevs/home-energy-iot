using Xunit;
using Moq;
using Moq.EntityFrameworkCore;
using home_energy_iot_core;
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

        public DeviceManagerTests()
        {
            _logger = new Mock<ILogger<DeviceManager>>(MockBehavior.Loose);
            _deviceManagerRepository = new Mock<IDeviceManagerRepository>();
        }

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
            var device = new Device
            {
                Name = "Computer",
                IdHouse = 1,
                Description = "New Computer",
                RegisterDate = DateTime.Now,
                Watts = 500
            };

            _deviceManagerRepository.Setup(x => x.Create(device)).Verifiable();

            var instance = GetInstance();

            await instance.Create(device);

            _deviceManagerRepository.Verify();
        }

        public DeviceManager GetInstance()
        {
            return new DeviceManager(_logger.Object, _deviceManagerRepository.Object);
        }
    }
}