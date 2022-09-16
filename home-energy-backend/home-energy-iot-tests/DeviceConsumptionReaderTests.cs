using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Logging;
using home_energy_iot_core;
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

        public DeviceConsumptionReader GetInstance()
        {
            return new DeviceConsumptionReader(_logger.Object, _deviceConsumptionReaderRepository.Object);
        }
    }
}