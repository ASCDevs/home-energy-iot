using home_energy_iot_entities;
using home_energy_iot_entities.Entities;
using home_energy_iot_repository.Interfaces;

namespace home_energy_iot_repository
{
    public class DeviceReportReaderRepository : IDeviceReportReaderRepository
    {
        private readonly DataBaseContext _databaseContext;

        public DeviceReportReaderRepository(DataBaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public List<DeviceReport> GetDeviceConsumption(string deviceIdentificationCode)
        {
            var reports = _databaseContext.DevicesReports.Where(x => x.IdentificationCode == deviceIdentificationCode).ToList();

            return reports;
        }


        public List<DeviceReport> GetDeviceConsumptionBetween(string deviceIdentificationCode, DateTime initialDate, DateTime finalDate)
        {
            var reports = _databaseContext.DevicesReports.Where(
                x => x.IdentificationCode == deviceIdentificationCode && 
                x.ReportDate >= initialDate && 
                x.ReportDate <= initialDate).ToList();

            return reports;
        }
    }
}
