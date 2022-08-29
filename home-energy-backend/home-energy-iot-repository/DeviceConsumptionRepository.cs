using home_energy_iot_entities;
using home_energy_iot_entities.Entities;
using home_energy_iot_repository.Interfaces;

namespace home_energy_iot_repository
{
    public class DeviceConsumptionRepository : IDeviceConsumptionReaderRepository
    {
        private readonly DataBaseContext _databaseContext;

        public DeviceConsumptionRepository(DataBaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public List<DeviceReport> GetDeviceConsumption(string deviceIdentificationCode)
        {
            var reports = _databaseContext.DevicesReports.Where(x => x.IdentificationCode == deviceIdentificationCode).ToList();

            return reports;
        }


        public List<DeviceReport> GetDeviceConsumptionBetweenDates(string deviceIdentificationCode, DateTime initialDate, DateTime finalDate)
        {
            var reports = _databaseContext.DevicesReports.Where(
                x => x.IdentificationCode == deviceIdentificationCode && 
                x.ReportDate >= initialDate && 
                x.ReportDate <= finalDate).ToList();

            return reports;
        }
    }
}
