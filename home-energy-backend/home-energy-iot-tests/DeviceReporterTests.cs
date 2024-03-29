﻿using home_energy_iot_core;
using home_energy_iot_core.Exceptions;
using home_energy_iot_entities.Entities;
using home_energy_iot_repository.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace home_energy_iot_tests
{
    public class DeviceReporterTests
    {
        private readonly Mock<ILogger<DeviceReporter>> _logger;
        private readonly Mock<IDeviceReporterRepository> _deviceReporterRepository;

        public DeviceReporterTests()
        {
            _logger = new Mock<ILogger<DeviceReporter>>();
            _deviceReporterRepository = new Mock<IDeviceReporterRepository>();
        }

        [Fact]
        public void ReportDeviceNullDeviceTest()
        {
            DeviceReport report = null;

            var instance = GetInstance();

            Assert.Throws<ArgumentNullException>(() => instance.Report(report));
        }

        [Fact]
        public void ReportDeviceInvalidWattsUsageTest()
        {
            var report = new DeviceReport
            {
                IdentificationCode = "ABC123",
                WattsUsage = -1
            };

            var instance = GetInstance();

            Assert.Throws<InvalidEntityNumericValueException>(() => instance.Report(report));
        }

        [Fact]
        public void ReportDeviceNullDeviceIdentificationCodeTest()
        {
            var report = new DeviceReport
            {
                IdentificationCode = null,
                WattsUsage = 0.10m
            };

            var instance = GetInstance();

            Assert.Throws<InvalidEntityTextValueException>(() => instance.Report(report));
        }

        [Fact]
        public void ReportDeviceEmptyDeviceIdentificationCodeTest()
        {
            var report = new DeviceReport
            {
                IdentificationCode = "",
                WattsUsage = 0.10m
            };

            var instance = GetInstance();

            Assert.Throws<InvalidEntityTextValueException>(() => instance.Report(report));
        }

        [Fact]
        public void ReportDeviceWhiteSpaceDeviceIdentificationCodeTest()
        {
            var report = new DeviceReport
            {
                IdentificationCode = " ",
                WattsUsage = 0.10m
            };

            var instance = GetInstance();

            Assert.Throws<InvalidEntityTextValueException>(() => instance.Report(report));
        }

        [Fact]
        public void ReportDeviceSuccessTest()
        {
            var report = new DeviceReport
            {
                IdentificationCode = "ABC123",
                WattsUsage = 0.10m
            };

            var instance = GetInstance();
            
            _deviceReporterRepository.Setup(x => x.Report(report)).Verifiable();

             instance.Report(report);

            _deviceReporterRepository.Verify(x => x.Report(report), Times.Exactly(1));
        }

        public DeviceReporter GetInstance()
        {
            return new DeviceReporter(_logger.Object, _deviceReporterRepository.Object);
        }
    }
}
