using home_energy_iot_monitoring.Domains;

namespace home_energy_iot_monitoring.Interfaces
{
    public interface IPanelHubControl
    {
        Task PanelUIRemoveDeviceCard(string IdConnDeviceSocket);
        Task PanelUIUpdateDeviceId(string IdConnDeviceSocket, string DeviceId);
        Task PanelUIUpdateDeviceClientsOn(int NumDevices);
        Task PanelUISendPanelLog(string Msg);
        Task PanelUIAddNewDeviceCard(ClientDeviceConnection CliDeviceConn);
        Task PanelUIReceiveEnergyValue(string IdConnDeviceFrom, string ValueEnergy);
        Task PanelUIReceiveListDevicesClients(List<object> ListDevices);
        Task PanelUIDisableButton(string IdConnDeviceFrom, string Button);

    }
}
