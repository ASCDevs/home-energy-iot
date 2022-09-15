namespace home_energy_iot_monitoring.Interfaces
{
    public interface ICostumerHubControl
    {
        Task CostumerUIReceiveEnergyValue(string CostumerConnId, string ValueEnergy);
    }
}
