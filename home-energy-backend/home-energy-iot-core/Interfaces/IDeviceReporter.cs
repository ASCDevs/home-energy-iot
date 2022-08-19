using home_energy_iot_entities.Entities;

namespace home_energy_iot_core.Interfaces
{
    public interface IDeviceReporter
    {
        Task Report(DeviceReport device);
        bool ReportExists(DeviceReport device);
    }
}