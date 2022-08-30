using home_energy_iot_monitoring.Interfaces;

namespace home_energy_iot_monitoring.Domains
{
    public class ReportAPI : IReportAPI
    {
        private string urlAPISaveValue { get; set; }

        public ReportAPI(IConfiguration configuration)
        {
            if (configuration["Ambiente"] == "prod") {
                urlAPISaveValue = configuration["APISaveValueProd"];
            }
            else
            {
                urlAPISaveValue = configuration["APISaveValueDev"];
            }
            
        }
        public async Task SaveEnergyValue(string energyValue, string deviceId)
        {
            if (!(string.IsNullOrWhiteSpace(energyValue)) && !(string.IsNullOrWhiteSpace(deviceId)))
            {
                await Task.Run(() =>
                {
                    //Chama api e salva
                });
            }
        }
    }
}
