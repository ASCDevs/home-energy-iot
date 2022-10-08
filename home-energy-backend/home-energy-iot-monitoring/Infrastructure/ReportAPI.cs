using home_energy_iot_monitoring.Domains;
using home_energy_iot_monitoring.Interfaces;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace home_energy_iot_monitoring.Infrasctructure
{
    public class ReportAPI : IReportAPI
    {
        private readonly ILogger<ReportAPI> _logger;
        private string urlAPISaveValue { get; set; }
        private bool useAPI { get; set; }
        private string tokenAPI { get; set; }

        public ReportAPI(IConfiguration configuration, ILogger<ReportAPI> logger)
        {
            _logger = logger;

            try
            {
                if (configuration["Environment"] == "prod")
                {
                    urlAPISaveValue = configuration["APISaveValueProd"];
                }
                else
                {
                    urlAPISaveValue = configuration["APISaveValueDev"];
                }
                tokenAPI = configuration["TokenApi"];
                useAPI = Convert.ToBoolean(configuration["flAPISaveValue"]);
            }
            catch (Exception ex)
            {
                _logger.LogCritical("[Erro Critico ReportAPI] > Erro no construtor ("+DateTime.Now+"), Erro: "+ex.Message);
            }
        }
        public async Task SaveEnergyValue(string energyValue, string deviceId)
        {
            try
            {
                if (useAPI)
                {
                    if (!(string.IsNullOrWhiteSpace(energyValue)) && !(string.IsNullOrWhiteSpace(deviceId)))
                    {
                        await Task.Run(async () =>
                        {
                            if (!string.IsNullOrWhiteSpace(urlAPISaveValue))
                            {
                                DeviceReport deviceReport = new DeviceReport(energyValue, deviceId);

                                var httpClient = new HttpClient();
                                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",tokenAPI);

                                var contentReport = ToRequest(deviceReport);
                                var response = await httpClient.PostAsync(urlAPISaveValue, contentReport);
                                if (!response.IsSuccessStatusCode)
                                {
                                    throw new Exception("Valor não salvo para "+deviceId+" - "+response.StatusCode);
                                }
                            }
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical("[Erro Critico ReportAPI] > Erro ao salvar valor de energia ("+energyValue+") lido no dispositivo ("+deviceId+"), ("+DateTime.Now+"), Erro: "+ex.Message);
            }
        }

        private StringContent ToRequest(object obj)
        {
            var json = JsonConvert.SerializeObject(obj);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            return data;
        }
    }

}
