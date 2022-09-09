using home_energy_iot_monitoring.Domains;
using home_energy_iot_monitoring.Hubs;
using home_energy_iot_monitoring.Interfaces;
using Microsoft.AspNetCore.SignalR;

using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace home_energy_iot_monitoring.Sockets
{
    public class DeviceSocketHolder : IDeviceSocketHolder
    {
        private readonly ILogger<DeviceSocketHolder> logger;
        private readonly ConcurrentDictionary<string, ClientDeviceConnection> clients = new();
        private readonly IHubContext<PanelsHub> _panelsHub;
        private readonly IHubContext<CostumersHub> _costumersHub;
        private readonly IReportAPI _reportAPI;

        public DeviceSocketHolder(ILogger<DeviceSocketHolder> logger, IHubContext<PanelsHub> panelsHub,IHubContext<CostumersHub> costumersHub, IReportAPI reportAPI)
        {
            this.logger = logger;
            _panelsHub = panelsHub;
            _costumersHub = costumersHub;
            _reportAPI = reportAPI;
        }

        public async Task AddAsync(HttpContext context)
        {
            WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();
            try
            {
                string idConnection = CreateId();
                if (clients.TryAdd(idConnection, new ClientDeviceConnection(webSocket, idConnection)))
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
                if (msgReceive.Contains("server>") || msgReceive.Contains("client>")) await HandleAction(msgReceive, idConnection);

                //Enviar para todos os clients
                foreach (var c in clients)
                {
                    if (!msgReceive.Contains("server>") && !msgReceive.Contains("client>"))
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

        public async Task SendActionToClient(string idConnection, string command)
        {
            var client = clients.First(x => x.Key == idConnection);
            byte[] bytes = Encoding.ASCII.GetBytes(command);

            string action = command.Split(">")[1];
            await this.HandleChangeState(idConnection, action);
            await client.Value.web_socket.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, CancellationToken.None);
        }

        private async Task HandleAction(string txtCommand, string idConnection)
        {
            ClientDeviceConnection device = clients.First(x => x.Key == idConnection).Value;
            string actionTo = txtCommand.Split(">")[0];

            //Ações para o servidor executar
            if (actionTo == "server")
            {
                string action = txtCommand.Split(">")[1];

                if (action == "energyvalue")
                {
                    string value = txtCommand.Split(">")[2];
                    await _reportAPI.SaveEnergyValue(value, device.device_id);
                    await SendEnergyValueToPanel(idConnection, value);
                    await SendEnergyValueToCostumer(idConnection, value);
                }

                if (action == "addiddevice")
                {
                    string value = txtCommand.Split(">")[2];
                    await Task.Run(async () =>
                    {
                        device.AddDeviceId(value);
                        await this.NotifyDeviceID(idConnection, value);
                    });
                }

                if (action == "confirmstop") {
                    await this.HandleChangeState(idConnection, "stopenergy");
                    await this.ConfirmButtonAction(idConnection,"stop");
                }

                if (action == "confirmcontinue")
                {
                    await this.HandleChangeState(idConnection, "continueenergy");
                    await this.ConfirmButtonAction(idConnection, "continue");
                }
            }

            //Ações para enviar ao client
            if(actionTo == "client")
            {
                string action = txtCommand.Split(">")[1];
                await this.HandleChangeState(idConnection, action);
                await this.SendActionToClient(idConnection, txtCommand);
            }
        }

        private async Task NotifyDeviceID(string idConnection, string deviceid)
        {
            ClientDeviceConnection device = clients.First(x => x.Key == idConnection).Value;
            await _panelsHub.Clients.All.SendAsync("updateDeviceId", idConnection, deviceid);
        }
        private async Task HandleChangeState(string idConnection, string action)
        {
            if (action == "stopenergy" || action == "continueenergy" || action == "timerenergy")
            {
                var client = clients.First(x => x.Key == idConnection);
                await Task.Run(() => client.Value.ChangeCurrentState(action));
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
            await _panelsHub.Clients.All.SendAsync("addNewDeviceCard", string.Format("{0}\n", JsonSerializer.Serialize(new { deviceid = device.device_id, connectionid = device.conn_id, dateconn = device.dateconn, currentstate = device.current_sate })));
        }

        private async Task SendEnergyValueToPanel(string idConnectionFrom, string valueEnergy)
        {
            await _panelsHub.Clients.All.SendAsync("receiveEnergyValue", idConnectionFrom, valueEnergy);
        }

        private async Task SendEnergyValueToCostumer(string IdConnectionFrom, string valueEnergy)
        {
            ClientDeviceConnection device = clients.First(x => x.Key == IdConnectionFrom).Value;
            if(device != null)
            {
                List<CostumerConnection> costumersConn = CostumersHandler.GetCostumerByDevice(device.device_id);

                if (costumersConn.Count > 0)
                {
                    foreach (var costumer in costumersConn)
                    {
                        await _costumersHub.Clients.Client(costumer.conn_id).SendAsync("receiveEnergyValue", valueEnergy);
                    }
                }
            }               
        }

        public async Task SendListClientsOn(string connectionId)
        {
            var listClients = clients.Select(c => new { deviceid = c.Value.device_id, connectionid = c.Value.conn_id, dateconn = c.Value.dateconn, state = c.Value.current_sate }).ToList();
            if (listClients.Any())
            {
                await _panelsHub.Clients.Client(connectionId).SendAsync("receiveListClients", string.Format("{0}\n", JsonSerializer.Serialize(listClients)));
            }
        }

        public ClientDeviceConnection GetDeviceOnlineInfo(string DeviceID)
        {
            return clients.First(x => x.Value.device_id == DeviceID).Value;
        }

        public async Task CostumerActionStopDevice(string DevideID)
        {
            string idConnection = clients.First(x => x.Value.device_id == DevideID).Key;
            string command = "client>stopenergy";
            await HandleAction(command, idConnection);
        }

        public async Task CostumerActionContinueDevice(string DevideID)
        {
            string idConnection = clients.First(x => x.Value.device_id == DevideID).Key;
            string command = "client>continueenergy";
            await HandleAction(command, idConnection);
        }

        private async Task ConfirmButtonAction(string IdConnectionFrom, string buttom)
        {
            await _panelsHub.Clients.All.SendAsync("disableButton", IdConnectionFrom,buttom);
        }
    }
}