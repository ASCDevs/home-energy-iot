using home_energy_iot_monitoring.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace home_energy_iot_monitoring.Hubs
{
    public class PanelsHub : Hub
    {
        private readonly ILogger<PanelsHub> _logger;
        private readonly IDeviceSocketHolder _webSocket;

        public PanelsHub(IDeviceSocketHolder webSocket, ILogger<PanelsHub> logger)
        {
            _logger = logger;
            _webSocket = webSocket;
        }

        public override async Task OnConnectedAsync()
        {
            try
            {
                PanelsHandler._connectedPanels.Add(Context.ConnectionId);
                Console.WriteLine("[Panel on] Painel " + Context.ConnectionId + " conectou (" + DateTime.Now + ")");
                await Clients.All.SendAsync("updatePanelsOn", PanelsHandler._connectedPanels.Count());
                await Clients.All.SendAsync("updateQtdUsersOnline", CostumersHandler._connectedCostumers.Count());
                await Clients.All.SendAsync("updateClientsOn", _webSocket.CountClients());
                await GetListClientsOn();
                await Clients.AllExcept(Context.ConnectionId).SendAsync("sendPanelLog", "Um painel conectou (" + DateTime.Now + ")");
                _logger.LogInformation("[Info PanelHub] Painel de monitoramento foi conectado ("+DateTime.Now+"), conn-id: " + Context.ConnectionId + ")");

                await base.OnConnectedAsync();
            }catch (Exception ex)
            {
                _logger.LogError("[Erro PanelHub] > Erro ao receber conexão no Hub (" + DateTime.Now + "), Erro: " + ex.Message);
            }


        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            try
            {
                PanelsHandler._connectedPanels.Remove(Context.ConnectionId);
                Console.WriteLine("[Panel off] Painel " + Context.ConnectionId + " desconectou");
                await Clients.All.SendAsync("updatePanelsOn", PanelsHandler._connectedPanels.Count());
                await Clients.All.SendAsync("updateClientsOn", _webSocket.CountClients());
                await Clients.AllExcept(Context.ConnectionId).SendAsync("sendPanelLog", "Um painel desconectou (" + DateTime.Now + ")");
                await base.OnConnectedAsync();

                _logger.LogInformation("[Info PanelHub] Painel de monitoramento foi desconectado (" + DateTime.Now + "), conn-id: " + Context.ConnectionId + ")");
            }
            catch (Exception ex)
            {
                _logger.LogError("[Erro PanelHub] > Erro em desconectar um Painel de monitoramento ("+DateTime.Now+"), conn-id"+Context.ConnectionId+", Erro: "+ex.Message);
            }
            
        }

        public async Task ServerUpdatesClientsPanel()
        {
            try
            {
                Console.WriteLine("[Panel hub] websocket atualizado enviado para painéis");
                await Clients.All.SendAsync("updateClientsOn", _webSocket.CountClients());
            }
            catch (Exception ex)
            {
                _logger.LogError("[Erro PanelHub] > Erro ao fazer update da contagem de dispositivos clientes ("+DateTime.Now+"), Erro: "+ex.Message);
            }
            
        }

        public async Task GetListClientsOn()
        {
            await _webSocket.SendListClientsOn(Context.ConnectionId);
            await Clients.All.SendAsync("updateClientsOn", _webSocket.CountClients());
        }

        public async Task SendActionToClient(string connId, string txtCommand)
        {
            await _webSocket.SendActionToClient(connId, txtCommand);
        }
    }
}