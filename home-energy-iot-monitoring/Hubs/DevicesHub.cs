using Microsoft.AspNetCore.SignalR;

namespace home_energy_iot_monitoring.Hubs
{
    public class DevicesHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            await SendAdvice("","Dispositivo conectado!");
            await base.OnConnectedAsync();
        }

        public async Task SendAdvice(string device, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage",device,message);
        }
    }
}
