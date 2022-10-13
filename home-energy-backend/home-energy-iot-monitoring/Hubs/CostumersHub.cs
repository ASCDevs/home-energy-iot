using home_energy_iot_monitoring.Domains;
using home_energy_iot_monitoring.Interfaces;
using Microsoft.AspNetCore.SignalR;
using System.Text.Json;

namespace home_energy_iot_monitoring.Hubs
{
    public class CostumersHub : Hub
    {
        private readonly ILogger<CostumersHub> _logger;
        private readonly IDeviceSocketHolder _webSocket;

        public CostumersHub(IDeviceSocketHolder webSocket, ILogger<CostumersHub> logger)
        {
            _logger = logger;
            _webSocket = webSocket;
        }

        public override async Task OnConnectedAsync() {

            try
            {
                CostumersHandler._connectedCostumers.Add(new CostumerConnection(Context.ConnectionId));
                _webSocket.NotifyPanelUsersOnline(CostumersHandler._connectedCostumers.Count());
                _logger.LogInformation("[Info CostumerHub] > Hub interface de usuário conectou (" + DateTime.Now + "), id-conn: "+Context.ConnectionId);
                await base.OnConnectedAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("[Erro CostumersHub] > Erro ao receber conexão do Hub da interface do usuário ("+DateTime.Now+"), Erro: "+ex.Message);
            }
        }

        public override async Task OnDisconnectedAsync(Exception? exception) {

            try
            {
                string connectionId = Context.ConnectionId;

                CostumerConnection costumerConnection = CostumersHandler._connectedCostumers.First(x => x.conn_id == connectionId);
                _logger.LogInformation("[Info CostumerHub] > Hub interface de usuário desconectou (" + DateTime.Now + "), device-id: " + costumerConnection.device_id);
                CostumersHandler._connectedCostumers.Remove(costumerConnection);
                _webSocket.NotifyPanelUsersOnline(CostumersHandler._connectedCostumers.Count());
                await base.OnDisconnectedAsync(exception);
            }
            catch (Exception ex)
            {
                _logger.LogError("[Erro CostumersHub] > Erro na desconexão do Hub da interface do usuário (" + DateTime.Now + "), Erro: " + ex.Message);
            }
            
        }

        public async Task GetInfoDeviceConnection(string IdConnFrom)
        {
            try
            {
                string connectionId = Context.ConnectionId;
                CostumerConnection costumerFrom = CostumersHandler.GetCostumerByConnection(IdConnFrom);
                ClientDeviceConnection deviceInfo = _webSocket.GetDeviceOnlineInfo(costumerFrom.device_id);
                await Clients.Client(costumerFrom.conn_id).SendAsync("receiveInfoDevice", string.Format("{0}\n", JsonSerializer.Serialize(new { deviceid = deviceInfo.device_id, })));
            }
            catch (Exception ex)
            {
                _logger.LogError("[Erro CostumersHub] > Erro ao fornecer informações do dispositivo para a interface do usuário (" + DateTime.Now + "), Erro: " + ex.Message);
            }
        }

        public async Task ActionStopDevice()
        {
            try
            {
                string connectionId = Context.ConnectionId;
                CostumerConnection costumerFrom = CostumersHandler.GetCostumerByConnection(connectionId);
                await _webSocket.CostumerActionStopDevice(costumerFrom.device_id);
            }
            catch (Exception ex)
            {
                _logger.LogError("[Erro CostumersHub] > Erro ao solicitar ação de parar energia do dispositivo para o socket do dispositivo (" + DateTime.Now + "), Erro: " + ex.Message);
            }
        }

        public async Task ActionContinueDevice()
        {
            try
            {
                string connectionId = Context.ConnectionId;
                CostumerConnection costumerFrom = CostumersHandler.GetCostumerByConnection(connectionId);
                await _webSocket.CostumerActionContinueDevice(costumerFrom.device_id);
            }
            catch (Exception ex)
            {
                _logger.LogError("[Erro CostumersHub] >  Erro ao solicitar ação de continuar energia do dispositivo para o socket do dispositivo (" + DateTime.Now + "), Erro: " + ex.Message);
            }
            
        }

        public async Task CompleteInfo(string DeviceId, string costumer_id)
        {
            try
            {
                string connectionId = Context.ConnectionId;
                await Task.Run(() =>
                {
                    CostumerConnection costumerFrom = CostumersHandler.GetCostumerByConnection(connectionId);
                    costumerFrom.AddInfoCostumer(DeviceId, costumer_id);
                });
            }
            catch (Exception ex)
            {
                _logger.LogError("[Erro CostumersHub] > Erro ao realizar ação solicitada da interface do usuário em completar informações (" + DateTime.Now + "), Erro: " + ex.Message);
            }
            
        }

        public async Task GetDeviceIP()
        {
            try
            {
                string connectionId = Context.ConnectionId;
                CostumerConnection costumerFrom = CostumersHandler.GetCostumerByConnection(connectionId);
                ClientDeviceConnection deviceInfo = _webSocket.GetDeviceOnlineInfo(costumerFrom.device_id);
                if (deviceInfo == null)
                {
                    await Clients.Client(connectionId).SendAsync("DeviceIsDisconnected");
                }
                else
                {
                    await Clients.Client(connectionId).SendAsync("DeviceConnected");
                    if (deviceInfo.device_ip != null)
                    {
                        await Clients.Client(connectionId).SendAsync("ReceiveDeviceIP", deviceInfo.device_ip);
                    }
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("[Erro CostumersHub] > Erro em fornecer IP do dispositivo solicitado pela interface do usuário (" + DateTime.Now + "), Erro: " + ex.Message);
            }
            
        }

        public async Task GetCurrentState()
        {
            try
            {
                string connectionId = Context.ConnectionId;
                CostumerConnection costumerFrom = CostumersHandler.GetCostumerByConnection(connectionId);
                ClientDeviceConnection deviceInfo = _webSocket.GetDeviceOnlineInfo(costumerFrom.device_id);
                if (deviceInfo == null)
                {
                    await Clients.Client(connectionId).SendAsync("DeviceIsDisconnected");
                }
                else
                {
                    if (deviceInfo.current_sate)
                    {
                        await Clients.Client(connectionId).SendAsync("ReceiveCurrentState", "ligado");
                    }

                    if (!deviceInfo.current_sate)
                    {
                        await Clients.Client(connectionId).SendAsync("ReceiveCurrentState", "interrompido");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("[Erro CostumersHub] > Erro em fornecer estado atual do fluco de energia do dispostivo para a interface do usuário (" + DateTime.Now + "), Erro: " + ex.Message);
            }
        }

    }
}
