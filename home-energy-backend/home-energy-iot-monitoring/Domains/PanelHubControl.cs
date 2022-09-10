using home_energy_iot_monitoring.Hubs;
using home_energy_iot_monitoring.Interfaces;
using Microsoft.AspNetCore.SignalR;
using System.Text.Json;

namespace home_energy_iot_monitoring.Domains
{
    public class PanelHubControl : IPanelHubControl
    {
        private readonly IHubContext<PanelsHub> _panelsHub;
        public PanelHubControl(IHubContext<PanelsHub> panelsHub)
        {
            _panelsHub = panelsHub;
        }
        public async Task PanelUIAddNewDeviceCard(KeyValuePair<string, ClientDeviceConnection> DeviceClient)
        {
            await this.PanelUISendPanelLog("Dispositivo conectou (Conn: " + DeviceClient.Key + ")");
            ClientDeviceConnection DeviceClientConn = DeviceClient.Value;
            
            var objToSend = new { 
                deviceid = DeviceClientConn.device_id, 
                connectionid = DeviceClientConn.conn_id, 
                dateconn = DeviceClientConn.dateconn, 
                currentstate = DeviceClientConn.current_sate 
            };
            
            await _panelsHub.Clients.All.SendAsync("addNewDeviceCard", string.Format("{0}\n", JsonSerializer.Serialize(objToSend)));
        }

        public async Task PanelUIDisableButtonConfirmed(string IdConnDeviceFrom, string Button)
        {
            await _panelsHub.Clients.All.SendAsync("disableButton", IdConnDeviceFrom, Button);
        }

        public async Task PanelUIReceiveEnergyValue(string IdConnDeviceFrom, string ValueEnergy)
        {
            await _panelsHub.Clients.All.SendAsync("receiveEnergyValue", IdConnDeviceFrom, ValueEnergy);
        }

        public async Task PanelUIReceiveListDevicesClients(string idConnPanel, List<ItemDeviceClient> ListDevices)
        {
            if (ListDevices.Any())
            {
                await _panelsHub.Clients.Client(idConnPanel).SendAsync("receiveListClients", string.Format("{0}\n", JsonSerializer.Serialize(ListDevices)));
            }
        }

        public async Task PanelUIRemoveDeviceCard(KeyValuePair<string, ClientDeviceConnection> DeviceClient)
        {
            string IdConnDeviceSocket = DeviceClient.Key;
            await this.PanelUISendPanelLog("Dispositivo ("+DeviceClient.Value.device_id+") desconectou");
            await _panelsHub.Clients.All.SendAsync("removeDeviceCard", IdConnDeviceSocket);
        }

        public async Task PanelUISendPanelLog(string Msg)
        {
            await _panelsHub.Clients.All.SendAsync("sendPanelLog", Msg + " (" + DateTime.Now + ")");
        }

        public async Task PanelUIUpdateDeviceClientsOn(int NumDevices)
        {
            await _panelsHub.Clients.All.SendAsync("updateClientsOn", NumDevices);
        }

        public async Task PanelUINotifyDeviceClientsCount(int DevicesOnlineCount)
        {
            await _panelsHub.Clients.All.SendAsync("updateClientsOn", DevicesOnlineCount);
        }

        public async Task PanelUINotifyDeviceIdUpdated(KeyValuePair<string, ClientDeviceConnection> DeviceClient)
        {
            await _panelsHub.Clients.All.SendAsync("updateDeviceId", DeviceClient.Key, DeviceClient.Value.device_id);
        }
    }
}
