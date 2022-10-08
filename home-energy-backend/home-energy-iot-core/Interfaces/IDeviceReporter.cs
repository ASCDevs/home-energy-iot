using home_energy_iot_entities.Entities;

namespace home_energy_iot_core.Interfaces
{
    public interface IDeviceReporter
    {
        void Report(DeviceReport device);
    }
}