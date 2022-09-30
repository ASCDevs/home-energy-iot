using home_energy_iot_monitoring.Domains;
using home_energy_iot_monitoring.Hubs;
using home_energy_iot_monitoring.Interfaces;
using Microsoft.AspNetCore.SignalR;

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace home_energy_iot_monitoring.Sockets
{
    public class DeviceSocketHolder : IDeviceSocketHolder
    {
        private readonly ILogger<DeviceSocketHolder> logger;
        private readonly ConcurrentDictionary<string, ClientDeviceConnection> clients = new();
        private readonly IReportAPI _reportAPI;
        private readonly IPanelHubControl _panelHubControl;
        private readonly ICostumerHubControl _costumerHubControl;

        public DeviceSocketHolder(ILogger<DeviceSocketHolder> logger, IReportAPI reportAPI, 
            IPanelHubControl panelHubControl, ICostumerHubControl costumerHubControl)
        {
            this.logger = logger;
            _reportAPI = reportAPI;
            _panelHubControl = panelHubControl;
            _costumerHubControl = costumerHubControl;
        }

        public async Task AddAsync(HttpContext context)
        {
            WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();
            try
            {
                string idConnection = Guid.NewGuid().ToString();
                if (clients.TryAdd(idConnection, new ClientDeviceConnection(webSocket, idConnection)))
                {
                    Console.WriteLine("[connected] id-connection: " + idConnection);
                    
                    await _panelHubControl.PanelUINotifyDeviceClientsCount(clients.Count());
                    await _panelHubControl.PanelUIAddNewDeviceCard(this.GetClientByIdConn(idConnection));

                    await EchoAsycn(webSocket);
                }
            }
            catch (Exception ex)
            {
                var deviceClient = this.GetClientBySocket(webSocket);
                Console.WriteLine("[disconnected] ("+deviceClient.Value.device_id+") id-connection: " + deviceClient.Key);
                await _panelHubControl.PanelUIRemoveDeviceCard(deviceClient);
                clients.TryRemove(deviceClient);
                await _panelHubControl.PanelUINotifyDeviceClientsCount(clients.Count());
                
                Console.WriteLine("[ERRO] > " + ex.Message + " || [[" + ex.StackTrace + "]]");
            }
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
                    var deviceClient = clients.First(x => x.Value.web_socket == webSocket);
                    if (clients.TryRemove(clients.First(w => w.Value.web_socket == webSocket)))
                    {
                        Console.WriteLine("[disconnected] ("+ deviceClient.Value.device_id+ ") id-connection: " + deviceClient.Key);

                        await _panelHubControl.PanelUINotifyDeviceClientsCount(clients.Count());
                        await _panelHubControl.PanelUIRemoveDeviceCard(deviceClient);
                    }

                    webSocket.Dispose();
                    break;
                }

                string? msgReceive = Encoding.UTF8.GetString(new ArraySegment<byte>(buffer, 0, result.Count));
                string idConnection = clients.First(x => x.Value.web_socket == webSocket).Key;
                if (msgReceive.Contains("server>") || msgReceive.Contains("client>")) await HandleAction(msgReceive, idConnection);

                //Enviar para todos os clients
                if (!msgReceive.Contains("server>") && !msgReceive.Contains("client>"))
                {
                    foreach (var c in clients)
                    {
                        await c.Value.web_socket.SendAsync(new ArraySegment<byte>(buffer, 0, result.Count), result.MessageType, result.EndOfMessage, CancellationToken.None);
                    }
                }
            }
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
            var deviceClient = this.GetClientByIdConn(idConnection);
            ClientDeviceConnection device = deviceClient.Value;
            string actionTo = txtCommand.Split(">")[0];

            //Ações para o servidor executar
            if (actionTo == "server")
            {
                string action = txtCommand.Split(">")[1];

                if (action == "energyvalue")
                {
                    string value = txtCommand.Split(">")[2];
                    await _reportAPI.SaveEnergyValue(value, device.device_id);
                    await _panelHubControl.PanelUIReceiveEnergyValue(idConnection, value);
                    await _costumerHubControl.CostumerUIReceiveEnergyValue(idConnection,value);
                    await SendEnergyValueToCostumer(idConnection, value);
                }

                if (action == "addiddevice")
                {
                    string value = txtCommand.Split(">")[2];
                    await Task.Run(async () =>
                    {
                        try
                        {
                            device.AddDeviceId(value);
                        }
                        finally
                        {
                            await _panelHubControl.PanelUINotifyDeviceIdUpdated(deviceClient);
                        }
                    });
                }

                if (action == "confirmstop") {
                    await this.HandleChangeState(idConnection, "stopenergy");
                    await this._panelHubControl.PanelUIDisableButtonConfirmed(idConnection, "stop");
                }

                if (action == "confirmcontinue")
                {
                    await this.HandleChangeState(idConnection, "continueenergy");
                    await this._panelHubControl.PanelUIDisableButtonConfirmed(idConnection, "continue");
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

        private async Task HandleChangeState(string idConnection, string action)
        {
            if (action == "stopenergy" || action == "continueenergy" || action == "timerenergy")
            {
                var deviceClient = clients.First(x => x.Key == idConnection);
                await Task.Run(() => deviceClient.Value.ChangeCurrentState(action));
            }
        }

        private async Task SendEnergyValueToCostumer(string IdConnectionFrom, string valueEnergy)
        {
            ClientDeviceConnection device = this.GetClientByIdConn(IdConnectionFrom).Value;
            if (device != null)
            {
                List<CostumerConnection> costumersConn = CostumersHandler.GetCostumerByDevice(device.device_id);

                if (costumersConn.Count > 0)
                {
                    foreach (var costumer in costumersConn)
                    {
                        await _costumerHubControl.CostumerUIReceiveEnergyValue(costumer.conn_id, valueEnergy);
                    }
                }
            }               
        }

        public async Task SendListClientsOn(string PanelConnectionId)
        {
            await _panelHubControl.PanelUIReceiveListDevicesClients(PanelConnectionId, this.GetDevicesClientList());
        }

        private List<ItemDeviceClient> GetDevicesClientList()
        {
            return clients.Select(c => new ItemDeviceClient { deviceid = c.Value.device_id, connectionid = c.Value.conn_id, dateconn = c.Value.dateconn, state = c.Value.current_sate }).ToList();
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

        private KeyValuePair<string, ClientDeviceConnection> GetClientByIdConn(string IdConn)
        {
            return clients.First(x => x.Key == IdConn);
        }

        private KeyValuePair<string, ClientDeviceConnection> GetClientBySocket(WebSocket webSocket)
        {
            return clients.First(w => w.Value.web_socket == webSocket);
        }

        public int CountClients()
        {
            return clients.Count();
        }
    }
}