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

        public async Task CostumerUIReceiveEnergyValue(string CostumerConnId, string ValueEnergy)
        {
            await _costumersHub.Clients.Client(CostumerConnId).SendAsync("receiveEnergyValue", ValueEnergy);
        }
    }
}
