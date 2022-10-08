using home_energy_iot_monitoring.Hubs;
using home_energy_iot_monitoring.Interfaces;
using Microsoft.AspNetCore.SignalR;
using System.Text.Json;

namespace home_energy_iot_monitoring.Domains
{
    public class PanelHubControl : IPanelHubControl
    {
        private readonly ILogger<PanelHubControl> _logger;
        private readonly IHubContext<PanelsHub> _panelsHub;
        public PanelHubControl(IHubContext<PanelsHub> panelsHub, ILogger<PanelHubControl> logger)
        {
            _logger = logger;
            _panelsHub = panelsHub;
        }
        public async Task PanelUIAddNewDeviceCard(KeyValuePair<string, ClientDeviceConnection> DeviceClient)
        {
            try
            {
                await this.PanelUISendPanelLog("Dispositivo conectou (Conn: " + DeviceClient.Key + ")");
                ClientDeviceConnection DeviceClientConn = DeviceClient.Value;

                var objToSend = new
                {
                    deviceid = DeviceClientConn.device_id,
                    connectionid = DeviceClientConn.conn_id,
                    dateconn = DeviceClientConn.dateconn,
                    currentstate = DeviceClientConn.current_sate
                };

                await _panelsHub.Clients.All.SendAsync("addNewDeviceCard", string.Format("{0}\n", JsonSerializer.Serialize(objToSend)));

            }
            catch (Exception ex)
            {
                _logger.LogError("[Erro PanelHubControl] > Erro ao enviar ação de adição do card de dispositivo no painel ("+DateTime.Now+"), device-id: " + DeviceClient.Value.device_id + ", Erro: "+ex.Message);
            }
            
        }

        public async Task PanelUIDisableButtonConfirmed(string IdConnDeviceFrom, string Button)
        {
            try
            {
                await _panelsHub.Clients.All.SendAsync("disableButton", IdConnDeviceFrom, Button);
            }
            catch (Exception ex)
            {
                _logger.LogError("[Erro PanelHubControl] > Erro ao enviar confirmação de botão ("+Button+") no painel de monitoramento ("+DateTime.Now+"), Erro: "+ex.Message);
            }
        }

        public async Task PanelUIReceiveEnergyValue(string IdConnDeviceFrom, string ValueEnergy)
        {
            try
            {
                await _panelsHub.Clients.All.SendAsync("receiveEnergyValue", IdConnDeviceFrom, ValueEnergy);
            }
            catch (Exception ex)
            {
                _logger.LogError("[Erro PanelHubControl] > Problema ao enviar valor de energia no painel de monitoramento (" + DateTime.Now + "), id-conn: "+ IdConnDeviceFrom + ", Erro: " + ex.Message);
            }
        }

        public async Task PanelUIReceiveListDevicesClients(string idConnPanel, List<ItemDeviceClient> ListDevices)
        {
            try
            {
                if (ListDevices.Any())
                {
                    await _panelsHub.Clients.Client(idConnPanel).SendAsync("receiveListClients", string.Format("{0}\n", JsonSerializer.Serialize(ListDevices)));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("[Erro PanelHubControl] > Erro enviar lista dos devices dos dispositivos clientes ao Painel de Monitoramento (" + DateTime.Now + "), Erro: " + ex.Message);

            }
            
        }

        public async Task PanelUIRemoveDeviceCard(KeyValuePair<string, ClientDeviceConnection> DeviceClient)
        {
            try
            {
                string IdConnDeviceSocket = DeviceClient.Key;
                await this.PanelUISendPanelLog("Dispositivo (" + DeviceClient.Value.device_id + ") desconectou");
                await _panelsHub.Clients.All.SendAsync("removeDeviceCard", IdConnDeviceSocket);
            }
            catch (Exception ex)
            {
                _logger.LogError("[Erro PanelHubControl] > Erro ao enviar comando de remover card do dispositivo no Painel de monitoramento (" + DateTime.Now + "), device-id: "+ DeviceClient.Value.device_id+ ", Erro: " + ex.Message);
            }
            
        }

        public async Task PanelUISendPanelLog(string Msg)
        {
            try
            {
                await _panelsHub.Clients.All.SendAsync("sendPanelLog", Msg + " (" + DateTime.Now + ")");
            }
            catch (Exception ex)
            {
                _logger.LogError("[Erro PanelHubControl] > Problema ao enviar mensagem simples de log no painel (" + DateTime.Now + "), Erro: " + ex.Message);
            }
        }

        public async Task PanelUIUpdateDeviceClientsOn(int NumDevices)
        {
            try
            {
                await _panelsHub.Clients.All.SendAsync("updateClientsOn", NumDevices);
            }
            catch (Exception ex)
            {
                _logger.LogError("[Erro PanelHubControl] > Erro ao enviar número de dispositivos online (" + DateTime.Now + "), Erro: " + ex.Message);
            }
        }

        public async Task PanelUINotifyDeviceClientsCount(int DevicesOnlineCount)
        {
            try
            {
                await _panelsHub.Clients.All.SendAsync("updateClientsOn", DevicesOnlineCount);
            }
            catch (Exception ex)
            {
                _logger.LogError("[ERRO PanelHubControl] > Erro no método de atualizar contagem de dispositivos Online (" + DateTime.Now + "), Erro: " + ex.Message);
            }
            
        }

        public async Task PanelUINotifyDeviceIdUpdated(KeyValuePair<string, ClientDeviceConnection> DeviceClient)
        {
            try
            {
                await _panelsHub.Clients.All.SendAsync("updateDeviceId", DeviceClient.Key, DeviceClient.Value.device_id);
            }
            catch (Exception ex)
            {
                _logger.LogError("[Erro PanelHubControl] > Problema ao atualizar ID do device (" + DateTime.Now + "), device-id: "+ DeviceClient.Value.device_id + ", Erro: " + ex.Message);
            }
            
        }

        public async Task PanelUINotifyDeviceIpUpdated(KeyValuePair<string, ClientDeviceConnection> DeviceClient)
        {
            try
            {
                await _panelsHub.Clients.All.SendAsync("updateDeviceIp", DeviceClient.Key, DeviceClient.Value.device_ip);
            }
            catch (Exception ex)
            {
                _logger.LogError("[Erro PanelHubControl] > Problema ao notificar o ip do device (" + DateTime.Now + "), device-id: "+ DeviceClient.Value.device_id + ", Erro: " + ex.Message);
            }
            
        }
    }
}
