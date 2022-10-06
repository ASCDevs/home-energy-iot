using home_energy_iot_core;
using home_energy_iot_core.Exceptions;
using home_energy_iot_repository.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace home_energy_iot_tests
{
    public class HouseManagerTests
    {
        private readonly Mock<ILogger<HouseManager>> _logger;
        private readonly Mock<IHouseManagerRepository> _houseManagerRepository;

        public HouseManagerTests()
        {
            _logger = new Mock<ILogger<HouseManager>>();
            _houseManagerRepository = new Mock<IHouseManagerRepository>();
        }

        #region Delete

        [Fact]
        public async void DeleteHouseNegativeIdTest()
        {
            var id = -1;

            var instance = GetInstance();

            await Assert.ThrowsAsync<InvalidEntityNumericValueException>(() => instance.Delete(id));
        }

        [Fact]
        public async void DeleteHouseZeroIdTest()
        {
            var id = 0;

            var instance = GetInstance();

            await Assert.ThrowsAsync<InvalidEntityNumericValueException>(() => instance.Delete(id));
        }

        [Fact]
        public async void DeleteHouseSuccessIdTest()
        {
            var id = 2;

            _houseManagerRepository.Setup(x => x.Delete(id)).Verifiable();

            var instance = GetInstance();

            await instance.Delete(id);

            _houseManagerRepository.Verify(x => x.Delete(id), Times.Exactly(1));
        }

        #endregion


        public HouseManager GetInstance()
        {
            return new HouseManager(_logger.Object, _houseManagerRepository.Object);
        }
    }
}
