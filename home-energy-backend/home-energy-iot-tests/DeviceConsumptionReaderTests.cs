using home_energy_iot_core;
using home_energy_iot_entities.Entities;
using home_energy_iot_repository.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace home_energy_iot_tests
{
    public class DeviceConsumptionReaderTests
    {
        private readonly Mock<ILogger<DeviceConsumptionReader>> _logger;
        private readonly Mock<IDeviceConsumptionReaderRepository> _deviceConsumptionReaderRepository;

        public DeviceConsumptionReaderTests()
        {
            _logger = new Mock<ILogger<DeviceConsumptionReader>>(MockBehavior.Loose);
            _deviceConsumptionReaderRepository = new Mock<IDeviceConsumptionReaderRepository>();
        }

        [Fact]
        public void GetDeviceConsumptionTotalValueDeviceIdentificationNullTest()
        {
            string deviceIdentificationCode = null;

            var instance = GetInstance();

            Assert.Throws<ArgumentNullException>(() => instance.GetDeviceConsumptionTotalValue(deviceIdentificationCode));
        }

        [Fact]
        public void GetDeviceConsumptionTotalValueDeviceIdentificationEmptyTest()
        {
            string deviceIdentificationCode = "";

            var instance = GetInstance();

            Assert.Throws<ArgumentNullException>(() => instance.GetDeviceConsumptionTotalValue(deviceIdentificationCode));
        }

        [Fact]
        public void GetDeviceConsumptionTotalValueDeviceIdentificationEmptySpaceTest()
        {
            string deviceIdentificationCode = " ";

            var instance = GetInstance();

            Assert.Throws<ArgumentNullException>(() => instance.GetDeviceConsumptionTotalValue(deviceIdentificationCode));
        }

        [Fact]
        public void GetDeviceConsumptionTotalValueDeviceIdentificationNullDevicesTest()
        {
            string deviceIdentificationCode = "PC:12:XD:23";

            _deviceConsumptionReaderRepository.Setup(x => x.GetDeviceConsumption(deviceIdentificationCode));

            var instance = GetInstance();

            Assert.Null(instance.GetDeviceConsumptionTotalValue(deviceIdentificationCode).IdentificationCode);

            _deviceConsumptionReaderRepository.Verify();
        }

        [Fact]
        public void GetDeviceConsumptionTotalValueDeviceIdentificationCountZeroDevicesTest()
        {
            string deviceIdentificationCode = "PC:12:XD:23";

            List<DeviceReport> devices = new List<DeviceReport>();

            _deviceConsumptionReaderRepository.Setup(x => x.GetDeviceConsumption(deviceIdentificationCode)).Returns(devices);

            var instance = GetInstance();

            Assert.Null(instance.GetDeviceConsumptionTotalValue(deviceIdentificationCode).IdentificationCode);

            _deviceConsumptionReaderRepository.Verify();
        }


        public DeviceConsumptionReader GetInstance()
        {
            return new DeviceConsumptionReader(_logger.Object, _deviceConsumptionReaderRepository.Object);
        }
    }
}