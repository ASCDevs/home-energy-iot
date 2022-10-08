namespace home_energy_iot_monitoring.Interfaces
{
    public interface ICostumerHubControl
    {
        Task CostumerUIReceiveEnergyValue(string CostumerConnId, string ValueEnergy);
        Task CostumerUIDisableButtonConfirmed(string CostumerConnId, string Button);
        Task CostumerUINotifyDisconnection(string CostumerConnId);
        Task CostumerUINotifyConnection(string CostumerConnId);
        Task CostumerUIReceiveIP(string CostumerConnId, string DeviceIP);
        Task CostumerUIRemoveIP(string CostumerConnId);
    }
}
