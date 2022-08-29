using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace home_energy_iot_repository.Interfaces
{
    public interface IDeviceReportReaderRepository
    {
        decimal GetDeviceConsumptionTotalValueInReal(string deviceIdentificationCode);
        decimal GetDeviceConsumptionValueBetweenInReal(string deviceIdentificationCode, DateTime initialDate, DateTime finalDate);
        decimal GetDeviceConsumptionTotalValueInWatts(string deviceIdentificationCode);
        decimal GetDeviceConsumptionValueBetweenInWatts(string deviceIdentificationCode, DateTime initialDate, DateTime finalDate);
    }
}