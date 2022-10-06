using home_energy_iot_monitoring.Domains;

namespace home_energy_iot_monitoring.Interfaces
{
    public interface IPanelHubControl
    {
        Task PanelUIRemoveDeviceCard(KeyValuePair<string, ClientDeviceConnection> DeviceClient);
        Task PanelUIUpdateDeviceClientsOn(int NumDevices);
        Task PanelUISendPanelLog(string Msg);
        Task PanelUIAddNewDeviceCard(KeyValuePair<string, ClientDeviceConnection> DeviceClient);
        Task PanelUIReceiveEnergyValue(string IdConnDeviceFrom, string ValueEnergy);
        Task PanelUIReceiveListDevicesClients(string idConnPanel, List<ItemDeviceClient> ListDevices);
        Task PanelUIDisableButtonConfirmed(string IdConnDeviceFrom, string Button);
        Task PanelUINotifyDeviceClientsCount(int DevicesOnlineCount);
        Task PanelUINotifyDeviceIdUpdated(KeyValuePair<string, ClientDeviceConnection> DeviceClient);
        Task PanelUINotifyDeviceIpUpdated(KeyValuePair<string, ClientDeviceConnection> DeviceClient);
    }
}
