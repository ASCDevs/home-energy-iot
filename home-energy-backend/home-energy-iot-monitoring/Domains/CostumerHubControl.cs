using home_energy_iot_monitoring.Hubs;
using home_energy_iot_monitoring.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace home_energy_iot_monitoring.Domains
{
    public class CostumerHubControl : ICostumerHubControl
    {
        private readonly ILogger<CostumerHubControl> _logger;
        private readonly IHubContext<CostumersHub> _costumersHub;

        public CostumerHubControl(IHubContext<CostumersHub> costumersHub, ILogger<CostumerHubControl> logger)
        {
            _logger = logger;
            _costumersHub = costumersHub;
        }

        public async Task CostumerUIDisableButtonConfirmed(string CostumerConnId, string Button)
        {
            try
            {
                if (Button == "stop")
                {
                    await _costumersHub.Clients.Client(CostumerConnId).SendAsync("ActionStopDevice");
                }

                if (Button == "continue")
                {
                    await _costumersHub.Clients.Client(CostumerConnId).SendAsync("ActionContinueDevice");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("[Erro CostumerHubControl] > Erro ao enviar confirmação de ação ("+Button+") para a interface do usuário ("+DateTime.Now+"), Erro: "+ex.Message);
            }
            
        }

        public async Task CostumerUIReceiveEnergyValue(string CostumerConnId, string ValueEnergy)
        {
            try
            {
                await _costumersHub.Clients.Client(CostumerConnId).SendAsync("receiveEnergyValue", ValueEnergy);
            }
            catch (Exception ex)
            {
                _logger.LogError("[Erro CostumerHubControl] > Erro ao informar valor de energia na interface do Usuário (" + DateTime.Now + "), Erro: " + ex.Message);
            }
            
        }

        public async Task CostumerUINotifyDisconnection(string CostumerConnId)
        {
            try
            {
                await _costumersHub.Clients.Client(CostumerConnId).SendAsync("DeviceIsDisconnected");
            }
            catch (Exception ex)
            {
                _logger.LogError("[Erro CostumerHubControl] > Erro ao notificar desconexão do dispositivo na interface do usuário (" + DateTime.Now + "), Erro: " + ex.Message);
            }
            
        }

        public async Task CostumerUINotifyConnection(string CostumerConnId)
        {
            try
            {
                await _costumersHub.Clients.Client(CostumerConnId).SendAsync("DeviceConnected");
            }
            catch (Exception ex)
            {
                _logger.LogError("[Erro CostumerHubControl] > Erro ao informar conexão de dispositivo na interface do usuário (" + DateTime.Now + "), Erro: " + ex.Message);
            }
        }

        public async Task CostumerUIReceiveIP(string CostumerConnId, string DeviceIP)
        {
            try
            {
                await _costumersHub.Clients.Client(CostumerConnId).SendAsync("ReceiveDeviceIP", DeviceIP);
            }
            catch (Exception ex)
            {
                _logger.LogError("[Erro CostumerHubControl] > Erro ao enviar IP cadastrado no dispositivo para a interface do usuário (" + DateTime.Now + "), Erro: " + ex.Message);
            }
        }

        public async Task CostumerUIRemoveIP(string CostumerConnId)
        {
            try
            {
                await _costumersHub.Clients.Client(CostumerConnId).SendAsync("RemoveDeviceIP");
            }
            catch (Exception ex)
            {
                _logger.LogError("[Erro CostumerHubControl] > Erro ao enviar ação para remover IP da interface do usuário (" + DateTime.Now + "), Erro: " + ex.Message);
            }
            
        }
    }
}
