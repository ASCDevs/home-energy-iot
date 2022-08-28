using home_energy_iot_entities.Entities;

namespace home_energy_iot_repository.Interfaces
{
    public interface IDeviceReporterRepository
    {
        Task Report(DeviceReport device);
    }
}
