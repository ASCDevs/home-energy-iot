using home_energy_iot_monitoring.Hubs;
using Microsoft.AspNetCore.SignalR;

using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace home_energy_iot_monitoring.Sockets
{
    public class WebSocketHolder : IWebSocketHolder
    {
        private readonly ILogger<WebSocketHolder> logger;
        private readonly ConcurrentDictionary<string, ClientDeviceConnection> clients = new();
        private readonly IHubContext<PanelsHub> _panelsHub;

        public WebSocketHolder(ILogger<WebSocketHolder> logger, IHubContext<PanelsHub> panelsHub)
        {
            this.logger = logger;
            _panelsHub = panelsHub;
        }

        public async Task AddAsync(HttpContext context)
        {
            WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();
            try
            {
                string idConnection = CreateId();
                if (clients.TryAdd(idConnection, new ClientDeviceConnection(webSocket, "", idConnection)))
                {
                    Console.WriteLine("[connected] id-connection: " + idConnection);
                    await NotifyLogPanel("Device conectou");
                    await NotifyClientsCount();
                    await AddNewDeviceInPanel(idConnection);

                    await EchoAsycn(webSocket);
                }
            }
            catch (Exception ex)
            {
                var client = clients.First(w => w.Value.web_socket == webSocket);
                Console.WriteLine("[disconnected] id-connection: " + client.Key);
                await NotifyLogPanel("Device desconectou");
                await _panelsHub.Clients.All.SendAsync("removeDeviceCard", client.Key);
                clients.TryRemove(client);
                await NotifyClientsCount();
                Console.WriteLine("[ERRO] > " + ex.Message + " || [[" + ex.StackTrace + "]]");
            }
        }

        private string CreateId()
        {
            return Guid.NewGuid().ToString();
        }

        private async Task EchoAsycn(WebSocket webSocket)
        {
            //Para enviar dados
            byte[] buffer = new byte[1024 * 4];
            //Receber e enviar mensagens até as conexões serem fechadas.
            while (true)
            {
                WebSocketReceiveResult result = await webSocket.ReceiveAsync(
                    new ArraySegment<byte>(buffer), CancellationToken.None);
                if (result.CloseStatus.HasValue)
                {
                    await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
                    var client = clients.First(x => x.Value.web_socket == webSocket);
                    if (clients.TryRemove(clients.First(w => w.Value.web_socket == webSocket)))
                    {
                        Console.WriteLine("[disconnected] id-connection: " + client.Key);
                        await NotifyLogPanel("Device desconectou");
                        await NotifyClientsCount();
                        await _panelsHub.Clients.All.SendAsync("removeDeviceCard", client.Key);
                    }

                    webSocket.Dispose();
                    break;
                }

                string? msgReceive = Encoding.UTF8.GetString(new ArraySegment<byte>(buffer, 0, result.Count));
                string idConnection = clients.First(x => x.Value.web_socket == webSocket).Key;
                //if (msgReceive.Contains("server>")) Console.WriteLine("[From " + idConnection + "]" + msgReceive);
                if (msgReceive.Contains("server>")) await HandleAction(msgReceive, idConnection);

                //Enviar para todos os clients
                foreach (var c in clients)
                {
                    if (!msgReceive.Contains("server>"))
                    {
                        await c.Value.web_socket.SendAsync(new ArraySegment<byte>(buffer, 0, result.Count), result.MessageType, result.EndOfMessage, CancellationToken.None);
                    }
                }
            }
        }

        public int CountClients()
        {
            return clients.Count();
        }

        public async Task SendActionToClient(string idConnection, string Action)
        {
            var client = clients.First(x => x.Key == idConnection);
            byte[] bytes = Encoding.ASCII.GetBytes(Action);

            await client.Value.web_socket.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, CancellationToken.None);
        }

        private async Task HandleAction(string txtCommand, string idConnection)
        {
            //{para-quem}>{acao}>{valor}
            //ex: server>updateid>1239
            //{para-quem}>{acao}>{idclient,acao}
            //ex: client>sendaction>122,desligarsensor

            string para_quem = txtCommand.Split(">")[0];
            string acao = txtCommand.Split(">")[1];
            string valor = txtCommand.Split(">")[2];

            var um = 1;
            if (acao == "energyvalue")
            {
                await SendEneryValueToPanel(idConnection, valor);
            }
            else if (acao == "keepalive")
            {
                await PingHoldConnection(idConnection);
            }
        }

        private async Task NotifyClientsCount()
        {
            await _panelsHub.Clients.All.SendAsync("updateClientsOn", clients.Count());
        }

        private async Task NotifyLogPanel(string msg)
        {
            await _panelsHub.Clients.All.SendAsync("sendPanelLog", msg + " (" + DateTime.Now + ")");
        }

        private async Task AddNewDeviceInPanel(string connId)
        {
            var device = clients.First(x => x.Key == connId).Value;
            await _panelsHub.Clients.All.SendAsync("addNewDeviceCard", string.Format("{0}\n", JsonSerializer.Serialize(new { deviceid = device.device_id, connectionid = device.conn_id, dateconn = device.dateconn })));
        }

        private async Task SendEneryValueToPanel(string idConnectionFrom, string valueEnergy)
        {
            await _panelsHub.Clients.All.SendAsync("receiveEnergyValue", idConnectionFrom, valueEnergy);
        }

        public async Task SendListClientsOn(string connectionId)
        {
            var listClients = clients.Select(c => new { deviceid = c.Value.device_id, connectionid = c.Value.conn_id, dateconn = c.Value.dateconn }).ToList();
            if (listClients.Any())
            {
                await _panelsHub.Clients.Client(connectionId).SendAsync("receiveListClients", string.Format("{0}\n", JsonSerializer.Serialize(listClients)));
            }
        }

        public async Task PingHoldConnection(string idConnection)
        {
            await SendActionToClient(idConnection, "pong");
        }
    }
}