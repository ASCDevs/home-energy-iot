using home_energy_iot_core.Interfaces;

namespace home_energy_iot_core
{
    internal class DeviceReportReader : IDeviceReportReader
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
