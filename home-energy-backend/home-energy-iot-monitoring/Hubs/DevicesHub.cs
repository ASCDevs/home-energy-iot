using home_energy_iot_monitoring.Sockets;
using Microsoft.AspNetCore.SignalR;

namespace home_energy_iot_monitoring.Hubs
{
    public class DevicesHub : Hub
    {
        private readonly IWebSocketHolder _webSocket;

        public DevicesHub(IWebSocketHolder webSocket)
        {
            _webSocket = webSocket;
        }

        public override async Task OnConnectedAsync()
        {
            Console.WriteLine(">> Dispositivo " + Context.ConnectionId + " conectou as " + DateTime.Now);
            DevicesHandler._connectedDevices.Add(Context.ConnectionId);
            await this.NotifyConnection(Context.ConnectionId, "Conectou (" + DevicesHandler._connectedDevices.Count + " conectados)");
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            string connectionId = Context.ConnectionId;

            DevicesHandler._connectedDevices.Remove(Context.ConnectionId);
            Console.WriteLine(">> Dispositivo " + Context.ConnectionId + " desconectou. ");
            await this.NotifyConnection(Context.ConnectionId, "Desconectou (" + DevicesHandler._connectedDevices.Count + " conectados)");
            await base.OnDisconnectedAsync(exception);
        }

        public async Task NotifyConnection(string device, string message)
        {
            Console.WriteLine(">> " + device + " avisou conexão");
            await Clients.All.SendAsync("connectionsLog", device, message);
        }

        public async Task UpdateClientLists()
        {
            Console.WriteLine(">> Atualização as listas dos clientes");
            await Clients.All.SendAsync("updateList", DevicesHandler._connectedDevices);
        }

        public async Task MandarMensagem(string idConnect, string mensagem)
        {
            await _webSocket.SendActionToClient(idConnect, mensagem);
        }
    }
}