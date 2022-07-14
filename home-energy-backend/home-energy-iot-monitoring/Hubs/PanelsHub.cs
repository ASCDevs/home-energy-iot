using home_energy_iot_monitoring.Sockets;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace home_energy_iot_monitoring.Hubs
{
    public class PanelsHub : Hub
    {
        private readonly IWebSocketHolder _webSocket;

        public PanelsHub(IWebSocketHolder webSocket)
        {
            _webSocket = webSocket;
        }

        public override async Task OnConnectedAsync()
        {
            PanelsHandler._connectedPanels.Add(Context.ConnectionId);
            Console.WriteLine("[Panel on] Painel "+Context.ConnectionId+" conectou");
            await Clients.All.SendAsync("updatePanelsOn", PanelsHandler._connectedPanels.Count());
            await Clients.All.SendAsync("updateClientsOn", _webSocket.CountClients());
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            PanelsHandler._connectedPanels.Remove(Context.ConnectionId);
            Console.WriteLine("[Panel off] Painel " + Context.ConnectionId + " desconectou");
            await Clients.All.SendAsync("updatePanelsOn", PanelsHandler._connectedPanels.Count());
            await Clients.All.SendAsync("updateClientsOn", _webSocket.CountClients());
            await base.OnConnectedAsync();
        }

        public async Task ServerUpdatesClientsPanel()
        {
            Console.WriteLine("[Panel hub] websocket atualizado enviado para painéis");
            await Clients.All.SendAsync("updateClientsOn", _webSocket.CountClients());
        }
    }
}
