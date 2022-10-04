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
                await _costumersHub.Clients.Client(CostumerConnId).SendAsync("stopConfirmed");
            }

            if(Button == "continue")
            {
                await _costumersHub.Clients.Client(CostumerConnId).SendAsync("continueConfirmed");
            }
        }

        public async Task CostumerUIReceiveEnergyValue(string CostumerConnId, string ValueEnergy)
        {
            await _costumersHub.Clients.Client(CostumerConnId).SendAsync("receiveEnergyValue", ValueEnergy);
        }
    }
}
