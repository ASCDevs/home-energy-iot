﻿using home_energy_iot_entities.Entities;

namespace home_energy_iot_repository.Interfaces
{
    public interface IDeviceReporterRepository
    {
        void Report(DeviceReport device);
    }
}
