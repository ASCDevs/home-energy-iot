﻿using home_energy_iot_monitoring.Domains;
using home_energy_iot_monitoring.Interfaces;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace home_energy_iot_monitoring.Infrasctructure
{
    public class ReportAPI : IReportAPI
    {
        private string urlAPISaveValue { get; set; }
        private bool useAPI { get; set; }
        private string tokenAPI { get; set; }

        public ReportAPI(IConfiguration configuration)
        {
            if (configuration["Environment"] == "prod") {
                urlAPISaveValue = configuration["APISaveValueProd"];
            }
            else
            {
                urlAPISaveValue = configuration["APISaveValueDev"];
            }
            tokenAPI = configuration["TokenApi"];
            useAPI = Convert.ToBoolean(configuration["flAPISaveValue"]);
            
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
                Console.WriteLine("[Erro API SaveEnergy] - "+ex.Message+" - ["+ex.StackTrace+"]");
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