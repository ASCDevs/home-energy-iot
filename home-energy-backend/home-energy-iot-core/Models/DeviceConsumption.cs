using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace home_energy_iot_core.Models
{
    public class DeviceConsumption
    {
        public string IdentificationCode { get; set; }
        public double ConsumptionInReal { get; set; }
        public double ConsumptionInWatts { get; set; }
        public int ConsumptionDates { get; set; }
        public DateTime InitialDate { get; set; }
        public DateTime FinalDate { get; set; }
    }
}