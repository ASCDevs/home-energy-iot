using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using home_energy_iot_repository.Interfaces;

namespace home_energy_iot_repository
{
    public class DeviceReportReaderRepository : IDeviceReportReaderRepository
    {
        public decimal GetDeviceConsumptionTotalValueInReal(string deviceIdentificationCode)
        {
            throw new NotImplementedException();
        }

        public decimal GetDeviceConsumptionValueBetweenInReal(string deviceIdentificationCode, DateTime initialDate,
            DateTime finalDate)
        {
            throw new NotImplementedException();
        }

        public decimal GetDeviceConsumptionTotalValueInWatts(string deviceIdentificationCode)
        {
            throw new NotImplementedException();
        }

        public decimal GetDeviceConsumptionValueBetweenInWatts(string deviceIdentificationCode, DateTime initialDate,
            DateTime finalDate)
        {
            throw new NotImplementedException();
        }
    }
}
