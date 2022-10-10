using home_energy_iot_core;
using home_energy_iot_core.Exceptions;
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
        private readonly Mock<IHouseManagerRepository> _houseManagerRepository;

        public DeviceConsumptionReaderTests()
        {
            _logger = new Mock<ILogger<DeviceConsumptionReader>>(MockBehavior.Loose);
            _deviceConsumptionReaderRepository = new Mock<IDeviceConsumptionReaderRepository>();
            _houseManagerRepository = new Mock<IHouseManagerRepository>();
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

            _deviceConsumptionReaderRepository.Setup(x => x.GetDeviceConsumption(deviceIdentificationCode))
                .Verifiable();

            var instance = GetInstance();

            Assert.Throws<EntityNotFoundException>(() =>
                instance.GetDeviceConsumptionTotalValue(deviceIdentificationCode));

            _deviceConsumptionReaderRepository.Verify();
        }

        [Fact]
        public void GetDeviceConsumptionTotalValueDeviceIdentificationNoReportFoundTest()
        {
            string deviceIdentificationCode = "PC:12:XD:23";

            var reports = new List<DeviceReport>();

            _deviceConsumptionReaderRepository.Setup(x => x.GetDeviceConsumption(deviceIdentificationCode))
                .Returns(reports).Verifiable();

            var instance = GetInstance();

            Assert.Throws<EntityNotFoundException>(() =>
                instance.GetDeviceConsumptionTotalValue(deviceIdentificationCode));

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

            var khwHour = 0.85;

            var wattsTotal = Convert.ToDouble(reports.Average(x => x.WattsUsage));

            var consumption = new DeviceConsumption
            {
                IdentificationCode = deviceIdentificationCode,
                ConsumptionInReal = CalculateUsage(khwHour, wattsTotal, reports.Count),
                ConsumptionInWatts = wattsTotal,
                ConsumptionDates = reports.Select(x => x.ReportDate).ToList(),
                InitialDate = initialDate,
                FinalDate = finalDate
            };

            _deviceConsumptionReaderRepository.Setup(x => x.GetDeviceConsumption(deviceIdentificationCode))
                .Returns(reports).Verifiable();

            _houseManagerRepository.Setup(x => x.GetHouseBaseKwhByDeviceIdentificationCode(deviceIdentificationCode))
                .Returns(0.85).Verifiable();

            var instance = GetInstance();

            var result = instance.GetDeviceConsumptionTotalValue(deviceIdentificationCode);

            Assert.Equal(consumption.IdentificationCode, result.IdentificationCode);
            Assert.Equal(consumption.ConsumptionInReal, result.ConsumptionInReal);
            Assert.Equal(consumption.ConsumptionInWatts, result.ConsumptionInWatts);
            Assert.Equal(consumption.ConsumptionDates, result.ConsumptionDates);
            Assert.Equal(consumption.InitialDate, result.InitialDate);
            Assert.Equal(consumption.FinalDate, result.FinalDate);

            _deviceConsumptionReaderRepository.Verify();
            _houseManagerRepository.Verify();
        }

        [Fact]
        public void GetDeviceConsumptionValueBetweenDatesDeviceIdentificationNullTest()
        {
            string deviceIdentificationCode = null;

            var initialDate = new DateTime(2022, 01, 02, 5, 0, 0);
            var finalDate = new DateTime(2022, 01, 02, 7, 0, 0);

            var instance = GetInstance();

            Assert.Throws<ArgumentNullException>(() =>
                instance.GetDeviceConsumptionValueBetweenDates(deviceIdentificationCode, initialDate, finalDate));
        }

        [Fact]
        public void GetDeviceConsumptionValueBetweenDatesDeviceIdentificationEmptyTest()
        {
            string deviceIdentificationCode = "";

            var initialDate = new DateTime(2022, 01, 02, 5, 0, 0);
            var finalDate = new DateTime(2022, 01, 02, 7, 0, 0);

            var instance = GetInstance();

            Assert.Throws<ArgumentNullException>(() =>
                instance.GetDeviceConsumptionValueBetweenDates(deviceIdentificationCode, initialDate, finalDate));
        }

        [Fact]
        public void GetDeviceConsumptionValueBetweenDatesDeviceIdentificationWhiteSpaceTest()
        {
            string deviceIdentificationCode = " ";

            var initialDate = new DateTime(2022, 01, 02, 5, 0, 0);
            var finalDate = new DateTime(2022, 01, 02, 7, 0, 0);

            var instance = GetInstance();

            Assert.Throws<ArgumentNullException>(() =>
                instance.GetDeviceConsumptionValueBetweenDates(deviceIdentificationCode, initialDate, finalDate));
        }

        [Fact]
        public void GetDeviceConsumptionValueBetweenDatesInvalidDateTest()
        {
            string deviceIdentificationCode = "ABC123";

            var initialDate = new DateTime(2022, 01, 02, 7, 0, 0);
            var finalDate = new DateTime(2022, 01, 02, 5, 0, 0);

            var instance = GetInstance();

            Assert.Throws<Exception>(() =>
                instance.GetDeviceConsumptionValueBetweenDates(deviceIdentificationCode, initialDate, finalDate));
        }

        [Fact]
        public void GetDeviceConsumptionValueBetweenDatesReportNotFoundTest()
        {
            string deviceIdentificationCode = "ABC123";

            var initialDate = new DateTime(2022, 01, 02, 5, 0, 0);
            var finalDate = new DateTime(2022, 01, 02, 7, 0, 0);

            var reports = new List<DeviceReport>();

            _deviceConsumptionReaderRepository.Setup(x => x.GetDeviceConsumptionValueBetweenDates(deviceIdentificationCode, initialDate, finalDate)).
                Returns(reports).Verifiable();

            var instance = GetInstance();

            Assert.Throws<EntityNotFoundException>(() => instance.GetDeviceConsumptionValueBetweenDates(deviceIdentificationCode, initialDate, finalDate));

            _deviceConsumptionReaderRepository.Verify();
        }

        [Fact]
        public void GetDeviceConsumptionValueBetweenDatesSuccessTest()
        {
            string deviceIdentificationCode = "ABC123";

            var initialDate = new DateTime(2022, 01, 02, 5, 0, 0);
            var finalDate = new DateTime(2022, 01, 02, 7, 0, 0);

            var reports = new List<DeviceReport>
            {
                new DeviceReport
                {
                    Id = 1,
                    IdentificationCode = deviceIdentificationCode,
                    ReportDate = new DateTime(2022, 01, 02, 5, 0, 1),
                    WattsUsage = 1.20m
                },
                new DeviceReport
                {
                    Id = 1,
                    IdentificationCode = deviceIdentificationCode,
                    ReportDate = new DateTime(2022, 01, 02, 5, 0, 2),
                    WattsUsage = 1.20m
                }
            };

            var wattsTotal = Convert.ToDouble(reports.Average(x => x.WattsUsage));

            var khwPrice = 0.85;

            var consumption = new DeviceConsumption
            {
                IdentificationCode = deviceIdentificationCode,
                ConsumptionInReal = CalculateUsage(khwPrice, wattsTotal, reports.Count),
                ConsumptionInWatts = wattsTotal,
                ConsumptionDates = reports.Select(x => x.ReportDate).ToList(),
                InitialDate = initialDate,
                FinalDate = finalDate
            };

            _deviceConsumptionReaderRepository.Setup(x => x.GetDeviceConsumptionValueBetweenDates(deviceIdentificationCode, initialDate, finalDate))
                .Returns(reports).Verifiable();

            _houseManagerRepository.Setup(x => x.GetHouseBaseKwhByDeviceIdentificationCode(deviceIdentificationCode))
                .Returns(khwPrice).Verifiable();

            var instance = GetInstance();

            var result = instance.GetDeviceConsumptionValueBetweenDates(deviceIdentificationCode, initialDate, finalDate);

            Assert.Equal(consumption.IdentificationCode, result.IdentificationCode);
            Assert.Equal(consumption.ConsumptionInReal, result.ConsumptionInReal);
            Assert.Equal(consumption.ConsumptionInWatts, result.ConsumptionInWatts);
            Assert.Equal(consumption.ConsumptionDates, result.ConsumptionDates);
            Assert.Equal(consumption.InitialDate, initialDate);
            Assert.Equal(consumption.FinalDate, finalDate);

            _deviceConsumptionReaderRepository.Verify();
            _houseManagerRepository.Verify();
        }

        private double CalculateUsage(double kwhPrice, double watts, double totalSecondsUsage)
        {
            var totalHours = totalSecondsUsage / 3600;

            return (watts * totalHours / 1000) * kwhPrice;
        }

        public DeviceConsumptionReader GetInstance()
        {
            return new DeviceConsumptionReader(_logger.Object, _deviceConsumptionReaderRepository.Object, _houseManagerRepository.Object);
        }
    }
}