using home_energy_iot_core;
using home_energy_iot_core.Exceptions;
using home_energy_iot_entities.Entities;
using home_energy_iot_repository.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace home_energy_iot_tests
{
    public class HouseManagerTests
    {
        private readonly Mock<ILogger<HouseManager>> _logger;
        private readonly Mock<IHouseManagerRepository> _houseManagerRepository;
        private readonly House _housesMock;

        public HouseManagerTests()
        {
            _logger = new Mock<ILogger<HouseManager>>();
            _houseManagerRepository = new Mock<IHouseManagerRepository>();
            _housesMock = new House
            {
                Id = 1,
                IdUser = 1,
                Name = "House2",
                NameAddress = "NameAddress2",
                NumberAddress = 2,
                PeriodDaysReport = 1,
                RegisterDate = DateTime.Now,
                TypeAddress = "House",
                ValuePerKWH = 0.80
            };
        }

        #region Create

        [Fact]
        public void CreateHouseNullTest()
        {
            House house = null;

            var instance = GetInstance();

            Assert.Throws<ArgumentNullException>(() => instance.Create(house));
        }

        #region ValidateHouseId

        [Fact]
        public void CreateHouseIdUserNegativeTest()
        {
            var house = new House
            {
                IdUser = -1
            };

            var instance = GetInstance();

            Assert.Throws<InvalidEntityNumericValueException>(() => instance.Create(house));
        }

        [Fact]
        public void CreateHouseIdUserZeroTest()
        {
            var house = new House
            {
                IdUser = 0
            };

            var instance = GetInstance();

            Assert.Throws<InvalidEntityNumericValueException>(() => instance.Create(house));
        }

        #endregion

        #region ValidateHouseName

        [Fact]
        public void CreateHouseNameNullTest()
        {
            var house = new House
            {
                Id = _housesMock.Id,
                IdUser = _housesMock.IdUser,
                Name = null,
                TypeAddress = _housesMock.TypeAddress,
                NameAddress = _housesMock.NameAddress,
                NumberAddress = _housesMock.NumberAddress,
                RegisterDate = DateTime.Now,
                PeriodDaysReport = _housesMock.PeriodDaysReport,
                ValuePerKWH = _housesMock.ValuePerKWH
            };

            var instance = GetInstance();

            Assert.Throws<InvalidEntityTextValueException>(() => instance.Create(house));
        }

        [Fact]
        public void CreateHouseNameEmptyTest()
        {
            var house = new House
            {
                Id = _housesMock.Id,
                IdUser = _housesMock.IdUser,
                Name = "",
                TypeAddress = _housesMock.TypeAddress,
                NameAddress = _housesMock.NameAddress,
                NumberAddress = _housesMock.NumberAddress,
                RegisterDate = DateTime.Now,
                PeriodDaysReport = _housesMock.PeriodDaysReport,
                ValuePerKWH = _housesMock.ValuePerKWH
            };

            var instance = GetInstance();

            Assert.Throws<InvalidEntityTextValueException>(() => instance.Create(house));
        }

        [Fact]
        public void CreateHouseNameWhiteSpaceTest()
        {
            var house = new House
            {
                Id = _housesMock.Id,
                IdUser = _housesMock.IdUser,
                Name = " ",
                TypeAddress = _housesMock.TypeAddress,
                NameAddress = _housesMock.NameAddress,
                NumberAddress = _housesMock.NumberAddress,
                RegisterDate = DateTime.Now,
                PeriodDaysReport = _housesMock.PeriodDaysReport,
                ValuePerKWH = _housesMock.ValuePerKWH
            };

            var instance = GetInstance();

            Assert.Throws<InvalidEntityTextValueException>(() => instance.Create(house));
        }

        #endregion

        #region ValidateHouseTypeAddress

        [Fact]
        public void CreateHouseTypeAddressNullTest()
        {
            var house = new House
            {
                Id = _housesMock.Id,
                IdUser = _housesMock.IdUser,
                Name = _housesMock.Name,
                TypeAddress = null,
                NameAddress = _housesMock.NameAddress,
                NumberAddress = _housesMock.NumberAddress,
                RegisterDate = DateTime.Now,
                PeriodDaysReport = _housesMock.PeriodDaysReport,
                ValuePerKWH = _housesMock.ValuePerKWH
            };

            var instance = GetInstance();

            Assert.Throws<InvalidEntityTextValueException>(() => instance.Create(house));
        }

        [Fact]
        public void CreateHouseTypeAddressEmptyTest()
        {
            var house = new House
            {
                Id = _housesMock.Id,
                IdUser = _housesMock.IdUser,
                Name = _housesMock.Name,
                TypeAddress = "",
                NameAddress = _housesMock.NameAddress,
                NumberAddress = _housesMock.NumberAddress,
                RegisterDate = DateTime.Now,
                PeriodDaysReport = _housesMock.PeriodDaysReport,
                ValuePerKWH = _housesMock.ValuePerKWH
            };

            var instance = GetInstance();

            Assert.Throws<InvalidEntityTextValueException>(() => instance.Create(house));
        }

        [Fact]
        public void CreateHouseTypeAddressWhiteSpaceTest()
        {
            var house = new House
            {
                Id = _housesMock.Id,
                IdUser = _housesMock.IdUser,
                Name = _housesMock.Name,
                TypeAddress = " ",
                NameAddress = _housesMock.NameAddress,
                NumberAddress = _housesMock.NumberAddress,
                RegisterDate = DateTime.Now,
                PeriodDaysReport = _housesMock.PeriodDaysReport,
                ValuePerKWH = _housesMock.ValuePerKWH
            };

            var instance = GetInstance();

            Assert.Throws<InvalidEntityTextValueException>(() => instance.Create(house));
        }

        #endregion


        #endregion

        #region Delete

        [Fact]
        public void DeleteHouseNegativeIdTest()
        {
            var id = -1;

            var instance = GetInstance();

            Assert.Throws<InvalidEntityNumericValueException>(() => instance.Delete(id));
        }

        [Fact]
        public void DeleteHouseZeroIdTest()
        {
            var id = 0;

            var instance = GetInstance();

            Assert.Throws<InvalidEntityNumericValueException>(() => instance.Delete(id));
        }

        [Fact]
        public void DeleteHouseSuccessIdTest()
        {
            var id = 2;

            _houseManagerRepository.Setup(x => x.Delete(id)).Verifiable();

            var instance = GetInstance();

            instance.Delete(id);

            _houseManagerRepository.Verify(x => x.Delete(id), Times.Exactly(1));
        }

        #endregion

        #region Get

        [Fact]
        public void GetHouseIdNegativeTest()
        {
            var id = -1;

            var instance = GetInstance();

            Assert.Throws<InvalidEntityNumericValueException>(() => instance.Get(id));
        }

        [Fact]
        public void GetHouseIdZeroTest()
        {
            var id = 0;

            var instance = GetInstance();

            Assert.Throws<InvalidEntityNumericValueException>(() => instance.Get(id));
        }

        [Fact]
        public void GetHouseNotFoundTest()
        {
            var id = 1;

            House house = null;

            var instance = GetInstance();

            _houseManagerRepository.Setup(x => x.Get(id)).Returns(house).Verifiable();

            Assert.Throws<EntityNotFoundException>(() => instance.Get(id));

            _houseManagerRepository.Verify();
        }

        [Fact]
        public void GetHouseSuccessTest()
        {
            var id = 1;

            House house = _housesMock;

            var instance = GetInstance();

            _houseManagerRepository.Setup(x => x.Get(id)).Returns(house).Verifiable();

            var result = instance.Get(id);

            Assert.NotNull(result);

            _houseManagerRepository.Verify();
        }

        #endregion

        #region GetAll

        [Fact]
        public void GetAllHousesNotFoundTest()
        {
            var houses = new List<House>();

            _houseManagerRepository.Setup(x => x.GetAll())
                .Returns(houses).Verifiable();

            var instance = GetInstance();

            Assert.Throws<EntityNotFoundException>(() => instance.GetAll());

            _houseManagerRepository.Verify();
        }

        [Fact]
        public void GetAllHousesSuccessTest()
        {
            var houses = new List<House>()
            {
                _housesMock,
                _housesMock
            };

            _houseManagerRepository.Setup(x => x.GetAll())
                .Returns(houses).Verifiable();

            var instance = GetInstance();

            var result = instance.GetAll();

            Assert.Equal(houses, result);

            _houseManagerRepository.Verify();
        }

        #endregion

        #region GetByUserId

        [Fact]
        public void GetByUserIdNegativeIdTest()
        {
            var id = -1;

            var instance = GetInstance();

            Assert.Throws<InvalidEntityNumericValueException>(() => instance.GetByUserId(id));
        }

        [Fact]
        public void GetByUserIdZeroIdTest()
        {
            var id = 0;

            var instance = GetInstance();

            Assert.Throws<InvalidEntityNumericValueException>(() => instance.GetByUserId(id));
        }

        [Fact]
        public void GetByUserIdNotFoundHousesTest()
        {
            var id = 1;

            var houses = new List<House>();

            _houseManagerRepository.Setup(x => x.GetByUserId(id))
                .Returns(houses).Verifiable();

            var instance = GetInstance();

            Assert.Throws<EntityNotFoundException>(() => instance.GetByUserId(id));

            _houseManagerRepository.Verify();
        }

        [Fact]
        public void GetByUserIdSuccessTest()
        {
            var id = 1;

            var houses = new List<House>()
            {
                _housesMock,
                _housesMock
            };

            _houseManagerRepository.Setup(x => x.GetByUserId(id))
                .Returns(houses).Verifiable();

            var instance = GetInstance();

            var result = instance.GetByUserId(id);

            Assert.Equal(houses, result);

            _houseManagerRepository.Verify();
        }

        #endregion



        public HouseManager GetInstance()
        {
            return new HouseManager(_logger.Object, _houseManagerRepository.Object);
        }
    }
}
