using home_energy_iot_monitoring.Hubs;
using home_energy_iot_monitoring.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace home_energy_iot_monitoring.Domains
{
    public class CostumerHubControl : ICostumerHubControl
    {
        private readonly IHubContext<CostumersHub> _costumersHub;

        public CostumerHubControl(IHubContext<CostumersHub> costumersHub)
        {
            _costumersHub = costumersHub;
        }

        public async Task CostumerUIDisableButtonConfirmed(string CostumerConnId, string Button)
        {    
            if(Button == "stop")
            {
                await _costumersHub.Clients.Client(CostumerConnId).SendAsync("ActionStopDevice");
            }
            
            if(Button == "continue")
            {
                await _costumersHub.Clients.Client(CostumerConnId).SendAsync("ActionContinueDevice");
            }
        }

        public async Task CostumerUIReceiveEnergyValue(string CostumerConnId, string ValueEnergy)
        {
            await _costumersHub.Clients.Client(CostumerConnId).SendAsync("receiveEnergyValue", ValueEnergy);
        }

        public async Task CostumerUINotifyDisconnection(string CostumerConnId)
        {
            await _costumersHub.Clients.Client(CostumerConnId).SendAsync("DeviceIsDisconnected");
        }

        public async Task CostumerUINotifyConnection(string CostumerConnId)
        {
            await _costumersHub.Clients.Client(CostumerConnId).SendAsync("DeviceConnected");
        }

        public async Task CostumerUIReceiveIP(string CostumerConnId, string DeviceIP)
        {
            await _costumersHub.Clients.Client(CostumerConnId).SendAsync("ReceiveDeviceIP", DeviceIP);
        }

        public async Task CostumerUIRemoveIP(string CostumerConnId)
        {
            await _costumersHub.Clients.Client(CostumerConnId).SendAsync("RemoveDeviceIP");
        }
    }
}
