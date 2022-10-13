using home_energy_iot_monitoring.Domains;

namespace home_energy_iot_monitoring.Interfaces
{
    public interface IDeviceSocketHolder
    {
        Task AddAsync(HttpContext context);
        Task SendActionToClient(string idConnection, string Action);
        int CountClients();
        Task SendListClientsOn(string PanelConnectionId);
        ClientDeviceConnection GetDeviceOnlineInfo(string DeviceID);
        Task CostumerActionStopDevice(string DevideID);
        Task CostumerActionContinueDevice(string DevideID);
        void NotifyPanelUsersOnline(int qtdUsers);

    }
}