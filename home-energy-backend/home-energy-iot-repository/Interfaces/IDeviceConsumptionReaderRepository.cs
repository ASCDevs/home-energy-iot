using home_energy_iot_entities.Entities;

namespace home_energy_iot_repository.Interfaces
{
    public interface IDeviceConsumptionReaderRepository
    {
        List<DeviceReport> GetDeviceConsumption(string deviceIdentificationCode);
        List<DeviceReport> GetDeviceConsumptionValueBetweenDates(string deviceIdentificationCode, DateTime initialDate, DateTime finalDate);
    }
}