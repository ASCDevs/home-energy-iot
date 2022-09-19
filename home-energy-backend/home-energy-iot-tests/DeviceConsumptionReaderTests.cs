using home_energy_iot_core;
using home_energy_iot_core.Models;
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

        [Fact]
        public void GetDeviceConsumptionTotalValueTest()
        {
            string deviceIdentificationCode = "PC:12:XD:23";

            var reports = new List<DeviceReport>
            {
                new DeviceReport
                {
                    Id = 1,
                    IdentificationCode = deviceIdentificationCode,
                    ReportDate = new DateTime(2022, 01, 02, 5, 0, 0),
                    WattsUsage = 1.20m
                },
                new DeviceReport
                {
                    Id = 1,
                    IdentificationCode = deviceIdentificationCode,
                    ReportDate = new DateTime(2022, 01, 02, 7, 0, 0),
                    WattsUsage = 1.20m
                }
            };

            var initialDate = reports[0].ReportDate;
            var finalDate = reports[reports.Count - 1].ReportDate;

            var wattsTotal = Convert.ToDouble(reports.Sum(x => x.WattsUsage));

            var consumption = new DeviceConsumption
            {
                IdentificationCode = deviceIdentificationCode,
                ConsumptionInReal = CalculateWatts(wattsTotal, initialDate, finalDate),
                ConsumptionInWatts = wattsTotal,
                ConsumptionDates = reports.Select(x => x.ReportDate).ToList(),
                InitialDate = initialDate,
                FinalDate = finalDate
            };

            _deviceConsumptionReaderRepository.Setup(x => x.GetDeviceConsumption(deviceIdentificationCode))
                .Returns(reports);

            var instance = GetInstance();

            var result = instance.GetDeviceConsumptionTotalValue(deviceIdentificationCode);

            Assert.Equal(consumption.IdentificationCode, result.IdentificationCode);
            Assert.Equal(consumption.ConsumptionInReal, result.ConsumptionInReal);
            Assert.Equal(consumption.ConsumptionInWatts, result.ConsumptionInWatts);
            Assert.Equal(consumption.ConsumptionDates, result.ConsumptionDates);
            Assert.Equal(consumption.InitialDate, result.InitialDate);
            Assert.Equal(consumption.FinalDate, result.FinalDate);

            _deviceConsumptionReaderRepository.Verify();
        }

        private double CalculateWatts(double watts, DateTime initialDate, DateTime finalDate)
        {
            var totalHours = (finalDate - initialDate).TotalHours;

            var kwhPrice = 0.80;

            return (watts * totalHours / 1000) * kwhPrice;
        }

        public DeviceConsumptionReader GetInstance()
        {
            return new DeviceConsumptionReader(_logger.Object, _deviceConsumptionReaderRepository.Object);
        }
    }
}