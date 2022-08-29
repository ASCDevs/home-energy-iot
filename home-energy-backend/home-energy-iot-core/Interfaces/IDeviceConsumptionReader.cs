using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using home_energy_iot_core.Models;

namespace home_energy_iot_core.Interfaces
{
    public interface IDeviceConsumptionReader
    {
        DeviceConsumption GetDeviceConsumptionTotalValue(string deviceIdentificationCode);
        DeviceConsumption GetDeviceConsumptionValueBetweenDates(string deviceIdentificationCode, DateTime initialDate, DateTime finalDate);
    }
}
