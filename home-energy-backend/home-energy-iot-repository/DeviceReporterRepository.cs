using home_energy_iot_entities;
using home_energy_iot_entities.Entities;
using home_energy_iot_repository.Interfaces;

namespace home_energy_iot_repository
{
    public class DeviceReporterRepository : IDeviceReporterRepository
    {
        private DataBaseContext _dataBaseContext;

        public DeviceReporterRepository(DataBaseContext dataBaseContext)
        {
            _dataBaseContext = dataBaseContext;
        }

        public void Report(DeviceReport device)
        {
            _dataBaseContext.Add(device);
            _dataBaseContext.SaveChanges();
        }
    }
}