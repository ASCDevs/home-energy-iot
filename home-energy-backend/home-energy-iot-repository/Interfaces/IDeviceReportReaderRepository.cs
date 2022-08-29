using home_energy_iot_entities.Entities;

namespace home_energy_iot_repository.Interfaces
{
    public interface IDeviceReportReaderRepository
    {
        List<DeviceReport> GetDeviceConsumption(string deviceIdentificationCode);

        List<DeviceReport> GetDeviceConsumptionBetween(string deviceIdentificationCode, DateTime initialDate, DateTime finalDate);
    }
}