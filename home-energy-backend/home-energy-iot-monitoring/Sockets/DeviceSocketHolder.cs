using home_energy_iot_monitoring.Domains;
using home_energy_iot_monitoring.Hubs;
using home_energy_iot_monitoring.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace home_energy_iot_monitoring.Sockets
{
    public class DeviceSocketHolder : IDeviceSocketHolder
    {
        private readonly ILogger<DeviceSocketHolder> _logger;
        private readonly ConcurrentDictionary<string, ClientDeviceConnection> clients = new();
        private readonly IReportAPI _reportAPI;
        private readonly IPanelHubControl _panelHubControl;
        private readonly ICostumerHubControl _costumerHubControl;

        public DeviceSocketHolder(ILogger<DeviceSocketHolder> logger, IReportAPI reportAPI, 
            IPanelHubControl panelHubControl, ICostumerHubControl costumerHubControl)
        {
            _logger = logger;
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
                    _logger.LogInformation("[Info] [socket device connected] - id-connection: "+idConnection+" ("+DateTime.Now+")");
                    await _panelHubControl.PanelUINotifyDeviceClientsCount(clients.Count());
                    await _panelHubControl.PanelUIAddNewDeviceCard(this.GetClientByIdConn(idConnection));
                    await this.NotifyCostumerConnection(idConnection);

                    await EchoAsycn(webSocket);
                }
            }
            catch (Exception ex)
            {
                var deviceClient = this.GetClientBySocket(webSocket);
                await _panelHubControl.PanelUIRemoveDeviceCard(deviceClient);
                await this.OnDeviceDisconnect(deviceClient.Key);
                clients.TryRemove(deviceClient);
                await _panelHubControl.PanelUINotifyDeviceClientsCount(clients.Count());
                
                _logger.LogError("[Erro] [socket device disconnected]> id-conn: "+deviceClient.Key+", device-id: "+deviceClient.Value.device_id+", device-ip: "+deviceClient.Value.device_ip+" ("+DateTime.Now+") Erro: "+ex.Message);
            }
        }

        public async Task SendListClientsOn(string PanelConnectionId)
        {
            await _panelHubControl.PanelUIReceiveListDevicesClients(PanelConnectionId, this.GetDevicesClientList());
        }

        public async Task SendActionToClient(string idConnection, string command)
        {
            try
            {
                var client = clients.First(x => x.Key == idConnection);
                byte[] bytes = Encoding.ASCII.GetBytes(command);

                string action = command.Split(">")[1];
                await this.HandleChangeState(idConnection, action);
                await client.Value.web_socket.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, CancellationToken.None);
            }
            catch (Exception ex)
            {
                _logger.LogError("[Erro socket dispositivo ] > Erro em enviar ação ("+command+") para o dispositivo cliente ("+DateTime.Now+"), Erro: "+ex.Message);
            }
            
        }

        public ClientDeviceConnection GetDeviceOnlineInfo(string DeviceID)
        {
            return clients.FirstOrDefault(x => x.Value.device_id == DeviceID).Value;
        }

        public async Task CostumerActionStopDevice(string DeviceID)
        {
            try
            {
                string idConnection = clients.FirstOrDefault(x => x.Value.device_id == DeviceID).Key;
                if (idConnection != null)
                {
                    string command = "client>stopenergy";
                    await HandleAction(command, idConnection);
                }
                else
                {
                    throw new Exception("Dispositivo "+DeviceID+" não foi encontrado");
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("[Erro socket dispositivo ] > Erro em executar método CostumerActionStopDevice para o dispositivo "+ DeviceID + " (" + DateTime.Now + "), Erro: " + ex.Message);
            }
        }

        public async Task CostumerActionContinueDevice(string DeviceID)
        {
            try
            {
                string idConnection = clients.FirstOrDefault(x => x.Value.device_id == DeviceID).Key;

                if (idConnection != null)
                {
                    string command = "client>continueenergy";
                    await HandleAction(command, idConnection);
                }
                else
                {
                    throw new Exception("Dispositivo " + DeviceID + " não foi encontrado");
                }
            }
            catch(Exception ex)
            {
                _logger.LogError("[Erro socket dispositivo ] > Erro em executar método CostumerActionContinueDevice para o dispositivo " + DeviceID + " (" + DateTime.Now + "), Erro: " + ex.Message);
            }
            
        }

        public int CountClients()
        {
            return clients.Count();
        }

        private async Task EchoAsycn(WebSocket webSocket)
        {
            try
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
                        await this.OnDeviceDisconnect(deviceClient.Key);
                        if (clients.TryRemove(clients.First(w => w.Value.web_socket == webSocket)))
                        {
                            _logger.LogInformation("[Info] [socket device disconnected]> id-conn: " + deviceClient.Key + ", device-id: " + deviceClient.Value.device_id + ", device-ip: " + deviceClient.Value.device_ip + " (" + DateTime.Now + ")");

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
            catch (Exception ex)
            {
                _logger.LogCritical("[Erro Critico socket dispositivo] > Erro no método EchoAsycn ("+DateTime.Now+"), Erro: "+ex.Message);
            }
        }

        private async Task HandleAction(string txtCommand, string idConnection)
        {
            try
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
                                _logger.LogInformation("[Info socket dispositivo] > Dispositivo ("+value+") informou o ID ("+DateTime.Now+")");
                            }
                            finally
                            {
                                await _panelHubControl.PanelUINotifyDeviceIdUpdated(deviceClient);
                                await this.NotifyCostumerConnection(idConnection);
                            }
                        });
                    }

                    if (action == "addipdevice")
                    {
                        string value = txtCommand.Split(">")[2];
                        await Task.Run(async () =>
                        {
                            try
                            {
                                device.AddDeviceIp(value);
                                _logger.LogInformation("[Info socket dispositivo] > Dispositivo informou o IP ("+value+"), (" + DateTime.Now + ")");
                            }
                            finally
                            {
                                await _panelHubControl.PanelUINotifyDeviceIpUpdated(deviceClient);
                                await this.NotifyCostumerConnection(idConnection);
                                await this.NotifyCostumerDeviceIP(idConnection);

                            }
                        });
                    }

                    if (action == "confirmstop")
                    {
                        await this.HandleChangeState(idConnection, "stopenergy");
                        await this._panelHubControl.PanelUIDisableButtonConfirmed(idConnection, "stop");
                        await SendCostumerConfirmationAction(idConnection, "stop");
                    }

                    if (action == "confirmcontinue")
                    {
                        await this.HandleChangeState(idConnection, "continueenergy");
                        await this._panelHubControl.PanelUIDisableButtonConfirmed(idConnection, "continue");
                        await SendCostumerConfirmationAction(idConnection, "continue");
                    }
                }//Ações para enviar ao client
                else if (actionTo == "client")
                {
                    string action = txtCommand.Split(">")[1];
                    await this.HandleChangeState(idConnection, action);
                    await this.SendActionToClient(idConnection, txtCommand);
                }
                else
                {
                    _logger.LogWarning("[Warn socket dispositivo] > Comando ("+txtCommand+ ") não válido para o serviço de monitoramento ("+DateTime.Now+"), id-conn: "+ idConnection);
                }

            }
            catch (Exception ex)
            {
                _logger.LogCritical("[Erro Critico socket dispositivo] > Erro ao executar método HandleAction com o comando ("+ txtCommand + "), (" + DateTime.Now+"), id-conn: "+idConnection+" ,Erro: "+ex.Message);
            }
            
        }

        private async Task HandleChangeState(string idConnection, string action)
        {
            try
            {
                if (action == "stopenergy" || action == "continueenergy" || action == "timerenergy")
                {
                    var deviceClient = clients.First(x => x.Key == idConnection);
                    await Task.Run(() => deviceClient.Value.ChangeCurrentState(action));
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical("[Erro Critico socket dispositivo] > Erro ao lidar com a mudança de estado ("+action+") do dispositivo ("+DateTime.Now+"), id-conn: "+idConnection+", Erro: "+ex.Message);
            }
            
        }

        private async Task SendEnergyValueToCostumer(string IdConnectionFrom, string valueEnergy)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError("[Erro socket dispositivo] > Erro em enviar o valor para interface do usuário ("+DateTime.Now+"), id-conn: "+IdConnectionFrom+", Erro: "+ex.Message);
            }
        }

        private async Task SendCostumerConfirmationAction(string idConnection, string action)
        {
            try
            {
                ClientDeviceConnection device = this.GetClientByIdConn(idConnection).Value;
                if (device != null)
                {
                    List<CostumerConnection> costumersConn = CostumersHandler.GetCostumerByDevice(device.device_id);

                    if (costumersConn.Count > 0)
                    {
                        foreach (var costumer in costumersConn)
                        {
                            await _costumerHubControl.CostumerUIDisableButtonConfirmed(costumer.conn_id, action);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("[Erro socket dispositivo] > Erro em enviar confirmação de ação para interface do usuário ("+DateTime.Now+"), id-conn: "+idConnection+", Erro: "+ex.Message);
            }
        }

        private async Task NotifyCostumerConnection(string idConnection)
        {
            try
            {
                ClientDeviceConnection device = this.GetClientByIdConn(idConnection).Value;
                if (device != null)
                {
                    List<CostumerConnection> costumersConn = CostumersHandler.GetCostumerByDevice(device.device_id);

                    if (costumersConn.Count > 0)
                    {
                        foreach (var costumer in costumersConn)
                        {
                            await _costumerHubControl.CostumerUINotifyConnection(costumer.conn_id);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("[Erro socket dispositivo] > Erro em notificar conexão de dispositivo para interface de usuário ("+DateTime.Now+"), id-conn: "+idConnection+", Erro: "+ex.Message);
            }
            
        }

        private async Task NotifyCostumerDisconnection(string idConnection)
        {
            try
            {
                ClientDeviceConnection device = this.GetClientByIdConn(idConnection).Value;
                if (device != null)
                {
                    List<CostumerConnection> costumersConn = CostumersHandler.GetCostumerByDevice(device.device_id);

                    if (costumersConn.Count > 0)
                    {
                        foreach (var costumer in costumersConn)
                        {
                            await _costumerHubControl.CostumerUINotifyDisconnection(costumer.conn_id);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("[Erro socket dispositivo] > erro em notificar desconexão de dispostivo para interface de usuário ("+DateTime.Now+"), id-conn: "+idConnection+", Erro: "+ex.Message);
            }
        }

        private async Task NotifyCostumerDeviceIP(string idConnection)
        {
            try {
                ClientDeviceConnection device = this.GetClientByIdConn(idConnection).Value;
                if (device != null)
                {
                    List<CostumerConnection> costumersConn = CostumersHandler.GetCostumerByDevice(device.device_id);

                    if (costumersConn.Count > 0)
                    {
                        foreach (var costumer in costumersConn)
                        {
                            await _costumerHubControl.CostumerUIReceiveIP(costumer.conn_id, device.device_ip);
                        }
                    }
                }
            }catch(Exception ex)
            {
                _logger.LogError("[Erro socket dispositivo] > Erro em notificar IP do dispositivo para interface do usuário ("+DateTime.Now+"), id-conn: "+idConnection+", Erro: "+ex.Message);
            }
            
        }

        private async Task CostumerRemoveDeviceIP(string idConnection)
        {
            try
            {
                ClientDeviceConnection device = this.GetClientByIdConn(idConnection).Value;
                if (device != null)
                {
                    List<CostumerConnection> costumersConn = CostumersHandler.GetCostumerByDevice(device.device_id);

                    if (costumersConn.Count > 0)
                    {
                        foreach (var costumer in costumersConn)
                        {
                            await _costumerHubControl.CostumerUIRemoveIP(costumer.conn_id);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("[Erro socket dispositivo] > Erro em remover IP de dispositivo da interface do usuário ("+DateTime.Now+"), id-conn: "+idConnection+", Erro: "+ex.Message);
            }
        }

        private async Task OnDeviceDisconnect(string idConnection)
        {
            try
            {
                if (idConnection == null) throw new Exception("Não foi possível realizar a desconexão do dispostivo pois o idConnection é nulo");
                
                await this.NotifyCostumerDisconnection(idConnection);
                await this.CostumerRemoveDeviceIP(idConnection);
                await _panelHubControl.PanelUIReceiveEnergyValue(idConnection, "0");
                await SendEnergyValueToCostumer(idConnection, "0");
            }
            catch (Exception ex)
            {
                _logger.LogError("[Erro socket dispositivo] > Erro em realizar a desconexão do dispositivo ("+DateTime.Now+"), id-conn: "+idConnection+", Erro: "+ex.Message);
            }
        }

        private List<ItemDeviceClient> GetDevicesClientList()
        {
            return clients.Select(c => new ItemDeviceClient { deviceid = c.Value.device_id, connectionid = c.Value.conn_id, dateconn = c.Value.dateconn, state = c.Value.current_sate }).ToList();
        }

        private KeyValuePair<string, ClientDeviceConnection> GetClientByIdConn(string IdConn)
        {
            return clients.FirstOrDefault(x => x.Key == IdConn);
        }

        private KeyValuePair<string, ClientDeviceConnection> GetClientBySocket(WebSocket webSocket)
        {
            return clients.FirstOrDefault(w => w.Value.web_socket == webSocket);
        }
    }
}