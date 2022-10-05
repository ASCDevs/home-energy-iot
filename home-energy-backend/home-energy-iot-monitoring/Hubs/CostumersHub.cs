using home_energy_iot_monitoring.Domains;
using home_energy_iot_monitoring.Interfaces;
using Microsoft.AspNetCore.SignalR;
using System.Text.Json;

namespace home_energy_iot_monitoring.Hubs
{
    public class CostumersHub : Hub
    {
        private readonly IDeviceSocketHolder _webSocket;

        public CostumersHub(IDeviceSocketHolder webSocket)
        {
            _webSocket = webSocket;
        }

        public override async Task OnConnectedAsync() {
            
            CostumersHandler._connectedCostumers.Add(new CostumerConnection(Context.ConnectionId));
            Console.WriteLine("[Costumer on] Costumer "+Context.ConnectionId);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception) { 
            string connectionId = Context.ConnectionId;

            CostumerConnection costumerConnection = CostumersHandler._connectedCostumers.First(x => x.conn_id == connectionId);

            CostumersHandler._connectedCostumers.Remove(costumerConnection);
            Console.WriteLine("[Costumer off] Desconectou ("+connectionId+")");
            await base.OnDisconnectedAsync(exception);
        }

        public async Task GetInfoDeviceConnection(string IdConnFrom)
        {
            string connectionId = Context.ConnectionId;
            CostumerConnection costumerFrom = CostumersHandler.GetCostumerByConnection(IdConnFrom);
            ClientDeviceConnection deviceInfo = _webSocket.GetDeviceOnlineInfo(costumerFrom.device_id);
            await Clients.Client(costumerFrom.conn_id).SendAsync("receiveInfoDevice", string.Format("{0}\n", JsonSerializer.Serialize(new { deviceid = deviceInfo.device_id, }))); ;
        }

        public async Task ActionStopDevice()
        {
            string connectionId = Context.ConnectionId;
            CostumerConnection costumerFrom = CostumersHandler.GetCostumerByConnection(connectionId);
            await _webSocket.CostumerActionStopDevice(costumerFrom.device_id);
        }

        public async Task ActionContinueDevice()
        {
            string connectionId = Context.ConnectionId;
            CostumerConnection costumerFrom = CostumersHandler.GetCostumerByConnection(connectionId);
            await _webSocket.CostumerActionContinueDevice(costumerFrom.device_id);
        }

        public async Task CompleteInfo(string DeviceId, string costumer_id)
        {
            string connectionId = Context.ConnectionId;
            await Task.Run(() =>
            {
                CostumerConnection costumerFrom = CostumersHandler.GetCostumerByConnection(connectionId);
                costumerFrom.AddInfoCostumer(DeviceId, costumer_id);
            });

        }

    }
}
